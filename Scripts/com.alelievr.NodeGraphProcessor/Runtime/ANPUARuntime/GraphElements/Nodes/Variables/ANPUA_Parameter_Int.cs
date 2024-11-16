
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Parameters/Int")]
    public class ANPUA_Parameter_Int : ANPUA_Parameter_Node
    {


        //[VisibleIf(nameof(paramMode), ANPUA_ParameterBoolMode.Set)]
        //[Input(name = "Set", allowMultiple = true)]
        //public ANPUA_NodeLink_Parameter link_IN;


        //[VisibleIf(nameof(paramMode), ANPUA_ParameterBoolMode.Get)]
        [Output(name = "Get", allowMultiple = true)]
        public ANPUA_NodeLink_Parameter link_OUT;


    }
}