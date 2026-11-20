using System.Collections.Generic;
using UnityEngine;
using static BaitDatas;

public class ProgressionManager : Singleton<ProgressionManager>
{
	[field:SerializeField] public int LocationsUnlocked { get; set; } = 1;
	public int tutorialProgress = 0;
	[SerializeField] public BaitShopStock firstShopTutorialStock;
	
	public List<OwnedItemTypeData> GetCurrentBaitShopStock()
	{
		return null;
	}

	public void progressTutorial()
	{
		tutorialProgress++;
	}
}
