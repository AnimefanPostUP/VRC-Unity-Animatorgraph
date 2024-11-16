using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;

namespace GraphProcessor
{
    [System.Serializable]
    public class ANPUA_Parameter_Node : BaseNode, ANPUA_INode
    {
        public override string name => parameter != null ? "P: " + parameter.name : "Parameter Int";
        public override Color color => new Color(0.8f, 0.4f, 0.4f);


        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        [HideInInspector]
        public ANPUA_GenericParameter parameter;


        const string p_NoParameter = "Parameter doesnt Exist";

        [Setting("Message Type")]
        public NodeMessageType messageType = NodeMessageType.Error;

        protected override void Process()
        {
            if (parameter == null)
                AddMessage(p_NoParameter, messageType);
            else
                RemoveMessage(p_NoParameter);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();



    }
}