
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Menu Elements/Menu")]
    public class ANPUA_Menu : BaseNode, ANPUA_INode
    {
        public override string name => "Menu";

        //[Input(name = "Title", allowMultiple = false), SerializeField]
        public string menuname;

        //[Input(name = "Icon", allowMultiple = false), SerializeField]
        public Texture2D icon;



        [Input, Vertical]
        public ANPUA_NodeLink_Menu link_IN;

        [Output, Vertical] 
        public IEnumerable<ANPUA_NodeLink_Menu> outputs;




        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }


        protected override void Process()
        {
            base.Process();
            // link_OUT =  new ANPUA_NodeLink_Menu();

            // if (link_OUT == null)
            //     return;

            // foreach (float input in inputs)
            //     output += input;
        }

        [CustomPortBehavior(nameof(outputs))]
        IEnumerable<PortData> GetPortsForOutputs(List<SerializableEdge> edges)
        {
            yield return new PortData { displayName = "Menu ", displayType = typeof(ANPUA_NodeLink_Menu), acceptMultipleEdges = true };
        }

        [CustomPortOutput(nameof(outputs), typeof(ANPUA_NodeLink_Menu), allowCast = true)]
        public void GetOutputs(List<SerializableEdge> edges)
        {
            outputs = edges.Select(e => (ANPUA_NodeLink_Menu)e.passThroughBuffer);
        }
 
        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();


    }
}