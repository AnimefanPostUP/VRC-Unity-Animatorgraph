
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;
using MA_Wrapper = GraphProcessor.ANPUA_ModularAvatar_Wrapper;
using nadena.dev.modular_avatar.core;

namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Menu Elements/Menu")]
    public class ANPUA_Menu : ANPUA_BaseNode_Menu, ANPUA_INode
    {
        public override string name => "Menu";

        //[Input(name = "Title", allowMultiple = false), SerializeField]
        public string menuname;

        //[Input(name = "Icon", allowMultiple = false), SerializeField]
        public Texture2D icon;

        //ProcessMenuNode

        public override void ProcessOnBuild()
        {
            ANPUA_BaseNode_Menu inputnode=GetPort(nameof(link_Menu_IN), null).GetEdges().First().inputNode as ANPUA_BaseNode_Menu;
            ANPUA_Container_Menu inputMenuContainer = inputnode.menuContainer;

            GameObject newMenuObject;
            ModularAvatarMenuItem newMenuItem;

            (newMenuObject, newMenuItem)= MA_Wrapper.createSubMenu (inputMenuContainer.maS, inputMenuContainer.menuObject, menuname, icon);
            menuContainer=new ANPUA_Container_Menu( inputMenuContainer.maS, newMenuObject, newMenuItem);
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
        protected override void Process()
        {
            base.Process();
        }


        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();


    }
}