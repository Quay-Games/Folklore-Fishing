using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.DialogueEditor {
	//A class whose purpose is to expose methods for getting nodes from the node graph
	public class NodeGraphGetData : MonoBehaviour {

		[SerializeField] protected DialogueContainer dialogueContainer;
		protected BaseData GetNodeByGuid(string targetNodeGuid) {
            //some null checks from Codex that I may remove, makes the method kinda bloated
            if (dialogueContainer == null || string.IsNullOrEmpty(targetNodeGuid)) {
				return null;
			}

			if (dialogueContainer.NodeDatas == null) {
				Debug.LogWarning($"Dialogue '{dialogueContainer.name}' has no node data.");
				return null;
			}

			BaseData nodeData = dialogueContainer.NodeDatas.Find(node => node != null && node.NodeGuid == targetNodeGuid);
			if (nodeData == null) {
				Debug.LogWarning($"Dialogue '{dialogueContainer.name}' could not find node '{targetNodeGuid}'.");
			}

			return nodeData;
		}

		protected BaseData GetNodeByNodePort(DialogueData_Port nodePort) {
			if (nodePort == null) {
				return null;
			}

			return GetNodeByGuid(nodePort.InputGuid);
		}
		protected BaseData GetNextNode(BaseData baseNodeData) {
			//some null checks from Codex that I may remove

			if (dialogueContainer.NodeLinkDatas == null) {
				Debug.LogWarning($"Dialogue '{dialogueContainer.name}' has no link data.");
				return null;
			}

			DialogueContainer.NodeLinkData nodeLinkData = dialogueContainer.NodeLinkDatas.Find(edge => edge.BaseNodeGuid == baseNodeData.NodeGuid);
			if (nodeLinkData == null || string.IsNullOrEmpty(nodeLinkData.TargetNodeGuid)) {
				Debug.LogWarning($"Dialogue '{dialogueContainer.name}' has no outgoing link from {baseNodeData.GetType().Name} '{baseNodeData.NodeGuid}'.");
				return null;
			}

			return GetNodeByGuid(nodeLinkData.TargetNodeGuid);
		}

	}
}
