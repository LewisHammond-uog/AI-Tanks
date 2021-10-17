using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.BehaviourTrees.BaseTypes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using BTNode = AI.BehaviourTrees.BaseTypes.Node;

public class BehaviourTreeView : GraphView
{
    public new class  UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> {}

    private BehaviourTree currentTree;

    public BehaviourTreeView()
    {
        //Create a grid background
        Insert(0, new GridBackground());
        
        //Add mainpulators so that we can move around the grid view
        this.AddManipulator(new ContentDragger()); //Allow to pan around the graph
        this.AddManipulator(new ContentZoomer()); //Allow zooming in the graph
        this.AddManipulator(new SelectionDragger()); //Allow the movement of nodes in the graph
        this.AddManipulator(new RectangleSelector()); //Allow box selection
        
        //Subscribe to any graph view changes
        graphViewChanged += OnGraphViewChanged;
        
        // Add stylesheet to give elements their style - similar to CSS
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/EditorWindow/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    /// <summary>
    /// Populate the behaviour tree view with the nodes from a behaviour tree
    /// </summary>
    /// <param name="tree">Tree to populate the view with</param>
    public void PopulateView(BehaviourTree tree)
    {
        this.currentTree = tree;
        
        //Delete anything from an old population of the BT view
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;
        
        //Create Nodes for all of the nodes in the tree
        foreach (BTNode node in currentTree.Nodes)
        {
            CreateNodeView(node);
        }
        
        //Create Edges by getting the children of each node and connecting their children
        foreach (BTNode currentTreeNode in currentTree.Nodes)
        {
            List<BTNode> children = tree.GetChildren(currentTreeNode);
            NodeView parentView = FindNodeView(currentTreeNode);
            foreach (BTNode child in children)
            {
                NodeView childView = FindNodeView(child);

                Edge connection = parentView.OutputPort.ConnectTo(childView.InputPort);
                AddElement(connection);
            }
        }
    }

    //Called when any graph view elements are changed
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        //Remove elements that are deleted from the graph in the Behaviour Tree
        if (graphViewChange.elementsToRemove != null)
        {
            foreach (GraphElement graphElement in graphViewChange.elementsToRemove)
            {
                //Remove nodes when they are deleted
                NodeView nodeView = graphElement as NodeView;
                if (nodeView != null)
                {
                    currentTree.DeleteNode(nodeView.node);
                }
                
                //Remove edges when they are deleted
                if (graphElement is Edge edge)
                {
                    if (edge.output.node is NodeView parentNodeView && edge.input.node is NodeView childNodeView)
                    {
                        currentTree.RemoveChild(parentNodeView.node, childNodeView.node);
                    }
                }
            }
        }

        if (graphViewChange.edgesToCreate != null)
        {
            foreach (Edge edge in graphViewChange.edgesToCreate)
            {
                //Parent node is the node that we output from
                //Child node is the node that we are inputting to
                if (edge.output.node is NodeView parentView && edge.input.node is NodeView childView)
                {
                    currentTree.AddChild(parentView.node, childView.node);
                }
            }
        }
        
        return graphViewChange;
    }

    //Override GetCompatiblePorts to define which ports can connect to which other ports
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
            endPort.direction != startPort.direction && //Make sure that we are connecting to different port directions (no input -> input)
            endPort.node != startPort.node //Do not allow nodes to connect to themselves
        ).ToList();
    }

    //todo change this to use search
    //Override the default behabiour of the content menu (when you right click on the graph)
    //to show the nodes we can create
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);

        //todo clean this up
        TypeCache.TypeCollection actionTypes = TypeCache.GetTypesDerivedFrom<BTNode>();

        foreach (Type type in actionTypes)
        {
            //Exclude abstract classes
            if (type.IsAbstract)
            {
                continue;
            }
            
            if (type.BaseType != null)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }
    }

    /// <summary>
    /// Create a node in the BT and create a visual element to represent it
    /// </summary>
    /// <param name="type"></param>
    private void CreateNode(System.Type type)
    {
        BTNode createdNode = currentTree.CreateNode(type);
        if (createdNode)
        {
            CreateNodeView(createdNode);
        }
    }

    /// <summary>
    /// Create a visual representation of a BT node in the graph view
    /// </summary>
    /// <param name="node"></param>
    private void CreateNodeView(BTNode node)
    {
        NodeView nodeVisualRep = new NodeView(node);
        AddElement(nodeVisualRep);
    }   

    /// <summary>
    /// Find a node view from a given BT Node
    /// </summary>
    /// <param name="btNode"> Behavior Tree node to find node view of</param>
    /// <returns>Node View of the given behaviour tree node</returns>
    private NodeView FindNodeView(BTNode btNode)
    {
        return GetNodeByGuid(btNode.GraphInfo.guid) as NodeView;
    }
}
