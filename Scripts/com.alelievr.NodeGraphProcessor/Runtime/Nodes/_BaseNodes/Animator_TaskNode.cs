using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphProcessor
{
    public class Animator_TaskNode : AnimatorNode
    {
        public TaskContainer taskContainer;

        public virtual TaskContainer ProcessTaskOnBuild(
            TaskContainer taskContainer,
            ANPUA_ParameterManager parameterManager
        )
        {
            return null;
        }
    }
}
