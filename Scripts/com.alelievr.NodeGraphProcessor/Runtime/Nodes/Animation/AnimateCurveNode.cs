
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Animation/Curves/Combine Curves")]
    public class AnimateCurveNode : BaseNode, Animator_INode
    {
        [Input(name = "Clip A", allowMultiple = false)]
        public ANPUA_NodeLink_Anim_Clip link_A;

        [Input(name = "Clip B", allowMultiple = false)]
        public ANPUA_NodeLink_Anim_Clip link_B;

        //Float Length
        [Input(name = "Length", allowMultiple = false), SerializeField]
        public float length;



        [Output(name = "Clip", allowMultiple = true), SerializeField]
        public ANPUA_NodeLink_Anim_Clip link_OUT;


        public override string name => "Combine Curves";

        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}