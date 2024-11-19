using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{

	[System.Serializable, NodeMenuItem("Basic/Layer")]
	public class ANPUA_NodeLayer : BaseNode, ANPUA_INode
	{

		public override Color color => new Color(0.7f, 0.7f, 0.7f);

		public override string name => "Layer";
		

		[Input(name = "Layer", allowMultiple = true)]
		public ANPUA_NodeLink_Layer link_IN;


		[Output(name = "Entry", allowMultiple = false)]
		public ANPUA_NodeLink_Task link_OUT_Entry;

		[Output(name = "Anystate", allowMultiple = true)]
		public ANPUA_NodeLink_Task link_OUT_AnyState;


		//String for layer name
		//[Input(name = "Layer Name", allowMultiple = false), SerializeField]
		public string layerName;

		//Weight
		//[Input(name = "Weight", allowMultiple = false), SerializeField]
		public float weight;


		//Needs to be Added:

		//Mask
		//[Input(name = "Mask", allowMultiple = false), SerializeField]
		//public int mask;

		//Blending
		//[Input(name = "Blending", allowMultiple = false), SerializeField]
		//public int blending;


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