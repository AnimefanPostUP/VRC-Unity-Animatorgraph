
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Variables/Integer")]
    public class ANPUA_Var_Int : BaseNode, Animator_INode
    {
        public override string name => "Variable";

        public bool int_IN;

        [Output(name = "Int", allowMultiple = true)]
        public ANPUA_NodeLink link_OUT;



        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}