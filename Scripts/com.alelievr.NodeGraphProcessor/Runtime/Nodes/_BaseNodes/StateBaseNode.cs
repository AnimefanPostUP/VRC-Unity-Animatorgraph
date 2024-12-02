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
using MA_Wrapper = GraphProcessor.MaWrapper;
namespace GraphProcessor
{
	[System.Serializable]
	public class StateBaseNode : AnimatorNode, Animator_INode
	{

		AacFlState state;
	}
}
#endif
