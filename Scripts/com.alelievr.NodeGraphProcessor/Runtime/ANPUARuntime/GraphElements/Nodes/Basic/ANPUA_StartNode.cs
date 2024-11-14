using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{

	[System.Serializable, NodeMenuItem("Basic/Descriptor")]
	public class ANPUA_StartNode : BaseNode, ANPUA_INode
	{

		public override Color color => new Color(0.7f, 0.7f, 0.7f);


		[Output(name = "Tasks", allowMultiple = true)]
		public ANPUA_NodeLink_Task link_OUT;

		[Output(name = "Menus", allowMultiple = true)]
		public ANPUA_NodeLink_Menu link_OUT_Menu;

		//Input NodeLinkParameter
		[Input(name = "Parameters", allowMultiple = true)]
		public ANPUA_NodeLink_Parameter link_Parameter;

		public override string name => "Descriptor";

		public IEnumerable<ConditionalNode> GetExecutedNodes()
		{
			// Return all the nodes connected to the executes port
			return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
		}

		public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
	}
}