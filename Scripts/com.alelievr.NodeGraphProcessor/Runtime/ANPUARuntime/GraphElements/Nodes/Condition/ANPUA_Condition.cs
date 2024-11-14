
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;



namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Logic/If")]
    public class ANPUA_Condition : BaseNode, ANPUA_INode
    {
        public override string name => "If Statement";
        public override Color color =>  new Color(0.2f, 0.3f, 0.5f);

        [Input(name = "Task", allowMultiple = true)]
        public ANPUA_NodeLink_Task link_IN;

        //Bool Input
        [Input(name = "Condition")]
        public ANPUA_NodeLink_Logic conditions_IN;

        [Output(name = "true", allowMultiple = true)]
        public ANPUA_NodeLink_Task true_OUT;

        [Output(name = "false", allowMultiple = true)]
        public ANPUA_NodeLink_Task false_OUT;

        [Setting(name = "Unidirectional")]
        public bool unidirectional = false;


        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}