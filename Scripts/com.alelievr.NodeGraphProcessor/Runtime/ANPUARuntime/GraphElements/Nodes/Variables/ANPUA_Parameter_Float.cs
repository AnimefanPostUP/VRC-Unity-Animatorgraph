
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    public enum ANPUA_ParameterBoolMode
    {
        Set,
        Get
    }

    [System.Serializable, NodeMenuItem("Parameters/Float")]
    public class ANPUA_Parameter_Float : ANPUA_Parameter_Node
    {


        //public ANPUA_ParameterBoolMode paramMode;

        //Make a Input only Visible if a Bool is true
        //[VisibleIf(nameof(paramMode), ANPUA_ParameterBoolMode.Set)]
        //[Input(name = "Set", allowMultiple = true)]
        //public ANPUA_NodeLink_Parameter link_IN;


        //[VisibleIf(nameof(paramMode), ANPUA_ParameterBoolMode.Get)]
        [Output(name = "Get", allowMultiple = true)]
        public ANPUA_NodeLink_Parameter link_OUT;
    }
}