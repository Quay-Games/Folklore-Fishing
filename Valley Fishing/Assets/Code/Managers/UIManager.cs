using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    List<GameUIItem> uiItems = new List<GameUIItem>();
    public Action DoInstead;
    public UIType IfType;

    public void Register(GameUIItem uiItem)
    {
        uiItems.Add(uiItem);
    }

    public void Deregister(GameUIItem uiItem)
    {
        uiItems.Remove(uiItem);
    }

    public bool DeferButtonPressIfNeeded(UIType pressingButton)
    {
        if(pressingButton == IfType && DoInstead != null)
        {
            DoInstead.Invoke();
            return false;
        } else
        {
            return true;
        }
    }
}
