using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
	public UpgradesData RodShopData;
	public int ReelSpeed;
	public int StrafeSpeed;
	public int FailSpeed;
	public int AIRod;
	public int InstaWin;
	public LevelController LevelController { get => GameManager.Instance.LevelController; }
}
