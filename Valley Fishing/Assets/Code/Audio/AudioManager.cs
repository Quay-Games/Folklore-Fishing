using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class AudioManager : Singleton<AudioManager>
{

	#region Properties

	public EventInstance MusicEventInstance;
	public EventInstance AmbienceEventInstance;
	public EventInstance VoiceLineEventInstance;
	public List<EventReference> LastVoiceLine = new List<EventReference>();
	public EventInstance CurrentReelInstance;
	public EventInstance BaitEventInstance;
	public EventInstance UnspoolEventInstance;
	public EventInstance FliesEventInstance;

	[field:SerializeField]
	public StudioEventEmitter FishActivityLevelInstance { get; set;	}
	private List<EventInstance> SFXEventInstances {	get; set; } = new List<EventInstance>();
    public Action<bool> OnVoiceLineOver { get; set; }
	public Action OnVoiceLineStarted { get; set; }
	[field: SerializeField] public bool VoiceLineInProgress { get; set;	}
	public List<EventReference> VoiceOverChain { get; set; } = new List<EventReference>();
	public int VoiceOverChainPosition {	get; set; }
	[field:SerializeField] public bool InVoiceOverChain { get; set;	}
	private bool CanSkip { get; set; } = true;
	private bool Paused { get {
			bool paused;
			gameplayBus.getPaused(out paused);
			return paused;
		} }
	FMOD.Studio.Bus gameplayBus;

	#endregion


	#region Mono Behaviours

	public override void Awake() {
		base.Awake();
		gameplayBus = FMODUnity.RuntimeManager.GetBus("bus:/Gameplay");
	}

	public void OnDestroy() {
		CleanUpSFX();
		CleanUpMusic();
	}

	#endregion


	#region Public Methods
	public void PlayOneShot(EventReference sound, Vector3 position = default) {
		RuntimeManager.PlayOneShot(sound, position);
	}

	public void PlayBaitSound(bool play, int index) {
		if (play) {
			this.BaitEventInstance = CreateSFXInstance(FMODManager.Instance.BaitSounds[index]);
			this.BaitEventInstance.start();
		} else {
			this.BaitEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			this.BaitEventInstance.release();
		}
	}

	public void PlayUnspoolSound(bool play, float pitch) {
		if (play) {
			this.UnspoolEventInstance = CreateSFXInstance(FMODManager.Instance.Unspool);
			this.UnspoolEventInstance.setParameterByName("Pitch", pitch);
			this.UnspoolEventInstance.start();
		} else {
			this.UnspoolEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			this.UnspoolEventInstance.release();
		}
	}

	public void PlayFliesSound(bool play) {
		if (play) {
			this.FliesEventInstance = CreateSFXInstance(FMODManager.Instance.FliesWarning);
			this.FliesEventInstance.start();
		} else {
			this.FliesEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			this.FliesEventInstance.release();
		}
	}

	public void PlayVoiceOver(EventReference voiceLineReference) {		
		if (!this.InVoiceOverChain) {
			List<EventReference> voicelines = new List<EventReference>();
			voicelines.Add(voiceLineReference);
			this.LastVoiceLine.Clear();
			this.LastVoiceLine = voicelines;
		}
		if (this.VoiceLineEventInstance.isValid()) {			
			this.VoiceLineEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			this.VoiceLineEventInstance.release();
			this.VoiceLineEventInstance.clearHandle();
		}
		this.VoiceLineEventInstance = CreateSFXInstance(voiceLineReference);
		if (InputManager.Instance.CurrentDevice is Gamepad) {
			{
				Gamepad gamepad = InputManager.Instance.CurrentDevice as Gamepad;
				if (gamepad.layout == "XInputController" || gamepad.layout == "XInputControllerWindows" || gamepad.layout == "XboxGamepadMacOS" || gamepad.layout == "XboxOneGamepadMacOSWireless" || gamepad.layout == "XboxOneGamepadiOS") {
					this.VoiceLineEventInstance.setParameterByName("ControlScheme", 0);
				}
				else if (gamepad.layout == "DualShock3GamepadHID" || gamepad.layout == "DualShock4GamepadHID" || gamepad.layout == "DualShock4GamepadiOS" || gamepad.layout == "DualSenseGamepadHID") {
					this.VoiceLineEventInstance.setParameterByName("ControlScheme", 1);
				}
				else if (Gamepad.current.layout == "SwitchProControllerHID") {
					this.VoiceLineEventInstance.setParameterByName("ControlScheme", 2);
				}
			}
		}
		if(InputManager.Instance.CurrentDevice is Keyboard) {
            this.VoiceLineEventInstance.setParameterByName("ControlScheme", 3);
        }

		this.VoiceLineEventInstance.start();
		this.VoiceLineInProgress = true;
		this.OnVoiceLineStarted?.Invoke();
		StartCoroutine(WaitForVoiceLineEnd());
	}

	public void SkipVoiceOver() {
		if (!this.CanSkip) {
			return;
		}
		this.VoiceLineEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		this.VoiceLineEventInstance.release();
		this.VoiceLineEventInstance.clearHandle();
		this.VoiceLineInProgress = false;
		if (!this.Paused) {
			this.OnVoiceLineOver?.Invoke(true);
		}
		if (this.InVoiceOverChain) {
			this.VoiceOverChainPosition = this.VoiceOverChain.Count;
			this.InVoiceOverChain = false;
		}
		StopCoroutine(WaitForVoiceLineEnd());
	}

	public void DisableSkipping() {
		StartCoroutine(RunDisableSkipping());
	}

	public void PlayVoiceOverChain(List<EventReference> voiceOverChain) {
		this.VoiceOverChainPosition = 0;
		this.VoiceOverChain = voiceOverChain;		
		this.InVoiceOverChain = true;
		this.LastVoiceLine = this.VoiceOverChain;
		PlayVoiceOver(voiceOverChain[0]);
	}

	public void ReplayVoiceLine() {
		this.VoiceLineEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		this.VoiceLineEventInstance.release();
		this.VoiceLineEventInstance.clearHandle();
		this.VoiceLineInProgress = false;
		if (this.InVoiceOverChain) {
			this.VoiceOverChainPosition = this.VoiceOverChain.Count;
			this.InVoiceOverChain = false;
		}
		StopCoroutine(WaitForVoiceLineEnd());
		if (this.LastVoiceLine.Count == 1) {
			PlayVoiceOver(this.LastVoiceLine[0]);
		}
		if (this.LastVoiceLine.Count > 1) {
			PlayVoiceOverChain(this.LastVoiceLine);
		}
	}

	public void PlayFishActivitySound(Fish fish, int activityLevel, bool play) {
		this.FishActivityLevelInstance = fish.ActivitySplashSFX;
		this.FishActivityLevelInstance.SetParameter("ActivityLevel", activityLevel);
		if (this.FishActivityLevelInstance.IsPlaying()) {
			StartCoroutine(RunQuietReelSound());
		}
		if (play && !this.FishActivityLevelInstance.IsPlaying()) {
			this.FishActivityLevelInstance.Play();
		} else if(!play) {
			this.FishActivityLevelInstance.Stop();
		}
	}

	public void PlayReelSound(EventReference reelSound) {
		this.CurrentReelInstance = CreateSFXInstance(reelSound);
		int reelSpeed = 0;
		this.CurrentReelInstance.setParameterByName("ActivityLevel", reelSpeed);
		this.CurrentReelInstance.start();
	}

	public void SetReelRate(float reelSpeed) {
		this.CurrentReelInstance.setParameterByName("ActivityLevel", reelSpeed);
	}

	public EventInstance CreateSFXInstance(EventReference eventReference) {
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
		SFXEventInstances.Add(eventInstance);
		return eventInstance;
	}

	//public EventInstance CreateMusicInstance(EventReference eventReference) {
	//	EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
	//	//MusicEventInstances.Add(eventInstance);
	//	return eventInstance;
	//}
 //   public EventInstance CreateAmbienceInstance(EventReference eventReference)
 //   {
 //       EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
 //       //AmbienceEventInstance.Add(eventInstance);
 //       return eventInstance;
 //   }

    public void InitializeMusic(EventReference musicEventReference) {
        CleanUpMusic();
		this.MusicEventInstance = RuntimeManager.CreateInstance(musicEventReference);
        this.MusicEventInstance.start();
	}

	public void InitializeAmbience(EventReference ambienceEventReference)
	{
		CleanUpAmbience();
		this.AmbienceEventInstance = RuntimeManager.CreateInstance(ambienceEventReference);
		this.AmbienceEventInstance.start();
    }

	public void SetMusicParameter(string name, float value) {
		this.MusicEventInstance.setParameterByName(name, value);
	}

	public void CleanUpEverything() {
		CleanUpSFX();
		CleanUpMusic();
		CleanUpAmbience();
		CleanUpVoiceOver();
	}

	#endregion


	#region Private Methods

	private void CleanUpSFX() {
		foreach (EventInstance eventInstance in SFXEventInstances) {
			eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			eventInstance.release();
		}
	}

	private void CleanUpMusic() {
		//foreach (EventInstance eventInstance in MusicEventInstances) {
		MusicEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		MusicEventInstance.release();
		//}
	}

	public void CleanUpAmbience()
	{
		AmbienceEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		AmbienceEventInstance.release();
	}

	private void CleanUpVoiceOver() {
		this.VoiceLineEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		this.VoiceLineEventInstance.release();
	}

	private IEnumerator WaitForVoiceLineEnd() {
		this.CurrentReelInstance.setVolume(0.25f);
		PLAYBACK_STATE playbackState;
		while (true) {			
			if (!this.VoiceLineEventInstance.isValid()) {
				this.CurrentReelInstance.setVolume(1f);
				this.VoiceLineInProgress = false;
				yield break;
			}

			bool paused;
			this.VoiceLineEventInstance.getPaused(out paused);
			if (paused) {
				yield return null;
			}

			this.VoiceLineEventInstance.getPlaybackState(out playbackState);
			if (playbackState == PLAYBACK_STATE.STOPPED)
				break;

			yield return null;
		}
		this.VoiceLineEventInstance.release();
		this.VoiceLineEventInstance.clearHandle();
		this.CurrentReelInstance.setVolume(1f);
		if (this.InVoiceOverChain) {
			this.VoiceOverChainPosition++;
			if (this.VoiceOverChainPosition < this.VoiceOverChain.Count) {
				yield return new WaitForEndOfFrame();
				PlayVoiceOver(this.VoiceOverChain[this.VoiceOverChainPosition]);
			} else {
				if (!this.Paused) {
					this.InVoiceOverChain = false;
					this.OnVoiceLineOver?.Invoke(false);
				}
			}
		} else {
			if (!this.Paused) {
				this.VoiceLineInProgress = false;
				this.OnVoiceLineOver?.Invoke(false);
			}
		}
	}

	private IEnumerator RunDisableSkipping() {
		this.CanSkip = false;
		yield return new WaitForEndOfFrame();
		this.CanSkip = true;
	}

	private IEnumerator RunQuietReelSound() {
		this.CurrentReelInstance.setVolume(0.25f);
		yield return new WaitForSeconds(0.2f);
		this.CurrentReelInstance.setVolume(1f);
	}

	public void PlayMusic(EventReference musicReference) {
		InitializeMusic(musicReference);
	}

	public void PlayAmbience(EventReference ambienceReference)
	{
		InitializeAmbience(ambienceReference);
	}
}

	#endregion
