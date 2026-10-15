using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Dialogue/New Dialoguee")]
[System.Serializable]
public class NodeGraphDataContainer : ScriptableObject {
    public List<NodeLinkData> NodeLinkDatas = new List<NodeLinkData>();
	[SerializeReference]
	public List<BaseData> NodeDatas = new List<BaseData>();

	public StartData getStartData ()
	{
        foreach (BaseData data in NodeDatas)
        {
            if (data is StartData startData)
            {
                return startData;
            }
        }
        return null;
    }

	[System.Serializable]
	public class NodeLinkData {
		public string BaseNodeGuid;
		public string BasePortName;
		public string TargetNodeGuid;
		public string TargetPortName;
	}
}
