
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using GraphProcessor;
using UnityEngine;
using MA_Wrapper = GraphProcessor.MaWrapper;

namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Menu Elements/Radial")]
    public class MenuNode_Radial : MenuBaseNode, Animator_INode
    {
        public override string name => "Menu Radial";


        public override void ProcessOnBuild()
        {
            //to be coded

        }

        //Float value
        [Output(name = "Float", allowMultiple = false), SerializeField]
        public float float_IN;


        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().Where(n => n is ConditionalNode).Select(n => n as ConditionalNode);
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}