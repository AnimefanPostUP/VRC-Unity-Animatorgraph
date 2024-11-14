
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;

namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Logic/Compare")]
    public class ANPUA_Condition_Compare : BaseNode, ANPUA_INode
    {
        public override string name => "Compare";
        public override Color color =>  new Color(0.2f, 0.3f, 0.5f);

        //Booleans
        [Input(name = "Parameter", allowMultiple = false)]
        public ANPUA_NodeLink_Parameter  varA_IN;

        //Booleans
        [Input(name = "Variable", allowMultiple = false)]
        public ANPUA_NodeLink_Variable varB_IN;

        

        //Enum for comparison, e.g. Greater, Less, Equal, GreaterOrEqual, LessOrEqual, NotEqual
        public CompareEnum compare_IN;

        public enum CompareEnum
        {
            Greater,
            Less,
            Equal,
            GreaterOrEqual,
            LessOrEqual,
            NotEqual
        }



        [Output(name = "Logic", allowMultiple = true)]
        public ANPUA_NodeLink_Logic link_OUT;


 

        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}