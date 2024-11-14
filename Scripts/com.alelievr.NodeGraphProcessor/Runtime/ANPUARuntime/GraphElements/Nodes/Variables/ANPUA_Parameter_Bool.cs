
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Parameters/Bool")]
    public class ANPUA_Parameter_Bool : BaseNode, ANPUA_INode
    {
        public override string name => "Bool Parameter";

        [Output(name = "Bool Parameter", allowMultiple = true)]
        public ANPUA_NodeLink_Parameter link_OUT;

        //String name
        public string parameterName;

        //Default bool
        public bool defaultValue;

        //isSynched
        public bool isSynched;

        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}