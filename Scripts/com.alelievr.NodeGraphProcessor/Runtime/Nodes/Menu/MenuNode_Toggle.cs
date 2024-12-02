
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using GraphProcessor;
using UnityEngine;
using MA_Wrapper = GraphProcessor.MaWrapper;
using nadena.dev.modular_avatar.core;
using AnimatorAsCode.V1;

namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Menu Elements/Toggle")]
    public class MenuNode_Toggle : MenuBaseNode, Animator_INode
    {
        public override string name => "Menu Toggle";


        public string togglename;

        public Texture2D icon;


        [Input(name = "Parameter", allowMultiple = false)]
        public ANPUA_NodeLink_Parameter link_IN_Parameter;


        [Output(name = "Parameter", allowMultiple = true)]
        public ANPUA_NodeLink_Parameter link_OUT_Parameter;

        public float value;

        //Uses the Value of the Default Parameter Currently, needs to be adjusted!
        public override MaItemContainer ProcessMenuOnBuild(MaItemContainer menuContainer, ANPUA_ParameterManager parameterTable)
        {
            ANPUA_BuildCache_Parameter param = parameterTable.FindParameter("Test1");
            AacFlParameter parameterAnyType = param.aacParameter;
            //Add toggle to menu container
            if (param.type == ANPUA_ParameterType.Bool)
            {
                AacFlBoolParameter boolParam = (AacFlBoolParameter)parameterAnyType;
                MA_Wrapper.createToggle(menuContainer.maS, menuContainer.menuObject, togglename, boolParam, param.boolValue, icon);
            }
            else if (param.type == ANPUA_ParameterType.Float)
            {
                AacFlFloatParameter floatParam = (AacFlFloatParameter)parameterAnyType;
                MA_Wrapper.createToggle(menuContainer.maS, menuContainer.menuObject, togglename, floatParam, param.floatValue, icon);

            }
            else if (param.type == ANPUA_ParameterType.Int)
            {
                AacFlIntParameter intParam = (AacFlIntParameter)parameterAnyType;
                MA_Wrapper.createToggle(menuContainer.maS, menuContainer.menuObject, togglename, intParam, param.intValue, icon);
            }


            menuContainer = menuContainer;
            return menuContainer;
        }

        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return null;
        }
        public IEnumerable<BaseNode> GetConnectedNodes(BaseNode node)
        {
            return node.GetOutputNodes();
        }
        public override FieldInfo[] GetNodeFields() => base.GetNodeFields();
    }
}