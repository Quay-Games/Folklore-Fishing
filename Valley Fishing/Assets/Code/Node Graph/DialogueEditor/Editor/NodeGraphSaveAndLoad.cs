using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class NodeGraphSaveAndLoad {
		private List<Edge> edges => graphView.edges.ToList();
		private List<BaseNode> nodes => graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();

		private NodeGraphView graphView;

		public NodeGraphSaveAndLoad(NodeGraphView graphView) {
			this.graphView = graphView;
		}

		public void Save(NodeGraphDataContainer dialogueContainer) {
			List<NodeGraphDataContainer.NodeLinkData> nodeLinks = GetNodeLinks();
			List<BaseData> nodeData = nodes.Select(node => node.Save()).ToList();

			dialogueContainer.NodeLinkDatas = nodeLinks;
			dialogueContainer.NodeDatas = nodeData;
			EditorUtility.SetDirty(dialogueContainer);
			AssetDatabase.SaveAssets();
		}

		public void Load(NodeGraphDataContainer dialogueContainer) {
			ClearGraph();
			GenerateNodes(dialogueContainer);
			ConnectNodes(dialogueContainer);
		}

		#region Save
		private List<NodeGraphDataContainer.NodeLinkData> GetNodeLinks()
		{
			List<NodeGraphDataContainer.NodeLinkData> nodeLinks = new List<NodeGraphDataContainer.NodeLinkData>();
			foreach (Edge edge in edges.Where(edge => edge.input.node != null))
			{
				BaseNode outputNode = (BaseNode)edge.output.node;
				BaseNode inputNode = (BaseNode)edge.input.node;

				nodeLinks.Add(new NodeGraphDataContainer.NodeLinkData
				{
					BaseNodeGuid = outputNode.NodeGuid,
					BasePortName = edge.output.portName,
					TargetNodeGuid = inputNode.NodeGuid,
					TargetPortName = edge.input.portName,
				});
			}
			return nodeLinks;
		}

		#endregion

		#region Load

		private void ClearGraph() {
			edges.ForEach(edge => graphView.RemoveElement(edge));
			foreach (BaseNode node in nodes) {
				graphView.RemoveElement(node);
			}
		}

		private void GenerateNodes(NodeGraphDataContainer dialogueContainer) {
			foreach(BaseData data in dialogueContainer.NodeDatas)
			{
				graphView.CreateNodeFromData(data);
            }
		}

		private void ConnectNodes(NodeGraphDataContainer dialogueContainer) {
			// Make connection for all node.
			for (int i = 0; i < nodes.Count; i++) {
				List<NodeGraphDataContainer.NodeLinkData> connections = dialogueContainer.NodeLinkDatas.Where(edge => edge.BaseNodeGuid == nodes[i].NodeGuid).ToList();

				List<Port> allOutputPorts = nodes[i].outputContainer.Children().Where(x => x is Port).Cast<Port>().ToList();

				for (int j = 0; j < connections.Count; j++) {
					string targetNodeGuid = connections[j].TargetNodeGuid;
					BaseNode targetNode = nodes.First(node => node.NodeGuid == targetNodeGuid);

					if (targetNode == null)
						continue;

					foreach (Port item in allOutputPorts) {
						if (item.portName == connections[j].BasePortName) {
							LinkNodesTogether(item, (Port)targetNode.inputContainer[0]);
						}
					}
				}
			}
		}

		private void LinkNodesTogether(Port outputPort, Port inputPort) {
			Edge tempEdge = new Edge() {
				output = outputPort,
				input = inputPort
			};
			tempEdge.input.Connect(tempEdge);
			tempEdge.output.Connect(tempEdge);
			graphView.Add(tempEdge);
		}

		#endregion
	}
}
