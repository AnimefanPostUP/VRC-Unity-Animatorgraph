
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Menu Elements/Radial")]
    public class ANPUA_Menu_Radial : BaseNode, ANPUA_INode
    {
        public override string name => "Menu Radial";


        [Input(name = "Menu", allowMultiple = false)]
        public ANPUA_NodeLink_Menu link_IN;

        //Float value
        [Output(name = "Float", allowMultiple = false), SerializeField]
        public float float_IN;


        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}