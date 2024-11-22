using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using GraphProcessor;

[NodeCustomEditor(typeof(ANPUA_NodeState))]
public class ANPUA_NodeView : BaseNodeView
{
    public override void Enable()
    {
        AddControlField(nameof(ANPUA_NodeState.color));
        style.width = 200;
        //target

        //Create fields for conditionContainer

        // controlsContainer.Add(new Label("controlsContainer"));
        // debugContainer.Add(new Label("debugContainer"));
        // rightTitleContainer.Add(new Label("rightTitleContainer"));
        // topPortContainer.Add(new Label("topPortContainer"));
        // bottomPortContainer.Add(new Label("bottomPortContainer"));
        //inputContainerElement.Add(new Label("inputContainerElement"));

    }
}
