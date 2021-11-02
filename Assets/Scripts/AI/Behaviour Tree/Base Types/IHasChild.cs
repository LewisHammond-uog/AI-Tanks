using System.Collections;
using System.Collections.Generic;
using AI.BehaviourTrees.BaseTypes;
using UnityEngine;

public interface IHasChild
{
    public Node GetChild();
    public void SetChild(Node child);
}
