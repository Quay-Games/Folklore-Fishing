using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpgradesData;

public class RodShop : Shop {

	#region Serialized Fields

	[SerializeField] private UpgradesData rodShopData;

	#endregion


	#region Mono Behaviours

	public void OnEnable() {
		GameManager.Instance.EventSystem.SetSelectedGameObject(this.InitialButton);
	}

	#endregion


	#region Public Methods

	

	public void ReelSpeedButtonClicked(bool clicked) {
		UpgradeButtonInteracted(clicked, rodShopData.ReelSpeedData, ref UpgradeManager.Instance.ReelSpeed);
    }
    public void StrafeSpeedButtonClicked(bool clicked) {
		UpgradeButtonInteracted(clicked, rodShopData.StrafeSpeedData, ref UpgradeManager.Instance.StrafeSpeed);
	}
    public void FailSpeedButtonClicked(bool clicked) {
		UpgradeButtonInteracted(clicked, rodShopData.FailSpeedData, ref UpgradeManager.Instance.FailSpeed);
	}

	#endregion


	#region Private Methods

	private void UpgradeButtonInteracted(bool clicked, UpgradeData upgradeData, ref int upgradeProgress) {
		if (clicked) {
			UpgradeButtonClicked(upgradeData, ref upgradeProgress);
		} else {
			UpgradeButtonHovered(upgradeData, upgradeProgress);
		}
	}

	private void UpgradeButtonHovered(UpgradeData upgradeData, int upgradeProgress) {
		List<EventReference> voiceOverChain = new List<EventReference>();
		voiceOverChain.Add(upgradeData.UpgradeHoverEvents[upgradeProgress]);
		for (int i = 0; i < FMODManager.Instance.GetNumber(upgradeData.UpgradePrices[upgradeProgress]).Count; i++) {
			voiceOverChain.Add(FMODManager.Instance.GetNumber(upgradeData.UpgradePrices[upgradeProgress])[i]);
		}
		voiceOverChain.Add(FMODManager.Instance.Gold);
		AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
	}

	private void UpgradeButtonClicked(UpgradeData upgradeData, ref int upgradeProgress) {
		int upgradeprice = upgradeData.UpgradePrices[upgradeProgress];
		if (GameManager.Instance.Money >= upgradeprice) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ItemBuy);
			AudioManager.Instance.PlayVoiceOver(upgradeData.UpgradeBoughtEvents[upgradeProgress]);
			upgradeProgress++;
			this.OnSaleMade?.Invoke();
		}
		else {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
		}
	}
	private void UpgradeReelSpeed() {
	}

	private void UpgradeStrafeSpeed() {
	}

	private void UpgradeFailSpeed() {

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
