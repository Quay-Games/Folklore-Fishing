using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaitView : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField] private EventSystem eventSystem;
	[SerializeField] private GameObject buttonsGameobject;
	[SerializeField] private Button[] baitButtons;
	[SerializeField] private float buttonMoveTime;
	[SerializeField] private float baitBoxOpeningTime;

	#endregion


	#region Properties

	[field:SerializeField]private int BaitIndex {	get; set; }

	#endregion


	#region MonoBehaviours

	public void Start() {
		buttonsGameobject.SetActive(false);
		AudioManager.Instance.OnVoiceLineOver -= SkipBaitSelection;
		AudioManager.Instance.OnVoiceLineOver += SkipBaitSelection;
	}

	public void OnDestroy() {
		AudioManager.Instance.OnVoiceLineOver -= SkipBaitSelection;
	}

	#endregion


	#region Public Methods

	public void EnableBaitUI(bool enable) {
		StartCoroutine(EnableUI(enable));
		return;
	}

	public void BaitSelected(int baitIndex) {
		if(baitIndex == this.BaitIndex) {
			return;
		}
		AudioManager.Instance.PlayBaitSound(false, this.BaitIndex);
		this.BaitIndex = baitIndex;
		AudioManager.Instance.PlayBaitSound(true, this.BaitIndex);
	}

	public void BaitClicked(int baitIndex) {
		AudioManager.Instance.PlayBaitSound(false, 0);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.AttatchBaitSounds[baitIndex]);
		InventoryManager.Instance.CurrentBait = InventoryManager.Instance.BaitDatas.Datas[baitIndex];
		InventoryManager.Instance.OwnedBaitTypeDatas[baitIndex].quantity--;
		EnableBaitUI(false);
	}

	#endregion


	#region Private Methods
	
	private IEnumerator EnableUI(bool enable) {
		yield return new WaitForEndOfFrame();
		if (enable == false)
		{
			buttonsGameobject.SetActive(enable);
			GameManager.Instance.LevelController.SetState(LevelController.State.IdleWithBait);
			GameManager.Instance.EventSystem.SetSelectedGameObject(null);
			yield break;
		}
		else
		{
			if (InventoryManager.Instance.OneBaitTypeLeft)
			{
				for (int i = 0; i < InventoryManager.Instance.OwnedBaitTypeDatas.Count; i++)
				{
					if (InventoryManager.Instance.OwnedBaitTypeDatas[i].quantity > 0)
					{
						BaitClicked(i);
					}
				}
			} else
			{
                buttonsGameobject.SetActive(enable);

                AudioManager.Instance.PlayBaitSound(false, 0);
                AudioManager.Instance.PlayOneShot(FMODManager.Instance.BaitBoxOpen);
                StartCoroutine(WaitToOpenBaitBox());
            }
		}
	}

	private void SkipBaitSelection(bool value) {
		if (!buttonsGameobject.activeSelf) {
			return;
		}
		if (GameManager.Instance.LevelController.CurrentState != LevelController.State.AttatchBait) {
			return;
		}
		if (InventoryManager.Instance.OneBaitTypeLeft) {
			BaitClicked(this.BaitIndex);
		}
	}

	private IEnumerator SelectButtonAfterOneFrame(GameObject button) {
		yield return new WaitForEndOfFrame();
		GameManager.Instance.InputController.SelectButton(button);
	}

	private IEnumerator WaitToOpenBaitBox() {
		yield return new WaitForSeconds(baitBoxOpeningTime);
		AudioManager.Instance.PlayBaitSound(true, this.BaitIndex);
	}

	#endregion
}
