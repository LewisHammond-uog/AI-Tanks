using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AI.BehaviourTrees.BaseTypes
{
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
        //todo private this
        public Node rootNode;
        public NodeStatus treeState;

        //All of the nodes in the tree - including those that are not connected to
        //the root
        public List<Node> Nodes { private set; get; }

        public BehaviourTree()
        {
            Nodes = new List<Node>();
            treeState = NodeStatus.Running;
        }
        
        public NodeStatus Update()
        {
            return rootNode.Update();
        }

        public Node CreateNode(System.Type nodeType)
        {
            //Create an instance of the node
            Node node = ScriptableObject.CreateInstance(nodeType) as Node;
            if (!node)
            {
                return null;
            }
            
            //Set the node properties and give it a GUID to be a unique indenifier
            node.name = nodeType.Name;
            node.guid = GUID.Generate().ToString();
            Nodes.Add(node);

            //Add this as a sub asset of the tree asset
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            
            return node;
        }

        public void DeleteNode(Node node)
        {
            //Null check node
            if (node == null)
            {
                return;
            }
            
            //Remove node from our refrence list and remove it as sub asset
            Nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parentNode, Node childNode)
        {
            if (parentNode == null || childNode == null)
            {
                return;
            }
            
            //Change the decorators single child
            DecoratorNode decoratorNode = parentNode as DecoratorNode;
            if (decoratorNode != null)
            {
                decoratorNode.Child = childNode;
                return;
            }
            
            //Add to the composite nodes children
            CompositeNode compositeNode = parentNode as CompositeNode;
            if (compositeNode)
            {
                compositeNode.AddChild(childNode);
                return;
            }
        }

        public void RemoveChild(Node parentNode, Node childNode)
        {   
            if (parentNode == null || childNode == null)
            {
                return;
            }
            
            //Remove the single child from the decorator node - only if the supplied child is the actual child
            DecoratorNode decoratorNode = parentNode as DecoratorNode;
            if (decoratorNode != null)
            {
                if (decoratorNode.Child == childNode)
                {
                    decoratorNode.Child = null;
                    return;
                }
            }
            
            //Remove child from the list of children in the Composite Node
            CompositeNode compositeNode = parentNode as CompositeNode;
            if (compositeNode != null)
            {
                compositeNode.RemoveChild(compositeNode);
                return;
            }
        }

        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();

            DecoratorNode decoratorNode = parent as DecoratorNode;
            if (decoratorNode && decoratorNode.Child != null)
            {
                children.Add(decoratorNode.Child);
            }
            
            CompositeNode compositeNode = parent as CompositeNode;
            if (compositeNode)
            {
                children = (List<Node>) compositeNode.GetChildren();
            }

            return children;
        }
    }
}