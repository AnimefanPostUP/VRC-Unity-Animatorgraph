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
using MA_Wrapper = GraphProcessor.MaWrapper;

namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Menu Elements/Installer")]
    public class MenuNode_Installer : MenuBaseNode
    {
        public override string name => "Menu Installer";

        //[Input(name = "Title", allowMultiple = false), SerializeField]
        public string menuname;

        public Texture2D icon;

        //[Input(name = "Icon", allowMultiple = false), SerializeField]
        public VRCExpressionsMenu targetmenu;

        //public List<ConditionContainer> objs = new List<ConditionContainer>();

        public override MaItemContainer ProcessMenuOnBuild(MaItemContainer menuContainer, ANPUA_ParameterManager parameterManager)
        {
            Debug.Log("AnimatorGraph: ...Creating Installer");
            var newObject = MA_Wrapper.createSubMenu(menuContainer.maS, menuContainer.menuObject, menuname, icon);
            ModularAvatarMenuInstaller menuinstaller = MA_Wrapper.createMenuInstaller(newObject); //needs targetMenuImplementation!!!


            menuContainer = new MaItemContainer(menuContainer.maS, newObject, null);
            return menuContainer;

        }


        public IEnumerable<BaseNode> GetConnectedNodes(BaseNode node)
        {
            return node.GetOutputNodes();
        }

    }
}