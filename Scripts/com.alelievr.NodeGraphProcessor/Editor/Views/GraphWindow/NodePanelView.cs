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

	//Draggable Label to create Nodes using Add

	public class DraggableLabel : Label
	{
		private Type nodeType;

		public DraggableLabel(string text, Type nodeType) : base(text)
		{
			this.nodeType = nodeType;
			RegisterCallback<MouseDownEvent>(OnMouseDown);
			RegisterCallback<MouseMoveEvent>(OnMouseMove);
			RegisterCallback<MouseUpEvent>(OnMouseUp);
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
	}



	public class NodePanelView : PinnedElementView
	{
		BaseGraphProcessor processor;

		//Nodelist
		IEnumerable<(string path, System.Type type)> nodeList;
		BaseGraphView graphView;

		private bool showNames = false;

		//Dict for getting icons NodeType to get the Path Text
		private Dictionary<Type, string> nodeTypeToPath = new Dictionary<Type, string>(){
			{typeof(BuildNode), "sv_label_3"},
			{typeof(ANPUA_Condition), "d_AnimatorStateTransition Icon"},
			{typeof(JumpNode), "d_AnyStateNode Icon"},
			{typeof(LoopNode), "TestFailed"}
		};


		public NodePanelView()
		{
			title = "NodePanel";
			scrollable = true;

			//Drag events

		}

		protected override void Initialize(BaseGraphView graphView)
		{
			processor = new ProcessGraphProcessor(graphView.graph);

			this.graphView = graphView;

			graphView.computeOrderUpdated += processor.UpdateComputeOrder;

			Button b = new Button(OnHelp)
			{
				text = "",
				style =
						{
							width = 17,
							height = 17,
								  backgroundImage = new StyleBackground((Texture2D)EditorGUIUtility.IconContent( "d__Help@2x").image),
							paddingBottom = 5,
							marginBottom = 5,
						}

			};
			header.Add(b);


			LoadNodes();
			GenerateUI();


			//Callbacks
			RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
			RegisterCallback<DragPerformEvent>(OnDragPerformEvent);



		}

		//Load Nodes
		public void LoadNodes()
		{
			nodeList = NodeProvider.GetNodeMenuEntries();
		}

		// Generate UI
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


					DraggableLabel nodeButton;
					if (showNames)
					{
						nodeButton = new DraggableLabel("", node.Item2)
						{
							text = name,
							tooltip = name,
							style =
						{
							marginBottom = 3,
							paddingLeft = 3,
							paddingRight = 3,
							paddingTop = 3,

						}
						};
					}
					else
					{
						nodeButton = new DraggableLabel("", node.Item2)
						{
							tooltip = name,


							style =
						{
							width = 50,
							height = 50,
								  backgroundImage = new StyleBackground((Texture2D)EditorGUIUtility.IconContent( getIconPath(node.Item2)).image),
						}
						};
					}



					// Add the node button to the current row container
					if (showNames)
					{

						AddToRowContainer(parent, nodeButton);
					}
					else
					{
						parent.Add(nodeButton);
					}
				}
				else if (kvp.Value is Dictionary<string, object> subHierarchy)
				{
					// Create a box container for the group or subgroup
					var box = new Box
					{
						style =
						{
							marginBottom = 3,
							paddingLeft = 3,
							paddingRight = 3,
							paddingTop = 3,
							paddingBottom = 3,
							borderLeftWidth = 1,
							borderRightWidth = 1,
							borderTopWidth = 1,
							borderBottomWidth = 1,
							//BackgroundColor
							backgroundColor = new Color(0.02f, 0.02f, 0.02f, 0.25f)

						}
					};

					// Add a title for the group or subgroup
					var title = new Label(kvp.Key)
					{
						style =
						{
							unityFontStyleAndWeight = FontStyle.Bold,
							fontSize = level == 0 ? 14 : 10,
							marginBottom = 10
						}
					};
					//box.Add(title);

					// Create a new container for the subgroup
					var subgroupContainer = new VisualElement
					{
						style =
						{
							flexDirection = FlexDirection.Row,
							flexWrap = Wrap.Wrap,
							marginBottom = 5
						}
					};
					box.Add(subgroupContainer);

					// Add the box to the parent container
					parent.Add(box);

					// Recursively generate UI for the subgroup
					GenerateUIRecursive(subHierarchy, subgroupContainer, level + 1);
				}
			}
		}
		public string getIconPath(Type type)
		{
			//use dictionary to get the path
			//check if the type is in the dictionary
			// if (nodeTypeToPath.ContainsKey(type))
			// 	return nodeTypeToPath[type];

			//using following path for now:
			//\Assets\VRC-Unity-Animatorgraph-\Scripts\com.alelievr.NodeGraphProcessor\Editor\Views

			//Get folderpath
			string folderPath = "Assets/VRC-Unity-Animatorgraph-/Scripts/com.alelievr.NodeGraphProcessor/Editor/Views/NodePanelIcons/";

			//Get the name of the Node
			string name = type.Name;
			//Debug.Log(name); 



			//Use Unity to try finding the Icon at the relative path name.png
			Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>(folderPath + name + ".png");
			if (icon != null)
			{
				//Debug.Log("Icon Found");
				return folderPath + name + ".png";
			}
			else
			{
				Texture2D icon2 = AssetDatabase.LoadAssetAtPath<Texture2D>(folderPath + "NONE" + ".png");
				if (icon2 != null)
				{
					//Debug.Log("Icon Found");
					return folderPath + "NONE" + ".png";
				}
				else
				{
					//Debug.Log("Icon Not Found");
					return "TestFailed";
				}
			}


		}

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

				// Check if the release position is within the graph's window
				if (graphView.contentViewContainer.ContainsPoint(graphPosition))
				{
					Debug.Log($"Node type: {nodeType}, Release position: {graphPosition}");
				}

				DragAndDrop.AcceptDrag();
				DragAndDrop.SetGenericData("NodeType", null);
				evt.StopPropagation();
			}
		}

		void OnHelp()
		{
			showNames = !showNames;
			//Reload the UI
			content.Clear();
			GenerateUI();

		}

		// Callbacks
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
				var graphPosition = graphView.contentViewContainer.WorldToLocal(mousePosition);

				// Create the node at the drop position
				graphView.AddNode(BaseNode.CreateFromType(nodeType, graphPosition));

				DragAndDrop.AcceptDrag();
				DragAndDrop.SetGenericData("NodeType", null);
				evt.StopPropagation();
			}
		}


	}
}
