
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
using System;

namespace GraphProcessor
{

    using UnityEngine.UIElements;

    public class ANPUA_GenericParameterFieldView : BlackboardField
    {
        protected BaseGraphView graphView;

        public ANPUA_GenericParameter parameter { get; private set; }

        public ANPUA_GenericParameterFieldView(BaseGraphView graphView, ANPUA_GenericParameter param)
            //: base(null, param.name, param.type.ToString())
            : base(null, param.name, "")
        {
            this.graphView = graphView;
            parameter = param;
            this.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));
            this.Q("icon").AddToClassList("parameter-" + param.type.ToString());
            this.Q("icon").visible = true;

            (this.Q("textField") as TextField).RegisterValueChangedCallback((e) =>
            {
                param.name = e.newValue;
                text = e.newValue;
                graphView.graph.UpdateParameterName(param, e.newValue);
            });
        }

        void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Rename", (a) => OpenTextEditor(), DropdownMenuAction.AlwaysEnabled);
            evt.menu.AppendAction("Delete", (a) => graphView.graph.RemoveParameter(parameter), DropdownMenuAction.AlwaysEnabled);
            evt.StopPropagation();
        }
    }

    public class ANPUA_GenericParameterPropertyView : VisualElement
    {
        protected BaseGraphView baseGraphView;

        public ANPUA_GenericParameter parameter { get; private set; }

        public Toggle hideInInspector { get; private set; }

        public ANPUA_GenericParameterPropertyView(BaseGraphView graphView, ANPUA_GenericParameter param)
        {
            baseGraphView = graphView;
            parameter = param;
        }
    }
 
    public class SettingsPanel : VisualElement
    {
        private Toggle syncStateToggle;
        private Toggle stateToggle;
        private EnumField typeField;
        private IntegerField intField;
        private FloatField floatField;
        private Toggle boolField;
 

        public SettingsPanel(ANPUA_GenericParameter param, BaseGraphView graphView)
        {
            style.flexDirection = FlexDirection.Column;
            style.marginBottom = 5;

            // Data row containing fields
            var dataRow = new VisualElement
            {
                style = { flexDirection = FlexDirection.Row, justifyContent = Justify.SpaceBetween }
            };

            var board = new BlackboardRow(new ANPUA_GenericParameterFieldView(graphView, param), new ANPUA_GenericParameterPropertyView(graphView, param))

            {
                style = { flexDirection = FlexDirection.Column, flexGrow = 1 }
            };

            dataRow.Add(board);

            typeField = new EnumField(param.type);
            typeField.RegisterValueChangedCallback(evt => UpdateFieldsVisibility((ANPUA_ParameterType)evt.newValue));
            dataRow.Add(CreateGridItem(typeField));

            intField = new IntegerField();
            dataRow.Add(CreateGridItem(intField));

            floatField = new FloatField();
            dataRow.Add(CreateGridItem(floatField));

            boolField = new Toggle();
            dataRow.Add(CreateGridItem(boolField));

            syncStateToggle = new Toggle();
            dataRow.Add(CreateGridItem(syncStateToggle));

            stateToggle = new Toggle();
            dataRow.Add(CreateGridItem(stateToggle));

            Add(dataRow);

            UpdateFieldsVisibility(param.type);
        }



        private VisualElement CreateGridItem(VisualElement element)
        {
            var gridItem = new VisualElement
            {
                style = { flexDirection = FlexDirection.Column, flexGrow = 1 }
            };
            gridItem.Add(element);
            return gridItem;
        }

        public void SetParameter(ANPUA_GenericParameter parameter)
        {
            syncStateToggle.value = parameter.syncState == ANPUA_ParameterSyncState.Synch;
            stateToggle.value = parameter.state == ANPUA_ParameterState.Saved;
            typeField.value = parameter.type;
            intField.value = parameter.IntValue;
            floatField.value = parameter.FloatValue;
            boolField.value = parameter.BoolValue;

            UpdateFieldsVisibility(parameter.type);
        }

        private void UpdateFieldsVisibility(ANPUA_ParameterType type)
        {
            intField.style.display = type == ANPUA_ParameterType.Int ? DisplayStyle.Flex : DisplayStyle.None;
            floatField.style.display = type == ANPUA_ParameterType.Float ? DisplayStyle.Flex : DisplayStyle.None;
            boolField.style.display = type == ANPUA_ParameterType.Bool ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }


    public class ANPUA_ParametersView : PinnedElementView
    {
        protected BaseGraphView graphView;

        new const string title = "Parameters";

        readonly string exposedParameterViewStyle = "GraphProcessorStyles/ExposedParameterView";

        List<Rect> blackboardLayouts = new List<Rect>();



        public ANPUA_ParametersView()
        {
            var style = Resources.Load<StyleSheet>(exposedParameterViewStyle);
            if (style != null)
                styleSheets.Add(style);
        }

        protected virtual void OnAddClicked()
        {
            var parameterType = new GenericMenu();

            //Create a new parameter for each type of ANPUA_ParamterType Enum

            //Get the Enums
            var enumType = typeof(ANPUA_ParameterType);
            var enumValues = Enum.GetValues(enumType);


            //Create a new parameter for each enum value
            foreach (ANPUA_ParameterType paramType in enumValues)
            {
                parameterType.AddItem(new GUIContent(paramType.ToString()), false, () =>
                {
                    string uniqueName = "New " + paramType.ToString();

                    uniqueName = GetUniquePropertyName(uniqueName);
                    graphView.graph.AddParameter(uniqueName, paramType);
                });
            }

            parameterType.ShowAsContext();
        }

        protected string GetNiceNameFromType(Type type)
        {
            string name = type.Name;

            // Remove parameter in the name of the type if it exists
            name = name.Replace("Parameter", "");

            return ObjectNames.NicifyVariableName(name);
        }

        protected string GetUniquePropertyName(string name)
        {
            // Generate unique name
            string uniqueName = name;
            int i = 0;
            while (graphView.graph.parameters.Any(e => e.name == name))
                name = uniqueName + " " + i++;
            return name;

        }

        private Label CreateHeaderLabel(string text)
        {
            return new Label(text)
            {
                style = { unityFontStyleAndWeight = FontStyle.Bold, unityTextAlign = TextAnchor.MiddleCenter, marginBottom = 5 }
            };
        }

        protected virtual void UpdateParameterList()
        {
            content.Clear();


            content.style.display = DisplayStyle.Flex;
            content.style.flexDirection = FlexDirection.Column;
            content.style.flexWrap = Wrap.NoWrap;
            content.style.justifyContent = Justify.FlexStart;
            //mainGridContainer.style.alignItems = Align.Stretch;


            // Header row with labels
            var headerRow = new VisualElement
            {
                style = { flexDirection = FlexDirection.Row, justifyContent = Justify.SpaceBetween }
            };

            headerRow.Add(CreateHeaderLabel("Name"));
            headerRow.Add(CreateHeaderLabel("Type"));
            headerRow.Add(CreateHeaderLabel("Value"));
            headerRow.Add(CreateHeaderLabel("Sync"));
            headerRow.Add(CreateHeaderLabel("Save")); 

            content.Add(headerRow);

            foreach (var param in graphView.graph.parameters)
            {
                //Create Visual Children for each parameter
                var settingsPanel = new SettingsPanel(param, graphView);
                content.Add(settingsPanel);
            }

        }
        protected override void Initialize(BaseGraphView graphView)
        {
            this.graphView = graphView;
            base.title = title;
            scrollable = true;

            graphView.onParameterListChanged += UpdateParameterList;
            graphView.initialized += UpdateParameterList;
            Undo.undoRedoPerformed += UpdateParameterList;

            RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
            RegisterCallback<DragPerformEvent>(OnDragPerformEvent);
            RegisterCallback<MouseDownEvent>(OnMouseDownEvent, TrickleDown.TrickleDown);
            RegisterCallback<DetachFromPanelEvent>(OnViewClosed);

            UpdateParameterList();

            // Add exposed parameter button
            header.Add(new Button(OnAddClicked)
            {
                text = "+"
            });
        }

        void OnViewClosed(DetachFromPanelEvent evt)
            => Undo.undoRedoPerformed -= UpdateParameterList;

        void OnMouseDownEvent(MouseDownEvent evt)
        {
            blackboardLayouts = content.Children().Select(c => c.layout).ToList();
        }

        int GetInsertIndexFromMousePosition(Vector2 pos)
        {
            pos = content.WorldToLocal(pos);
            // We only need to look for y axis;
            float mousePos = pos.y;

            if (mousePos < 0)
                return 0;

            int index = 0;
            foreach (var layout in blackboardLayouts)
            {
                if (mousePos > layout.yMin && mousePos < layout.yMax)
                    return index + 1;
                index++;
            }

            return content.childCount;
        }

        void OnDragUpdatedEvent(DragUpdatedEvent evt)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Move;
            int newIndex = GetInsertIndexFromMousePosition(evt.mousePosition);
            var graphSelectionDragData = DragAndDrop.GetGenericData("DragSelection");

            if (graphSelectionDragData == null)
                return;

            // foreach (var obj in graphSelectionDragData as List<ISelectable>)
            // {
            //     if (obj is ANPUA_GenericParameterFieldView view)
            //     {
            //         var blackBoardRow = view.parent.parent.parent.parent.parent.parent;
            //         int oldIndex = content.Children().ToList().FindIndex(c => c == blackBoardRow);
            //         // Try to find the blackboard row
            //         content.Remove(blackBoardRow);

            //         if (newIndex > oldIndex)
            //             newIndex--;

            //         content.Insert(newIndex, blackBoardRow);
            //     }
            // }


            //Changed to use the SettingsPanel
            foreach (var obj in graphSelectionDragData as List<ISelectable>)
            {
                if (obj is SettingsPanel view)
                {
                    var blackBoardRow = view.parent;
                    int oldIndex = content.Children().ToList().FindIndex(c => c == blackBoardRow);
                    // Try to find the blackboard row
                    content.Remove(blackBoardRow);

                    if (newIndex > oldIndex)
                        newIndex--;

                    content.Insert(newIndex, blackBoardRow);
                }
            }
        }

        void OnDragPerformEvent(DragPerformEvent evt)
        {
            bool updateList = false;

            // int newIndex = GetInsertIndexFromMousePosition(evt.mousePosition);
            // foreach (var obj in DragAndDrop.GetGenericData("DragSelection") as List<ISelectable>)
            // {
            //     if (obj is ExposedParameterFieldView view)
            //     {
            //         if (!updateList)
            //             graphView.RegisterCompleteObjectUndo("Moved parameters");

            //         int oldIndex = graphView.graph.parameters.FindIndex(e => e == view.parameter);
            //         var parameter = graphView.graph.parameters[oldIndex];
            //         graphView.graph.parameters.RemoveAt(oldIndex);

            //         // Patch new index after the remove operation:
            //         if (newIndex > oldIndex)
            //             newIndex--;

            //         graphView.graph.parameters.Insert(newIndex, parameter);

            //         updateList = true;
            //     }
            // }

            // if (updateList)
            // {
            //     evt.StopImmediatePropagation();
            //     UpdateParameterList();
            // }
        }
    }
}