using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;
using UnityEngine;

//Condition to check if the health is > or < or == a given value
public class Condition_HealthQuery : ActionNode
{
    public enum Comparison
    {
        GreaterThan,
        LessThan,
        Equal,
    }

    //Amount of differnce in values allowed for the health and comparision values to be considered 'equal'
    private const float EqualTolerance = float.Epsilon;
    
    [SerializeField] private Comparison comparisonType;
    [SerializeField] private float comparisionValue;
    
    protected override NodeStatus Update_Internal()
    {
        float tankHealth = Owner.HealthComponent.m_CurrentHealth;
        
        //Compare Values by comparision type, success if valid
        switch (comparisonType)
        {
            case Comparison.GreaterThan:
                return tankHealth > comparisionValue ? NodeStatus.Success : NodeStatus.Fail;
            case Comparison.LessThan:
                return tankHealth < comparisionValue ? NodeStatus.Success : NodeStatus.Fail;
            case Comparison.Equal:
                return Math.Abs(tankHealth - comparisionValue) < EqualTolerance ? NodeStatus.Success : NodeStatus.Fail;
            default:
                return NodeStatus.Fail;
        }

        return NodeStatus.Fail;
    }
}
