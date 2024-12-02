
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;
using MA_Wrapper = GraphProcessor.MaWrapper;
using nadena.dev.modular_avatar.core;

namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Menu Elements/Menu")]
    public class MenuNode : MenuBaseNode, Animator_INode
    {
        public override string name => "Menu";

        //[Input(name = "Title", allowMultiple = false), SerializeField]
        public string menuname;

        //[Input(name = "Icon", allowMultiple = false), SerializeField]
        public Texture2D icon;

        //ProcessMenuNode
        public override MaItemContainer ProcessMenuOnBuild(MaItemContainer menuContainer, ANPUA_ParameterManager parameterManager)
        {
            Debug.Log("AnimatorGraph: ...Creating SubMenu");
            GameObject newMenuObject;
            ModularAvatarMenuItem newMenuItem;

           newMenuObject= MA_Wrapper.createSubMenu(menuContainer.maS, menuContainer.menuObject, menuname, icon);
            menuContainer=new MaItemContainer( menuContainer.maS, newMenuObject, null);
            return menuContainer;
        }

        public override void ProcessOnBuild()
        {
           
        }
        

        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return null;
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