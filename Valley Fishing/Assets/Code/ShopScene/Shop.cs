using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Shop : MonoBehaviour {

	#region Serialized Fields

	public float shopEnterTime;

	#endregion


	#region Properties
	[field:SerializeField] public GameObject InitialButton { get; set; }
	public Coroutine RunEnterShop { get; set; }
	public Action OnGreeting {get;set;}
	public Action OnSaleMade { get; set; }
	public ShopController ShopController => GameManager.Instance.ShopController;

    #endregion


    #region Protected Variables

    protected EventInstance CurrentTutorialEventInstance;

	#endregion


	#region Mono Behaviours

	public virtual void Awake()
	{

	}

	public virtual void Start() {
		GameManager.Instance.InputController.OnSkip += Skip;
		AudioManager.Instance.OnVoiceLineOver += VoiceLineOver;		
	}

	public virtual void OnDestroy() {
		if (AudioManager.Instance != null) {
			AudioManager.Instance.OnVoiceLineOver -= VoiceLineOver;
		}
		if (GameManager.Instance != null) {
			GameManager.Instance.InputController.OnSkip -= Skip;
		}
	}

    public virtual IEnumerator EnterShop(bool enter)
    {
		if (enter)
        {
			yield return new WaitForSeconds(shopEnterTime);
			this.OnGreeting?.Invoke();			
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ShopEnter);
        }
        else
        {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ShopEnter);
			this.OnGreeting?.Invoke();
			yield return new WaitForSeconds(shopEnterTime);
        }
    }

    #endregion


    #region Public Methods

    public abstract void VoiceLineOver(bool skipped);

	public bool PlayNextTutotialVoiceOver(bool[] tutorialsCompleted, EventReference[] tutorialVoiceLines) {
		if(tutorialsCompleted == null) {
			return false;
		}
		for (int i = 0; i < tutorialsCompleted.Length; i++) {
			if (!tutorialsCompleted[i]) {
				AudioManager.Instance.PlayVoiceOver(tutorialVoiceLines[i]);
				this.CurrentTutorialEventInstance = AudioManager.Instance.VoiceLineEventInstance;
				return true;
			}
		}
		return false;
	}

	public void IncrementTutorial(bool[] tutorial) {
		if(tutorial == null) {
			return;
		}
		for (int i = 0; i < tutorial.Length; i++) {
			if (!tutorial[i]) {
				tutorial[i] = true;
				break;
			}
		}
	}

	public virtual void Skip() {

	}
    public virtual void LeaveShop()
    {
        this.RunEnterShop = StartCoroutine(EnterShop(false));
    }

    #endregion
}
