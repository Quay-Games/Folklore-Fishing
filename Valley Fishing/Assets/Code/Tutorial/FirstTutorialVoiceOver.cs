using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstTutorialVoiceOver : VoiceOverController
{
	[SerializeField] private float initialPracticeReelTime;
	[SerializeField] private float reelPracticeTime;
	[SerializeField] private float reelFailTime;
	[SerializeField] private float CurrentPracticeReelTime;
	private int LastReelLevel;


	public override void Start() {
		base.Start();
		AudioManager.Instance.PlayReelSound(FMODManager.Instance.ReelSound);
	}

	public void Update() {
		if (!this.ReelTutorialsCompleted[1]) {
			if (AudioManager.Instance.VoiceLineInProgress) {
				this.CurrentPracticeReelTime = 0;
				return;
			}
			if (GameManager.Instance.InputController.ReelLevel > 0) {
				if (this.CurrentPracticeReelTime < initialPracticeReelTime) {
					this.CurrentPracticeReelTime += Time.deltaTime;
				} else {
					this.CurrentPracticeReelTime = 0;
					PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
					IncrementTutorial(this.ReelTutorialsCompleted);
				}
			}
		}
		else if(GameManager.Instance.InputController.ReelLevel == this.LastReelLevel && GameManager.Instance.InputController.ReelLevel != 0) {
			if (GameManager.Instance.CurrentFish != null) {
				switch (GameManager.Instance.CurrentFish.CurrentActivityLevel) {
					case ActivityLevel.calm:
						CountReelInput(1);
						break;
					case ActivityLevel.medium:
						CountReelInput(2);
						break;
					case ActivityLevel.active:
						CountReelInput(3);
						break;
					default:
						break;
				}
			}
			if (this.ReelTutorialsCompleted[4]) {
				return;
			}
			if (!this.ReelTutorialsCompleted[2]) {
				CountReelInput(3);
			} else if (!this.ReelTutorialsCompleted[3]) {
				CountReelInput(1);
			} else if (!this.ReelTutorialsCompleted[4]) {
				CountReelInput(2);
			}
		} else {
			this.CurrentPracticeReelTime = 0;
		}
		this.LastReelLevel = GameManager.Instance.InputController.ReelLevel;
	}

	public override bool PerformStateSwitch() {
		if(this.LevelController.CurrentState == LevelController.State.Idle) {
			if (this.CastRodTutorialsCompleted[2] && !this.CastRodTutorialsCompleted[3]) {
				PlayNextTutotialVoiceOver(this.CastRodTutorialsCompleted, castRodTutorials);
				IncrementTutorial(this.CastRodTutorialsCompleted);
				return false;
			}
		}
		if (!base.PerformStateSwitch()) {
			return false;
		}
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Idle:
				if (!this.ReelTutorialsCompleted[0]) {
					PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
					IncrementTutorial(this.ReelTutorialsCompleted);
					LevelController.SetState(LevelController.State.ReelingFish);
				} else {
					PlayNextTutotialVoiceOver(this.CastRodTutorialsCompleted, castRodTutorials);
					IncrementTutorial(this.CastRodTutorialsCompleted);
				}
				break;
			case LevelController.State.AttatchBait:
				break;
			case LevelController.State.IdleWithBait:
				if (this.ReelTutorialsCompleted[4]) {
					PlayNextTutotialVoiceOver(this.CastRodTutorialsCompleted, castRodTutorials);
					IncrementTutorial(this.CastRodTutorialsCompleted);
				}
				break;
			case LevelController.State.ReelingFish:
				if (this.ReelTutorialsCompleted[3]) {
					PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
					IncrementTutorial(this.ReelTutorialsCompleted);
				}
				break;
			case LevelController.State.FishCaught:
				if (AllTutorialsCompleted(this.CaughtFishTutorialsCompleted) || !GameManager.Instance.CurrentFish.IsTutorial) {
					if (InventoryManager.Instance.TotalOwnedFish == 8) {
						List<EventReference> voiceLines = new List<EventReference>();
						voiceLines.Add(this.CurrentFish.FishData.fishCatchAudio);
						voiceLines.Add(FMODManager.Instance.LeaveBoatPrompts[0]);
						AudioManager.Instance.PlayVoiceOverChain(voiceLines);
						this.LevelController.StateLocked = true;
						break;
					}
					AudioManager.Instance.PlayVoiceOver(this.CurrentFish.FishData.fishCatchAudio);
					this.CurrentTutorialEventInstance = AudioManager.Instance.VoiceLineEventInstance;

				} else {
					PlayNextTutotialVoiceOver(this.CaughtFishTutorialsCompleted, tutorialCatchVoices);
					IncrementTutorial(this.CaughtFishTutorialsCompleted);
				}
				if (this.CaughtFishTutorialsCompleted[2] && InventoryManager.Instance.TotalOwnedFish == 3) {
					InventoryManager.Instance.OwnedBaitTypeDatas[3].quantity = 5;
					break;
				}
				if(InventoryManager.Instance.TotalOwnedFish > 3) {
					break;
				}
				if (this.CaughtFishTutorialsCompleted[1]) {
					InventoryManager.Instance.OwnedBaitTypeDatas[2].quantity++;
					break;
				}
				if (this.CaughtFishTutorialsCompleted[0]) {
					InventoryManager.Instance.OwnedBaitTypeDatas[1].quantity++;
					break;
				}
				break;
			default:
				break;
		}
		return true;
	}
	public override void VoiceOverFinished(bool skipped) {
		if (LevelController.CurrentState == LevelController.State.FishCaught) {
			if (InventoryManager.Instance.TotalOwnedFish == 8) {
				SceneManager.LoadScene(LevelManager.ShopTutorial_00);
				return;
			}
		}
		if (LevelController.CurrentState == LevelController.State.Idle) {
			if (!this.AttatchBaitTutorialsCompleted[0] && this.ReelTutorialsCompleted[3]) {
				PlayNextTutotialVoiceOver(this.AttatchBaitTutorialsCompleted, applyBaitTutorials);
				IncrementTutorial(this.AttatchBaitTutorialsCompleted);
				return;
			}
		}
		base.VoiceOverFinished(skipped);
		if (LevelController.CurrentState == LevelController.State.ReelingFish) {
			if (!this.AttatchBaitTutorialsCompleted[0] && this.ReelTutorialsCompleted[4]) {
				PlayNextTutotialVoiceOver(this.AttatchBaitTutorialsCompleted, applyBaitTutorials);
				IncrementTutorial(this.AttatchBaitTutorialsCompleted);
				return;
			}
			GameManager.Instance.InputController.SetState(InputController.State.NotReeling);
		}
	}

	private void CountReelInput(int desiredReelLevel) {
		if (AudioManager.Instance.VoiceLineInProgress) {
			this.CurrentPracticeReelTime = 0;
			return;
		}
		if (GameManager.Instance.InputController.ReelLevel == desiredReelLevel) {
			if (this.CurrentPracticeReelTime < reelPracticeTime) {
				this.CurrentPracticeReelTime += Time.deltaTime;
			} else {
				this.CurrentPracticeReelTime = 0;
				if (this.ReelTutorialsCompleted[3] && !this.ReelTutorialsCompleted[4]) {
					InventoryManager.Instance.OwnedBaitTypeDatas[0].quantity = 1;
					LevelController.SetState(LevelController.State.Idle);
					return;
				}
				if (GameManager.Instance.CurrentFish == null) {
					PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
					IncrementTutorial(this.ReelTutorialsCompleted);
				}
			}
		} else {
			if (this.CurrentPracticeReelTime < reelFailTime) {
				this.CurrentPracticeReelTime += Time.deltaTime;
			} else {
				if (GameManager.Instance.InputController.ReelLevel < desiredReelLevel) {
					AudioManager.Instance.PlayVoiceOver(tooSlowPrompt);
				} else {
					AudioManager.Instance.PlayVoiceOver(tooFastPrompt);
				}
				this.CurrentPracticeReelTime = 0;
			}
		}
	}
}
