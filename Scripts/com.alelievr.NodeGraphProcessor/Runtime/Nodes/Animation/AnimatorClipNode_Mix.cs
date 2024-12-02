
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;


namespace GraphProcessor
{
	[System.Serializable, NodeMenuItem("Animation/Clip/Mix Clips")]
	public class AnimatorClipNode_Mix : BaseNode, Animator_INode
	{

		[Input(name = "Clip A", allowMultiple = false)]
		public ANPUA_NodeLink_Anim_Clip curvedata_A_IN;

        [Input(name = "Clip B", allowMultiple = false)]
		public ANPUA_NodeLink_Anim_Clip curvedata_B_IN;

		//Out CurveData
		[Output(name = "Clip")]
		public ANPUA_NodeLink_Anim_Clip link_CurveData_OUT;

        //Enum for Addative, Overwrite, Paralell

        public CombineType combineType;
        public enum CombineType
        {
            Addative,
            Overwrite,
            Paralell
        }

		public override string name => "Mix Clips";

		public IEnumerable<ConditionalNode> GetExecutedNodes()
		{
			// Return all the nodes connected to the executes port
			return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
		}

		public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
	}
}