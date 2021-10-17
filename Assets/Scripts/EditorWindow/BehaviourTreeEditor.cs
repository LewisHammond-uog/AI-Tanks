using System;
using AI.BehaviourTrees.BaseTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


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
        
        //Manully call OnSelectionChange so we refresh the view after recompile
        OnSelectionChange();
    }


    private void OnSelectionChange()
    {
        //Check if the current object that the user has highliged is a behaviour tree
        BehaviourTree tree = Selection.activeObject as BehaviourTree;
        if (tree)
        {
            treeView.PopulateView(tree);
        }
    }
}