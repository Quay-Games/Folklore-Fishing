using FMODUnity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaitBoardButton : ButtonVoiceOverComponent {

    #region Serialized Fields

    [SerializeField] private BaitBoard baitBoard;
    [SerializeField] private int baitIndex;
    [SerializeField] private int sellQuantity;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemSellPrice;

    #endregion


    #region ButtonVoiceOverComponent

    public override void DoHoverEffect()
    {
        int baitValue = InventoryManager.Instance.BaitDatas.Datas[baitIndex].ItemSellPrice * sellQuantity;
        List<EventReference> voiceoverChain = new List<EventReference>();
        voiceoverChain.Add(FMODManager.Instance.BaitNames[baitIndex]);
        for (int i = 0; i < FMODManager.Instance.GetNumber(baitValue).Count; i++)
        {
            voiceoverChain.Add(FMODManager.Instance.GetNumber(baitValue)[i]);
        }
        voiceoverChain.Add(FMODManager.Instance.Gold);
        if (GameManager.Instance.ShopController.BaitShop.BaitBoard.BaitQuantities[baitIndex] == 0)
        {
            voiceoverChain.Add(FMODManager.Instance.SoldOut);
            AudioManager.Instance.PlayVoiceOverChain(voiceoverChain);
            return;
        }
        for (int i = 0; i < FMODManager.Instance.GetNumber(baitBoard.BaitQuantities[baitIndex]).Count; i++)
        {
            voiceoverChain.Add(FMODManager.Instance.GetNumber(baitBoard.BaitQuantities[baitIndex])[i]);
        }
        voiceoverChain.Add(FMODManager.Instance.Left);
        AudioManager.Instance.PlayVoiceOverChain(voiceoverChain);
    }

    #endregion


    #region Public Methods

    public void OnClick()
    {
        GameManager.Instance.ShopController.BaitShop.BuyBait(baitIndex, sellQuantity);
    }

    #endregion

}
