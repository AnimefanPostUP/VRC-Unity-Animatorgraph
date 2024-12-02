using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine; // Add this directive


namespace GraphProcessor
{
	[System.Serializable, NodeMenuItem("Basic/Jump")]
	public class ANPUA_NodeJumpPoint : BaseNode, ANPUA_INode
	{
		[Input(name = "Tasks", allowMultiple = true)]
		public ANPUA_NodeLink_Task link_OUT;

		public override string name => "Jump";

        //[Input(name = "Checkpoint Name")]
        public string nameID;

        public override Color color => new Color(0.9f, 0.9f, 0.5f);



		public IEnumerable<ConditionalNode> GetExecutedNodes()
		{
			// Return all the nodes connected to the executes port
			return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
		}

		public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
	}
}