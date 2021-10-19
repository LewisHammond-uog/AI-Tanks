using System;
using AI.BehaviourTrees;
using AI.BehaviourTrees.BaseTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;


public class BehaviourTreeEditor : UnityEditor.EditorWindow
{
    private BehaviourTreeView treeView;
    private InspectorView inspectorView;

    [MenuItem("BehaviourTreeEditor/Editor ...")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }
    
    private void OnEnable()
    {
        //Intercept when the playmode state changes
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    //Called when the editor changes playmode state
    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        //When we enter play/edit mode update the selection so that we cannot
        //show runtime graphs when we are not in play mode... we can enter really
        //invalid states if that happens (i.e editing graphs that are no longer in memory)
        switch(obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
            case PlayModeStateChange.ExitingPlayMode:
            default:
                break;
        }
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/EditorWindow/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/EditorWindow/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);
        
        //Get the behaviour tree view and inspector view from the Editor Window
        treeView = root.Q<BehaviourTreeView>();
        inspectorView = root.Q<InspectorView>();
        
        //Subscribe to when the node selection changes
        treeView.OnNodeSelected += OnNodeSelectionChanged;
        
        //Manully call OnSelectionChange so we refresh the view after recompile
        OnSelectionChange();
    }

    //Intercept when an asset is opened so that double clicking on a behaviour tree opens the editor
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        if (!(Selection.activeObject is BehaviourTree)) return false;
        OpenWindow();
        return true;

    } 


    //Called when the asset selection in the editor is changed
    private void OnSelectionChange()
    {
        //Check if the current object that the user has highliged is a behaviour tree
        BehaviourTree tree = Selection.activeObject as BehaviourTree;
        
        //If a tree was not selected see if we selected a gameObject that has a Agent attached and is running at BT
        if (!tree)
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject)
            {
                Agent aiAgent = selectedObject.GetComponent<Agent>();
                if (aiAgent)
                {
                    tree = aiAgent.RunningTree;
                }
            }
        }

        //If we have a valid tree then populate the tree, only allow the asset to be opened if it is ready or the game is playing
        if (tree && (AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()) || Application.isPlaying))
        {
            treeView.PopulateView(tree);
        }
        else
        {
            treeView.ClearGraph();
        }
    }

    //Called when the selected node in the TreeView is changed
    private void OnNodeSelectionChanged(NodeView nodeView)
    {
        //Update the inspector view to show properties 
        inspectorView.UpdateSelection(nodeView);
    }
}