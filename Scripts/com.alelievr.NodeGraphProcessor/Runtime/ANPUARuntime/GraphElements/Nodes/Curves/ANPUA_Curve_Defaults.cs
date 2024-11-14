
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Animation/Curves/Default Curve")]
    public class ANPUA_Curve_Defaults : BaseNode, ANPUA_INode
    {
        [Output(name = "Curve", allowMultiple = true)]
        public ANPUA_NodeLink_Curve link_OUT;


        //[Input(name = "isActive (bool)", allowMultiple = false)]
        public bool isActive;

        public override string name => "Default Curve";

        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}