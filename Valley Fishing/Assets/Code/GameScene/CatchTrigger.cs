using System;
using UnityEngine;

public class CatchTrigger : MonoBehaviour
{
    #region Mono Behaviours

    public void OnTriggerEnter(Collider other) {
        if(other.gameObject.TryGetComponent(out Fish fish)) {
            fish.FishCaught();
        }
    }

    #endregion
}
