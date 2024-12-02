
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using GraphProcessor;
using UnityEngine;
using MA_Wrapper = GraphProcessor.MaWrapper;
using AnimatorAsCode.V1.ModularAvatar;

namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Menu Elements/Radial")]
    public class MenuNode_Radial : MenuBaseNode, Animator_INode
    {
        public override string name => "Menu Radial";


        public override MaItemContainer ProcessMenuOnBuild(MaItemContainer menuContainer, ANPUA_ParameterManager parameterManager)
        {
            menuContainer = menuContainer;
            return menuContainer;
        }

        //Float value
        [Output(name = "Float", allowMultiple = false), SerializeField]
        public float float_IN;


        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return null;
        }

        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}