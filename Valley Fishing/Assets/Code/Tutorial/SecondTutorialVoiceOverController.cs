using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondTutorialVoiceOverController : VoiceOverController
{
	public override bool PerformStateSwitch() {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Idle:
				LevelController.SetState(LevelController.State.AttatchBait);
				break;
			case LevelController.State.IdleWithBait:
				PlayNextTutotialVoiceOver(this.CastRodTutorialsCompleted, castRodTutorials);
				IncrementTutorial(this.CastRodTutorialsCompleted);
				break;
			case LevelController.State.ReelingFish:
				this.CurrentFish.BecameCentered += BecameCentered;
				if (this.ReelTutorialsCompleted[0] == false) {
					this.CurrentFish.FishData.MovementDirections.Clear(); ;
					this.CurrentFish.FishData.MovementDirections.Add(MovementDirection.LEFT);
					this.CurrentFish.CurrentActivityLevel = ActivityLevel.active;
				} else if (this.ReelTutorialsCompleted[1] == false) {
					this.CurrentFish.FishData.MovementDirections.Clear(); ;
					this.CurrentFish.FishData.MovementDirections.Add(MovementDirection.RIGHT);
					this.CurrentFish.CurrentActivityLevel = ActivityLevel.calm;
				} else if (this.ReelTutorialsCompleted[2] == false) {
					PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
					IncrementTutorial(this.ReelTutorialsCompleted);
				}
				break;
			case LevelController.State.FishCaught:
				if (InventoryManager.Instance.TotalOwnedBaits == 0) {
					this.LevelController.StateLocked = true;
					List<EventReference> voiceOverChain = new List<EventReference>();
					voiceOverChain.Add(this.CurrentFish.FishData.fishCatchAudio);
					voiceOverChain.Add(FMODManager.Instance.LeaveBoatPrompts[0]);
					AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
				} else {
					AudioManager.Instance.PlayVoiceOver(this.CurrentFish.FishData.fishCatchAudio);
				}
				break;
			default:
				break;
		}
		if (!base.PerformStateSwitch()) {
			return false;
		}
		return true;
	}

	public override void FishStrafed(MovementDirection movementDirection) {
		if (AllTutorialsCompleted(this.ReelTutorialsCompleted)) {
			return;
		}
		PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
		IncrementTutorial(this.ReelTutorialsCompleted);
	}

	public override void FishSpawned() {
		if (this.ReelTutorialsCompleted[0] == false) {
			this.CurrentFish.FishData.ActivityLevels.Clear();
			this.CurrentFish.FishData.ActivityLevels.Add(ActivityLevel.active);
		} else if (this.ReelTutorialsCompleted[1] == false) {
			this.CurrentFish.FishData.ActivityLevels.Clear();
			this.CurrentFish.FishData.ActivityLevels.Add(ActivityLevel.calm);
		}
	}

	public override void VoiceOverFinished(bool skipped) {

		base.VoiceOverFinished(skipped);
		if (LevelController.CurrentState == LevelController.State.ReelingFish) {
			GameManager.Instance.InputController.SetState(InputController.State.NotReeling);
		}
		if(this.LevelController.CurrentState == LevelController.State.FishCaught) {
			if (this.LevelController.StateLocked) {
				SceneManager.LoadScene(LevelManager.ShopTutorial_01);
			}
		}
	}

	private void BecameCentered() {
		GameManager.Instance.InputController.SetState(InputController.State.NotReeling);
	}
}
