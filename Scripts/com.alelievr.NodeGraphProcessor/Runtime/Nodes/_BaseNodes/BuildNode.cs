using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;
using AnimatorAsCode.V1;
using AnimatorAsCode.V1.ModularAvatar;
using AnimatorAsCode.V1.VRC;
using nadena.dev.modular_avatar.core;

namespace GraphProcessor
{

	[System.Serializable, NodeMenuItem("Basic/Start Build Node")]
	public class BuildNode : BaseNode, Animator_INode
	{

		public override Color color => new Color(0.7f, 0.7f, 0.7f);


		[Output(name = "Layer", allowMultiple = true)]
		public ANPUA_NodeLink_Layer link_OUT;

		[Output(name = "Menu Installer", allowMultiple = true)]
		public ANPUA_NodeLink_Menu link_OUT_Menu;

		//For Menu Building
		[HideInInspector]
		public MaItemContainer menuContainer;

		public override string name => "Start Build Node";

		public IEnumerable<ConditionalNode> GetExecutedNodes()
		{
			// Return all the nodes connected to the executes port
			return null;
		}
		public IEnumerable<BaseNode> GetConnectedNodes(BaseNode node)
		{
			return node.GetOutputNodes();
		}

		public MaItemContainer InitializeMenuContainer(MaAc menuItem, GameObject menuObject)
		{
			menuContainer = new MaItemContainer(menuItem, menuObject, null);
			return menuContainer;

		}
		public List<MenuBaseNode> GetOutputMenuNodes()
		{
			return GetPort(nameof(link_OUT_Menu), null).GetEdges().Select(e => e.inputNode as MenuBaseNode).ToList();
		}
		public List<Animator_TaskNode> GetOutputTaskNodes()
		{
			return GetPort(nameof(link_OUT), null).GetEdges().Select(e => e.inputNode as Animator_TaskNode).ToList();
		}
		public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
	}
}