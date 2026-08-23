using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Shore : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField] protected GameObject[] shopButtons;
	[SerializeField] protected EventSystem eventSystem;

	#endregion


	#region Properties
	private ShopController ShopController { get => GameManager.Instance.ShopController; }

	#endregion


	#region Public Methods

	public virtual void OnEnable() {
		InputManager.Instance.SelectButton(shopButtons[0]);
	}

	public void EnterBaitShop() {
		this.ShopController.EnableMenu(this.ShopController.BaitShop.gameObject);
	}

	public void EnterRodShop() {
        this.ShopController.EnableMenu(this.ShopController.RodShop.gameObject);
    }

    public void EnterIcthyologists() {
        this.ShopController.EnableMenu(this.ShopController.Icthyologists.gameObject);
    }

    public void EneterInventorsLab() {
        this.ShopController.EnableMenu(this.ShopController.InventorsLab.gameObject);
    }

	public void LeaveShore() {
		SceneManager.LoadScene(LevelManager.CatchTutorial_01);
	}

	#endregion

}
