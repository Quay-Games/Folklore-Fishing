using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoiceOverController : MonoBehaviour
{

	#region Serialized Fields

	[SerializeField] protected EventReference[] applyBaitTutorials;
	[SerializeField] protected EventReference[] castRodTutorials;
	[SerializeField] protected EventReference[] reelTutorials;
	[SerializeField] protected EventReference[] tutorialCatchVoices;
	[SerializeField] protected EventReference tooFastPrompt;
	[SerializeField] protected EventReference tooSlowPrompt;
	[SerializeField] protected int levelIndex;

	#endregion


	#region Protected Variables

	protected EventInstance CurrentTutorialEventInstance;
	protected bool[] AttatchBaitTutorialsCompleted;
	[field:SerializeField] protected bool[] CastRodTutorialsCompleted;
	[field: SerializeField] protected bool[] ReelTutorialsCompleted;
	[field:SerializeField] protected bool[] CaughtFishTutorialsCompleted;

	#endregion


	#region Properties

	public LevelController LevelController => GameManager.Instance.LevelController;
	public Fish CurrentFish => GameManager.Instance.CurrentFish;

	#endregion


	#region Mono Behaviours

	public virtual void Start() {
		this.LevelController.OnStateChanged += LevelStateChanged;
		this.LevelController.OnFishSpawned += FishSpawned;
		AudioManager.Instance.OnVoiceLineOver += VoiceOverFinished;
		this.AttatchBaitTutorialsCompleted = new bool[applyBaitTutorials.Length];
		this.CastRodTutorialsCompleted = new bool[castRodTutorials.Length];
		this.ReelTutorialsCompleted = new bool[reelTutorials.Length];
		this.CaughtFishTutorialsCompleted = new bool[tutorialCatchVoices.Length];
		AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.IntroCutscenes[levelIndex]);
	}

	public virtual void OnDestroy() {
		if(this.LevelController == null) {
			return;
		}
		this.LevelController.OnStateChanged -= LevelStateChanged;
		this.LevelController.OnFishSpawned -= FishSpawned;
		AudioManager.Instance.OnVoiceLineOver -= VoiceOverFinished;
		if (this.CurrentFish != null) {
			this.CurrentFish.Strafe -= FishStrafed;
		}
	}

	#endregion


	#region Public Methods

	public void LevelStateChanged() {
		PerformStateSwitch();
	}

	public virtual bool PerformStateSwitch() {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Idle:
				if (AllTutorialsCompleted(this.AttatchBaitTutorialsCompleted)) {
					this.LevelController.SetState(LevelController.State.AttatchBait);
					return false;
				}
				return true;
			case LevelController.State.AttatchBait:
				return true;
			case LevelController.State.IdleWithBait:
				if (AllTutorialsCompleted(this.CastRodTutorialsCompleted)) {
					return false;
				}
				return true;
			case LevelController.State.WaitingForBite:
				break;
			case LevelController.State.ReelingFish:
				if (this.CurrentFish != null) {
					this.CurrentFish.Strafe -= FishStrafed;
					this.CurrentFish.Strafe += FishStrafed;
				}
				if (AllTutorialsCompleted(this.ReelTutorialsCompleted)) {
					return false;
				}
				return true;
			case LevelController.State.FishCaught:
				return true;
			default:
				return true;
		}
		return true;
	}

	#endregion


	#region Private Methods

	public bool AllTutorialsCompleted(bool[] tutorials) {
		bool allTutorialsComplete = true;
		if(tutorials.Length == 0) {
			return false;
		}
		for (int i = 0; i < tutorials.Length; i++) {
			if (!tutorials[i]) {
				allTutorialsComplete = false;
			}
		}
		return allTutorialsComplete;
	}

	public bool PlayNextTutotialVoiceOver(bool[] tutorialsCompleted, EventReference[] tutorialVoiceLines) {
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
		for (int i = 0; i < tutorial.Length; i++) {
			if (!tutorial[i]) {
				tutorial[i] = true;
				break;
			}
		}
	}

	public virtual void VoiceOverFinished(bool skipped) {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Cutscene:
				LevelController.SetState(LevelController.State.Idle);
				break;
			case LevelController.State.Idle:
				LevelController.SetState(LevelController.State.AttatchBait);
				break;
			default:
				break;
		}
	}

	public virtual void FishStrafed(MovementDirection movementDirection) {

	}

	public virtual void FishSpawned() {

	}

	#endregion

}
