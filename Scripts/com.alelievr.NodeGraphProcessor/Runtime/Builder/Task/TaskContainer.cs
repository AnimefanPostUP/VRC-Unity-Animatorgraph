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
        public AacFlLayer layer;
        public AacFlState entryState;
        public ConditionBundle conditionBundle;
        public AacFlController controller;

        

        //constructor
        public TaskContainer(  AacFlLayer layer, AacFlState entryState, ConditionBundle conditionBundle ,AacFlController controller)
        {
            this.layer = layer;
            this.entryState = entryState;
            this.conditionBundle = conditionBundle;
        }

        //New TaskContainer From an old one but with a new conditionBundle
        public TaskContainer(TaskContainer oldContainer, ConditionBundle newConditionBundle)
        {
            this.layer = oldContainer.layer;
            this.entryState = oldContainer.entryState;
            this.conditionBundle = newConditionBundle;
        }

    }
}
