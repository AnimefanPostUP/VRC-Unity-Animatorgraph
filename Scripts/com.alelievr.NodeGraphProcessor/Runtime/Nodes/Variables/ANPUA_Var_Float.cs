
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Variables/Float")]
    public class ANPUA_Var_Float : BaseNode, ANPUA_INode
    {
        public override string name => "Variable";

        public float float_IN;

        [Output(name = "Float", allowMultiple = true)]
        public float link_OUT;



        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}