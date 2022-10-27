// based mostly off of reading code by KiwiCoder and James La Fritz
// https://github.com/JamesLaFritz/GraphViewBehaviorTree
// modified because we have a different execution paradigm

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using MiniDini.Nodes;

namespace MiniDini
{
    /// <summary>
    /// Node Graph requires that the "output" Node be set, derived from <a hfref="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/ScriptableObject.html">UnityEngine.ScriptableObject</a>
    /// the basic idea is that if we have an output node, we evaluate it and it evaluates
    /// its parents on up the heirarchy of nodes
    /// </summary>
    [CreateAssetMenu(fileName = "NodeGraph", menuName = "NodeGraph")]
    [System.Serializable]
    public class NodeGraph : ScriptableObject
    {
        /// <value>
        /// The output node of the graph
        /// </value>
        [HideInInspector] public Node outputNode;

        /// <value>
        /// The Nodes that the graph has.
        /// </value>
        [SerializeField, HideInInspector] private List<Node> nodes = new List<Node>();
 
        /// <value>
        /// Get all of the Nodes in the Graph.
        /// </value>
        public List<Node> GetNodes()
        {
            return nodes;
        }

        /// <summary>
        /// Create a new Node and add it to the nodes.
        /// </summary>
        /// <param name="type">The Type of Node to create.</param>
        public Node CreateNode(System.Type type)
        {
            Node node = CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();

            nodes.Add(node);

            // todo: add the AND part of this where we check if the node is an output node
            if (outputNode == null)
                outputNode = node;

            return node;
        }
        
        /// <summary>
        /// Delete a Node from the tree.
        /// </summary>
        /// <param name="node">The Node to Delete.</param>
        public void DeleteNode(Node node)
        {
            nodes.Remove(node);

            if (outputNode == node)
            {
                outputNode = null;
                // note: we might want to look for other output nodes here? and automatically rewire?
            }
        }

        /// <summary>
        /// Add a child node to the parent.
        /// </summary>
        /// <param name="parent">The parent Node.</param>
        /// <param name="child">The Node to add to the parent.</param>
        public void AddChild(Node parent, Node child)
        {
            if (!nodes.Contains(parent)) return;

            nodes[nodes.IndexOf(parent)].AddChild(child);
            // pac - add parent so we can query from children to do things like merge
            child.AddParent(nodes[nodes.IndexOf(parent)]);

            if (nodes.Contains(child)) return;

            nodes.Add(child);
        }

        /// <summary>
        /// Remove a node from the parent.
        /// </summary>
        /// <param name="parent">The parent Node.</param>
        /// <param name="child">The Node to remove from the parent.</param>
        public void RemoveChild(Node parent, Node child)
        {
            if (!nodes.Contains(parent)) return;

            nodes[nodes.IndexOf(parent)].RemoveChild(child);
            child.RemoveParent(nodes[nodes.IndexOf(parent)]);
        }

        /// <summary>
        /// Get a list of children from the parent.
        /// </summary>
        /// <param name="parent">The node to get the children from</param>
        /// <returns>A list of children Nodes that the parent contains.</returns>
        public List<Node> GetChildren(Node parent)
        {
            return !nodes.Contains(parent)
                ? new List<Node>()
                : nodes[nodes.IndexOf(parent)].GetChildren();
        }


        /// <value>
        /// Does the Graph have an output Node?
        /// </value>
        private bool m_hasoutputNode;

        /// <summary>
        /// Traverse the node and run the Action.
        /// </summary>
        public void Traverse(Node node, System.Action<Node> visitor)
        {
            if (!node) return;
            visitor?.Invoke(node);
            node.GetChildren()?.ForEach((n) => Traverse(n, visitor));
        }

        /// <summary>
        /// Clone the graph.
        /// </summary>
        /// <returns>A Clone of the graph</returns>
        public NodeGraph Clone()
        {
            NodeGraph graph = Instantiate(this);
            graph.nodes = new List<Node>();
            foreach (Node node in nodes)
            {
                graph.nodes.Add(node.Clone());
            }

            graph.outputNode = graph.nodes[nodes.IndexOf(outputNode)];
            Traverse(outputNode, (n) =>
            {
                int nodeIndex = nodes.IndexOf(n);
                foreach (int childIndex in nodes[nodeIndex]?.GetChildren().Select(c => nodes.IndexOf(c)))
                {
                    graph.nodes[nodeIndex].RemoveChild(nodes[childIndex]);
                    graph.AddChild(graph.nodes[nodeIndex], graph.nodes[childIndex]);
                }
            });

            return graph;
        }
    }
}

