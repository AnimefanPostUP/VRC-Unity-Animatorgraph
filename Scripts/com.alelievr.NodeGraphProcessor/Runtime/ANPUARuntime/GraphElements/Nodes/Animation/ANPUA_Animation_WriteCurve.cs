
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;


namespace GraphProcessor
{
	[System.Serializable, NodeMenuItem("Animation/Clip/Overlap Curves")]
	public class ANPUA_Animation_WriteCurve : BaseNode, ANPUA_INode
	{

		[Input(name = "Curve A", allowMultiple = false)]
		public ANPUA_NodeLink_Curve curvedata_A_IN;

        [Input(name = "Curve B", allowMultiple = false)]
		public ANPUA_NodeLink_Curve curvedata_B_IN;

		//Out CurveData
		[Output(name = "Curve")]
		public ANPUA_NodeLink_Curve link_CurveData_OUT;

        //Enum for Addative, Overwrite, Paralell

        public CombineType combineType;
        public enum CombineType
        {
            Addative,
            Overwrite,
            Paralell
        }

		public override string name => "Overlap Curves";

		public IEnumerable<ConditionalNode> GetExecutedNodes()
		{
			// Return all the nodes connected to the executes port
			return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
		}

		public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
	}
}