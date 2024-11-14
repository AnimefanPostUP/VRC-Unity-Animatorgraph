using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;
using UnityEngine.UIElements;


namespace GraphProcessor
{

    public class ANPUA_GraphWindow : BaseGraphWindow
    {
        BaseGraph tmpGraph;
        ANPUA_GraphWindowToolbar toolbarView;

        [MenuItem("Animtools/Graph")] 
        public static BaseGraphWindow OpenWithTmpGraph()
        {
            var graphWindow = CreateWindow<ANPUA_GraphWindow>();

            // When the graph is opened from the window, we don't save the graph to disk
            graphWindow.tmpGraph = ScriptableObject.CreateInstance<BaseGraph>();
            graphWindow.tmpGraph.hideFlags = HideFlags.HideAndDontSave;
            graphWindow.InitializeGraph(graphWindow.tmpGraph);

            graphWindow.Show();

            return graphWindow;
        }

        //Open the Windows with a specific graph
        public static BaseGraphWindow Open(BaseGraph graph)
        {
            var graphWindow = CreateWindow<ANPUA_GraphWindow>();

            graphWindow.InitializeGraph(graph);

            graphWindow.Show();

            return graphWindow;
        }

        protected override void OnDestroy()
        {
            graphView?.Dispose();
            DestroyImmediate(tmpGraph);
        }

        protected override void InitializeWindow(BaseGraph graph)
        {
            titleContent = new GUIContent("ANPUA Graph");

            if (graphView == null)
            {
                graphView = new ANPUA_GraphView(this);
                toolbarView = new ANPUA_GraphWindowToolbar(graphView);
                graphView.Add(toolbarView);
            }

            rootView.Add(graphView);
        }


        

        protected override void InitializeGraphView(BaseGraphView view)
        {
            // graphView.OpenPinned< ExposedParameterView >();
            // toolbarView.UpdateButtonStatus();
        }
    }
}

