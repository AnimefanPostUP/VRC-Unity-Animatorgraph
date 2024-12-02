
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Animation/Clip")]
    public class AnimateClipNode : BaseNode, Animator_INode
    {
        [Output(name = "Motiondata", allowMultiple = true)]
        public ANPUA_NodeLink_Anim_Clip link_OUT;

        [Input(name = "Clip", allowMultiple = false), SerializeField]
        public AnimationClip clip;


        public override string name => "Clip";

        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}