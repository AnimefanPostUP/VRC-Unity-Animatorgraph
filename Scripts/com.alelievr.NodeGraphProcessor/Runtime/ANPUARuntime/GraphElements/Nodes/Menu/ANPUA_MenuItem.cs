
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable]
    public class ANPUA_MenuItem : BaseNode, ANPUA_INode
    {
        public override string name => "MenuItem";

        //[Input(name = "Title", allowMultiple = false), SerializeField]
        public string menuname;

        //[Input(name = "Icon", allowMultiple = false), SerializeField]
        public Texture2D icon;



        [Input, Vertical]
        public ANPUA_NodeLink_Menu link_IN;

        [Output, Vertical] 
        public IEnumerable<ANPUA_NodeLink_Menu> link_OUT;




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

        [CustomPortBehavior(nameof(link_OUT))]
        IEnumerable<PortData> GetPortsForOutputs(List<SerializableEdge> edges)
        {
            yield return new PortData { displayName = "Menu ", displayType = typeof(ANPUA_NodeLink_Menu), acceptMultipleEdges = true };
        }

        [CustomPortOutput(nameof(link_OUT), typeof(ANPUA_NodeLink_Menu), allowCast = true)]
        public void GetOutputs(List<SerializableEdge> edges)
        {
            link_OUT = edges.Select(e => (ANPUA_NodeLink_Menu)e.passThroughBuffer);
        }
 
        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();


    }
}