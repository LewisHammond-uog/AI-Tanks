using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AI.BehaviourTrees.BaseTypes
{
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
        //todo private this
        public RootNode rootNode;
        public NodeStatus treeState;

        //All of the nodes in the tree - including those that are not connected to
        //the root
        public List<Node> Nodes { private set; get; } = new List<Node>();

        public BehaviourTree()
        {
            //Nodes = new List<Node>();
            treeState = NodeStatus.Running;
        }
        
        public NodeStatus Update()
        {
            return rootNode.Update();
        }

        /// <summary>
        /// Get a clone of this behaviour tree
        /// </summary>
        /// <returns></returns>
        public BehaviourTree Clone()
        {
            BehaviourTree tree = Instantiate(this);
            tree.rootNode = (RootNode) tree.rootNode.Clone();
            return tree;
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

        /// <summary>
        /// Delete a node from the tree
        /// </summary>
        /// <param name="node"></param>
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

        /// <summary>
        /// Add a child to a given parent node
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childNode"></param>
        public void AddChild(Node parentNode, Node childNode)
        {
            if (parentNode == null || childNode == null)
            {
                return;
            }

            switch (parentNode)
            {
                //Change node with single children
                case IHasChild nodeWithSingleChild:
                    nodeWithSingleChild.SetChild(childNode);
                    break;
                //Change nodes with multiple children
                case IHasChildren nodeWithMultipleChildren:
                    nodeWithMultipleChildren.AddChild(childNode);
                    break;
            }
        }

        /// <summary>
        /// Remove a child from a given parent node
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childNode"></param>
        public void RemoveChild(Node parentNode, Node childNode)
        {   
            if (parentNode == null || childNode == null)
            {
                return;
            }

            switch (parentNode)
            {
                //Remove the single child from the decorator node - only if the supplied child is the actual child
                case IHasChild nodeWithSingleChild:
                    nodeWithSingleChild.SetChild(null);
                    return;
                //Remove child from the list of children in the Composite Node
                case IHasChildren nodeWithMultipleChildren:
                    nodeWithMultipleChildren.RemoveChild(childNode);
                    return;
            }
        }

        /// <summary>
        /// Get all of the children of a given node
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();

            switch (parent)
            {
                //Add the single child to the list as we only have one child
                case IHasChild nodeWithSingleChild when nodeWithSingleChild.GetChild() != null:
                    children.Add(nodeWithSingleChild.GetChild());
                    break;
                //Set the list to be all of the nodes children
                case IHasChildren nodeWithMultipleChildren:
                    children = nodeWithMultipleChildren.GetChildren().ToList();
                    break;
            }

            return children;
        }
    }
}