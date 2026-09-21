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
        Junk
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
            default:
                chosenList = InventoryManager.Instance.OwnedFishTypeDatas;
                break;
        }		
		List<bool> buttonsToEnable = new List<bool>();
		if (this.initialButton != null) {
			Debug.Log(initialButton);
			this.Buttons.Add(initialButton);
			buttonsToEnable.Insert(0, true);
		}
		for (int i = 0; i < chosenList.Count; i++) {
            if (chosenList[i].quantity > 0) {
                buttonsToEnable.Add(true);
            }
            else
            {
                buttonsToEnable.Add(false);
            }
        }
		if (!this.Initialized) { 
			for (int i = 0; i < chosenList.Count; i++) {
                ItemDataButton buttonInstance = Instantiate(itemButton, buttonParent.transform);
				buttonInstance.AssignData(chosenList[i]);
                buttonInstance.name = chosenList[i].OwnedItemData.ItemName;
				this.ItemDataButtons.Add(buttonInstance);
                Buttons.Add(buttonInstance.Button);
            }
		}
		else {
			for (int i = 0; i < this.ItemDataButtons.Count; i++) {
				this.ItemDataButtons[i].AssignData(chosenList[i]);
			}
		}
		Utilities.DisableUnusedButtons(buttonsToEnable, this.Buttons);
		if (navigationType == NavigationType.Horizontal) {
			Utilities.LinkHorizontalButtons(this.Buttons, leaveShopButton);
		}
		else {
			Utilities.LinkVerticalButtons(this.Buttons, leaveShopButton);
		}
        if (leaveShopButton != null)
        {
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
		return CheatManager.Instance.TempBaitBoardDatas;
    }

    #endregion

}
