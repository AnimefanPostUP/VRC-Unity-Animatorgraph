
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable]
    public class MenuBaseNode : AnimatorNode, Animator_INode
    {
        [Input, Vertical]
        public ANPUA_NodeLink_Menu link_Menu_IN;

        [Output, Vertical]
        public IEnumerable<ANPUA_NodeLink_Menu> link_Menu_OUT;

        [HideInInspector]
        public MaItemContainer menuContainer;

        protected override void Process()
        {
            base.Process();
        }

        [CustomPortBehavior(nameof(link_Menu_OUT))]
        IEnumerable<PortData> GetPortsForOutputs(List<SerializableEdge> edges)
        {
            yield return new PortData { displayName = "Menu ", displayType = typeof(ANPUA_NodeLink_Menu), acceptMultipleEdges = true };
        }

        [CustomPortOutput(nameof(link_Menu_OUT), typeof(ANPUA_NodeLink_Menu), allowCast = true)]
        public void GetOutputs(List<SerializableEdge> edges)
        {
            link_Menu_OUT = edges.Select(e => (ANPUA_NodeLink_Menu)e.passThroughBuffer);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();


    }
}