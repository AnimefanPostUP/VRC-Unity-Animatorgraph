using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Graphs;
using UnityEditor;
using System;
using System.Linq;

namespace GraphProcessor
{

	
	public class ANPUA_NodePanelView : PinnedElementView
	{
		BaseGraphProcessor processor;

		//Nodelist
		IEnumerable<(string path, System.Type type)> nodeList;
		BaseGraphView graphView;

		//Dict for getting icons NodeType to get the Path Text
		private Dictionary<Type, string> nodeTypeToPath = new Dictionary<Type, string>(){
			{typeof(ANPUA_StartNode), "sv_label_3"},
			{typeof(ANPUA_Condition), "d_AnimatorStateTransition Icon"},
			{typeof(ANPUA_NodeJump), "d_AnyStateNode Icon"},
			{typeof(ANPUA_NodeForLoop), "TestFailed"}
		};


		public ANPUA_NodePanelView()
		{
			title = "NodePanel";
		}

		protected override void Initialize(BaseGraphView graphView)
		{
			processor = new ProcessGraphProcessor(graphView.graph);

			this.graphView = graphView;

			graphView.computeOrderUpdated += processor.UpdateComputeOrder;

			//Button b = new Button(OnPlay) { name = "ActionButton", text = "Play !" };
			//content.Add(b);

			LoadNodes();
			GenerateUI();
			//Create a Grid with the nodes

		}

		//Load Nodes
		public void LoadNodes()
		{
			nodeList = NodeProvider.GetNodeMenuEntries();
		}

		// Generate UI
		public void GenerateUI()
		{
			// Create a dictionary to hold the hierarchical structure
			var groups = new Dictionary<string, object>();

			// Parse the paths and organize nodes into a hierarchical structure
			foreach (var node in nodeList)
			{
				var parts = node.path.Split('/');
				AddNodeToHierarchy(groups, parts, 0, node);
			}

			// Generate the UI elements recursively
			GenerateUIRecursive(groups, content, 0);
		}

		// Recursive method to add nodes to the hierarchical structure
        private void AddNodeToHierarchy(Dictionary<string, object> hierarchy, string[] parts, int index, (string path, Type type) node)
        {
            if (index >= parts.Length)
                return;

            if (!hierarchy.ContainsKey(parts[index]))
            {
                if (index == parts.Length - 1)
                {
                    hierarchy[parts[index]] = node;
                }
                else
                {
                    hierarchy[parts[index]] = new Dictionary<string, object>();
                }
            }

            if (hierarchy[parts[index]] is Dictionary<string, object> subHierarchy)
            {
                AddNodeToHierarchy(subHierarchy, parts, index + 1, node);
            }
        }

		// Recursive method to generate UI
		// Recursive method to generate UI

		public string getIconPath(Type type) {
			//use dictionary to get the path
			//check if the type is in the dictionary
			if (nodeTypeToPath.ContainsKey(type)) 
				return nodeTypeToPath[type];
			// unity icon
			return "d__Help";
		}
	

        private void GenerateUIRecursive(Dictionary<string, object> hierarchy, VisualElement parent, int level)
        {
            foreach (var kvp in hierarchy)
            {
                 if (kvp.Value is ValueTuple<string, Type> node)
                {

					string path = node.Item1;
					//remove path
					string[] parts = path.Split('/');
					string name = parts[parts.Length - 1];
					
                    // Create a button for the node
                    Label nodeButton = new Label(
                    )
                    {
                        text = "",

						//Display name over hover without the path, but just the name like "name" not "path/name"
						tooltip = name,

                        style =
                        {
                            width = 40,
                            height = 40,
                            backgroundImage = new StyleBackground((Texture2D)EditorGUIUtility.IconContent( getIconPath(node.Item2)).image),
                        }
                    };

                    // Add drag-and-drop handlers
                    nodeButton.RegisterCallback<MouseDownEvent>(evt => OnMouseDown(evt, node.Item2));
                    nodeButton.RegisterCallback<MouseMoveEvent>(OnMouseMove); 
                    nodeButton.RegisterCallback<MouseUpEvent>(OnMouseUp);

                    // Add the node button to the current row container
                    AddToRowContainer(parent, nodeButton);
                }
                else if (kvp.Value is Dictionary<string, object> subHierarchy)
                {
                    // Add a title for the group or subgroup
                    var title = new Label(kvp.Key)
                    {
                        style =
                        {
                            unityFontStyleAndWeight = FontStyle.Bold,
                            fontSize = level == 0 ? 16 : 14,
                            marginBottom = 2,
							flexWrap = Wrap.Wrap
                        }
                    };
                    parent.Add(title);

                    // Create a new container for the subgroup
                    var subgroupContainer = new VisualElement
                    { 
                        style =
                        {
                            flexDirection = FlexDirection.Column,
                            marginBottom = 10
                        }
                    };
                    parent.Add(subgroupContainer);

                    // Recursively generate UI for the subgroup
                    GenerateUIRecursive(subHierarchy, subgroupContainer, level + 1);
                }
            }
        }
		// Method to add elements to row containers


 // Method to add elements to row containers
        private void AddToRowContainer(VisualElement parent, VisualElement element)
        {
            // Get the last row container or create a new one if necessary
            VisualElement rowContainer = parent.Children().LastOrDefault() as VisualElement;
            if (rowContainer == null || rowContainer.childCount >= 5)
            {
                rowContainer = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
						flexWrap = Wrap.Wrap,
                        marginBottom = 5
                    }
                };
                parent.Add(rowContainer);
            }

            // Add the element to the row container
            rowContainer.Add(element);
        }

        // Drag-and-drop handlers
        private void OnMouseDown(MouseDownEvent evt, Type nodeType)
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
                var nodeType = DragAndDrop.GetGenericData("NodeType") as Type;
                var mousePosition = evt.mousePosition;
                var graphPosition = graphView.contentViewContainer.WorldToLocal(mousePosition);

				var center = graphView.contentViewContainer.layout.center;

                // Check if the release position is within the graph's window
                if (graphView.contentViewContainer.ContainsPoint(graphPosition))
                {
                    graphView.AddNode(BaseNode.CreateFromType(nodeType, center));
                }

                DragAndDrop.AcceptDrag();
                DragAndDrop.SetGenericData("NodeType", null);
                evt.StopPropagation();
            }
        }

		void OnPlay()
		{
			processor.Run();
		}
	}
}
