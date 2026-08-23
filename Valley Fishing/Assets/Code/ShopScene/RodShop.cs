using FMODUnity;
using System.Collections;
using UnityEngine;

public class RodShop : Shop {

	#region Serialized Fields

	[SerializeField] private RodShopData rodShopData;

	#endregion


	#region Mono Behaviours

	public void OnEnable() {
		GameManager.Instance.EventSystem.SetSelectedGameObject(this.InitialButton);
	}

	#endregion


	#region Public Methods

	public void ReelSpeedButtonHovered() {
        AudioManager.Instance.PlayVoiceOver(rodShopData.ReelSpeedDatas[UpgradeManager.Instance.ReelSpeed + 1].ReelSpeedHoverEvents);
    }
    public void StrafeSpeedButtonHovered() {
		AudioManager.Instance.PlayVoiceOver(rodShopData.StrafeSpeedDatas[UpgradeManager.Instance.StrafeSpeed + 1].StrafeSpeedHoverEvents);
	}
    public void FailSpeedButtonHovered() {
		AudioManager.Instance.PlayVoiceOver(rodShopData.FailSpeedDatas[UpgradeManager.Instance.FailSpeed + 1].FailSpeedHoverEvents);
	}

    public void UpgradeReelSpeed()
    {
        int upgradeprice = rodShopData.ReelSpeedDatas[UpgradeManager.Instance.ReelSpeed + 1].ReelSpeedPrices;
        if (GameManager.Instance.Money >= upgradeprice)
        {            
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ItemBuy);
			AudioManager.Instance.PlayVoiceOver(rodShopData.ReelSpeedDatas[UpgradeManager.Instance.ReelSpeed + 1].ReelSpeedBoughtEvents);
			UpgradeManager.Instance.ReelSpeed++;
			this.OnSaleMade?.Invoke();
		}
        else
        {
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
        }
    }

    public void UpgradeStrafeSpeed()
    {
        int upgradeprice = rodShopData.StrafeSpeedDatas[UpgradeManager.Instance.StrafeSpeed + 1].StrafeSpeedPrices;
        if (GameManager.Instance.Money >= upgradeprice)
        {
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ItemBuy);
			AudioManager.Instance.PlayVoiceOver(rodShopData.StrafeSpeedDatas[UpgradeManager.Instance.StrafeSpeed + 1].StrafeSpeedBoughtEvents);
			UpgradeManager.Instance.StrafeSpeed++;
			this.OnSaleMade?.Invoke();
		}
        else
        {
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
        }
    }

    public void UpgradeFailSpeed()
    {
        int upgradeprice = rodShopData.FailSpeedDatas[UpgradeManager.Instance.FailSpeed + 1].FailSpeedPrices;
        if (GameManager.Instance.Money >= upgradeprice)
        {
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ItemBuy);
			AudioManager.Instance.PlayVoiceOver(rodShopData.FailSpeedDatas[UpgradeManager.Instance.FailSpeed + 1].FailSpeedBoughtEvents);
			UpgradeManager.Instance.FailSpeed++;
			this.OnSaleMade?.Invoke();
		}
        else
        {
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
        }
    }

	#endregion


	#region Shop

	public override void VoiceLineOver(bool skipped) {

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
