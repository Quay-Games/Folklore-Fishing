using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaitShop : Shop {

	#region Serialized Fields

	[SerializeField] private GameObject baitShopObject;
	[SerializeField] protected FishBoard fishBoard;
	[SerializeField] protected BaitBoard baitBoard;
	[SerializeField] protected EventSystem eventSystem;
	[SerializeField] protected EventReference[] fishboardTutorials;
	[SerializeField] protected EventReference[] baitboardTutorials;
	[SerializeField] protected EventReference[] fishBasketTutorials;
	[SerializeField] protected ButtonVoiceOverComponent fishBoardButton;
	[SerializeField] protected ButtonVoiceOverComponent fishBasketButton;
	[SerializeField] protected ButtonVoiceOverComponent baitBoardButton;
	[SerializeField] protected ButtonVoiceOverComponent leaveShopButton;
	[SerializeField] protected EventReference fishBoardEvent;
	[SerializeField] protected EventReference fishBasketEvent;
	[SerializeField] protected EventReference baitBoardEvent;
	[SerializeField] protected EventReference leaveShopEvent;
	[SerializeField] protected LerpObjectToPosition[] lerpObjectToPositions;

	#endregion


	#region Properties

	public int FishSellPrice { get;	set; }
	public bool TutorialBaitBought { get; set; }
	public bool[] FishboardTutorialsCompleted { get; set; }
	public bool[] BaitboardTutorialsCompleted { get; set; }
	public bool[] FishBasketTutorialsCompleted { get; set; }
	[field:SerializeField] public int[] BaitQuantities { get; set; }
	public BaitBoard BaitBoard => baitBoard;
	public FishBoard FishBoard => fishBoard;

    #endregion


    #region Shop

    public override void Start()
    {
		base.Start();
		for (int i = 0; i < fishboardTutorials.Length; i++)	{
			this.FishboardTutorialsCompleted = new bool[fishboardTutorials.Length];
		}
		for (int i = 0; i < baitboardTutorials.Length; i++)	{
			this.BaitboardTutorialsCompleted = new bool[baitboardTutorials.Length];
		}
		for (int i = 0; i < fishBasketTutorials.Length; i++) {
			this.FishBasketTutorialsCompleted = new bool[fishBasketTutorials.Length];
		}
		fishBoardButton.SelectAction += FishBoardSelected;
		fishBasketButton.SelectAction += FishBasketSelected;
		baitBoardButton.SelectAction += BaitBoardSelected;
		leaveShopButton.SelectAction += LeaveShopSelected;
        this.RunEnterShop = StartCoroutine(EnterShop(true));
    }

    public override void OnDestroy() {
		base.OnDestroy();
		fishBoardButton.SelectAction -= FishBoardSelected;
		fishBasketButton.SelectAction -= FishBasketSelected;
		baitBoardButton.SelectAction -= BaitBoardSelected;
		leaveShopButton.SelectAction -= LeaveShopSelected;
	}

	#endregion


	#region Private Methods

	public override void VoiceLineOver(bool skipped) {
	}

	#endregion


	#region Public Methods

	public virtual void SellFish(int fishIndex) {
		if(InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].quantity == 0) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
			return;
		}
		AudioManager.Instance.SkipVoiceOver();
		for (int i = 0; i < InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].quantity; i++) {
			GameManager.Instance.Money += InventoryManager.Instance.FishDatas.Datas[fishIndex].ItemSellPrice;
			this.FishSellPrice += InventoryManager.Instance.FishDatas.Datas[fishIndex].ItemSellPrice;
		}
		if (InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].quantity > 0) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.MoneyEarnt);
			List<EventReference> voiceOverChain = new List<EventReference>();
			voiceOverChain.Add(FMODManager.Instance.BaitShopSoldFish);
			for (int i = 0; i < FMODManager.Instance.GetNumber(GameManager.Instance.Money).Count; i++) {
				voiceOverChain.Add(FMODManager.Instance.GetNumber(GameManager.Instance.Money)[i]);
			}
			voiceOverChain.Add(FMODManager.Instance.Gold);
			AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
		}
		this.OnSaleMade?.Invoke();
		StartCoroutine(WaitOneFrame(PerformSellFish, fishIndex));
	}

	public virtual void SellAllFish() {
		if(InventoryManager.Instance.TotalOwnedFish == 0) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
			return;
		}
		for (int i = 0; i < InventoryManager.Instance.OwnedFishTypeDatas.Count; i++) {
			if (InventoryManager.Instance.OwnedFishTypeDatas[i].quantity == 0) {
				continue;
			}
			for (int j = InventoryManager.Instance.OwnedFishTypeDatas[i].quantity - 1; j >= 0; j--) {
				GameManager.Instance.Money += InventoryManager.Instance.FishDatas.Datas[i].ItemSellPrice;
                this.FishSellPrice += InventoryManager.Instance.FishDatas.Datas[i].ItemSellPrice;
                InventoryManager.Instance.OwnedFishTypeDatas[i].quantity--;
			}
		}
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.MoneyEarnt);
		List<EventReference> voiceOverChain = new List<EventReference>();
		voiceOverChain.Add(FMODManager.Instance.BaitShopSoldFish);
		for (int i = 0; i < FMODManager.Instance.GetNumber(GameManager.Instance.Money).Count; i++) {
			voiceOverChain.Add(FMODManager.Instance.GetNumber(GameManager.Instance.Money)[i]);
		}
		voiceOverChain.Add(FMODManager.Instance.Gold);
		for (int i = 0; i < voiceOverChain.Count; i++) {
		}
		this.OnSaleMade?.Invoke();
		AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
	}

	public virtual void BuyBait(int baitIndex, int sellQuantity) {
		if(this.BaitQuantities[baitIndex] == 0) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
			return;
		}
		if (InventoryManager.Instance.BaitDatas.Datas[baitIndex].ItemSellPrice * sellQuantity > GameManager.Instance.Money) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
			return;
		}
		GameManager.Instance.Money -= InventoryManager.Instance.BaitDatas.Datas[baitIndex].ItemSellPrice * sellQuantity;
		InventoryManager.Instance.OwnedBaitTypeDatas[baitIndex].quantity += sellQuantity;
		this.BaitQuantities[baitIndex] -= sellQuantity;
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.MoneyEarnt);
		List<EventReference> voiceOverChain = new List<EventReference>();
		voiceOverChain.Add(FMODManager.Instance.BaitShopBoughtBait);
		for (int i = 0; i < FMODManager.Instance.GetNumber(GameManager.Instance.Money).Count; i++) {
			voiceOverChain.Add(FMODManager.Instance.GetNumber(GameManager.Instance.Money)[i]);
		}
		voiceOverChain.Add(FMODManager.Instance.Gold);
		this.OnSaleMade?.Invoke();
		AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
	}

	#endregion


	#region Private Methods

	private void PerformSellFish(int fishIndex) {
		InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].quantity = 0;
	}

	public virtual IEnumerator WaitOneFrame(Action<int> callback, int integer) {
		yield return new WaitForEndOfFrame();
		callback?.Invoke(integer);
	}

	public virtual void OpenFishBoard() {
		lerpObjectToPositions[1].BeginLerp();
		fishBoard.OpenFishBoard();
	}
	public virtual void OpenBaitBoard() {
		baitBoard.OpenBaitBoard();
	}

	public override void Skip() {
		base.Skip();
		AudioManager.Instance.DisableSkipping();
	}

	public virtual void FishBoardSelected(ButtonType buttonType, ButtonVoiceOverComponent buttonVoiceOverComponent) {
		lerpObjectToPositions[1].BeginLerp();
	}

	public virtual void FishBasketSelected(ButtonType buttonType, ButtonVoiceOverComponent buttonVoiceOverComponent) {
		lerpObjectToPositions[0].BeginLerp();
	}

	public virtual void BaitBoardSelected(ButtonType buttonType, ButtonVoiceOverComponent buttonVoiceOverComponent) {
		lerpObjectToPositions[2].BeginLerp();
	}
	public virtual void LeaveShopSelected(ButtonType buttonType, ButtonVoiceOverComponent buttonVoiceOverComponent) {
		lerpObjectToPositions[3].BeginLerp();
	}

	#endregion

}
