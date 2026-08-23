using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icthyologists : Shop
{

	#region Serialized Fields

	[SerializeField] private IcthyologistData icthyologistData;
	[SerializeField] private ItemDataButton fishButton;
	[SerializeField] private Transform buttonParent;
	[SerializeField] private Button leaveShopButton;

	#endregion


	#region Properties

	public List<Button> Buttons;
	private bool JustSoldFish { get; set; }
	private int LastSoldFish { get; set; }
	private bool Initialized { get; set; }

	#endregion


	#region Mono Behaviours

	public void OnEnable() {
		InputManager.Instance.SelectButton(InitialButton);
	}

	#endregion


	#region Public Methods

	public void HoverFish(int fishIndex) {
		
		if(InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].quantity == 0 || IcthyologistManager.Instance.SoldFish[fishIndex]) { 
			AudioManager.Instance.PlayVoiceOver((InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].OwnedItemData as FishDatas.FishData).ItemNameEvent);
		} else {
			AudioManager.Instance.PlayVoiceOver((InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].OwnedItemData as FishDatas.FishData).ItemNameEvent);
		}
	}

	public void SellFish(int fishIndex) {
		if (!IcthyologistManager.Instance.SoldFish[fishIndex]) {
			GameManager.Instance.Money += InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].OwnedItemData.ItemSellPrice;
			InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].quantity--;
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ItemBuy);
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.YouHave);
			this.LastSoldFish = fishIndex;
			this.JustSoldFish = true;
			this.OnSaleMade?.Invoke();
		} else {
			PlayFishInfo(fishIndex);
		}		
	}

	public void PlayFishInfo(int fishIndex) {
		AudioManager.Instance.PlayVoiceOver((InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].OwnedItemData as FishDatas.FishData).icthyologistInfoAudio);
		this.JustSoldFish = false;
	}

	#endregion


	#region Shop

	public override void VoiceLineOver(bool skipped) {
		if (this.JustSoldFish) {
			PlayFishInfo(this.LastSoldFish);
		}
	}

    public override IEnumerator EnterShop(bool enable) {
        base.EnterShop(enable);
        yield return null;
        if (!enable) {
            this.ShopController.EnableMenu(this.ShopController.Shore.gameObject);
        }
    }

    #endregion

}
