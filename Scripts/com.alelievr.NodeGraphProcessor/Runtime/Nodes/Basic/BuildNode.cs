using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


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

		

		public override string name => "Start Build Node";

		public IEnumerable<ConditionalNode> GetExecutedNodes()
		{
			// Return all the nodes connected to the executes port
			return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
		}
        public IEnumerable<BaseNode> GetConnectedNodes(BaseNode node)
        {
            return node.GetOutputNodes();
        }
		public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
	}
}