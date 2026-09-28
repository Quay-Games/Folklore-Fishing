using FMOD.Studio;
using FMODUnity;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishView : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField]
	private GameObject[] fishUis;

	[SerializeField]
	private TMP_Text fishText;

	[SerializeField]
	private string[] fishCaughtTexts;

	#endregion


	#region Mono Behaviours

	public void Start() {
		GameManager.Instance.InputController.OnClick -= DisableFishUI;
		GameManager.Instance.InputController.OnSkip -= DisableFishUI;
		GameManager.Instance.InputController.OnClick += DisableFishUI;
		GameManager.Instance.InputController.OnSkip += DisableFishUI;
		AudioManager.Instance.OnVoiceLineOver -= DisableUI;
		AudioManager.Instance.OnVoiceLineOver += DisableUI;
	}

	public void OnDestroy() {
		if(GameManager.Instance == null) {
			return;
		}
		GameManager.Instance.InputController.OnClick -= DisableFishUI;
		GameManager.Instance.InputController.OnSkip -= DisableFishUI;
		AudioManager.Instance.OnVoiceLineOver -= DisableUI;
	}

	#endregion


	#region Public Methods

	public void EnableFishUI(bool enable) {
		if (enable) {
			for (int i = 0; i < InventoryManager.Instance.FishDatas.Datas.Length; i++) {
				if (InventoryManager.Instance.FishDatas.Datas[i].ItemName == GameManager.Instance.CurrentFish.FishData.ItemName) {
					fishUis[i].SetActive(enable);
				}
			}
			int randomCaughtTextIndex = Random.Range(0, fishCaughtTexts.Length);
			fishText.text = fishCaughtTexts[randomCaughtTextIndex] + " " + GameManager.Instance.CurrentFish.FishData.ItemName + "!";
			fishText.gameObject.SetActive(enable);
			PlayCorrectFishCatchSFX();
        }
		else {
			for (int i = 0; i < InventoryManager.Instance.FishDatas.Datas.Length; i++) {
				fishUis[i].SetActive(enable);
			}
		fishText.gameObject.SetActive(enable);
			if(InventoryManager.Instance.TotalOwnedBaits == 0) {
				SceneManager.LoadScene(LevelManager.CalvinShore);
			}
		}
	}

	#endregion


	#region Private Methods

	private void DisableUI(bool value) {
		DisableFishUI();
	}

	public void DisableFishUI() {
		if (GameManager.Instance.LevelController.StateLocked) {
			return;
		}
		bool allreadyDisabled = true;
		for (int i = 0; i < InventoryManager.Instance.FishDatas.Datas.Length; i++) {
			if (fishUis[i].activeSelf) {
				allreadyDisabled = false;
			}
		}
		if (allreadyDisabled) {
			return;
		}
		EnableFishUI(false);
		AudioManager.Instance.SkipVoiceOver();
		if(GameManager.Instance.LevelController.CurrentState == LevelController.State.FishCaught) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.BaitBoxOpen);
			GameManager.Instance.LevelController.SetState(LevelController.State.Idle);
		}
	}

	private void PlayCorrectFishCatchSFX()
	{
		switch(GameManager.Instance.CurrentFish.FishData.catchSFX)
		{
			case FishDatas.FishData.CatchSFX.DEFAULT:
                AudioManager.Instance.PlayOneShot(FMODManager.Instance.FishCatch);
				break;
			case FishDatas.FishData.CatchSFX.BOSS:
                AudioManager.Instance.PlayOneShot(FMODManager.Instance.BossCatch);
                break;
        }
    }

	#endregion

}
