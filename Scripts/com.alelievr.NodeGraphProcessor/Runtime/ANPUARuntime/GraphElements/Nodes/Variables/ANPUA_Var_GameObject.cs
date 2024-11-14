using UnityEngine;
using GraphProcessor;
using System.Collections.Generic;


namespace GraphProcessor
{
    [System.Serializable, NodeMenuItem("Variables/GameObject")]
    public class ANPUA_Var_GameObject  : BaseNode
    {

        [Output(name = "Out"), SerializeField]
        public GameObject obj;


    }
}