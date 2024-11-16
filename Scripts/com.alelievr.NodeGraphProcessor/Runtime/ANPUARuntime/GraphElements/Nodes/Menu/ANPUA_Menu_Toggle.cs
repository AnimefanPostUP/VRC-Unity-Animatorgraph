
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Menu Elements/Toggle")]
    public class ANPUA_Menu_Toggle : BaseNode, ANPUA_INode
    {
        public override string name => "Menu Toggle";

        [Vertical]
        [Input(name = "Menu", allowMultiple = false)]
        public ANPUA_NodeLink_Menu link_IN;


        public string togglename;

        public Texture2D icon;


        [Input(name = "Parameter", allowMultiple = false)]
        public ANPUA_NodeLink_Parameter link_IN_Parameter;

        //menu out
        [Vertical]
        [Output(name = "Menu", allowMultiple = true)]
        public ANPUA_NodeLink_Menu link_OUT;

        //parameter out

        [Output(name = "Parameter", allowMultiple = true)]
        public ANPUA_NodeLink_Parameter link_OUT_Parameter;

        //Value

        public float value;




        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }
        public IEnumerable<BaseNode> GetConnectedNodes(BaseNode node)
        {
            return node.GetOutputNodes();
        }
        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}