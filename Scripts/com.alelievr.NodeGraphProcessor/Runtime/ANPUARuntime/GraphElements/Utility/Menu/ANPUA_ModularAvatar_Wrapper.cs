#if UNITY_EDITOR
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
namespace GraphProcessor
{
    public class ANPUA_ModularAvatar_Wrapper : MonoBehaviour
    {

        public static ModularAvatarMenuInstaller createMenuInstaller(GameObject target, VRCExpressionsMenu menu )
        {
            // Check if the target already has a ModularAvatarMenuInstaller component
            var installer = target.GetComponent<ModularAvatarMenuInstaller>();
            if (installer == null)
            {
                installer = target.AddComponent<ModularAvatarMenuInstaller>();
            }

            //public VRCExpressionsMenu menuToAppend;
            //public VRCExpressionsMenu installTargetMenu;

            installer.installTargetMenu = menu;

            return installer;
        }

        public static (GameObject menuObject, ModularAvatarMenuItem menu) createSubMenu(MaAc maS, GameObject target, string name, Texture2D ico = null)
        {
            GameObject menuObject = new GameObject(name);
            menuObject.transform.parent = target.transform;
            if (ico != null)
            {
                maS.EditMenuItem(menuObject).Name(name).WithIcon(ico);
            }
            else
            {
                maS.EditMenuItem(menuObject).Name(name);
            }

            ModularAvatarMenuItem menu = menuObject.GetComponent<ModularAvatarMenuItem>();
            if (menu != null)
            {
                menu.MenuSource = SubmenuSource.Children;
                menu.Control.type = VRCExpressionsMenu.Control.ControlType.SubMenu;
            }
            return (menuObject, menu);
        }


        public static (GameObject menuObject, ModularAvatarMenuItem menu) createToggle(MaAc maS, GameObject target, string name, AacFlIntParameter parameter, int index, Texture2D ico = null)
        {
            //Create a new Gameobject under the target, with the given name with the type toggle using the given parameter and index
            GameObject subItem = new GameObject(name);
            subItem.transform.parent = target.transform;

            if (ico == null)
            {
                maS.EditMenuItem(subItem).Name(name).ToggleSets(parameter, index);
            }
            else
            {
                maS.EditMenuItem(subItem).Name(name).ToggleSets(parameter, index).WithIcon(ico);
            }

            return (subItem, subItem.GetComponent<ModularAvatarMenuItem>());
        }


    }
}
#endif
