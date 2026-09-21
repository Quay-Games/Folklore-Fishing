using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatManager : Singleton<CheatManager>
{
	[System.Serializable]
	public struct BaitCheatData {
		public int BaitIndex;
		public int BaitAmount;
	}

	[System.Serializable]
	public struct CaughtFishCheatData {
		public int FishIndex;
		public int FishAmount;
	}

	private int ScenesLoaded;	
	public List<OwnedItemTypeData> TempBaitBoardDatas;

	[field:SerializeField] private bool useCheats = true;

	[field: SerializeField] private bool superFastCatch;

	#region Mono Behaviours

	public void Start() {
		SceneManager.sceneLoaded -= SceneLoaded;
		SceneManager.sceneLoaded += SceneLoaded;
		if (ScenesLoaded > 0 && !useCheats) {
			return;
		}
		if (superFastCatch) {
			GameManager.Instance.LevelController.OnFishSpawned += SetFishSpeed;
		}
		ShowFirstShopTutorialCheats();
		ShowSecondShopTutorialCheats();
		ShowThirdShopTutorialCheats();
		ShowSecondCatchTutorialCheats();
		ShowThirdCatchTutorialCheats();
		ShowFirstBossTutorialCheats();
		ShowGameScenneCheats();
	}

	public void OnDestroy() {
		SceneManager.sceneLoaded -= SceneLoaded;
	}

	private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
		ScenesLoaded++;
		if (InventoryManager.Instance.TotalOwnedBaits == 0) {
			ShowThirdCatchTutorialCheats();
		}
		if (superFastCatch) {
			GameManager.Instance.LevelController.OnFishSpawned += SetFishSpeed;
		}
		if (useCheats) {
			ShowFirstShopTutorialCheats();
			ShowSecondShopTutorialCheats();
			ShowThirdShopTutorialCheats();
			ShowSecondCatchTutorialCheats();
			ShowThirdCatchTutorialCheats();
			ShowFirstBossTutorialCheats();
			ShowGameScenneCheats();
			ShowCalvinShopTutorialCheats();

        }
	}

	#endregion


	#region Private Methods

	private void SetFishSpeed() {
		GameManager.Instance.CurrentFish.ReelSpeed = 1000;
	}

	#endregion


	#region Level 01 Catch Tutorial

	[System.Serializable]
	public class Level_01_Catch_Tutorial {
		public BaitCheatData[] Baits;
	}

	[SerializeField]
	private Level_01_Catch_Tutorial level_01_Catch_Tutorial;

	private void ShowSecondCatchTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.CatchTutorial_01) {
			for (int i = 0; i < level_01_Catch_Tutorial.Baits.Length; i++) {
				InventoryManager.Instance.OwnedBaitTypeDatas[level_01_Catch_Tutorial.Baits[i].BaitIndex].quantity = level_01_Catch_Tutorial.Baits[i].BaitAmount;
			}
		}
	}

	#endregion


	#region Level_02_Catch_Tutorial

	[System.Serializable]
	public class Level_02_Catch_Tutorial {
		public BaitCheatData[] Baits;
	}

	[SerializeField]
	private Level_02_Catch_Tutorial level_02_Catch_Tutorial;

	private void ShowThirdCatchTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.CatchTutorial_02) {
			for (int i = 0; i < level_02_Catch_Tutorial.Baits.Length; i++) {
				InventoryManager.Instance.OwnedBaitTypeDatas[level_02_Catch_Tutorial.Baits[i].BaitIndex].quantity = level_02_Catch_Tutorial.Baits[i].BaitAmount;
			}
		}
	}

	#endregion


	#region Level_03_Boss

	[System.Serializable]
	public class Level_03_Boss {
		public BaitCheatData[] Baits;
	}

	[SerializeField]
	private Level_03_Boss level_03_Boss;

	private void ShowFirstBossTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.BossTutorial_00) {
			for (int i = 0; i < level_03_Boss.Baits.Length; i++) {
				InventoryManager.Instance.OwnedBaitTypeDatas[level_03_Boss.Baits[i].BaitIndex].quantity = level_03_Boss.Baits[i].BaitAmount;
			}
		}
	}

	#endregion


	#region Level 01 Catch Tutorial

	[System.Serializable]
	public class GameSceneCheats {
		public BaitCheatData[] Baits;
	}

	[SerializeField]
	private GameSceneCheats gameSceneCheats;

	private void ShowGameScenneCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.GameSence) {
			for (int i = 0; i < gameSceneCheats.Baits.Length; i++) {
                InventoryManager.Instance.OwnedBaitTypeDatas[gameSceneCheats.Baits[i].BaitIndex].quantity = gameSceneCheats.Baits[i].BaitAmount;
			}
		}
	}

	#endregion


	#region First Shop Turotial

	[System.Serializable]
	public class FirstShopTutorialCheats {
		public CaughtFishCheatData[] CaughtFish;
	}

	[SerializeField]
	private FirstShopTutorialCheats firstShopTutorialCheats;

	private void ShowFirstShopTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.ShopTutorial_00) {
			for (int i = 0; i < firstShopTutorialCheats.CaughtFish.Length; i++) {
				foreach (var fish in firstShopTutorialCheats.CaughtFish) {
					InventoryManager.Instance.OwnedFishTypeDatas[fish.FishIndex].quantity = fish.FishAmount;
				}
			}
		}
	}

	#endregion


	#region  Second Shop Turotial

	[System.Serializable]
	public class SecondShopTutorialCheats {
		public CaughtFishCheatData[] CaughtFish;
	}

	[SerializeField]
	private SecondShopTutorialCheats secondShopTutorialCheats;

	private void ShowSecondShopTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.ShopTutorial_01) {
			for (int i = 0; i < secondShopTutorialCheats.CaughtFish.Length; i++) {
				foreach (var fish in secondShopTutorialCheats.CaughtFish) {
					InventoryManager.Instance.OwnedFishTypeDatas[fish.FishIndex].quantity = fish.FishAmount;
				}
			}
		}
	}

	#endregion


	#region  Third Shop Turotial

	[System.Serializable]
	public class ThirdShopTutorialCheats {
		public CaughtFishCheatData[] CaughtFish;
	}

	[SerializeField]
	private ThirdShopTutorialCheats thirdShopTutorialCheats;

	private void ShowThirdShopTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.ShopTutorial_02) {
			for (int i = 0; i < thirdShopTutorialCheats.CaughtFish.Length; i++) {
				foreach (var fish in thirdShopTutorialCheats.CaughtFish) {
					InventoryManager.Instance.OwnedFishTypeDatas[fish.FishIndex].quantity = fish.FishAmount;
				}
			}
		}
	}

    #endregion


    #region Calvin Shop

    [System.Serializable]
    public class CalvinShoreCheats {
        public CaughtFishCheatData[] CaughtFish;
    }

    [SerializeField] private CalvinShoreCheats calvinShoreCheats;

    private void ShowCalvinShopTutorialCheats() {
        if (SceneManager.GetActiveScene().name == LevelManager.ShopTutorial_02) {
            for (int i = 0; i < calvinShoreCheats.CaughtFish.Length; i++) {
                foreach (var fish in calvinShoreCheats.CaughtFish) {
                    InventoryManager.Instance.OwnedFishTypeDatas[fish.FishIndex].quantity = fish.FishAmount;
                }
            }
        }
    }

    #endregion

}
