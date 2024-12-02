
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;
using UnityEngine;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Parameters/Bool")]
    public class ANPUA_Parameter_Bool : ANPUA_Parameter_Node
    {
       

        //[VisibleIf(nameof(paramMode), ANPUA_ParameterBoolMode.Get)]
        [Output(name = "Get", allowMultiple = true)]
        public ANPUA_NodeLink_Parameter link_OUT;


       
    }
}