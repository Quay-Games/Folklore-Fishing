using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopController : MonoBehaviour {
	#region Serialized Fields

	[SerializeField] private EventSystem eventSystem;
	[SerializeField] private EventReference levelMusic;
	[SerializeField] private bool isTutorial;

	#endregion


	#region Properties
	[field: SerializeField] public Shore Shore { get; set; }
    [field: SerializeField] public CalvinBaitShop BaitShop { get; set; }
    //[field: SerializeField] public BaitShop BaitShop { get; set; }
	[field: SerializeField] public RodShop RodShop { get; set; }
	[field: SerializeField] public Icthyologists Icthyologists { get; set; }
	[field: SerializeField] public InventorsLab InventorsLab { get; set; }

	public List<GameObject> AllMenus => new List<GameObject>() {
		this.Shore.gameObject,this.BaitShop.gameObject, this.RodShop.gameObject, this.Icthyologists.gameObject, this.InventorsLab.gameObject
	};

	#endregion


	#region Mono Behaviours

	public void Awake() {
		GameManager.Instance.ShopController = this;
		GameManager.Instance.EventSystem = eventSystem;
		AudioManager.Instance.PlayMusic(levelMusic);
		AudioManager.Instance.CleanUpAmbience();
		if (isTutorial) {
			return;
		}
		EnableMenu(this.Shore.gameObject);
	}

	#endregion


	#region Public Methods

	public void EnableMenu(GameObject menuToEnable) {
		for (int i = 0; i < this.AllMenus.Count; i++) {
			if (this.AllMenus[i].gameObject == menuToEnable) {
				this.AllMenus[i].SetActive(true);
			} else {
				this.AllMenus[i].SetActive(false);
			}
		}
	}

		#endregion
}
