using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

namespace Project.DialogueEditor {
	public class NodeGraphView : GraphView {
		private string graphViewStyleSheetName = "USS/GraphView/GraphViewStyleSheet";
		private NodeGraphEditorWindow editorWindow;
		private NodeSearchWindow searchWindow;

		public NodeGraphView(NodeGraphEditorWindow editorWindow) {
			this.editorWindow = editorWindow;
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

			StyleSheet tmpStyleSheeet = Resources.Load<StyleSheet>(graphViewStyleSheetName);
			styleSheets.Add(tmpStyleSheeet);

			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
			this.AddManipulator(new FreehandSelector());

			GridBackground grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();

			AddSearchWindow();
		}

		private void AddSearchWindow() {
			searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
			searchWindow.Configure(editorWindow, this);
			nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
		}

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
			List<Port> compatiblePorts = new List<Port>();
			Port startPortView = startPort;

			ports.ForEach((port) => {
				Port portView = port;
				if (startPortView != portView && startPortView.node != portView.node && startPortView.direction != port.direction) {
					compatiblePorts.Add(port);
				}
			});

			return compatiblePorts;
		}

		public void ReloadLanguage() {
			List<BaseNode> allNodes = nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();
			foreach (BaseNode node in allNodes) {
				node.ReloadLanguage();
			}
		}

		public bool CreateEmptyNodeOfType(BaseNode searchResultNode, Vector2 pos)
		{
			BaseNode nodeToCreate = Activator.CreateInstance(searchResultNode.GetType(), pos, editorWindow, this, null) as BaseNode;
			if (nodeToCreate != null)
			{
                AddElement(nodeToCreate);
                return true;
            } else
			{
				return false;
			}
		}
		
		public void CreateNodeFromData(BaseData data) 
		{
			BaseNode node = null;

			var mydata = data.GetType().GetCustomAttributes(typeof(NodeDataTypeAttribute), false);
			Type dataType = data.GetType();
			var nodeTypes = TypeCache.GetTypesDerivedFrom<BaseNode>();
			for(int i = 0; i < nodeTypes.Count; i++)
			{
                var nodeType = nodeTypes[i];
                var attributes = nodeType.GetCustomAttributes(typeof(NodeDataTypeAttribute), false);
                foreach (var attribute in attributes)
                {
                    if (attribute is NodeDataTypeAttribute nodeDataTypeAttribute)
                    {
                        if (nodeDataTypeAttribute.DataType == dataType)
                        {
                            node = Activator.CreateInstance(nodeType, new Vector2(), editorWindow, this, data) as BaseNode;
                            break;
                        }
                    }
                }
                if (node != null)
                {
                    break;
                }
            }
			if (node != null)
			{
                this.AddElement(node);
            }
        }
	}
}
