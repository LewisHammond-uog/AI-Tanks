using System.Collections;
using System.Collections.Generic;
using AI.BehaviourTrees.BaseTypes;
using UnityEngine;

public interface IHasChildren
{
    public void AddChild(Node child);
    public void RemoveChild(Node child);
    public IEnumerable<Node> GetChildren();
}
