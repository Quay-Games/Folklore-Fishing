using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class BaitBoard : ItemDataButtonGenerator {

	#region Serialized Fields

	[SerializeField] private GameObject baitBoardObject;
	[SerializeField] private GameObject[] baitshopComponents;
	

	#endregion


	#region Properties

	public bool Initialized { get; set; }
	[field: SerializeField] public int[] BaitQuantities;

	#endregion

	#region Mono Behaviours

	public void Start() {
		AudioManager.Instance.OnVoiceLineOver += InitiallizeBaitBoard;
	}

	public void OnDestroy() {
		if (AudioManager.Instance != null) {
			AudioManager.Instance.OnVoiceLineOver -= InitiallizeBaitBoard;
		}
	}

	#endregion


	#region Public Methods

	public void OpenBaitBoard() {
		if (!baitBoardObject.activeSelf) {
			AudioManager.Instance.SetMusicParameter("BaitBoardVolume", 1);
			baitBoardObject.SetActive(true);
			for (int i = 0; i < baitshopComponents.Length; i++) {
				InitiallizeBaitBoard(false);
			}
		} else {
			AudioManager.Instance.SetMusicParameter("BaitBoardVolume", 0);
			for (int i = 0; i < baitshopComponents.Length; i++) {
				baitshopComponents[i].SetActive(true);
			}
			GameManager.Instance.EventSystem.SetSelectedGameObject(baitshopComponents[0]);
			this.Initialized = false;
			baitBoardObject.SetActive(false);
		}
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.BaitBoardOpenClose);
	}

	public override List<OwnedItemTypeData> GetTempBaitListForSelling() {
		List<OwnedItemTypeData> data = new List<OwnedItemTypeData>();
		for (int i = 0; i < InventoryManager.Instance.OwnedBaitTypeDatas.Count; i++) {
			OwnedItemTypeData itemData = new OwnedItemTypeData();
			itemData.OwnedItemData = InventoryManager.Instance.OwnedBaitTypeDatas[i].OwnedItemData;
			itemData.quantity = BaitQuantities[i];
			data.Add(itemData);
		}
		return data;
	}

	#endregion


	#region Private Methods

	private void InitiallizeBaitBoard(bool skipped) {
		if (!baitBoardObject.activeSelf) {
			return;
		}
		if (this.Initialized) {
			return;
		}
		this.Initialized = true;
	}

	#endregion

}
