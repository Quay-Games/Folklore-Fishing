using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemDataButtonGenerator : MonoBehaviour
{
    #region Enums

    enum ListUsed
    {
        Fish,
        Bait,
        Inventions
    }

	enum NavigationType {
		Horizontal,
		Vertical
	}

    #endregion


    #region Serialized Fields

    [SerializeField] private ItemDataButton itemButton;
    [SerializeField] private GameObject buttonParent;
    [SerializeField] private Button initialButton;
    [SerializeField] private Button leaveShopButton;
    [SerializeField] private ListUsed listUsed = ListUsed.Fish;
	[SerializeField] private NavigationType navigationType;

	#endregion


	#region Properties

	[field:SerializeField]private List<ItemDataButton> ItemDataButtons = new List<ItemDataButton>();
   [field:SerializeField] private List<Button> Buttons = new List<Button>();
    private bool Initialized { get; set; }

    #endregion


    #region Mono Behaviours

    public void OnEnable()
    {
        List<OwnedItemTypeData> chosenList;
        switch(listUsed)
        {
            case ListUsed.Fish:
                chosenList = InventoryManager.Instance.OwnedFishTypeDatas;
                break;
            case ListUsed.Bait:
				if (GameManager.Instance.ShopController != null) {
					chosenList = GetTempBaitListForSelling();
				} else {
					chosenList = InventoryManager.Instance.OwnedBaitTypeDatas;
				}
                break;
			case ListUsed.Inventions:
				chosenList = InventionManager.Instance.CraftableInventions;
				break;
			default:
                chosenList = InventoryManager.Instance.OwnedFishTypeDatas;
                break;
        }
		
		List<bool> buttonsToEnable = new List<bool>();
		if (this.initialButton != null) {
			if (!this.Initialized) {
				this.Buttons.Add(initialButton);
			}
			buttonsToEnable.Insert(0, true);
		}
		for (int i = 0; i < chosenList.Count; i++) {
			if (chosenList[i].quantity > 0 ||
				listUsed == ListUsed.Inventions) {
				buttonsToEnable.Add(true);
			}
			else {
				buttonsToEnable.Add(false);
			}
		}
		for (int i = 0; i < chosenList.Count; i++) {
			ItemDataButton buttonInstance;
			if (this.ItemDataButtons.Count <= i) {
				buttonInstance = Instantiate(itemButton, buttonParent.transform);
				buttonInstance.name = chosenList[i].OwnedItemData.ItemName;
				this.ItemDataButtons.Add(buttonInstance);
				this.Buttons.Add(buttonInstance.Button);
			}
			else {
				buttonInstance = this.ItemDataButtons[i];
				this.ItemDataButtons[i]= buttonInstance;
				this.Buttons[i] = buttonInstance.Button;
			}
			buttonInstance.AssignData(chosenList[i]);				
		}
		Utilities.DisableUnusedButtons(buttonsToEnable, this.Buttons);
		if (navigationType == NavigationType.Horizontal) {
			Utilities.LinkHorizontalButtons(this.Buttons, leaveShopButton);
		}
		else {
			Utilities.LinkVerticalButtons(this.Buttons, leaveShopButton);
		}
        if (leaveShopButton != null) {
            leaveShopButton.transform.SetAsLastSibling();
			Buttons.Add(leaveShopButton);
		}
        this.Initialized = true;        
        for (int i = 0; i < this.Buttons.Count; i++)
        {
            if (this.Buttons[i].gameObject.activeSelf)
            {
                this.Buttons[i].gameObject.AddComponent<SelectButtonUtility>();
                return;
            }
        }
    }

    #endregion


    #region Methods 
    //This list is representative of the shopkeeper's stock, because we havent figured out representing it proper
    //This will be removed

    public virtual List<OwnedItemTypeData> GetTempBaitListForSelling(){
		List<OwnedItemTypeData> tempBaitListForSelling = new List<OwnedItemTypeData>();
		for (int i = 0; i < CheatManager.Instance.TempBaitBoardDatas.Count; i++) {
			OwnedItemTypeData baitData = new OwnedItemTypeData();
			baitData.OwnedItemData =	InventoryManager.Instance.OwnedBaitTypeDatas[CheatManager.Instance.TempBaitBoardDatas[i].BaitIndex].OwnedItemData;
			baitData.quantity = CheatManager.Instance.TempBaitBoardDatas[i].BaitAmount;
			tempBaitListForSelling.Add(baitData);
			GameManager.Instance.ShopController.BaitShop.BaitBoard.BaitQuantities[CheatManager.Instance.TempBaitBoardDatas[i].BaitIndex] = CheatManager.Instance.TempBaitBoardDatas[i].BaitAmount;
		}
		return tempBaitListForSelling;
    }

    #endregion

}
