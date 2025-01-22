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
}
