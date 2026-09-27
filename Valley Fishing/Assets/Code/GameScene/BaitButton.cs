using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaitButton : MonoBehaviour {

	#region Serialized Fields

	[SerializeField] private int baitIndex;
	[SerializeField] private BaitView baitView;
	[SerializeField] private ButtonVoiceOverComponent buttonVoiceOverComponent;
	[SerializeField] private TMP_Text baitCountText;

	#endregion


	#region Mono Behaviours
	
	public void Awake() {
		buttonVoiceOverComponent.SelectAction -= SelectBait;
		buttonVoiceOverComponent.SelectAction += SelectBait;
	}

	public void OnDestroy() {
		buttonVoiceOverComponent.SelectAction -= SelectBait;
	}

	public void OnEnable() {
		baitCountText.text = "x" + InventoryManager.Instance.OwnedBaitTypeDatas[baitIndex].quantity.ToString();
	}

	#endregion


	#region Public Methods

	public void SelectBait(ButtonType buttonType, ButtonVoiceOverComponent buttonVoiceOverComponent) {
		baitView.BaitSelected(baitIndex);
		if (!InventoryManager.Instance.BaitDatas.Datas[baitIndex].IsTutorial) {
			if (AudioManager.Instance.VoiceLineInProgress) {
				return;
			}
			List<EventReference> voiceOverChain = new List<EventReference>();
			voiceOverChain.Add(FMODManager.Instance.BaitNames[baitIndex]);
			for (int i = 0; i < FMODManager.Instance.GetNumber(InventoryManager.Instance.OwnedBaitTypeDatas[baitIndex].quantity).Count; i++) {
				voiceOverChain.Add(FMODManager.Instance.GetNumber(InventoryManager.Instance.OwnedBaitTypeDatas[baitIndex].quantity)[i]);
			}
			voiceOverChain.Add(FMODManager.Instance.Left);
			AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
		}
	}

	public void ClickBait() {
		baitView.BaitClicked(baitIndex);
	}

	#endregion

}
