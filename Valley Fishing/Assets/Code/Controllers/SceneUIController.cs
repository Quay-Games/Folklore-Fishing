using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneUIController : MonoBehaviour
{
    public List<GameUIItem> uiItems = new List<GameUIItem>();

    [ContextMenu("Register UI to Controller")]
    public void RegisterAllUIItemsInScene()
    {
        var uiItemsInScene = Resources.FindObjectsOfTypeAll<GameUIItem>();
        uiItems.AddRange(uiItemsInScene);
    }
    //the two methods below should allow the UI Manager to stay up to date with all the UI Items in a scene

    public void Start()
    {
        UIManager.Instance.RegisterAllUIItems(uiItems);
    }

    private void OnDestroy()
    {
        UIManager.Instance.DeregisterAllUIItems(uiItems);
    }
}
