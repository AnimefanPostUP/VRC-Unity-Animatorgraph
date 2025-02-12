using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

public class ConditionStack
{
    public List<ConditionPackage> Conditions { get; private set; }

    public ConditionStack()
    {
        Conditions = new List<ConditionPackage>();
    }

    public void AddCondition(ConditionPackage condition)
    {
        Conditions.Add(condition);
    }

    public void ClearConditions()
    {
        Conditions.Clear();
    }

    public bool isEmpty()
    {
        return Conditions.Count == 0;
    }
}

public class ConditionBundle
{
    public List<ConditionStack> Stacks { get; private set; }

    public ConditionBundle()
    {
        Stacks = new List<ConditionStack>();
    }

    public void AddStack(ConditionStack stack)
    {
        Stacks.Add(stack);
    }

    public void ClearStacks()
    {
        Stacks.Clear();
    }

    public bool isEmpty()
    {
       bool isEmpty = true;
       foreach (var stack in Stacks)
         {
              if (!stack.isEmpty())
              {
                isEmpty = false;
                break;
              }
         }
         return isEmpty;
    }
}