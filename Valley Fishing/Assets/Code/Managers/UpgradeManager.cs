using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
	[SerializeField] private UpgradesData rodShopData;
	public int ReelSpeed;
	public int StrafeSpeed;
	public int FailSpeed;
	public int AIRod;
	public int InstaWin;
	public LevelController LevelController { get => GameManager.Instance.LevelController; }

	public void Start() {
		if (this.LevelController != null) {
			GameManager.Instance.LevelController.OnFishSpawned += SetFishSpeed;
		}
	}


	public void OnDestroy() {
		if (this.LevelController != null) {
			GameManager.Instance.LevelController.OnFishSpawned -= SetFishSpeed;
		}
	}

	private void SetFishSpeed() {
		GameManager.Instance.CurrentFish.ReelSpeed *= rodShopData.ReelSpeedData.UpgradeValues[ReelSpeed];
	}
}
