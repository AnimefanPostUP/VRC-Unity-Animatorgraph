
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Animation/Curves/Combine Curves")]
    public class ANPUA_Curve_Curve : BaseNode, ANPUA_INode
    {
        [Input(name = "Curve A", allowMultiple = false)]
        public ANPUA_NodeLink_Curve link_A;

        [Input(name = "Curve B", allowMultiple = false)]
        public ANPUA_NodeLink_Curve link_B;

        //Float Length
        [Input(name = "Length", allowMultiple = false), SerializeField]
        public float length;



        [Output(name = "Curve(s)", allowMultiple = true), SerializeField]
        public ANPUA_NodeLink_Curve link_OUT;


        public override string name => "Combine Curves";

        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}