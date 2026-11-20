using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] public List<GameUIItem> uiItems = new List<GameUIItem>();
    public Func<bool> DoInstead;
    public Action<UIType, bool> UIOpenClosed;
    public UIType IfType;

    public void Register(GameUIItem uiItem)
    {
        uiItems.Add(uiItem);
    }

    public void RegisterAllUIItems(List<GameUIItem> items)
    {
        uiItems.AddRange(items);
    }

    public void Deregister(GameUIItem uiItem)
    {
        uiItems.Remove(uiItem);
    }

    public void DeregisterAllUIItems(List<GameUIItem> items)
    {
        foreach (var item in items)
        {
            uiItems.Remove(item);
        }
    }

    public bool CheckMembership(GameUIItem uiItem)
    {
        return uiItems.Contains(uiItem);
    }

    public void NotifyOfOpenClose(UIType type, bool active)
    {
        UIOpenClosed?.Invoke(type, active);
    }

    public bool DeferButtonPressIfNeeded(UIType pressingButton)
    {
        if(pressingButton == IfType && DoInstead != null)
        {
            return DoInstead.Invoke();
        } else
        {
            return true;
        }
    }

    public void ShowHideUIElement(UIType type, bool active)
    {
        uiItems.Find(x => x.ButtonType == type)?.gameObject.SetActive(active);
    }
}
