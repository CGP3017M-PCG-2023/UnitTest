using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using MiniDini.Editor.Views;
using MiniDini.Nodes;
using MiniDini.Runner;


namespace MiniDini.Editor
{
    /// <summary>
    /// Derive from <a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/EditorWindow.html" rel="external">UnityEditor.EditorWindow</a> class to create an editor window to Edit Behavior Tree Scriptable Objects.
    /// Requires file named "NodeGraphEditor.uxml" to be in an Editor Resources Folder
    /// Uses Visual Elements requires a <see cref="NodeGraphView"/> an an <a href="https://docs.unity3d.com/ScriptReference/UIElements.IMGUIContainer.html" rel="external">UnityEngine.UIElements.IMGUIContainer</a> with a name of InspectorView.
    /// </summary>
    public class NodeGraphEditor : EditorWindow
    {
        /// <value> The <see cref="NodeGraphView"/> associated with this view. </value>
        private NodeGraphView m_graphView;

        /// <value> The <a href="https://docs.unity3d.com/ScriptReference/UIElements.IMGUIContainer.html" rel="external">UnityEngine.UIElements.IMGUIContainer</a> associated with the view. </value>
        private IMGUIContainer m_inspectorView;

        /// <value> The <a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Editor.html" rel="external">UnityEditor.Editor</a> associated with this view. </value>
        private UnityEditor.Editor m_editor;

        /// <summary>
        /// Adds a Entry to Window/Behavior Tree/Editor
        /// Will Open the Behavior Tree Editor to Edit Behavior Trees
        /// </summary>
        [MenuItem("Window/Minidini-NodeGraph/Editor")]
        public static void OpenEditor()
        {
            GetWindow<NodeGraphEditor>("Node Graph Editor");
        }

        /// <summary>
        /// Use Unity Editor Call Back On Open Asset.
        /// </summary>
        /// <returns>True if this method handled the asset. Else return false.</returns>
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject is not NodeGraph) return false;
            OpenEditor();
            return true;
        }

        /// <summary>
        /// CreateGUI is called when the EditorWindow's rootVisualElement is ready to be populated.
        ///
        /// Clones a Visual Tree Located in an Editor Resources Folder BehaviorTreeEditor.uxml";
        /// </summary>
        private void CreateGUI()
        {
            //VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("BehaviorTreeEditor.uxml");
            VisualTreeAsset vt = Resources.Load<VisualTreeAsset>("NodeGraphEditor");
            vt.CloneTree(rootVisualElement);

            m_graphView = rootVisualElement.Q<NodeGraphView>();
            m_inspectorView = rootVisualElement.Q<IMGUIContainer>("InspectorView");
            m_graphView.onNodeSelected = OnNodeSelectionChange;

            OnSelectionChange();
        }

        /// <summary>
        /// This function is called when the object is loaded.
        /// </summary>
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnplayModeStateChanged;
            EditorApplication.playModeStateChanged += OnplayModeStateChanged;
        }

        /// <summary>
        /// This function is called when the scriptable object goes out of scope.
        /// </summary>
        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnplayModeStateChanged;
        }

        /// <summary>
        /// OnInspectorUpdate is called at 10 frames per second to give the inspector a chance to update.
        /// </summary>
        private void OnInspectorUpdate()
        {
            m_graphView?.UpdateNodeStates();
        }

        /// <summary>
        /// Called whenever the selection has changed.
        ///
        /// 
        /// </summary>
        private void OnSelectionChange()
        {
            NodeGraph graph = Selection.activeObject as NodeGraph;
            if (graph == null)
            {
                if (Selection.activeGameObject)
                {
                    NodeGraphRunner graphRunner = Selection.activeGameObject.GetComponent<NodeGraphRunner>();
                    if (graphRunner)
                    {
                        graph = graphRunner.graph;
                    }
                }
            }

            if (graph != null)
            {
                if (Application.isPlaying || AssetDatabase.CanOpenAssetInEditor(graph.GetInstanceID()))
                {
                    SerializedObject so = new SerializedObject(graph);
                    rootVisualElement.Bind(so);
                    if (m_graphView != null)
                        m_graphView.PopulateView(graph);

                    return;
                }
            }

            rootVisualElement.Unbind();

            TextField textField = rootVisualElement.Q<TextField>("BehaviorTreeName");
            if (textField != null)
            {
                textField.value = string.Empty;
            }
        }

        /// <summary>
        /// Method registered to <a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/EditorApplication-playModeStateChanged.html" rel="external">UnityEditor.EditorApplication.playModeStateChanged</a>
        /// </summary>
        /// <param name="obj">The <a href="https://docs.unity3d.com/2021.3/Documentation/ScriptReference/PlayModeStateChange.html" rel="external">UnityEditor.PlayModeStateChange</a> object.</param>
        private void OnplayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                // Occurs during the next update of the Editor application if it is in edit mode and was previously in play mode.
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                // Occurs when exiting edit mode, before the Editor is in play mode.
                case PlayModeStateChange.ExitingEditMode:
                    break;
                // Occurs during the next update of the Editor application if it is in play mode and was previously in edit mode.
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                // Occurs when exiting play mode, before the Editor is in edit mode.
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }

        /// <summary>
        /// Used to Observe the tree view for when a Node is Selected.
        /// Causes the Node to display in the Inspector View.
        /// </summary>
        /// <param name="node">The Selected Node</param>
        private void OnNodeSelectionChange(Node node)
        {
            m_inspectorView.Clear();
            DestroyImmediate(m_editor);
            m_editor = UnityEditor.Editor.CreateEditor(node);
            m_inspectorView.onGUIHandler = () =>
            {
                if (m_editor.target)
                    m_editor.OnInspectorGUI();
            };
        }
    }
}


