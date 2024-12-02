using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;
namespace GraphProcessor
{

    [CustomEditor(typeof(ANPUA_AssetGraph))]
    public class AssetGraphInspector : Editor
    {
        //button to add a BaseGraph to the AssetGraph
        public override void OnInspectorGUI()
        {
            ANPUA_AssetGraph myTarget = (ANPUA_AssetGraph)target;
            EditorGUILayout.LabelField("Asset Graph");

            myTarget.graphData = (BaseGraph)EditorGUILayout.ObjectField("Graph Data", myTarget.graphData, typeof(BaseGraph), false);

            //gameobject field called menutarget
            myTarget.assetTarget = (GameObject)EditorGUILayout.ObjectField("assetTarget", myTarget.assetTarget, typeof(GameObject), true);

            //Button to Add a new BaseGraph
            if (GUILayout.Button("Add BaseGraph"))
            {
                myTarget.graphData = new BaseGraph();
            }

            //Button to open the Graph Editor
            if (GUILayout.Button("Open Graph Editor"))
            {
                AnimatorGraphWindow.Open(myTarget.graphData);
            }
        }

    }
}
