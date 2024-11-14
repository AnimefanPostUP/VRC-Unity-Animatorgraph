
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Logic/Invert")]
    public class ANPUA_Condition_Invert : BaseNode, ANPUA_INode
    {
        public override string name => "Invert";
        public override Color color =>  new Color(0.2f, 0.3f, 0.5f);

        [Output(name = "Tasks", allowMultiple = true)]
        public ANPUA_NodeLink_Logic link_OUT;

        //ANPUA_NodeLink_Logic
        [Input(name = "in", allowMultiple = false)] 
        public ANPUA_NodeLink_Logic link_Logic_Enter;

        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}