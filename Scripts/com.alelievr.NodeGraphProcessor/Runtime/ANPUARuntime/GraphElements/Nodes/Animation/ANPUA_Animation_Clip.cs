
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Animation/Clip/Use File")]
    public class ANPUA_Animation_Clip : BaseNode, ANPUA_INode
    {
        [Output(name = "Curve(s)", allowMultiple = true)]
        public ANPUA_NodeLink_Curve link_OUT;

        [Input(name = "Clip", allowMultiple = false), SerializeField]
        public AnimationClip clip;


        public override string name => "Clip (File)";

        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}