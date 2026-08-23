using FMODUnity;
using System.Collections;
using UnityEngine;

public class InventorsLab : Shop
{
    public override void VoiceLineOver(bool skipped)
    {
        
    }
    public void OnEnable() {
        InputManager.Instance.SelectButton(InitialButton);
    }

    public override IEnumerator EnterShop(bool enable) {
        base.EnterShop(enable);
        yield return null;
        if (!enable) {
            this.ShopController.EnableMenu(this.ShopController.Shore.gameObject);
        }
    }
}
