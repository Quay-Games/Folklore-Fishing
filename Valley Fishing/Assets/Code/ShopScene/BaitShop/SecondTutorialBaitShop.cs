using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondTutorialBaitShop : BaitShop {

	#region Properties

	public bool AllFishSold { get => InventoryManager.Instance.TotalOwnedFish > 0; }
	private bool FishBoardNotClosedForFirstTime { get; set; }


    #endregion

	public override void Awake()
	{
		base.Awake();
        leaveShopButton.gameObject.SetActive(false);
        GameManager.Instance.InputController.SelectButton(fishBasketButton.gameObject);
		GameManager.Instance.InputController.SelectionManuallySet = false;
		transform.position = lerpObjectToPositions[0].destination.position;
	}

	public override void VoiceLineOver(bool skipped) {
		base.VoiceLineOver(skipped);
		if (InventoryManager.Instance.OwnedBaitTypeDatas[5].quantity == 3 && InventoryManager.Instance.OwnedBaitTypeDatas[6].quantity == 3 && baitBoard.Initialized && !this.BaitboardTutorialsCompleted[1]) {
			PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
			IncrementTutorial(this.BaitboardTutorialsCompleted);
		}
		if(!fishBoard.Initialized && InventoryManager.Instance.TotalOwnedFish == 0) {
			PlayNextTutotialVoiceOver(this.FishBasketTutorialsCompleted, fishBasketTutorials);
			IncrementTutorial(this.FishBasketTutorialsCompleted);
		}
		if (fishBoard.Initialized && InventoryManager.Instance.TotalOwnedFish == 0) {
			PlayNextTutotialVoiceOver(this.FishboardTutorialsCompleted, fishboardTutorials);
			IncrementTutorial(this.FishboardTutorialsCompleted);
		}
	}

	public override void SellFish(int fishIndex) {
		base.SellFish(fishIndex);
	}

	public override void BuyBait(int baitIndex, int baitQuantity) {
		base.BuyBait(baitIndex, baitQuantity);
	}

	public override void OpenFishBoard() {
		if (fishBoard.Initialized && InventoryManager.Instance.TotalOwnedFish != 0) {
			return;
		}
		if (InventoryManager.Instance.TotalOwnedFish == 0) {
			GameManager.Instance.InputController.SelectButton(baitBoardButton.gameObject);
			GameManager.Instance.InputController.SelectionManuallySet = false;
		}		
		base.OpenFishBoard();
		if (InventoryManager.Instance.OwnedBaitTypeDatas[4].quantity != 5) {
			leaveShopButton.gameObject.SetActive(false);
		}
		if (InventoryManager.Instance.TotalOwnedFish == 0) {
			PlayNextTutotialVoiceOver(this.FishboardTutorialsCompleted, fishboardTutorials);
			IncrementTutorial(this.FishboardTutorialsCompleted);
			FishBoardNotClosedForFirstTime = true;
		}
		PlayNextTutotialVoiceOver(this.FishboardTutorialsCompleted, fishboardTutorials);
		IncrementTutorial(this.FishboardTutorialsCompleted);
	}

	public override void OpenBaitBoard() {
		base.OpenBaitBoard();
		if ((InventoryManager.Instance.TotalOwnedFish == 0)) {
			PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
			IncrementTutorial(this.BaitboardTutorialsCompleted);
		}
	}

	public override IEnumerator EnterShop(bool enter) {
		if (enter) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ShopEnter);
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopIntros[1]);
			yield return new WaitForSeconds(shopEnterTime);
			this.OnGreeting?.Invoke();			
		} else {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ShopEnter);
			this.OnGreeting?.Invoke();
			yield return new WaitForSeconds(shopEnterTime);
		}
		gameObject.SetActive(enter);
	}

	public override void FishBoardSelected() {
		base.FishBoardSelected();
		AudioManager.Instance.PlayVoiceOver(fishBoardEvent);
	}

	public override void FishBasketSelected() {
		base.FishBasketSelected();
		AudioManager.Instance.PlayVoiceOver(fishBasketEvent);
	}

	public override void BaitBoardSelected() {
		base.BaitBoardSelected();
		AudioManager.Instance.PlayVoiceOver(baitBoardEvent);
	}

	public override void LeaveShopSelected() {
		base.LeaveShopSelected();
		AudioManager.Instance.PlayVoiceOver(leaveShopEvent);
	}

	public override void Skip() {
		base.Skip();
		if (InventoryManager.Instance.TotalOwnedFish == 0 && fishBoard.Initialized) {
			OpenFishBoard();
		}
		if ((InventoryManager.Instance.OwnedBaitTypeDatas[5].quantity == 3 && InventoryManager.Instance.OwnedBaitTypeDatas[6].quantity == 3 && baitBoard.Initialized) || 
			(InventoryManager.Instance.TotalOwnedFish != 0 && baitBoard.Initialized)) {
			OpenBaitBoard();
		}
	}

	public override void LeaveShop() {
		if(InventoryManager.Instance.TotalOwnedBaits == 6) {
			SceneManager.LoadScene(LevelManager.CatchTutorial_02);
		} else {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
		}
	}
}
