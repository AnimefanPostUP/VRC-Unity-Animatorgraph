using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



namespace GraphProcessor
{
    [Serializable]
    public class ConditionContainer : MonoBehaviour
    {
        [SerializeField]
        public string parameter;
        [SerializeField]
        public float threshold;
    }

    [Serializable]
    public class ConditionPackage : MonoBehaviour
    {
        [SerializeField]
        public string parameterName;

        [SerializeField]
        public ANPUA_GenericParameter parameter;


        [SerializeField]
        public float threshold;
        public ConditionPackage(string name, ANPUA_GenericParameter param, float thresh)
        {
            this.parameterName = name;
            this.parameter = param;
            this.threshold = thresh;
        }

       
       

   
    }
}
