using System;
using System.Collections.Generic;
using System.Linq;
using AnimatorAsCode.V1;
using AnimatorAsCode.V1.ModularAvatar;
using AnimatorAsCode.V1.VRC;
using GraphProcessor;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Avatars.Components;
using VRC.SDK3.Avatars.ScriptableObjects;
using VRC.SDKBase;
using MA_Wrapper = GraphProcessor.ANPUA_ModularAvatar_Wrapper;

namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Menu Elements/Installer")]
    public class ANPUA_Menu_Installer : ANPUA_BaseNode_Menu, ANPUA_INode
    {
        public override string name => "Menu Installer";

        //[Input(name = "Title", allowMultiple = false), SerializeField]
        public string menuname;

        public Texture2D icon;

        //[Input(name = "Icon", allowMultiple = false), SerializeField]
        public VRCExpressionsMenu targetmenu;

        public void InitializedInstallerNode(MaAc maS, VRCExpressionsMenu targetmenu, GameObject targetobject)
        {
            ModularAvatarMenuInstaller menuinstaller = MA_Wrapper.createMenuInstaller(targetobject, targetmenu);
            menuContainer = new ANPUA_Container_Menu(maS, targetobject, null);
        }

        public override void ProcessMenuNode()
        {
            ANPUA_BaseNode_Menu inputnode = GetPort(nameof(link_Menu_IN), null).GetEdges().First().inputNode as ANPUA_BaseNode_Menu;
            ANPUA_Container_Menu inputMenuContainer = inputnode.menuContainer;

            GameObject newMenuObject;
            ModularAvatarMenuItem newMenuItem;

            (newMenuObject, newMenuItem) = MA_Wrapper.createSubMenu(inputMenuContainer.maS, inputMenuContainer.menuObject, menuname, icon);
            menuContainer = new ANPUA_Container_Menu(inputMenuContainer.maS, newMenuObject, newMenuItem);
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
  
    }
}