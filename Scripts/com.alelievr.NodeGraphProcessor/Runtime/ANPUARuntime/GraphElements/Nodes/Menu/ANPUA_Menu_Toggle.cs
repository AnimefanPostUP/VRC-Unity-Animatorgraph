
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using GraphProcessor;
using UnityEngine;
using MA_Wrapper = GraphProcessor.ANPUA_ModularAvatar_Wrapper;
using nadena.dev.modular_avatar.core;

namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Menu Elements/Toggle")]
    public class ANPUA_Menu_Toggle : ANPUA_BaseNode_Menu, ANPUA_INode
    {
        public override string name => "Menu Toggle";


        public string togglename;

        public Texture2D icon;


        [Input(name = "Parameter", allowMultiple = false)]
        public ANPUA_NodeLink_Parameter link_IN_Parameter;


        [Output(name = "Parameter", allowMultiple = true)]
        public ANPUA_NodeLink_Parameter link_OUT_Parameter;

        public float value;


        public override void ProcessOnBuild()
        {
            ANPUA_BaseNode_Menu inputnode = GetPort(nameof(link_Menu_IN), null).GetEdges().First().inputNode as ANPUA_BaseNode_Menu;
            menuContainer = inputnode.menuContainer;
        }

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