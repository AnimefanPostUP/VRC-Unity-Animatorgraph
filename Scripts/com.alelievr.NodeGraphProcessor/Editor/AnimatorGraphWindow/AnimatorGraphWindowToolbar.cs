using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;
using Status = UnityEngine.UIElements.DropdownMenuAction.Status;

namespace GraphProcessor
{

    public class AnimatorGraphWindowToolbar : ToolbarView
    {
        public AnimatorGraphWindowToolbar(BaseGraphView graphView) : base(graphView) { }

        protected override void AddButtons()
        {
            // Add the hello world button on the left of the toolbar
            AddButton("Hello !", () => Debug.Log("Hello World"), left: false);

            // add the default buttons (center, show processor and show in project)
            base.AddButtons();

            bool conditionalProcessorVisible = graphView.GetPinnedElementStatus<AnimatorGraphWindowNodeView>() != Status.Hidden;
            AddToggle("Show Conditional Processor", conditionalProcessorVisible, (v) => graphView.ToggleView<AnimatorGraphWindowNodeView>());
        }
    }
}