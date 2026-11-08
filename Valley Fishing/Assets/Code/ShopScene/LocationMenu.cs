using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocationMenu : MonoBehaviour
{
	[SerializeField] private Button[] locationButtons;
	[SerializeField] private Button initialButton;
	[SerializeField] private Button shoreButton;

	private ShopController ShopController => GameManager.Instance.ShopController;

	public void OnEnable() {
		List<Button> enabledButtons = new List<Button>();
		for (int i = 0; i < locationButtons.Length; i++) {
			if (ProgressionManager.Instance.LocationsUnlocked > i) {
				locationButtons[i].gameObject.SetActive(true);
				enabledButtons.Add(locationButtons[i]);
			} else {
				locationButtons[i].gameObject.SetActive(false);
			}
		}
		Utilities.LinkHorizontalButtons(enabledButtons, shoreButton);
		EventSystem.current.SetSelectedGameObject(initialButton.gameObject);
	}

	public void EnterZone(int zone) {
		//SceneManager.LoadScene()
		//LevelManager.
	}

	public void ReturnToShore() {
		GameManager.Instance.ShopController.EnableMenu(this.ShopController.Shore.gameObject);
	}
}
