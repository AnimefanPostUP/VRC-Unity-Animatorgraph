
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;
using GraphProcessor.Builder;



using AnimatorAsCode.V1;
using AnimatorAsCode.V1.ModularAvatar;
using AnimatorAsCode.V1.VRC;

using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SearchService;

using UnityEngine.UI;
using VRC.SDK3.Avatars.Components;
using VRC.SDK3.Avatars.ScriptableObjects;
using VRC.SDKBase;
using AAC_Wrapper = GraphProcessor.AAcWrapper;

namespace GraphProcessor
{

    //Enum BranchNode

    public enum BranchMode
    {
        None,
        Return,
        AnyReturn,
    }

    [System.Serializable, NodeMenuItem("Logic/Branch")]
    public class ANPUA_Condition : Animator_TaskNode, Animator_INode
    {
        public override string name => "Branch";
        public override bool isRenamable => true;
        public override Color color => new Color(0.2f, 0.3f, 0.5f);

        [Input(name = "Task", allowMultiple = true)]
        public ANPUA_NodeLink_Task link_IN;

        //Bool Input
        [Input(name = "Condition")]
        public ANPUA_NodeLink_Logic conditions_IN;

        [Output(name = "true", allowMultiple = true)]
        public ANPUA_NodeLink_Task true_OUT;

        [Output(name = "false", allowMultiple = true)]
        public ANPUA_NodeLink_Task false_OUT;

        [SerializeField]
        public Builder.AnimatorCondition[] conditionContainer = new Builder.AnimatorCondition[0];


        //[Input(name = "Return on False"), SerializeField]
        [SerializeField]
        public BranchMode onFalse = BranchMode.None;

        // [Setting(name = "Unidirectional")]
        // [SerializeField]
        // public bool unidirectional = false;


        //         public class IfNode : ConditionalNode
        // {
        // 	[Input(name = "Condition")]
        //     public bool				condition;

        // 	[Output(name = "True")]
        // 	public ConditionalLink	@true;
        // 	[Output(name = "False")]
        // 	public ConditionalLink	@false;

        // 	[Setting("Compare Function")]
        // 	public CompareFunction		compareOperator;

        // 	public override string		name => "If";

        // 	public override IEnumerable< ConditionalNode >	GetExecutedNodes()
        // 	{
        // 		string fieldName = condition ? nameof(@true) : nameof(@false);

        // 		// Return all the nodes connected to either the true or false node
        // 		return outputPorts.FirstOrDefault(n => n.fieldName == fieldName)
        // 			.GetEdges().Select(e => e.inputNode as ConditionalNode);
        // 	}

        public TaskContainer ProcessTaskOnBuild(TaskContainer taskContainer, ANPUA_ParameterManager parameterManager) 
        {
           this.taskContainer = taskContainer;
           this.taskContainer.entryState = AAC_Wrapper.createState(this.name, taskContainer.layer);

            return null;
        }


        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}