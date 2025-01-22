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

    public class TaskContainer
    {
        public ModularAvatarMenuItem menuItem;
        public GameObject menuObject;
        public MaAc maS;

        //constructor
        public TaskContainer(MaAc maS, GameObject menuObject, ModularAvatarMenuItem menuItem)
        {
            this.menuItem = menuItem;
            this.menuObject = menuObject;
            this.maS = maS;
        }

    }
}
