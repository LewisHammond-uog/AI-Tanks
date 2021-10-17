using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class BehaviourTreeView : GraphView
{
    public new class  UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> {}

    public BehaviourTreeView()
    {
        //Create a grid background
        Insert(0, new GridBackground());
        
        //Add mainpulators so that we can move around the grid view
        this.AddManipulator(new ContentDragger()); //Allow to pan around the graph
        this.AddManipulator(new ContentZoomer()); //Allow zooming in the graph
        this.AddManipulator(new SelectionDragger()); //Allow the movement of nodes in the graph
        this.AddManipulator(new RectangleSelector()); //Allow box selection
        
        // Add stylesheet to give elements their style - similar to CSS
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/EditorWindow/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }
}
