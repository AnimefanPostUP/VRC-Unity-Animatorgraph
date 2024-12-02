
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

    public class GenericParamerterFieldView : BlackboardField
    {
        protected BaseGraphView graphView;

        public ANPUA_GenericParameter parameter { get; private set; }

        private Type nodeType;
        public GenericParamerterFieldView()
            //: base(null, param.name, param.type.ToString())
            : base()
        {

        }

        public void initialize(BaseGraphView graphView, ANPUA_GenericParameter param)
        {
            this.graphView = graphView;

            //Setting the BlackboardFields  param.name, param.type.ToString()
            this.text = param.name;
            //this.typeText = param.type.ToString();

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

            //set type based on parameter type
            switch (param.type)
            {
                case ANPUA_ParameterType.Int:
                    nodeType = typeof(ANPUA_Parameter_Int);
                    break;
                case ANPUA_ParameterType.Float:
                    nodeType = typeof(ANPUA_Parameter_Float);
                    break;
                case ANPUA_ParameterType.Bool:
                    nodeType = typeof(ANPUA_Parameter_Bool);
                    break;
            }

            RegisterCallback<MouseDownEvent>(OnMouseDown);
            RegisterCallback<MouseMoveEvent>(OnMouseMove);
            RegisterCallback<MouseUpEvent>(OnMouseUp);

            RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
            RegisterCallback<DragPerformEvent>(OnDragPerformEvent);


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
            //if (graphView.contentViewContainer.ContainsPoint(graphView.contentViewContainer.WorldToLocal(evt.mousePosition)))
            if (DragAndDrop.GetGenericData("NodeType") != null)
            {

                //Make sure we are above the graphView


                var nodeType = DragAndDrop.GetGenericData("NodeType") as Type;
                var mousePosition = evt.mousePosition;
                var graphPosition = graphView.contentViewContainer.WorldToLocal(mousePosition);

                // Create the node at the drop position
                var newnode = BaseNode.CreateFromType(nodeType, graphPosition);

                //if ANPUA_Parameter_Bool
                if (newnode is ANPUA_Parameter_Bool boolNode)
                {
                    boolNode.parameter = parameter;
                }

                //if ANPUA_Parameter_Float
                if (newnode is ANPUA_Parameter_Float floatNode)
                {
                    floatNode.parameter = parameter;
                }

                //if ANPUA_Parameter_Int
                if (newnode is ANPUA_Parameter_Int intNode)
                {
                    intNode.parameter = parameter;
                }



                graphView.AddNode(newnode);

                DragAndDrop.AcceptDrag();
                DragAndDrop.SetGenericData("NodeType", null);
                evt.StopPropagation();
            }
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.SetGenericData("NodeType", nodeType);
            DragAndDrop.StartDrag("Dragging Node");
            evt.StopPropagation();
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (DragAndDrop.GetGenericData("NodeType") != null)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                evt.StopPropagation();
            }
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (DragAndDrop.GetGenericData("NodeType") != null)
            {
                DragAndDrop.AcceptDrag();
                DragAndDrop.SetGenericData("NodeType", null);
                evt.StopPropagation();
            }
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

    [Serializable]
    public class SettingsPanel : VisualElement
    {
        private TextField nameField;
        private Toggle syncStateToggle;
        private Toggle stateToggle;
        private EnumField typeField;
        private IntegerField intField;
        private FloatField floatField;
        private Toggle boolField;
        private BaseGraphView graphView;

        public SettingsPanel()
        {
            // Set up the style and layout for the settings panel
            style.flexDirection = FlexDirection.Row;
            style.flexWrap = Wrap.NoWrap;
            style.justifyContent = Justify.FlexStart; // Align items to the start horizontally
            style.alignItems = Align.FlexStart; // Align items to the start vertically
            style.marginBottom = 5;

            // Add fields to the row container
            nameField = new TextField("Name");
            nameField.style.minWidth = 32; // Set minimum width
            nameField.style.flexGrow = 0; // Prevent stretching
            Add(nameField);

            syncStateToggle = new Toggle("Sync State");
            nameField.style.minWidth = 12;
            syncStateToggle.style.flexGrow = 0; // Prevent stretching
            Add(syncStateToggle);

            stateToggle = new Toggle("State");
            stateToggle.style.flexGrow = 0; // Prevent stretching
            Add(stateToggle);

            typeField = new EnumField("Type");
            typeField.style.flexGrow = 0; // Prevent stretching
            Add(typeField);

            intField = new IntegerField("Int Value");
            intField.style.flexGrow = 0; // Prevent stretching
            Add(intField);

            floatField = new FloatField("Float Value");
            floatField.style.flexGrow = 0; // Prevent stretching
            Add(floatField);

            boolField = new Toggle("Bool Value");
            boolField.style.flexGrow = 0; // Prevent stretching
            Add(boolField);
        }

        public void SetParameter(ANPUA_GenericParameter parameter)
        {
            nameField.value = parameter.name;
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


    public class ParameterWindowView : PinnedElementView
    {
        protected BaseGraphView graphView;

        new const string title = "Parameters";

        readonly string exposedParameterViewStyle = "GraphProcessorStyles/ExposedParameterView";

        List<Rect> blackboardLayouts = new List<Rect>();

        private MultiColumnListView multiColumnListView;

        public ParameterWindowView()
        {
            var style = Resources.Load<StyleSheet>(exposedParameterViewStyle);
            if (style != null)
                styleSheets.Add(style);


        }

        public void createList()
        {

            content.Clear();

            // Create a MultiColumnListView
            multiColumnListView = new MultiColumnListView();
            multiColumnListView.style.flexGrow = 2;
            multiColumnListView.selectionType = SelectionType.None;
            multiColumnListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            // Add the MultiColumnListView to the content container
            content.Add(multiColumnListView);
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
            //Update
            UpdateParameterList();
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

            // Clear the list
            createList();

            // Define the columns
            multiColumnListView.columns.Add(new Column
            {
                title = "Name",
                makeCell = () => new GenericParamerterFieldView(),
                bindCell = (element, i) =>
                {
                    var parameterField = element as GenericParamerterFieldView;
                    var parameter = graphView.graph.parameters[i] as ANPUA_GenericParameter;
                    parameterField.initialize(graphView, parameter);
                },
                minWidth = 120,

            });

            multiColumnListView.columns.Add(new Column
            {
                title = "Sync",
                makeCell = () => new Toggle(),
                bindCell = (element, i) =>
                {
                    var toggle = element as Toggle;
                    var parameter = graphView.graph.parameters[i] as ANPUA_GenericParameter;
                    toggle.value = parameter.syncState == ANPUA_ParameterSyncState.Synch;
                    toggle.RegisterValueChangedCallback(evt => parameter.syncState = evt.newValue ? ANPUA_ParameterSyncState.Synch : ANPUA_ParameterSyncState.Unsynch);
                },
                width = 40
            });

            multiColumnListView.columns.Add(new Column
            {
                title = "Save",
                makeCell = () => new Toggle(),
                bindCell = (element, i) =>
                {
                    var toggle = element as Toggle;
                    var parameter = graphView.graph.parameters[i] as ANPUA_GenericParameter;
                    toggle.value = parameter.state == ANPUA_ParameterState.Saved;
                    toggle.RegisterValueChangedCallback(evt => parameter.state = evt.newValue ? ANPUA_ParameterState.Saved : ANPUA_ParameterState.Unsaved);
                },
                width = 40
            });

            multiColumnListView.columns.Add(new Column
            {
                title = "Type",
                makeCell = () => new EnumField(ANPUA_ParameterType.Int),
                bindCell = (element, i) =>
                {
                    var enumField = element as EnumField;
                    var parameter = graphView.graph.parameters[i] as ANPUA_GenericParameter;
                    enumField.value = parameter.type;
                    enumField.RegisterValueChangedCallback(evt => parameter.type = (ANPUA_ParameterType)evt.newValue);
                },
                //Flex Grow
                width = 50,



            });


            multiColumnListView.columns.Add(new Column
            {
                title = "Value",
                makeCell = () => new VisualElement(),
                bindCell = (element, i) =>
                {
                    var parameter = graphView.graph.parameters[i] as ANPUA_GenericParameter;
                    var container = element as VisualElement;
                    container.Clear();

                    switch (parameter.type)
                    {
                        case ANPUA_ParameterType.Int:
                            var intField = new IntegerField();
                            intField.value = parameter.IntValue;
                            intField.RegisterValueChangedCallback(evt => parameter.IntValue = evt.newValue);
                            container.Add(intField);
                            break;
                        case ANPUA_ParameterType.Float:
                            var floatField = new FloatField();
                            floatField.value = parameter.FloatValue;
                            floatField.RegisterValueChangedCallback(evt => parameter.FloatValue = evt.newValue);
                            container.Add(floatField);
                            break;
                        case ANPUA_ParameterType.Bool:
                            var boolField = new Toggle();
                            boolField.value = parameter.BoolValue;
                            boolField.RegisterValueChangedCallback(evt => parameter.BoolValue = evt.newValue);
                            container.Add(boolField);
                            break;
                    }
                },
                width = 70
            });

            multiColumnListView.itemsSource = graphView.graph.parameters;
            multiColumnListView.Rebuild();
        }
        protected override void Initialize(BaseGraphView graphView)
        {
            this.graphView = graphView;
            base.title = title;
            scrollable = true;

            graphView.onParameterListChanged += UpdateParameterList;
            graphView.initialized += UpdateParameterList;
            Undo.undoRedoPerformed += UpdateParameterList;


            //RegisterCallback<MouseDownEvent>(OnMouseDownEvent, TrickleDown.TrickleDown);
            RegisterCallback<DetachFromPanelEvent>(OnViewClosed);

            // Add exposed parameter button
            header.Add(new Button(OnAddClicked)
            {
                text = "+"
            });

            UpdateParameterList();
        }


        // void OnMouseDownEvent(MouseDownEvent evt)
        // {
        //     blackboardLayouts = content.Children().Select(c => c.layout).ToList();
        // }


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


        // void OnDragUpdatedEvent(DragUpdatedEvent evt)
        // {
        //     DragAndDrop.visualMode = DragAndDropVisualMode.Move;
        //     int newIndex = GetInsertIndexFromMousePosition(evt.mousePosition);
        //     var graphSelectionDragData = DragAndDrop.GetGenericData("DragSelection");

        //     if (graphSelectionDragData == null)
        //         return;

        //     foreach (var obj in graphSelectionDragData as List<ISelectable>)
        //     {
        //         if (obj is ANPUA_GenericParameterFieldView view)
        //         {
        //             var blackBoardRow = view.parent.parent.parent.parent.parent.parent;
        //             int oldIndex = content.Children().ToList().FindIndex(c => c == blackBoardRow);
        //             // Try to find the blackboard row
        //             content.Remove(blackBoardRow);

        //             if (newIndex > oldIndex)
        //                 newIndex--;

        //             content.Insert(newIndex, blackBoardRow);
        //         }
        //     }
        // }

        // void OnDragPerformEvent(DragPerformEvent evt)
        // {
        //     bool updateList = false;

        //     int newIndex = GetInsertIndexFromMousePosition(evt.mousePosition);
        //     foreach (var obj in DragAndDrop.GetGenericData("DragSelection") as List<ISelectable>)
        //     {
        //         if (obj is ANPUA_GenericParameterFieldView view)
        //         {
        //             if (!updateList)
        //                 graphView.RegisterCompleteObjectUndo("Moved parameters");

        //             int oldIndex = graphView.graph.parameters.FindIndex(e => e == view.parameter);
        //             var parameter = graphView.graph.parameters[oldIndex];
        //             graphView.graph.parameters.RemoveAt(oldIndex);

        //             // Patch new index after the remove operation:
        //             if (newIndex > oldIndex)
        //                 newIndex--;

        //             graphView.graph.parameters.Insert(newIndex, parameter);

        //             updateList = true;
        //         }
        //     }

        //     if (updateList)
        //     {
        //         evt.StopImmediatePropagation();
        //         UpdateParameterList();
        //     }
        // }


        // void OnMouseDown(MouseDownEvent evt)
        // {
        //     DragAndDrop.PrepareStartDrag();
        //     DragAndDrop.objectReferences = new UnityEngine.Object[] { evt.target as UnityEngine.Object };
        //     DragAndDrop.StartDrag("Dragging BlackboardField");
        //     evt.StopPropagation();
        // }

        // void OnMouseMove(MouseMoveEvent evt)
        // {
        //     if (DragAndDrop.objectReferences.Length > 0)
        //     {
        //         DragAndDrop.visualMode = DragAndDropVisualMode.Move;
        //         evt.StopPropagation();
        //     }
        // }

        // void OnMouseUp(MouseUpEvent evt)
        // {
        //     DragAndDrop.AcceptDrag();
        //     evt.StopPropagation();
        // }



        void OnViewClosed(DetachFromPanelEvent evt)
        {
            Undo.undoRedoPerformed -= UpdateParameterList; 
        }


    }
}