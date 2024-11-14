using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
	[System.Serializable, NodeMenuItem("Basic/Jump Point")]
	public class ANPUA_NodeJump : BaseNode, ANPUA_INode
	{
		[Output(name = "Tasks", allowMultiple = true)]
		public ANPUA_NodeLink_Task link_OUT;

        //Name of the node string input with field
        //[Input(name = "Checkpoint Name")]
        public string nameID;
		public override Color color => new Color(0.9f, 0.9f, 0.5f);

		public override string name => "Jump Point";

		public IEnumerable<ConditionalNode> GetExecutedNodes()
		{
			// Return all the nodes connected to the executes port
			return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
		}

		public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
	}
}