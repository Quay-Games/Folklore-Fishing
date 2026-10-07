using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Project.DialogueEditor {
	//A class whose job is to oversee the running of the node graph, but not control the behavior 
	//of individual nodes.
	public class NodeGraphRunner : NodeGraphGetData, IDialogueRuntime {

		#region Serialized Fields

		[SerializeField] private TMP_Text speakerName;

		#endregion


		#region Properties
		private BaseData CurrentNodeData { get; set; }
		private List<DialogueData_BaseContainer> BaseContainers { get; set;	} = new List<DialogueData_BaseContainer>();
		//[field:SerializeField]
		//public bool DialogueStarted { get; set;	}
		//private int CurrentIndex { get;	set; } = 0;
		//private string CurrentText { get; set; }
        //public MonoBehaviour Runner { get => this; }


        #endregion



        public void Start() {
			CurrentNodeData = dialogueContainer.getStartData();
			if(CurrentNodeData != null)
			{
				RunNodeGraph();
			}

		}

		private void RunNodeGraph()
		{
			bool running = true;
            while(running)
			{
				try
				{
                    this.CurrentNodeData.Run(this);
                    running = ChooseNextNode();
                } catch
				{
					Debug.LogError($"Dialogue '{dialogueContainer.name}' encountered an error while running node '{CurrentNodeData?.NodeGuid}'.");
					running = false;
                }
			}
		}

        //the idea here is to set the next instance of currentnode
        private bool ChooseNextNode()
		{
			//branch types will have a unique way of choosing the next node
			if(this.CurrentNodeData is BranchingTypeData)
			{
				CurrentNodeData = GetNodeByGuid((this.CurrentNodeData as BranchingTypeData).GetChosenNodeGuid());
			} else if(this.CurrentNodeData is EndData)
			{
                switch ((CurrentNodeData as EndData).EndNodeType.Value)
                {
					//why is this an option? repeating the end node over and over seems pointless
                    //case EndNodeType.Repeat:
                    //    RunNextNode(GetNodeByGuid(CurrentNode.NodeGuid));
                    //    return true;
                    case EndNodeType.ReturnToStart:
						CurrentNodeData = dialogueContainer.getStartData();
						break;
                    default:
                        break;
                }
            } else
			{
				CurrentNodeData = GetNextNode(CurrentNodeData);
            }
			if(CurrentNodeData != null)
            {
                return true;
            } else
			{
                return false;
            }
        }

		//public void Continue(int option) {
		//	if (this.CurrentNode is ResponseData responceData) {
		//		if (option == 0) {
		//			RunCurrentNode(GetNodeByGuid(responceData.branchOneNodeGuid));
		//		} else {
		//			RunCurrentNode(GetNodeByGuid(responceData.branchTwoNodeGuid));
		//		}
		//	} else {
		//		RunCurrentNode(GetNextNode(this.CurrentNode));
		//	}
		//}

        //could be moved elsewhere
        //private void RunNode(StartData nodeData) {
        //	CheckNodeType(GetNextNode(dialogueContainer.getStartData()));
        //}

        //could be moved elsewhere
  //      private void RunNode(ListenData nodeData) {
		//	nodeData.Container_ListenEventSOs[0].ListenEventSO.OnEventTriggered += ListenEventTriggered;
		//	nodeData.Container_ListenEventSOs[0].ListenEventSO.RunEvent(this);
		//}
		
		//could be moved elsewhere
		//private void RunNode(BranchData nodeData) {
		//	bool checkBranch = true;
		//	foreach (EventData_StringCondition item in nodeData.EventData_StringConditions) {
		//		if (!GameEvents.Instance.DialogueConditionEvents(item.StringEventText.Value, item.StringEventConditionType.Value, item.StringEventValue.Value)) {
		//			checkBranch = false;
		//			break;
		//		}
		//	}

		//	string nextNoce = (checkBranch ? nodeData.branchOneNodeGuid : nodeData.branchTwoNodeGuid);
		//	RunCurrentNode(GetNodeByGuid(nextNoce));
		//}

		//could be moved elsewhere
		//private void RunNode(NPCDialogueData nodeData) {
		//	AudioManager.Instance.PlayVoiceOver(nodeData.VoiceEvent);
		//	DialogueTextData tmp = nodeData.DialogueText;
		//	//this.CurrentText = tmp.Text.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType;
		//}

        //so this is always true??????
        //could be moved elsewhere
  //      private void RunNode(ConditionData nodeData) {
		//	bool checkBranch = true;
		//	string nextNoce = (checkBranch ? nodeData.branchOneNodeGuid : nodeData.branchTwoNodeGuid);
		//	RunCurrentNode(GetNodeByGuid(nextNoce));
		//}

		//a bit hard to move this, it uses functions from this class
		//private void RunNode(EventData nodeData) {
		//	switch (nodeData.EventType) {
		//		case EventType.None:
		//			break;
		//		case EventType.StartObjective:
		//			break;
		//		case EventType.FinishObjective:
		//			break;
		//		case EventType.StartChallenge:
		//			return;
		//		case EventType.FinishChallenge:
		//			break;
		//		case EventType.NpcEvent:
		//			break;
		//		case EventType.GiveMoney:
		//			break;
		//		default:
		//			break;
		//	}
		//	RunCurrentNode(GetNextNode(nodeData));
		//}

		//does this even do anything??
		//private void RunNode(ResponseData nodeData) {
		//	CurrentNode = nodeData;
		//	List<ResponseData_Text> tmp = new List<ResponseData_Text>(nodeData.ResponseData_Texts);
		//	List<string> texts = new List<string>();
		//	for (int i = 0; i < tmp.Count; i++) {
		//		texts.Add(tmp[i].Text.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
		//	}
		//}

		//private void RunNode(EndData nodeData) {
		//	switch (nodeData.EndNodeType.Value) {
		//		case EndNodeType.End:
		//			//EndDialogue();
		//			break;
		//		case EndNodeType.Repeat:
		//			RunCurrentNode(GetNodeByGuid(CurrentNode.NodeGuid));
		//			break;
		//		case EndNodeType.ReturnToStart:
		//			RunCurrentNode(GetNextNode(dialogueContainer.getStartData()));
		//			break;
		//		default:
		//			break;
		//	}
		//}

		//private void RunNode(DialogueData nodeData) {
		//	BaseContainers = new List<DialogueData_BaseContainer>();
		//	BaseContainers.AddRange(nodeData.DialogueData_Names);
		//	BaseContainers.AddRange(nodeData.DialogueData_Texts);

		//	BaseContainers.Sort(delegate (DialogueData_BaseContainer x, DialogueData_BaseContainer y) {
		//		return x.ID.Value.CompareTo(y.ID.Value);
		//	});

		//	DialogueToDo();
		//}

		//private void DialogueToDo() {
		//	for (int i = 1; i < BaseContainers.Count; i++) {
		//		if (BaseContainers[i] is DialogueData_Name) {
		//			DialogueData_Name tmp = BaseContainers[i] as DialogueData_Name;
		//			speakerName.text = tmp.CharacterName.Value;
		//		}
		//		if (BaseContainers[i] is DialogueData_Text) {
		//			DialogueData_Text tmp = BaseContainers[i] as DialogueData_Text;
		//			//dialogueText.text = tmp.Text.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType;
		//			break;
		//		}
		//	}
		//}

		//refactor this behavior into run methods returning booleans on wether to keep running
		//public void EndDialogue() {
		//	this.DialogueStarted = false;		
		//}

		//private void ListenEventTriggered() {
		//	ListenData listenData = CurrentNode as ListenData;
		//	listenData.Container_ListenEventSOs[0].ListenEventSO.OnEventTriggered -= ListenEventTriggered;
		//	StopAllCoroutines();
		//}

		void IDialogueRuntime.StopCoroutines()
		{
            StopAllCoroutines();
        }

        MonoBehaviour IDialogueRuntime.GetMonoBehaviour()
		{
            return this;
        }

		//the below function seems pointless because basecontainers is not used, but im keeping it here to keep things consistent
        public void SetBaseContainers(List<DialogueData_BaseContainer> containers)
        {
            BaseContainers = containers;
        }

        public void SetSpeakerName(string name)
        {
			speakerName.text = name;
        }
    }
}
