using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Dialogue/New Dialoguee")]
[System.Serializable]
public class DialogueContainer : ScriptableObject {
    //my ambition here is to remove all of these lists and just have a single list of BaseData
    public List<NodeLinkData> NodeLinkDatas = new List<NodeLinkData>();
	public List<BaseData> NodeDatas = new List<BaseData>();

	public List<DialogueData> DialogueDatas = new List<DialogueData>();
	public List<NPCDialogueData> NPCDialogueDatas = new List<NPCDialogueData>();
	public List<ResponseData> ResponceDatas = new List<ResponseData>();
	public List<EndData> EndDatas = new List<EndData>();
	public List<StartData> StartDatas = new List<StartData>();
	public List<EventData> EventDatas = new List<EventData>();
	public List<ConditionData> ConditionDatas = new List<ConditionData>();
	public List<BranchData> BranchDatas = new List<BranchData>();
	public List<ChoiceData> ChoiceDatas = new List<ChoiceData>();
	public List<ListenData> ListenDatas = new List<ListenData>();

	public List<BaseData> AllDatas {
		get {
			List<BaseData> tmp = new List<BaseData>();
			tmp.AddRange(DialogueDatas);
			tmp.AddRange(NPCDialogueDatas);
			tmp.AddRange(ResponceDatas);
			tmp.AddRange(EndDatas);
			tmp.AddRange(StartDatas);
			tmp.AddRange(EventDatas);
			tmp.AddRange(ConditionDatas);
			tmp.AddRange(BranchDatas);
			tmp.AddRange(ChoiceDatas);
			tmp.AddRange(ListenDatas);

			return tmp;
		}
	}

	[System.Serializable]
	public class NodeLinkData {
		public string BaseNodeGuid;
		public string BasePortName;
		public string TargetNodeGuid;
		public string TargetPortName;
	}
}
