using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using GraphProcessor;
using System;
using GraphProcessor.Builder;
namespace GraphProcessor
{
    [NodeCustomEditor(typeof(ANPUA_Condition))]
    public class NodeConditionView : BaseNodeView
    {


        public class ANPUA_ParameterListVisualElement : VisualElement
        {
            public ANPUA_Condition condition;

            MultiColumnListView multiColumnListView;

            public ANPUA_ParameterListVisualElement(BaseNode node)
            {
                this.condition = (ANPUA_Condition)node;
                style.flexGrow = 1;
                reloadFields();
            }


            public void reloadFields()
            {
                createList();
                UpdateConditionElements();

                //Label label = new Label($"Conditions: {condition.conditionContainer.Length}");
                //Add(label);
            }

            public void createList()
            {
                Clear();
                // Create a MultiColumnListView
                multiColumnListView = new MultiColumnListView();
                multiColumnListView.style.flexGrow = 2;
                multiColumnListView.selectionType = SelectionType.None;
                multiColumnListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
                //Set height to 100%
                multiColumnListView.style.height = 110;
                // Add the MultiColumnListView to the content container
                Add(multiColumnListView);
            }

            public void UpdateConditionElements()
            {
                multiColumnListView.columns.Add(new Column
                {
                    title = "Param",
                    makeCell = () => new Button(),
                    bindCell = (element, i) =>
                    {
                        Button button = (Button)element;
                        //align button text Left
                        button.style.unityTextAlign = TextAnchor.MiddleLeft;
                        if (condition.conditionContainer[i].parameterName == "")
                        {
                            button.text = "Select";
                        }
                        else
                        {
                            button.text = condition.conditionContainer[i].parameterName;
                        }

                        button.clickable.clicked += () =>
                        {
                            OpenParameterMenu(condition.conditionContainer[i]);
                        };
                    },
                    minWidth = 80,

                });

                multiColumnListView.columns.Add(new Column
                {
                    title = "Mode",
                    makeCell = () => new Button(),
                    bindCell = (element, i) =>
                    {
                        Button enumField = (Button)element;
                        //Text align
                        //enumField.style.unityTextAlign = TextAnchor.MiddleLeft;
                        ANPUA_ParameterType parametertype = ANPUA_ParameterType.Int;
                        if (condition.conditionContainer[i].parameter != null)
                            parametertype = condition.conditionContainer[i].parameter.type;

                        //Correct Mode Based on Type
                        List<CompareEnum> options = getEnumList(parametertype);
                        if (!options.Contains(condition.conditionContainer[i].compareMode))
                        {
                            condition.conditionContainer[i].compareMode = options[0];
                        }

                        enumField.text = condition.conditionContainer[i].compareMode.ToString();

                        //On button
                        enumField.clickable.clicked += () =>
                        {
                            OpenModeMenu(parametertype, condition.conditionContainer[i], enumField);
                        };

                    },
                    minWidth = 65,

                });

                multiColumnListView.columns.Add(new Column
                {
                    title = "Value",
                    makeCell = () => new VisualElement(),
                    bindCell = (element, i) =>
                    {
                        //TextField textField = (TextField)element;
                        //textField.SetValueWithoutNotify(condition.conditionContainer[i].GetValue().ToString());
                        var container = element as VisualElement;
                        //Make the container horizontal
                        container.style.flexDirection = FlexDirection.Row;

                        container.Clear();

                        ANPUA_ParameterType parametertype = ANPUA_ParameterType.Float;
                        if (condition.conditionContainer[i].parameter != null)
                            parametertype = condition.conditionContainer[i].parameter.type;

                        switch (parametertype)
                        {
                            case ANPUA_ParameterType.Bool:
                                Toggle boolField = new Toggle();
                                boolField.value = condition.conditionContainer[i].boolValue;
                                boolField.style.flexGrow = 1; // Make the Toggle use the available space
                                boolField.RegisterValueChangedCallback(evt =>
                                {
                                    condition.conditionContainer[i].boolValue = evt.newValue;
                                });
                                container.Add(boolField);
                                break;
                            case ANPUA_ParameterType.Int:
                                IntegerField intField = new IntegerField();
                                intField.value = condition.conditionContainer[i].intValue;
                                intField.style.flexGrow = 1; // Make the IntegerField use the available space
                                intField.RegisterValueChangedCallback(evt =>
                                {
                                    condition.conditionContainer[i].intValue = evt.newValue;
                                });
                                container.Add(intField);
                                break;
                            case ANPUA_ParameterType.Float:
                                FloatField floatField = new FloatField();
                                floatField.value = condition.conditionContainer[i].floatValue;
                                floatField.style.flexGrow = 1; // Make the FloatField use the available space
                                floatField.RegisterValueChangedCallback(evt =>
                                {
                                    condition.conditionContainer[i].floatValue = evt.newValue;
                                });
                                container.Add(floatField);
                                break;
                        }

                        //if the mode is InRange or OutRange, Display/Add a second field if float or int
                        if (condition.conditionContainer[i].compareMode == CompareEnum.Inside || condition.conditionContainer[i].compareMode == CompareEnum.Outside)
                        {
                            switch (parametertype)
                            {
                                case ANPUA_ParameterType.Int:
                                    IntegerField intField = new IntegerField();
                                    intField.value = condition.conditionContainer[i].intValue_B;
                                    intField.style.flexGrow = 1; // Make the IntegerField use the available space
                                    intField.RegisterValueChangedCallback(evt =>
                                    {
                                        condition.conditionContainer[i].intValue_B = evt.newValue;
                                    });
                                    container.Add(intField);
                                    break;
                                case ANPUA_ParameterType.Float:
                                    FloatField floatField = new FloatField();
                                    floatField.value = condition.conditionContainer[i].floatValue_B;
                                    floatField.style.flexGrow = 1; // Make the FloatField use the available space
                                    floatField.RegisterValueChangedCallback(evt =>
                                    {
                                        condition.conditionContainer[i].floatValue_B = evt.newValue;
                                    });
                                    container.Add(floatField);
                                    break;
                            }
                        }


                    },
                    minWidth = 60,

                });

                //Add Column with X Button to remove the Condition
                multiColumnListView.columns.Add(new Column
                {
                    title = "Remove",
                    makeCell = () => new Button(),
                    bindCell = (element, i) =>
                    {
                        Button button = (Button)element;
                        button.text = "x";
                        button.clickable.clicked += () =>
                        {
                            //Remove the Condition
                            ArrayUtility.RemoveAt(ref condition.conditionContainer, i);
                            reloadFields();
                        };
                    },
                    minWidth = 30,

                });

                multiColumnListView.itemsSource = condition.conditionContainer;
            }

            public List<CompareEnum> getEnumList(ANPUA_ParameterType parameterType)
            {
                List<CompareEnum> options = new List<CompareEnum>();
                switch (parameterType)
                {
                    case ANPUA_ParameterType.Bool:
                        options.Add(CompareEnum.Equal);
                        options.Add(CompareEnum.NotEqual);
                        break;
                    case ANPUA_ParameterType.Int:
                        options.Add(CompareEnum.Greater);
                        options.Add(CompareEnum.Less);
                        options.Add(CompareEnum.Equal);
                        options.Add(CompareEnum.NotEqual);
                        options.Add(CompareEnum.Inside);
                        options.Add(CompareEnum.Outside);
                        break;
                    case ANPUA_ParameterType.Float:
                        options.Add(CompareEnum.Greater);
                        options.Add(CompareEnum.Less);
                        options.Add(CompareEnum.Inside);
                        options.Add(CompareEnum.Outside);
                        break;
                }

                return options;
            }

            public void OpenModeMenu(ANPUA_ParameterType parameterType, Builder.AnimatorCondition container, Button button)
            {

                GenericMenu modemenu = new GenericMenu();
                List<CompareEnum> options = getEnumList(parameterType);


                // Update the EnumField options
                //choices = options.ConvertAll(option => option.ToString());

                CompareEnum currentMode = container.compareMode;

                // Adjust the current selected value to the next available if necessary
                if (!options.Contains(currentMode))
                {
                    container.compareMode = options[0];
                }

                foreach (var compareEnum in options)
                {
                    AddModeMenuItem(modemenu, compareEnum, container, button);
                }

                modemenu.ShowAsContext();

            }

            public void AddModeMenuItem(GenericMenu menu, CompareEnum compareEnum, AnimatorCondition container, Button button)
            {
                menu.AddItem(new GUIContent(compareEnum.ToString()), false, () => SelectMode(compareEnum, container));
                //Set text of Button to the current Mode
                if (container.compareMode == compareEnum)
                {
                    button.text = compareEnum.ToString();
                }
            }

            private void SelectMode(CompareEnum compareEnum, Builder.AnimatorCondition container)
            {
                //Set the Parameter in the Container
                container.compareMode = compareEnum;
                reloadFields();
                Debug.Log($"Selected Mode: {compareEnum}");
            }

            public void OpenParameterMenu(AnimatorCondition container)
            {
                GenericMenu menu = new GenericMenu();

                // Organize parameters into submenus based on their paths
                BaseGraph graph = condition.GetProtectedGraph();
                if (graph == null)
                    return;

                foreach (var parameter in graph.parameters)
                {
                    string[] parts = parameter.name.Split('/');
                    AddMenuItem(menu, parts, 0, parameter, container);
                }

                menu.ShowAsContext();
            }

            public void AddMenuItem(GenericMenu menu, string[] parts, int index, ANPUA_GenericParameter parameter, AnimatorCondition containter)
            {
                if (index >= parts.Length)
                    return;

                string currentPath = string.Join("/", parts, 0, index + 1);

                if (index == parts.Length - 1)
                {
                    // Add the final menu item
                    menu.AddItem(new GUIContent(currentPath), false, () => SelectParameter(parameter, containter));
                }
                else
                {
                    // Add a submenu
                    menu.AddItem(new GUIContent(currentPath + "/"), false, null);
                    AddMenuItem(menu, parts, index + 1, parameter, containter);
                }
            }

            private void SelectParameter(ANPUA_GenericParameter parameter, AnimatorCondition conditionContainer)
            {
                //Set the Parameter in the Container
                conditionContainer.SetParameter(parameter);
                reloadFields();
                Debug.Log($"Selected Parameter: {parameter.name}");
            }
        }

        EnumField selection_BranchMode;
        ANPUA_ParameterListVisualElement parameterListVisualElement;
        public override void Enable()
        {

            nodeTarget = (ANPUA_Condition)nodeTarget;
            ANPUA_Condition condition = (ANPUA_Condition)nodeTarget;
            AnimatorCondition[] conditionContainers = condition.conditionContainer;

            //BRANCH MODE
            selection_BranchMode = new EnumField("", condition.onFalse);
            //Add tooltip
            selection_BranchMode.tooltip =
            "When Condition isnt met=> \n" +
            "Nothing: -- \n" +
            "Return: will Return to this State from the State that this Branch Leads to \n" +
            "AnyReturn: will Return to this State, from all States after this one (Excluding Jumps)=> \n";

            selection_BranchMode.RegisterValueChangedCallback(evt => { condition.onFalse = (BranchMode)evt.newValue; });
            rightTitleContainer.Add(selection_BranchMode);
            //Make selection_BranchMode 100 wide
            //selection_BranchMode.style.width = 100;

            //Add Spacer with 4 units
            controlsContainer.Add(new VisualElement() { style = { flexGrow = 4 } });


            //Create a ANPUA_NodeView_Condition
            parameterListVisualElement = new ANPUA_ParameterListVisualElement(nodeTarget);
            parameterListVisualElement.reloadFields();
            controlsContainer.Add(parameterListVisualElement);

            //Button to add container and one to remove, both reload the fields
            Button addContainerButton = new Button(() =>
            {
                //Extend the Array and add a new Container
                //Check if length is 0
                if (condition.conditionContainer == null) condition.conditionContainer = new AnimatorCondition[0];
                Array.Resize<AnimatorCondition>(ref condition.conditionContainer, condition.conditionContainer.Length + 1);
                //Initialize the new Container
                condition.conditionContainer[condition.conditionContainer.Length - 1] = new AnimatorCondition();
                parameterListVisualElement.reloadFields();
            })
            { text = "+ Add" };


            controlsContainer.Add(addContainerButton);
            addContainerButton.style.width = 60;

            //Debug to Find Container Positions
            // controlsContainer.Add(new Label("controlsContainer"));
            // debugContainer.Add(new Label("debugContainer"));
            // rightTitleContainer.Add(new Label("rightTitleContainer"));
            // topPortContainer.Add(new Label("topPortContainer"));
            // bottomPortContainer.Add(new Label("bottomPortContainer"));
            //inputContainerElement.Add(new Label("inputContainerElement"));
        }


    }


}