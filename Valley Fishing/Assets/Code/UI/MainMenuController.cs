using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField] private EventSystem eventSystem;
	[SerializeField] private GameObject mainMenuObject;
	[SerializeField] private GameObject initialMenuButton;
	[SerializeField] private GameObject initialSettingsMenuButton;
	[SerializeField] private GameObject settingsMenuObject;
	[SerializeField] private EventReference menuMusic;

	#endregion



	public void Start()
    {
		GameManager.Instance.MainMenuController = this;
		GameManager.Instance.EventSystem = eventSystem;
		GameManager.Instance.InputController.SelectButton(initialMenuButton);
		List<EventReference> voiceOverChain = new List<EventReference>();
		voiceOverChain.Add(FMODManager.Instance.MenuGreeting);
		voiceOverChain.Add(FMODManager.Instance.SkipVoiceLine);
		voiceOverChain.Add(FMODManager.Instance.ReplayVoiceLine);
		AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
		AudioManager.Instance.PlayMusic(menuMusic);
    }

	public void EnterMainMenu() {
		mainMenuObject.SetActive(true);
		settingsMenuObject.SetActive(false);
		GameManager.Instance.InputController.SelectButton(initialMenuButton);
	}

	public void EnterSettingsMenu() {
		mainMenuObject.SetActive(false);
		settingsMenuObject.SetActive(true);
		GameManager.Instance.InputController.SelectButton(initialSettingsMenuButton);
	}
}
