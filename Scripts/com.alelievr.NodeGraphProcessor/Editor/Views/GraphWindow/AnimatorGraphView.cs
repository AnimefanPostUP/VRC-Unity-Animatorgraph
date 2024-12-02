using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using GraphProcessor;
using System;
using UnityEditor;

namespace GraphProcessor
{

    public class AnimatorGraphView : BaseGraphView
    {
        // Nothing special to add for now
        public AnimatorGraphView(EditorWindow window) : base(window)
        {

            RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
            RegisterCallback<DragPerformEvent>(OnDragPerformEvent);


        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            BuildStackNodeContextualMenu(evt);
            base.BuildContextualMenu(evt);
        }

        /// <summary>
        /// Add the New Stack entry to the context menu
        /// </summary>
        /// <param name="evt"></param>
        protected void BuildStackNodeContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 position = (evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
            evt.menu.AppendAction("New Stack", (e) => AddStackNode(new BaseStackNode(position)), DropdownMenuAction.AlwaysEnabled);
        }


        //Function for drag and Drop
        protected void OnDropObjects(UnityEngine.Object[] objects)
        {
            Debug.Log("Dropped objects");
            foreach (var obj in objects)
            {
                if (obj is GameObject go)
                {
                    ANPUA_Var_GameObject node = new ANPUA_Var_GameObject();
                    AddNode(node);
                    node.position.position = contentViewContainer.WorldToLocal(new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y));
                    AddNode(node);
                }
            }
        }

        private void OnDragUpdatedEvent(DragUpdatedEvent evt)
        {
            if (DragAndDrop.GetGenericData("NodeType") != null)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                evt.StopPropagation();
            }
        }

        private void OnDragPerformEvent(DragPerformEvent evt)
        {
            if (DragAndDrop.GetGenericData("NodeType") != null)
            {
                var nodeType = DragAndDrop.GetGenericData("NodeType") as Type;
                var mousePosition = evt.mousePosition;
                var graphPosition = contentViewContainer.WorldToLocal(mousePosition);

                // Create the node at the drop position
                AddNode(BaseNode.CreateFromType(nodeType, graphPosition));

                DragAndDrop.AcceptDrag();
                DragAndDrop.SetGenericData("NodeType", null);
                evt.StopPropagation();
            }
        }
    }
}