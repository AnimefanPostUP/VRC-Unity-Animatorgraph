
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Logic/Int Enumerate")]
    public class ANPUA_Condition_IntEnumerate : BaseNode, ANPUA_INode
    {
        public override string name => "Enumerate";
        public override Color color =>  new Color(0.2f, 0.3f, 0.5f);

        //Booleans
        [Input(name = "integer", allowMultiple = true)]
        public int int_IN;

        [Output(name = "out", allowMultiple = true)]
        public ANPUA_NodeLink_Logic link_OUT; 

        //Default int
        public int int_default = 0;

        //Offset
        public int int_offset = 0;

        //reverse
        public bool reverse = false;




        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}