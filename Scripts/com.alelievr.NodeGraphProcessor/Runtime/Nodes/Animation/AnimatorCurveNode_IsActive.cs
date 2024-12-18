
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Animation/Curves/SetActive")]
    public class AnimatorCurveNode_IsActive : BaseNode, Animator_INode
    {
        [Output(name = "Motiondata", allowMultiple = true)]
        public ANPUA_NodeLink_Anim_Key link_IN;

        //Gameobject Array
        [Input(name = "Gameobject(s)", allowMultiple = true), SerializeField] 
        public GameObject gameObject;

        //[Input(name = "isActive (bool)", allowMultiple = false)]
        public bool isActive;

        public override string name => "SetActive";

        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}