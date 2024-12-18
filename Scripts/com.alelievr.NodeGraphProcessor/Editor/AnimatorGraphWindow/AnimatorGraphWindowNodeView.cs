
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using GraphProcessor;
namespace GraphProcessor
{

    public class AnimatorGraphWindowNodeView : PinnedElementView
    {
        NodeProcessor processor;
        BaseGraphView graphView;

        public AnimatorGraphWindowNodeView() => title = "Conditional Processor";

        protected override void Initialize(BaseGraphView graphView)
        {
            processor = new NodeProcessor(graphView.graph);
            this.graphView = graphView;

            graphView.computeOrderUpdated += processor.UpdateComputeOrder;

            Button runButton = new Button(OnPlay) { name = "ActionButton", text = "Run" };
            Button stepButton = new Button(OnStep) { name = "ActionButton", text = "Step" };

            content.Add(runButton);
            content.Add(stepButton);
        }

        void OnPlay() => processor.Run();

        void OnStep()
        {
            BaseNodeView view;

            if (processor.currentGraphExecution != null)
            {
                // Unhighlight the last executed node
                view = graphView.nodeViews.Find(v => v.nodeTarget == processor.currentGraphExecution.Current);
                view.UnHighlight();
            }

            processor.Step();

            // Display debug infos, currentGraphExecution is modified in the Step() function above
            if (processor.currentGraphExecution != null)
            {
                view = graphView.nodeViews.Find(v => v.nodeTarget == processor.currentGraphExecution.Current);
                view.Highlight();
            }
        }
    }
}