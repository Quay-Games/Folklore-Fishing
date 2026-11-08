using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
	[SerializeField] private GameObject inputSelectionUI;
	[SerializeField] private Animator animator;


	private bool InputRecieved { get; set; }

    public void Start()
    {
		AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.SelectInputDevice);
    }

	public void Update() {
		CheckInputDevice();
	}

	public void PlayIntro() {
		inputSelectionUI.SetActive(false);
		animator.Play("Intro");
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.QuayPresents);
	}

	public void CheckInputDevice() {
		if(InputManager.Instance.LastButtons.Count != 3) {
			return;
		}
		string buttonPressed = InputManager.Instance.LastButtons[0];
		for (int i = 0; i < InputManager.Instance.LastButtons.Count; i++) {
			if (InputManager.Instance.LastButtons[i] != buttonPressed) {
				return;
			}
		}
		if (InputRecieved) {
			return;
		}
		PlayIntro();
		InputRecieved = true;
	}

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
