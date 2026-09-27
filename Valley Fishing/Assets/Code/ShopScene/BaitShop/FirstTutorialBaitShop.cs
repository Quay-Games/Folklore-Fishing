using FMODUnity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstTutorialBaitShop : BaitShop {

	#region Serialized Fields

	[SerializeField] protected EventReference fishBoardIntroEvent;
	[SerializeField] protected EventReference fishBasketIntroEvent;
	[SerializeField] protected EventReference baitBoardIntroEvent;
	[SerializeField] private GameObject lockedFishBoardButton;

	#endregion


	#region Properties
	public bool AllFishSold { get => InventoryManager.Instance.TotalOwnedFish > 0; }
	private bool FishBoardNotClosedForFirstTime { get; set; }
	private bool PlayFishBoardIntro { get; set; } = true;
	private bool PlayFishBasketIntro { get; set; } = true;
	private bool PlayBaitBoardIntro { get; set; } = true;
	private bool FishSold { get; set; }


    #endregion


    public override void Awake()
    {
		base.Awake();
        leaveShopButton.gameObject.SetActive(false);
        GameManager.Instance.InputController.SelectButton(lockedFishBoardButton.gameObject);
    }

	public override void VoiceLineOver(bool skipped) {
		base.VoiceLineOver(skipped);
		if (InventoryManager.Instance.OwnedBaitTypeDatas[4].quantity == 5 && !baitBoard.Initialized) {
			PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
			IncrementTutorial(this.BaitboardTutorialsCompleted);
		}
		if (InventoryManager.Instance.OwnedBaitTypeDatas[4].quantity == 5 && !this.BaitboardTutorialsCompleted[1]) {
			PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
			IncrementTutorial(this.BaitboardTutorialsCompleted);
		}
		if (InventoryManager.Instance.TotalOwnedFish > 0 && InventoryManager.Instance.TotalOwnedFish < 9 && fishBoard.Initialized) {
			if (!this.FishboardTutorialsCompleted[0]) {
				PlayNextTutotialVoiceOver(this.FishboardTutorialsCompleted, fishboardTutorials);
				IncrementTutorial(this.FishboardTutorialsCompleted);
			}
		}
		if (InventoryManager.Instance.TotalOwnedFish == 0) {
			if (!FishBoardNotClosedForFirstTime) {
				PlayNextTutotialVoiceOver(this.FishboardTutorialsCompleted, fishboardTutorials);
				IncrementTutorial(this.FishboardTutorialsCompleted);
				FishBoardNotClosedForFirstTime = true;
			}
		}
	}

	public override void SellFish(int fishIndex) {
		base.SellFish(fishIndex);
		this.FishSold = true;
	}

	public override void BuyBait(int baitIndex, int baitQuantity) {
		base.BuyBait(baitIndex, baitQuantity);
	}

	public override void OpenFishBoard() {
		if(fishBoard.Initialized && InventoryManager.Instance.TotalOwnedFish != 0) {
			return;
		}
		if(InventoryManager.Instance.TotalOwnedFish == 0) {
			GameManager.Instance.InputController.SelectButton(fishBoardButton.gameObject);
			GameManager.Instance.InputController.SelectionManuallySet = false;
		}
		base.OpenFishBoard();
		if (InventoryManager.Instance.OwnedBaitTypeDatas[4].quantity != 5) {
			leaveShopButton.gameObject.SetActive(false);
		}
		lockedFishBoardButton.gameObject.SetActive(false);
		PlayNextTutotialVoiceOver(this.FishboardTutorialsCompleted,fishboardTutorials);
		IncrementTutorial(this.FishboardTutorialsCompleted);
	}

	public override void OpenBaitBoard() {
		if (baitBoard.Initialized && InventoryManager.Instance.OwnedBaitTypeDatas[4].quantity == 0) {
			return;
		}
		if (InventoryManager.Instance.OwnedBaitTypeDatas[4].quantity == 5) {
			GameManager.Instance.InputController.SelectButton(baitBoardButton.gameObject);
			GameManager.Instance.InputController.SelectionManuallySet = false;
		}
		base.OpenBaitBoard();
		PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
		IncrementTutorial(this.BaitboardTutorialsCompleted);
	}

	public override IEnumerator EnterShop(bool enter) {
		yield return StartCoroutine(base.EnterShop(enter));
		if (enter) {
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopIntros[0]);
		} else {
			SceneManager.LoadScene(LevelManager.CatchTutorial_01);
		}
	}

	public override void FishBoardSelected(ButtonType buttonType, ButtonVoiceOverComponent buttonVoiceOverComponent) {
		base.FishBoardSelected(buttonType, buttonVoiceOverComponent);
		if (this.PlayFishBoardIntro) {
			AudioManager.Instance.PlayVoiceOver(fishBoardIntroEvent);
			this.PlayFishBoardIntro = false;
			return;
		}
		AudioManager.Instance.PlayVoiceOver(fishBoardEvent);
	}

	public override void FishBasketSelected(ButtonType buttonType, ButtonVoiceOverComponent buttonVoiceOverComponent) {
		base.FishBasketSelected(buttonType, buttonVoiceOverComponent);
		if (this.PlayFishBasketIntro) {
			AudioManager.Instance.PlayVoiceOver(fishBasketIntroEvent);
			this.PlayFishBasketIntro = false;
			return;
		}
		AudioManager.Instance.PlayVoiceOver(fishBasketEvent);
	}

	public override void BaitBoardSelected(ButtonType buttonType, ButtonVoiceOverComponent buttonVoiceOverComponent) {
		base.BaitBoardSelected(buttonType, buttonVoiceOverComponent);
		if (this.PlayBaitBoardIntro) {
			AudioManager.Instance.PlayVoiceOver(baitBoardIntroEvent);
			this.PlayBaitBoardIntro = false;
			return;
		}
		AudioManager.Instance.PlayVoiceOver(baitBoardEvent);
	}
	public override void LeaveShopSelected(ButtonType buttonType, ButtonVoiceOverComponent buttonVoiceOverComponent) {
		base.LeaveShopSelected(buttonType, buttonVoiceOverComponent);
		AudioManager.Instance.PlayVoiceOver(leaveShopEvent);
	}

	public override void Skip() {
		base.Skip();
		if (InventoryManager.Instance.TotalOwnedFish == 0 && fishBoard.Initialized) {
			OpenFishBoard();
		}
		if (InventoryManager.Instance.OwnedBaitTypeDatas[4].quantity == 5 && baitBoard.Initialized) {
			OpenBaitBoard();
		}
	}
	public override void LeaveShop() {
		if (InventoryManager.Instance.TotalOwnedBaits == 5) {
			SceneManager.LoadScene(LevelManager.CatchTutorial_01);
		} else {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
		}
	}
}