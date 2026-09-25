using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
//this class seems to do lots of operations on the dialogcontainer, but doesnt actually store a reference to it?

namespace Project.DialogueEditor {
	public class DialogueSaveAndLoad {
		private List<Edge> edges => graphView.edges.ToList();
		private List<BaseNode> nodes => graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();

		private DialogueGraphView graphView;

		public DialogueSaveAndLoad(DialogueGraphView graphView) {
			this.graphView = graphView;
		}

		public void Save(DialogueContainer dialogueContainer) {
			SaveEdges(dialogueContainer);
			SaveNodes(dialogueContainer);

			EditorUtility.SetDirty(dialogueContainer);
			AssetDatabase.SaveAssets();
		}

		public void Load(DialogueContainer dialogueContainer) {
			ClearGraph();
			GenerateNodes(dialogueContainer);
			ConnectNodes(dialogueContainer);
		}

		#region Save
		private void SaveEdges(DialogueContainer dialogueContainer) {
			dialogueContainer.NodeLinkDatas.Clear();

			Edge[] connectedEdges = edges.Where(edge => edge.input.node != null).ToArray();
			for (int i = 0; i < connectedEdges.Count(); i++) {
				BaseNode outputNode = (BaseNode)connectedEdges[i].output.node;
				BaseNode inputNode = connectedEdges[i].input.node as BaseNode;

				dialogueContainer.NodeLinkDatas.Add(new DialogueContainer.NodeLinkData {
					BaseNodeGuid = outputNode.NodeGuid,
					BasePortName = connectedEdges[i].output.portName,
					TargetNodeGuid = inputNode.NodeGuid,
					TargetPortName = connectedEdges[i].input.portName,
				});
			}
		}

		private void SaveNodes(DialogueContainer dialogueContainer) {
			dialogueContainer.EventDatas.Clear();
			dialogueContainer.NPCDialogueDatas.Clear();
			dialogueContainer.EndDatas.Clear();
			dialogueContainer.StartDatas.Clear();
			dialogueContainer.BranchDatas.Clear();
			dialogueContainer.DialogueDatas.Clear();
			dialogueContainer.ChoiceDatas.Clear();
			dialogueContainer.ConditionDatas.Clear();
			dialogueContainer.ResponceDatas.Clear();
			dialogueContainer.ListenDatas.Clear();

			dialogueContainer.NodeDatas.Clear();

			nodes.ForEach(node => {
				//the savenodedata function is hella overloaded, we need to make it on the nodedata class
				dialogueContainer.NodeDatas.Add(node.Save());
				switch (node) {
					case StartNode startNode:
						dialogueContainer.StartDatas.Add(SaveNodeData(startNode));
						break;
					case EndNode endNode:
						dialogueContainer.EndDatas.Add(SaveNodeData(endNode));
						break;
					case DialogueNode npcDialogueNode:
						dialogueContainer.NPCDialogueDatas.Add(SaveNodeData(npcDialogueNode));
						break;
					case ResponseNode responceNode:
						dialogueContainer.ResponceDatas.Add(SaveNodeData(responceNode));
						break;
					case EventNode npcEventNode:
						dialogueContainer.EventDatas.Add(SaveNodeData(npcEventNode));
						break;
					case ConditionNode npcConditionNode:
						dialogueContainer.ConditionDatas.Add(SaveNodeData(npcConditionNode));
						break;
					case BranchNode branchNode:
						dialogueContainer.BranchDatas.Add(SaveNodeData(branchNode));
						break;
						//case ListenNode listenNode:
						//dialogueContainer.ListenDatas.Add(SaveNodeData(listenNode));
						//break;
					default:
						break;
				}
			});
		}

		private NPCDialogueData SaveNodeData(DialogueNode node) {
			NPCDialogueData dialogueData = new NPCDialogueData {
				NodeGuid = node.NodeGuid,
				Position = node.GetPosition().position,
				VoiceEvent = node.DialogueData.VoiceEvent
			};
			Debug.Log(node.DialogueData.VoiceEvent);
			DialogueTextData tmp = node.DialogueData.DialogueText;

			if (tmp != null) {
				dialogueData.DialogueText = new DialogueTextData {
					ID = tmp.ID,
					GuidID = tmp.GuidID,
					Text = new List<LanguageGeneric<string>>(tmp.Text)
				};
			}

			return dialogueData;
		}

		private StartData SaveNodeData(StartNode node) {
			StartData nodeData = new StartData() {
				NodeGuid = node.NodeGuid,
				Position = node.GetPosition().position,
			};

			return nodeData;
		}

		private EndData SaveNodeData(EndNode node) {
			EndData nodeData = new EndData() {
				NodeGuid = node.NodeGuid,
				Position = node.GetPosition().position,
			};
			nodeData.EndNodeType.Value = node.EndData.EndNodeType.Value;

			return nodeData;
		}

		private EventData SaveNodeData(EventNode node) {
			EventData nodeData = new EventData() {
				NodeGuid = node.NodeGuid,
				Position = node.GetPosition().position,
				EventType = node.NPCEventData.EventType
			};

			nodeData.EventData_EventName.StringEventValue.Value =
				node.NPCEventData.EventData_EventName.StringEventValue.Value;

			return nodeData;
		}

		private BranchData SaveNodeData(BranchNode node) {
			List<Edge> tmpEdges = edges.Where(x => x.output.node == node).Cast<Edge>().ToList();

			Edge trueOutput = edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "True");
			Edge flaseOutput = edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "False");

			BranchData nodeData = new BranchData() {
				NodeGuid = node.NodeGuid,
				Position = node.GetPosition().position,
				trueGuidNode = (trueOutput != null ? (trueOutput.input.node as BaseNode).NodeGuid : string.Empty),
				falseGuidNode = (flaseOutput != null ? (flaseOutput.input.node as BaseNode).NodeGuid : string.Empty),
			};

			foreach (EventData_StringCondition stringEvents in node.BranchData.EventData_StringConditions) {
				EventData_StringCondition tmp = new EventData_StringCondition();
				tmp.StringEventValue.Value = stringEvents.StringEventValue.Value;
				tmp.StringEventText.Value = stringEvents.StringEventText.Value;
				tmp.StringEventConditionType.Value = stringEvents.StringEventConditionType.Value;

				nodeData.EventData_StringConditions.Add(tmp);
			}

			return nodeData;

		}

		private ConditionData SaveNodeData(ConditionNode node) {

			Edge trueOutput = edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "True");
			Edge falseOutput = edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "False");

			ConditionData nodeData = new ConditionData {
				NodeGuid = node.NodeGuid,
				Position = node.GetPosition().position,
				trueGuidNode = trueOutput != null ? (trueOutput.input.node as BaseNode)?.NodeGuid : string.Empty,
				falseGuidNode = falseOutput != null ? (falseOutput.input.node as BaseNode)?.NodeGuid : string.Empty,
			};

			nodeData.EventData_EventName.StringEventValue.Value =
			node.NPCConditionData.EventData_EventName.StringEventValue.Value;

			return nodeData;
		}
		//private ListenData SaveNodeData(ListenNode node) {
		//	ListenData nodeData = new ListenData() {
		//		NodeGuid = node.NodeGuid,
		//		Position = node.GetPosition().position,
		//	};

		//	// Save Dialogue Event
		//	foreach (Container_ListenEventSO dialogueEvent in node.ListenData.Container_ListenEventSOs) {
		//		nodeData.Container_ListenEventSOs.Add(dialogueEvent);
		//	}

		//	return nodeData;
		//}


		private ResponseData SaveNodeData(ResponseNode node) {

			List<Edge> tmpEdges = edges.Where(x => x.output.node == node).Cast<Edge>().ToList();

			Edge FirstOptionOutput = edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "Option 1");
			Edge SecondOptionOutput = edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "Option 2");


			ResponseData data = new ResponseData {
				NodeGuid = node.NodeGuid,
				Position = node.GetPosition().position,
				ResponceData_Texts = new List<ResponseData_Text>(),
				FirstOptionGuid = (FirstOptionOutput != null ? (FirstOptionOutput.input.node as BaseNode).NodeGuid : string.Empty),
				SecondOptionGuid = (SecondOptionOutput != null ? (SecondOptionOutput.input.node as BaseNode).NodeGuid : string.Empty),
			};

			// Assign unique IDs and store response texts
			for (int i = 0; i < node.ResponseData.ResponceData_Texts.Count; i++) {
				ResponseData_Text original = node.ResponseData.ResponceData_Texts[i];

				ResponseData_Text textCopy = new ResponseData_Text {
					ID = new Container_Int { Value = i },
					GuidID = original.GuidID,
					Text = new List<LanguageGeneric<string>>(original.Text)
				};

				data.ResponceData_Texts.Add(textCopy);
			}

			return data;
		}

		#endregion

		#region Load

		private void ClearGraph() {
			edges.ForEach(edge => graphView.RemoveElement(edge));
			foreach (BaseNode node in nodes) {
				graphView.RemoveElement(node);
			}
		}

		//this is what im working on at the moment, making the nodes themselves (or the data) own the loading of the data
		//I wanted to make it so that we can just loop over the whole types
		private void GenerateNodes(DialogueContainer dialogueContainer) {
			//this is what I want to achieve
			foreach(BaseData data in dialogueContainer.NodeDatas)
			{
				graphView.CreateNodeFromData(data);
            }


			// Start
			//foreach (StartData node in dialogueContainer.StartDatas) {
			//	StartNode tempNode = graphView.CreateStartNode(node.Position);
			//	tempNode.NodeGuid = node.NodeGuid;

			//	graphView.AddElement(tempNode);
			//}
			//Listen Node
			//foreach (ListenData node in dialogueContainer.ListenDatas) {
			//	//this needs to be replaced with a call to the contstructor of ListenNode
			//	ListenNode tempNode = graphView.CreateListenNode(node.Position);
			//	tempNode.NodeGuid = node.NodeGuid;

			//	foreach (Container_ListenEventSO item in node.Container_ListenEventSOs) {
			//		tempNode.AddScriptableEvent(item);
			//	}
			//	tempNode.LoadValueInToField();
			//	graphView.AddElement(tempNode);
			//}

			// End Node 
			//foreach (EndData node in dialogueContainer.EndDatas) {
			//	EndNode tempNode = graphView.CreateEndNode(node.Position);
			//	tempNode.NodeGuid = node.NodeGuid;
			//	tempNode.EndData.EndNodeType.Value = node.EndNodeType.Value;

			//	graphView.AddElement(tempNode);
			//}

			// NPCEvent Node
			//foreach (EventData node in dialogueContainer.EventDatas) {
			//	EventNode tempNode = graphView.CreateNPCEventNode(node.Position);
			//	tempNode.NodeGuid = node.NodeGuid;

			//	tempNode.NPCEventData = node;

			//	tempNode.TextLine(node.EventData_EventName, node.EventType);

			//	graphView.AddElement(tempNode);
			//}

			// NPCCondition Node
			//foreach (ConditionData node in dialogueContainer.ConditionDatas) {
			//	ConditionNode tempNode = graphView.CreateNPCConditionNode(node.Position);
			//	tempNode.NodeGuid = node.NodeGuid;
			//	tempNode.NPCConditionData = node;
			//	tempNode.TextLine(node.EventData_EventName);

			//	tempNode.ReloadLanguage();
			//	graphView.AddElement(tempNode);
			//}

			// Branch Node
			//foreach (BranchData node in dialogueContainer.BranchDatas) {
			//	BranchNode tempNode = graphView.CreateBranchNode(node.Position);
			//	tempNode.NodeGuid = node.NodeGuid;

			//	foreach (EventData_StringCondition item in node.EventData_StringConditions) {
			//		tempNode.AddCondition(item);
			//	}

			//	tempNode.ReloadLanguage();
			//	graphView.AddElement(tempNode);
			//}


			// Dialogue Node
			//foreach (NPCDialogueData node in dialogueContainer.NPCDialogueDatas) {
			//	DialogueNode tempNode = graphView.CreateNPCDialogueNode(node.Position);

			//	tempNode.NodeGuid = node.NodeGuid;
			//	tempNode.DialogueData.VoiceEvent = node.VoiceEvent;

			//	DialogueTextData textData = new DialogueTextData {
			//		GuidID = node.DialogueText.GuidID,
			//		Text = node.DialogueText.Text,
			//		ID = node.DialogueText.ID
			//	};

			//	tempNode.TextLine(textData);

			//	tempNode.EventReferenceBox();
			//	tempNode.ReloadLanguage();

			//	graphView.AddElement(tempNode);
			//}

			// Responce node
			//foreach (ResponseData node in dialogueContainer.ResponceDatas) {
			//	ResponseNode tempNode = graphView.CreateResponceNode(node.Position);
			//	tempNode.NodeGuid = node.NodeGuid;

			//	List<ResponseData_Text> textData = new List<ResponseData_Text>();
			//	if (node.ResponceData_Texts.Count > 0) {
			//		for (int i = 0; i < node.ResponceData_Texts.Count; i++) {
			//			ResponseData_Text tmp = new ResponseData_Text();
			//			tmp.GuidID = node.ResponceData_Texts[i].GuidID;
			//			tmp.Text = node.ResponceData_Texts[i].Text;
			//			tmp.ID = node.ResponceData_Texts[i].ID;
			//			textData.Add(tmp);
			//		}
			//		tempNode.TextLine(textData[0]);
			//		tempNode.TextLine(textData[1]);
			//	} else {
			//		tempNode.TextLine();
			//		tempNode.TextLine();
			//	}

			//	tempNode.ReloadLanguage();
			//	graphView.AddElement(tempNode);


				//what were these below????

				//if (!string.IsNullOrEmpty(node.FirstOptionGuid)) {
				//	dialogueContainer.NodeLinkDatas.Add(new DialogueContainer.NodeLinkData {
				//		BaseNodeGuid = node.NodeGuid,
				//		BasePortName = "Option 1",
				//		TargetNodeGuid = node.FirstOptionGuid
				//	});
				//}

				//if (!string.IsNullOrEmpty(node.SecondOptionGuid)) {
				//	dialogueContainer.NodeLinkDatas.Add(new DialogueContainer.NodeLinkData {
				//		BaseNodeGuid = node.NodeGuid,
				//		BasePortName = "Option 2",
				//		TargetNodeGuid = node.SecondOptionGuid
				//	});
				//}
			//}
		}

		private void ConnectNodes(DialogueContainer dialogueContainer) {
			// Make connection for all node.
			for (int i = 0; i < nodes.Count; i++) {
				List<DialogueContainer.NodeLinkData> connections = dialogueContainer.NodeLinkDatas.Where(edge => edge.BaseNodeGuid == nodes[i].NodeGuid).ToList();

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
