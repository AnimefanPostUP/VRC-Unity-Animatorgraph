using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphProcessor
{
    [Serializable]
    public class ANPUA_GenericParameter
    {
        //Public variable of both Enum

        //guid
        public string guid=System.Guid.NewGuid().ToString();
        public string name="New Parameter";

        public ANPUA_ParameterSyncState syncState=ANPUA_ParameterSyncState.Unsynch;
        public ANPUA_ParameterState state=ANPUA_ParameterState.Unsaved;
        public ANPUA_ParameterType type=ANPUA_ParameterType.Int;
        

        //Create a getter and setter based on the Type
        public int IntValue
        {
            get
            {
                return (int)value;
            }
            set
            {
                this.value = value;
            }
        }

        public float FloatValue
        {
            get
            {
                return (float)value;
            }
            set
            {
                this.value = value;
            }
        }

        public bool BoolValue
        {
            get
            {
                return (bool)value;
            }
            set
            {
                this.value = value;
            }
        }

        //Create a generic value
        public object value;

        //Create a generic constructor
        public ANPUA_GenericParameter(ANPUA_ParameterSyncState syncState, ANPUA_ParameterState state, ANPUA_ParameterType type, object value)
        {
            this.syncState = syncState;
            this.state = state;
            this.type = type;
            this.value = value;
        }


    }

    //Enum for Synch and Unsynch
    public enum ANPUA_ParameterSyncState
    {
        Synch,
        Unsynch
    }

    //Enum for Saved and Unsaved
    public enum ANPUA_ParameterState
    {
        Saved,
        Unsaved
    }

    public enum ANPUA_ParameterType
    {
        Int,
        Float,
        Bool

    }
}
