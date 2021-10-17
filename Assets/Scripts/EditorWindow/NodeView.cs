using System;
using AI.BehaviourTrees.BaseTypes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using GraphNode = UnityEditor.Experimental.GraphView.Node;
using BTNode = AI.BehaviourTrees.BaseTypes.Node;


//Visual represnetation of a Behaviour Tree node in the BehaviourTreeView
public sealed class NodeView : GraphNode
{
    //The Behaviour Tree node that this node view is displaying
    public readonly BTNode node;
    
    //Input and output ports of this node
    public Port InputPort { private set; get; }
    public Port OutputPort { private set; get; }
    
    //Event for when this node is selected
    public Action<NodeView> OnNodeSelected;
    
    public NodeView(BTNode node)
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.GraphInfo.guid;

        style.left = node.GraphInfo.position.x;
        style.top = node.GraphInfo.position.y;

        CreateInputPorts();
        CreateOutputPorts();
    }
    private void CreateInputPorts()
    {
        //Create input node - all types have the same number of inputs allowed - 1
        InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        InputPort.portName = "";
        inputContainer.Add(InputPort);
    }
    
    private void CreateOutputPorts()
    {
        Port.Capacity outputPortCapacity = Port.Capacity.Single;

        switch (node)
        {
            //Action Nodes have no output
            case ActionNode _:
                return;
            case CompositeNode _:
                //Composite Nodes have multiple outputs
                outputPortCapacity = Port.Capacity.Multi;
                break;
            case DecoratorNode _:
                //Decorator Nodes can only have 1 output
                outputPortCapacity = Port.Capacity.Single;
                break;
        }
        
        OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, outputPortCapacity, typeof(bool));
        OutputPort.portName = "";
        outputContainer.Add(OutputPort);
    }

    //Override position setting so that we can store it in the node
    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        if (node)
        {
            node.GraphInfo.position.x = newPos.xMin;
            node.GraphInfo.position.y = newPos.yMin;
        }
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnNodeSelected?.Invoke(this);
    }
}
