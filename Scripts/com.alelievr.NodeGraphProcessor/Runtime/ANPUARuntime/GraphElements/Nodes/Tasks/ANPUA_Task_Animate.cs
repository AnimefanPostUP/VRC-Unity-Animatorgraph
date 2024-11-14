
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using Unity.VisualScripting;
using UnityEngine;


namespace GraphProcessor
{
	[System.Serializable, NodeMenuItem("Basic/Animate")]
	public class ANPUA_Task_Animate : BaseNode, ANPUA_INode
	{

		[Input(name = "Task", allowMultiple = true)]
		public ANPUA_NodeLink_Task inputs;


		[Input(name = "Curve(s)", allowMultiple = true)]
		public ANPUA_NodeLink_Curve animations_IN;

		[Output(name = "Curve(s)")]
		public ANPUA_NodeLink_Curve curve_OUT; // TODO: custom function for this one



		//Out CurveData
		[Output(name = "Task- 1")]
		public ANPUA_NodeLink_Task link_OUT;


		public override string name => "Animate";


		// We keep the max port count so it doesn't cause binding issues
		[SerializeField, HideInInspector]
		int portCount = 1;


		[Output]
		public List<ANPUA_NodeLink_Task> outputs; // TODO: custom function for this one
		List<object> values = new List<object>();


		protected override void Process()
		{
			// do things with values
		}


		//Out CurveData




		[CustomPortBehavior(nameof(outputs))]
		IEnumerable<PortData> ListPortBehavior(List<SerializableEdge> edges)
		{
			portCount = Mathf.Max(portCount, edges.Count + 1);

			for (int i = 0; i < portCount; i++)
			{
				yield return new PortData
				{
					displayName = "Task- " + (i+2),
					displayType = typeof(ANPUA_NodeLink_Task),
					identifier = i.ToString(), // Must be unique
				};
			}
		}

		// This function will be called once per port created from the `inputs` custom port function
		// will in parameter the list of the edges connected to this port
		[CustomPortInput(nameof(outputs), typeof(ANPUA_NodeLink_Task))]
		void PullInputs(List<SerializableEdge> inputEdges)
		{
			values.AddRange(inputEdges.Select(e => e.passThroughBuffer).ToList());
		}

		[CustomPortOutput(nameof(outputs), typeof(ANPUA_NodeLink_Task))]
		void PushOutputs(List<SerializableEdge> connectedEdges)
		{
			// Values length is supposed to match connected edges length
			for (int i = 0; i < connectedEdges.Count; i++)
				connectedEdges[i].passThroughBuffer = values[Mathf.Min(i, values.Count - 1)];

			// once the outputs are pushed, we don't need the inputs data anymore
			values.Clear();
		}




		public IEnumerable<ConditionalNode> GetExecutedNodes()
		{
			// Return all the nodes connected to the executes port
			return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
		}

		public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
	}
}