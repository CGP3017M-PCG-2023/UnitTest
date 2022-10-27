// BehaviorTreeView.cs
// 05-15-2022
// James LaFritz

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Node = MiniDini.Nodes.Node;
using MiniDini;


namespace MiniDini.Editor.Views
{
    /// <summary>
    /// > [!WARNING]
    /// > Experimental: this API is experimental and might be changed or removed in the future.
    /// 
    /// A View for the Behavior Tree, derived from <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.GraphView.html" rel="external">UnityEditor.Experimental.GraphView.GraphView</a>
    /// Can be used in the UI Builder.
    /// </summary>
    public class NodeGraphView : GraphView
    {
        /// <summary>
        /// Required in order to have <see cref="NodeGraphView"/> show up in the UI Builder Library.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<NodeGraphView, UxmlTraits> { }

        /// <summary>
        /// <value>Notifies the Observers that a <see cref="Node"/> has been Selected and pass the <see cref="Node"/> that was selected.</value>
        /// </summary>
        public Action<Node> onNodeSelected;

        /// <summary>
        /// <value>The graph associated with this view.</value>
        /// </summary>
        private NodeGraph m_graph;

        /// <summary>
        /// <value>Does the view have a graph</value>
        /// </summary>
        private bool m_hasGraph;

        /// <summary>
        /// Creates a new <see cref="NodeGraphView"/>.
        /// Required in order to have this show up in the UI Builder Library.
        /// </summary>
        public NodeGraphView()
        {
            style.flexGrow = 1;
            Insert(0, new GridBackground() { name = "grid_background" });
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            Undo.undoRedoPerformed += UndoRedoPerformed;
        }

        /// <summary>
        /// <a href="https://docs.unity3d.com/ScriptReference/Undo-Undo.UndoRedoCallback.html" rel="external">UnityEditor.Undo.UndoRedoCallback</a> assigned to <a href="https://docs.unity3d.com/ScriptReference/Undo-undoRedoPerformed.html" rel="external">UnityEditor.Undo.undoRedoPerformed</a>
        /// </summary>
        private void UndoRedoPerformed()
        {
			PopulateView(m_graph);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Populate the View with the passed in graph
        /// </summary>
        /// <param name="graph">The <see cref="NodeGraph"/> to populate the View from</param>
        public void PopulateView(NodeGraph graph)
        {
            m_graph = graph;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            m_hasGraph = m_graph != null;
            if (!m_hasGraph) return;
            m_graph.GetNodes().ForEach(CreateNodeView);

            foreach (Edge edge in from node in m_graph.GetNodes()
                                  let parentView = GetNodeByGuid(node.guid) as NodeGraphNodeView
                                  where parentView is { output: { } }
                                  from child in m_graph.GetChildren(node)
                                  where child != null
                                  let childView = GetNodeByGuid(child.guid) as NodeGraphNodeView
                                  where childView is { input: { } }
                                  select parentView.output.ConnectTo(childView.input))
            {
                AddElement(edge);
            }
        }

        /// <summary>
        /// Hook into the Graph View Change to delete Nodes when the Node View Element is slated to be Removed.
        /// </summary>
        /// <param name="graphViewChange"><a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Experimental.GraphView.GraphViewChange.html">GraphViewChange</a></param>
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (GraphElement element in graphViewChange.elementsToRemove)
                {
                    switch (element)
                    {
                        case NodeGraphNodeView nodeView:
                            DeleteNode(nodeView.node);
                            break;
                        case Edge edge:
                            {
                                NodeGraphNodeView parentView = edge.output.node as NodeGraphNodeView;
                                NodeGraphNodeView childView = edge.input.node as NodeGraphNodeView;
                                m_graph.RemoveChild(parentView.node, childView.node);
                                int count = (childView.input.connections ?? Array.Empty<Edge>()).Count();
                                childView.node.hasMultipleParents = count > 2;

                                break;
                            }
                    }
                }
            }

            if (graphViewChange.edgesToCreate != null && m_hasGraph)
            {
                foreach (Edge edge in graphViewChange.edgesToCreate)
                {
                    NodeGraphNodeView parentView = edge.output.node as NodeGraphNodeView;
                    NodeGraphNodeView childView = edge.input.node as NodeGraphNodeView;

                    m_graph.AddChild(parentView.node, childView.node);
                    parentView.SortChildren();

                    int count = (childView.input.connections ?? Array.Empty<Edge>()).Count();
                    childView.node.hasMultipleParents = count > 0;
                }
            }

            if (graphViewChange.movedElements != null)
            {
                foreach (NodeGraphNodeView parentNodeView
                         in from movedElement in graphViewChange.movedElements
                            let movedNode = movedElement as NodeGraphNodeView
                            where movedNode is { input: { connections: { } } }
                            from edge in movedNode.input.connections
                            where edge.output is { node: NodeGraphNodeView }
                            select edge.output?.node as NodeGraphNodeView)
                {
                    parentNodeView?.SortChildren();
                }
            }

            return graphViewChange;
        }

        /// <summary>
        /// Adds a <see cref="NodeGraphNodeView"/> from the passed in Node.
        /// </summary>
        /// <param name="node">The <see cref="Node"/> to create a view for.</param>
        private void CreateNodeView(Node node)
        {
            NodeGraphNodeView nodeView = new NodeGraphNodeView(node)
            {
                onNodeSelected = onNodeSelected
            };
            if (m_hasGraph)
                nodeView.onSetRootNode = _ => m_graph.outputNode = node;
            AddElement(nodeView);
        }

        /// <summary>
        /// Create a new <see cref="Node"/> with a Node View.
        /// </summary>
        /// <param name="type">The Type of Node to create.</param>
        /// <param name="initialPosition">The initial position of the Node on the graph when created.</param>
        private void CreateNode(Type type, Vector2 initialPosition)
        {
            if (!m_graph) return;

            Node node = m_graph.CreateNode(type, initialPosition);
            CreateNodeView(node);
            Undo.RecordObject(m_graph, "Node Graph (Create Node)");

            if (Application.isPlaying) return;

            AssetDatabase.AddObjectToAsset(node, m_graph);
            AssetDatabase.SaveAssets();

            Undo.RegisterCreatedObjectUndo(node, "Node Graph (Create Node)");
            EditorUtility.SetDirty(node);
        }

        /// <summary>
        /// Delete a <see cref="Node"/> from the tree.
        /// </summary>
        /// <param name="node">The <see cref="Node"/> to Delete.</param>
        private void DeleteNode(Node node)
        {
            if (!m_graph) return;

            m_graph.DeleteNode(node);
            Undo.RecordObject(m_graph, "Node Graph (Delete Node)");

            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(m_graph);
        }

        /// <summary>
        /// Used to Update the Node State of all nodes in this tree for when Unity is in Play Mode.
        /// </summary>
        public void UpdateNodeStates()
        {
            foreach (NodeGraphNodeView nodeView in nodes.OfType<NodeGraphNodeView>())
            {
                nodeView.UpdateState();
            }
        }

        #region Overrides of GraphView

        /// <summary>
        /// Override <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.GraphView.BuildContextualMenu.html" rel="external">UnityEditor.Experimental.GraphView.GraphView.BuildContextualMenu</a>
        /// Add menu items to the contextual menu.
        /// </summary>
        /// <param name="evt">The (<a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/UIElements.ContextualMenuPopulateEvent.html" rel="external">UnityEngine.UIElements.ContextualMenuPopulateEvent</a>) event holding the menu to populate.</param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 mousePosition = evt.localMousePosition;
            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<Node>();
            foreach (Type type in types)
            {
                if (type.IsAbstract) continue;
                evt.menu.AppendAction($"{type.BaseType.Name}/{type.Name}",
                                      _ => CreateNode(type, mousePosition));
            }

            base.BuildContextualMenu(evt);
        }

        /// <summary>
        /// Override <a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Experimental.GraphView.GraphView.GetCompatiblePorts.html" rel="external">UnityEditor.Experimental.GraphView.GraphView.GetCompatiblePorts</a>
        /// Get all ports compatible with given port.
        /// </summary>
        /// <param name="startPort">
        /// <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.Port.html" rel="external">UnityEditor.Experimental.GraphView.Port</a>
        /// Start port to validate against.
        /// </param>
        /// <param name="nodeAdapter">
        /// <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.Port.html" rel="external">UnityEditor.Experimental.GraphView.Port</a>
        /// Node adapter.
        /// </param>
        /// <returns>List of <a href="https://docs.unity3d.com/ScriptReference/Experimental.GraphView.NodeAdapter.html" rel="external">UnityEditor.Experimental.GraphView.NodeAdapter</a> List of compatible ports.</returns>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList()!.Where(endPort =>
                                             endPort.direction != startPort.direction &&
                                             endPort.node != startPort.node &&
                                             endPort.portType == startPort.portType).ToList();
        }

        #endregion
    }
}