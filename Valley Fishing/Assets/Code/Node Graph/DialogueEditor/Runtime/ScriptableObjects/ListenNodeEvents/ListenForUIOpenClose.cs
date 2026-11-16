using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Listen For UI Open or Close")]
public class ListenForUIOpenClose : ListenEvent
{
    enum UIType
    {
        CatchUI
    }
    enum UIStatus
    {
        Open,
        Close
    }
    [SerializeField] private UIType UiType;
    [SerializeField] private UIStatus UiStatus;
    private bool satisfied = false;

    public override IEnumerator StartListening()
    {
        satisfied = false;
        yield return CheckUIStatus();
    }

    private IEnumerator CheckUIStatus()
    {
        
        SubscribeToCorrectMethod(Satisfy);
        while (!satisfied)
        {
            yield return null;
        }
        UnsubscribeFromCorrectMethod(Satisfy);

        OnEventTriggered?.Invoke();
    }

    private void Satisfy()
    {
        satisfied = true;
    }

    private void SubscribeToCorrectMethod(Action callbackFunction)
    {
        switch (UiType)
        {
            case UIType.CatchUI:
                if(UiStatus == UIStatus.Open)
                {
                    GameManager.Instance.LevelController.FishView.OnFishUIEnabled += callbackFunction;

                } else
                {
                    GameManager.Instance.LevelController.FishView.OnFishUIDisabled += callbackFunction;
                }
                break;
        }
    }

    private void UnsubscribeFromCorrectMethod(Action callbackFunction)
    {
        switch (UiType)
        {
            case UIType.CatchUI:
                if (UiStatus == UIStatus.Open)
                {
                    GameManager.Instance.LevelController.FishView.OnFishUIEnabled -= callbackFunction;
                }
                else
                {
                    GameManager.Instance.LevelController.FishView.OnFishUIDisabled -= callbackFunction;
                }
                break;
        }
    }
}
