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
    public class AAcWrapper : MonoBehaviour
    {

        public static AacFlLayer createLayer(string name, AacFlController controller, AacFlLayer parentLayer = null)
        {
            AacFlLayer layer = controller.NewLayer(name);
            return layer;
        }

        
        public static AacFlState createState(string name, AacFlLayer layer)
        {
            AacFlState state = layer.NewState(name);
            return state;
        }

        //Create transition with Condition
        public static AacFlTransition createTransition(AacFlState source, AacFlState dest, ConditionBundle condition)
        {
            AacFlTransition transition = source.TransitionsTo(dest);

            return transition;
        }
       


    }
}
#endif
