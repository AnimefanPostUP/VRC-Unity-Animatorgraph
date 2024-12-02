
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;

namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Logic/AND")]
    public class ANPUA_Condition_AND : BaseNode, Animator_INode
    {
        public override string name => "AND";
        public override Color color =>  new Color(0.2f, 0.3f, 0.5f);

        [Input(name = "Logic", allowMultiple = false)]
        public ANPUA_NodeLink_Logic link_IN;

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