
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace GraphProcessor
{
	[System.Serializable, NodeMenuItem("Basic/State")]
	public class ANPUA_NodeState : ANPUA_BaseNode_State, ANPUA_INode
	{

		public override string name => "Node";
		public override bool isRenamable => true;

		public override Color color => new Color(0.7f, 0.5f, 0.7f);

		[Input(name = "Task", allowMultiple = true)]
		public ANPUA_NodeLink_Task inputs;


		[Input(name = "Motion", allowMultiple = true)]
		public ANPUA_NodeLink_Anim_Clip animations_IN;

		//State Name, Speed, Motion Time, Multiplier that can be enabled by Settings

		//Settings
		[Setting(name = "Speed Multiplier"), SerializeField]
		public bool enableMultiplier = false;

		[Setting(name = "Motion"), SerializeField]
		public bool enableMotionTime = false;

		[Setting(name = "Speed"), SerializeField]
		public bool enableSpeed = false;

		//Speed
		[Input(name = "Speed", allowMultiple = false), SerializeField]
		public float speed;

		//Motion Time
		[Input(name = "Motion Time", allowMultiple = false)]
		public ANPUA_NodeLink_Parameter motionTime;

		//Multiplier
		[Input(name = "Multiplier", allowMultiple = false)]
		public ANPUA_NodeLink_Parameter multiplier;



		// [CustomPortBehavior(nameof(speed))] 
		// IEnumerable<PortData> GetInputPort(List<SerializableEdge> edges)
		// {
		// 	// Hide the port if the setting is disabled
		// 	if (enableSpeed)
		// 	{ 
		// 		//Add port if doesnt exist
		// 		yield return new PortData { displayName = "Speed", displayType = typeof(float), acceptMultipleEdges = false };
		// 	} 		
		// }



		// Method to update the ports
		//Maybe Displaying Based on a Setting
		//[Output(name = "Motion")] 
		//public ANPUA_NodeLink_Curve curve_OUT; // TODO: custom function for this one

		//Out CurveData
		// [Output(name = "Task- 1")]
		// public ANPUA_NodeLink_Task link_OUT;



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


		[CustomPortBehavior(nameof(speed))]
		IEnumerable<PortData> GetPortsForInputs(List<SerializableEdge> edges)
		{
			// Hide the port if the setting is disabled
			if (enableSpeed)
			{
				yield return new PortData { displayName = "Speed", displayType = typeof(float), acceptMultipleEdges = false };
			}
		}


		[CustomPortBehavior(nameof(outputs))]
		IEnumerable<PortData> ListPortBehavior(List<SerializableEdge> edges)
		{
			portCount = Mathf.Max(portCount, edges.Count + 1);

			for (int i = 0; i < portCount; i++)
			{
				yield return new PortData
				{
					displayName = "Task " + (i),
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