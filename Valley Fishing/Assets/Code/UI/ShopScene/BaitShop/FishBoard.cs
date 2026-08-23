using FMODUnity;
using UnityEngine;

public class FishBoard : ItemDataButtonGenerator
{

	#region Serialized Fields

	[SerializeField] private GameObject fishBoardObject;
	[SerializeField] private GameObject[] baitshopComponents;

	#endregion


	#region Properties

	[field:SerializeField]
	public bool Initialized { get; set; }

	#endregion


	#region Mono Behaviours

	public void Start() {
		AudioManager.Instance.OnVoiceLineOver += InitiallizeFishBoard;
	}

	public void OnDestroy() {
		if (AudioManager.Instance != null) {
			AudioManager.Instance.OnVoiceLineOver -= InitiallizeFishBoard;
		}
	}

	#endregion


	#region Public Methods

	public void OpenFishBoard() {
		if (!fishBoardObject.activeSelf) {
			AudioManager.Instance.SetMusicParameter("FishBoardVolume", 1);
			fishBoardObject.SetActive(true);			
			for (int i = 0; i < baitshopComponents.Length; i++) {
				InitiallizeFishBoard(false);
			}
		} else {
			AudioManager.Instance.SetMusicParameter("FishBoardVolume", 0);
			for (int i = 0; i < baitshopComponents.Length; i++) {
				baitshopComponents[i].SetActive(true);
			}
			GameManager.Instance.EventSystem.SetSelectedGameObject(baitshopComponents[0]);
			this.Initialized = false;
			fishBoardObject.SetActive(false);
		}
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.BaitBoardOpenClose);
	}

	#endregion


	#region Private Methods

	private void InitiallizeFishBoard(bool skipped) {
		if (!fishBoardObject.activeSelf) {
			return;
		}
		if (this.Initialized) {
			return;
		}
        this.Initialized = true;
	}

	#endregion

}
