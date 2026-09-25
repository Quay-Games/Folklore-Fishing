using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Project.DialogueEditor {
	public class DialogueTalk : DialogueGetData {

		#region Serialized Fields

		[SerializeField] private TMP_Text speakerName;

		#endregion


		#region Properties
		private BaseData CurrentNode { get; set; }
		private List<DialogueData_BaseContainer> BaseContainers { get; set;	} = new List<DialogueData_BaseContainer>();
		[field:SerializeField]
		public bool DialogueStarted { get; set;	}
		private int CurrentIndex { get;	set; } = 0;
		private string CurrentText { get; set; }

		#endregion



		public void Start() {
			StartDialogue();
		}

		public void StartDialogue() {
			if (this.DialogueStarted) {
				return;
			}
			this.DialogueStarted = true;
			CheckNodeType(GetNextNode(dialogueContainer.StartDatas[0]));
		}

		public void Continue(int option) {
			if (this.CurrentNode is ResponseData responceData) {
				if (option == 0) {
					CheckNodeType(GetNodeByGuid(responceData.FirstOptionGuid));
				} else {
					CheckNodeType(GetNodeByGuid(responceData.SecondOptionGuid));
				}
			} else {
				CheckNodeType(GetNextNode(this.CurrentNode));
			}
		}

		private void CheckNodeType(BaseData _baseNodeData) {
			switch (_baseNodeData) {
				case StartData nodeData:
					RunNode(nodeData);
					break;
				case DialogueData nodeData:
					RunNode(nodeData);
					break;
				case EventData nodeData:
					RunNode(nodeData);
					break;
				case EndData nodeData:
					RunNode(nodeData);
					break;
				case BranchData nodeData:
					RunNode(nodeData);
					break;
				case NPCDialogueData nodeData:
					RunNode(nodeData);
					break;
				case ConditionData nodeData:
					RunNode(nodeData);
					break;
				case ResponseData nodeData:
					RunNode(nodeData);
					break;
				case ListenData nodeData:
					RunNode(nodeData);
					break;
				default:
					break;
			}
		}

		private void RunNode(StartData nodeData) {
			CheckNodeType(GetNextNode(dialogueContainer.StartDatas[0]));
		}

		private void RunNode(ListenData nodeData) {
			CurrentNode = nodeData;
			nodeData.Container_ListenEventSOs[0].ListenEventSO.OnEventTriggered += ListenEventTriggered;
			nodeData.Container_ListenEventSOs[0].ListenEventSO.RunEvent(this);
		}

		private void RunNode(BranchData nodeData) {
			bool checkBranch = true;
			foreach (EventData_StringCondition item in nodeData.EventData_StringConditions) {
				if (!GameEvents.Instance.DialogueConditionEvents(item.StringEventText.Value, item.StringEventConditionType.Value, item.StringEventValue.Value)) {
					checkBranch = false;
					break;
				}
			}

			string nextNoce = (checkBranch ? nodeData.trueGuidNode : nodeData.falseGuidNode);
			CheckNodeType(GetNodeByGuid(nextNoce));
		}

		private void RunNode(NPCDialogueData nodeData) {
			CurrentNode = nodeData;
			Debug.Log(nodeData.VoiceEvent);
			AudioManager.Instance.PlayVoiceOver(nodeData.VoiceEvent);
			DialogueTextData tmp = nodeData.DialogueText;
			//this.CurrentText = tmp.Text.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType;
			Continue(0);
		}

		private void RunNode(ConditionData nodeData) {
			bool checkBranch = true;
			string nextNoce = (checkBranch ? nodeData.trueGuidNode : nodeData.falseGuidNode);
			CheckNodeType(GetNodeByGuid(nextNoce));
		}

		private void RunNode(EventData nodeData) {
			switch (nodeData.EventType) {
				case EventType.None:
					break;
				case EventType.StartObjective:
					break;
				case EventType.FinishObjective:
					break;
				case EventType.StartChallenge:
					EndDialogue();
					return;
				case EventType.FinishChallenge:
					break;
				case EventType.NpcEvent:
					break;
				case EventType.GiveMoney:
					break;
				default:
					break;
			}
			CheckNodeType(GetNextNode(nodeData));
		}

		private void RunNode(ResponseData nodeData) {
			CurrentNode = nodeData;
			List<ResponseData_Text> tmp = new List<ResponseData_Text>(nodeData.ResponceData_Texts);
			List<string> texts = new List<string>();
			for (int i = 0; i < tmp.Count; i++) {
				texts.Add(tmp[i].Text.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
			}
		}

		private void RunNode(EndData nodeData) {
			switch (nodeData.EndNodeType.Value) {
				case EndNodeType.End:
					EndDialogue();
					break;
				case EndNodeType.Repeat:
					CheckNodeType(GetNodeByGuid(CurrentNode.NodeGuid));
					break;
				case EndNodeType.ReturnToStart:
					CheckNodeType(GetNextNode(dialogueContainer.StartDatas[0]));
					break;
				default:
					break;
			}
		}

		private void RunNode(DialogueData nodeData) {
			CurrentNode = nodeData;
			BaseContainers = new List<DialogueData_BaseContainer>();
			BaseContainers.AddRange(nodeData.DialogueData_Names);
			BaseContainers.AddRange(nodeData.DialogueData_Texts);

			CurrentIndex = 0;

			BaseContainers.Sort(delegate (DialogueData_BaseContainer x, DialogueData_BaseContainer y) {
				return x.ID.Value.CompareTo(y.ID.Value);
			});

			DialogueToDo();
		}

		private void DialogueToDo() {
			for (int i = CurrentIndex; i < BaseContainers.Count; i++) {
				CurrentIndex = i + 1;
				if (BaseContainers[i] is DialogueData_Name) {
					DialogueData_Name tmp = BaseContainers[i] as DialogueData_Name;
					speakerName.text = tmp.CharacterName.Value;
				}
				if (BaseContainers[i] is DialogueData_Text) {
					DialogueData_Text tmp = BaseContainers[i] as DialogueData_Text;
					//dialogueText.text = tmp.Text.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType;
					break;
				}
			}
		}

		public void EndDialogue() {
			this.DialogueStarted = false;		
		}

		private void ListenEventTriggered() {
			ListenData listenData = CurrentNode as ListenData;
			listenData.Container_ListenEventSOs[0].ListenEventSO.OnEventTriggered -= ListenEventTriggered;
			StopAllCoroutines();
			Continue(0);
		}
	}
}
