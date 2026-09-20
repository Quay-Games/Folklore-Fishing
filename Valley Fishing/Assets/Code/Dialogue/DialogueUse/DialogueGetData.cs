using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.DialogueEditor {
	public class DialogueGetData : MonoBehaviour {

		[SerializeField] protected DialogueContainer dialogueContainer;

		//Griff: replace alldatas with nodedatas when it is done
		protected BaseData GetNodeByGuid(string targetNodeGuid) {
			return dialogueContainer.AllDatas.Find(node => node.NodeGuid == targetNodeGuid);
		}

		protected BaseData GetNodeByNodePort(DialogueData_Port nodePort) {
			return dialogueContainer.AllDatas.Find(node => node.NodeGuid == nodePort.InputGuid);
		}

		protected BaseData GetNextNode(BaseData baseNodeData) {
			DialogueContainer.NodeLinkData nodeLinkData = dialogueContainer.NodeLinkDatas.Find(egde => egde.BaseNodeGuid == baseNodeData.NodeGuid);
			return GetNodeByGuid(nodeLinkData.TargetNodeGuid);
		}

	}
}
