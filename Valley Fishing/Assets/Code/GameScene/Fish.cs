using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fish : MonoBehaviour
{

    #region States

    public FishDatas.FishData.ActivityLevel CurrentActivityLevel { get; set; }

	#endregion


	#region Serialized Fields
	[SerializeField] private float centreThreshold;
    [SerializeField] private Rigidbody rb;
	[SerializeField] private GameObject visuals;
	[SerializeField] private GameObject[] activityParticles;
	[SerializeField] private StudioEventEmitter activitySplashSFX;
	[SerializeField] private float reelStart;
	[SerializeField] private float reelEnd;
	[SerializeField] private float activityLevelChangeTime;
	[SerializeField] private float correctActivityChangeTime;
	[SerializeField] private float movementChangeTime;
	[SerializeField] private GameObject strafeAudio;
	[SerializeField] private Transform lineEnd;
	[SerializeField] private float unspoolRate;
	[SerializeField] private float uiAppearTime;

	#endregion


	#region Properties

	[field: SerializeField] public FishDatas.FishData FishData;
	public bool IsStrafer { get; set; }
    public int FishIndex { get; set; }
    private bool IsCentred { get; set; } = true;
	private InputController InputController { get => GameManager.Instance.InputController; }
	private float FailedCatchTime { get; set; }
	private bool CatchStarted { get; set; }

	private List<float> ActivityLevelIntervals = new List<float>();

	private List<float> MovementIntervals = new List<float>();
	private int MovementIntervalsCompleted { get; set; }
	private float CurrentLevelActivityChangeTime;
	private float CurrentCorrectActivityChangeTime;
	private bool ActivityLevelChanging;
	private float CurrentMovemetChangeTime;
	private bool MovementDirectionChanging;
	private bool IsUnspooling = true;
	public StudioEventEmitter ActivitySplashSFX { get => activitySplashSFX; }
	public Transform LineEnd { get => lineEnd; }
	public bool IsTutorial { get; set; }
	public Action OnInitialized { get; set; }
	public Action<FishDatas.FishData.MovementDirection> Strafe { get; set; }
	public Action BecameCentered { get; set; }
	public RodLineComponent RodLineComponent { get; set; }
	[field:SerializeField] public float ReelSpeed { get; set; }
	public bool HitBySeagull { get; set; }
	public bool Caught { get; set; }
	public bool HitByFlies { get; set; }

	#endregion


	#region Mono Behaviours

	public void Update() {
		if (this.Caught) {
			return;
		}
        HandleCentering();
		if(this.InputController.ReelLevel > 0) {
			this.CatchStarted = true;
		}
		if (this.FishData.IsFailable) {
			if (this.FailedCatchTime > 10) {
				FailedCatch();
			}
		}
		if (DistanceAsPercentage() < 0.01f) {
			AudioManager.Instance.PlayUnspoolSound(false, 0);
		}
		ChangeActivityLevels();
		ChangeMovementDirection();
	}


    public void FixedUpdate() {
		if (this.Caught) {
			return;
		}
		if (this.HitBySeagull) {
			ReelingSuccesfully(false);
			return;
		}
		Reel();
		Move();
    }
	#endregion


	#region Public Methods

	public void Initialize() {
		float activityLevelInterval = (reelStart - reelEnd) / this.FishData.ActivityLevels.Count;
		for (int i = 0; i < this.FishData.ActivityLevels.Count; i++) {
			this.ActivityLevelIntervals.Add(reelStart - (i * activityLevelInterval));
		}
		float movementInterval = (reelStart - reelEnd) / this.FishData.MovementDirections.Count;
		bool isStrafer = false;
		for (int i = 0; i < this.FishData.MovementDirections.Count; i++) {
			if(this.FishData.MovementDirections[i]!= FishDatas.FishData.MovementDirection.none) {
				isStrafer = true;
			}
			this.MovementIntervals.Add(reelStart - (i * movementInterval));
		}
		this.IsStrafer = isStrafer;
		SetActivityLevel(FishData.ActivityLevels[0]);
		GameManager.Instance.EventController.NewFishSpawned();
		AudioManager.Instance.PlayUnspoolSound(false, 0);
	}

	public void FishCaught() {
		if (this.Caught) {
			return;
		}
		this.Caught = true;
		AudioManager.Instance.PlayUnspoolSound(false, 0);
		InventoryManager.Instance.OwnedFishTypeDatas[(System.Array.IndexOf(InventoryManager.Instance.FishDatas.Datas, this.FishData))].quantity++;		
		AudioManager.Instance.PlayFishActivitySound(this, 0, true);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.CatchSplash);
		//Griff: maybe this shouldnt be here
		GameManager.Instance.InputController.SetState(InputController.State.ReelingLocked);
		VibrationManager.Instance.SetVibrationFrequency(true, 0, Mathf.Infinity);
		VibrationManager.Instance.SetVibrationFrequency(false, 0, Mathf.Infinity);
		StartCoroutine(SetCaughtFishState());
		
	}

	#endregion


	#region Private Methods

	private IEnumerator SetCaughtFishState() {
		yield return new WaitForSeconds(uiAppearTime);
		GameManager.Instance.LevelController.SetState(LevelController.State.FishCaught);
		Destroy(gameObject);
	}

	private void Move() {
        rb.AddForce(this.InputController.HorizontalInput.x * this.FishData.ReelInSpeed, 0, 0);
    }

    private void Reel() {
		if (this.ActivityLevelChanging) {
			return;
		}
		if (this.MovementDirectionChanging) {
			return;
		}
        switch (this.CurrentActivityLevel) {
            case FishDatas.FishData.ActivityLevel.none:
				break;
            case FishDatas.FishData.ActivityLevel.calm:
				ReelingSuccesfully(this.InputController.CurrentState == InputController.State.CalmReeling);
				break;
            case FishDatas.FishData.ActivityLevel.medium:
				ReelingSuccesfully(this.InputController.CurrentState == InputController.State.NormalReeling);
				break;
            case FishDatas.FishData.ActivityLevel.active:
				ReelingSuccesfully(this.InputController.CurrentState == InputController.State.FastReeling);
				break;
        }
    }

	private void ReelingSuccesfully(bool reelingSuccesfully) {
		this.RodLineComponent.IsStraight = reelingSuccesfully;
		if (reelingSuccesfully) {
			this.RodLineComponent.CurveAmount = Mathf.MoveTowards(this.RodLineComponent.CurveAmount, 0f, Time.deltaTime);
			if (!this.IsCentred) {
				return;
			}
			VibrationManager.Instance.SetVibrationFrequency(true, DistanceAsPercentage(), Mathf.Infinity);
			VibrationManager.Instance.SetVibrationFrequency(false, 0, Mathf.Infinity);
			rb.AddForce(0, 0, -this.ReelSpeed);
			AudioManager.Instance.PlayUnspoolSound(false, 0);
			this.IsUnspooling = false;
		} else {
			this.RodLineComponent.CurveAmount = Mathf.MoveTowards(this.RodLineComponent.CurveAmount, 1, Time.deltaTime);
			IncreaseFailTime();
			VibrationManager.Instance.SetVibrationFrequency(true, 0, Mathf.Infinity);
			if (GameManager.Instance.LevelController.FishSpawnTransform.position.z - transform.position.z < 0) {
				this.IsUnspooling = false;
				return;
			}
			if (this.IsCentred) {
				rb.AddForce(0, 0, this.FishData.SwimAwaySpeed);
			} else {
				rb.AddForce(0, 0, this.FishData.StrafedSwimAwaySpeed);
			}
			if (!this.IsUnspooling) {
				AudioManager.Instance.PlayUnspoolSound(true, 1);
			}
			this.IsUnspooling = true;
		}
	}

	private void ChangeActivityLevels() {
		for (int i = this.ActivityLevelIntervals.Count - 1; i >= 0; i--) {
			if (transform.position.z < (this.ActivityLevelIntervals[i])) {
				if (this.CurrentActivityLevel != this.FishData.ActivityLevels[i]) {
					SetActivityLevel(this.FishData.ActivityLevels[i]);
					GameManager.Instance.InputController.PauseReelSFX(correctActivityChangeTime * 2);
					if (i != 0) {
						this.ActivityLevelChanging = true;
					}
				}
				break;
			}
		}
		if (this.ActivityLevelChanging) {
			if (this.CurrentLevelActivityChangeTime <= activityLevelChangeTime) {
				this.CurrentLevelActivityChangeTime += Time.deltaTime;
				if (GameManager.Instance.InputController.ReelLevel == (int)this.CurrentActivityLevel) {
					if (this.CurrentCorrectActivityChangeTime < correctActivityChangeTime) {
						this.CurrentCorrectActivityChangeTime += Time.deltaTime;
					} else {
						this.CurrentCorrectActivityChangeTime = 0;
						this.CurrentLevelActivityChangeTime = 0;
						this.ActivityLevelChanging = false;
					}
				}
			} else {
				this.CurrentCorrectActivityChangeTime = 0;
				this.CurrentLevelActivityChangeTime = 0;
				this.ActivityLevelChanging = false;
			}
		}
	}

	private void ChangeMovementDirection() {
		for (int i = this.MovementIntervals.Count - 1; i >= 0; i--) {
			if (transform.position.z < (this.MovementIntervals[i])) {
				if (this.MovementIntervalsCompleted > i) {
					break;
				}
				if(this.FishData.MovementDirections[i] != FishDatas.FishData.MovementDirection.none) {
					SetStrafePosition(this.FishData.MovementDirections[i]);
					this.MovementDirectionChanging = true;
				}
				this.MovementIntervalsCompleted++;
			}
		}
		if (this.MovementDirectionChanging) {
			if (this.CurrentMovemetChangeTime <= movementChangeTime) {
				this.CurrentMovemetChangeTime += Time.deltaTime;
			} else {
				this.CurrentMovemetChangeTime = 0;
				this.MovementDirectionChanging = false;
			}
		}
	}

	private void HandleCentering() {
		if (transform.position.x > -centreThreshold && transform.position.x < centreThreshold) {
			if (!this.IsCentred) {
				this.BecameCentered?.Invoke();
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.CorrectSFX);
			}
			this.IsCentred = true;
		} else {
			this.IsCentred = false;
		}
		if(strafeAudio.activeSelf == this.IsCentred) {
			strafeAudio.SetActive(!this.IsCentred);
		}
	}

	private void IncreaseFailTime() {
		if (!this.FishData.IsFailable) {
			return;
		}
		if (this.CatchStarted) {
			this.FailedCatchTime += Time.deltaTime;
		}
	}

	private void FailedCatch() {
		AudioManager.Instance.PlayUnspoolSound(false, 0);
		GameManager.Instance.LevelController.SetState(LevelController.State.AttatchBait);
		AudioManager.Instance.PlayFishActivitySound(this, 0, false);
		VibrationManager.Instance.SetVibrationFrequency(true, 0, Mathf.Infinity);
		VibrationManager.Instance.SetVibrationFrequency(false, 0, Mathf.Infinity);
		Destroy(gameObject);
	}

	private void SetActivityLevel(FishDatas.FishData.ActivityLevel activityLevel) {
		this.CurrentActivityLevel = activityLevel;
		for (int i = 0; i < activityParticles.Length; i++) {
			activityParticles[i].SetActive(false);
		}
		GameManager.Instance.InputController.ResetReelInput();
		switch (this.CurrentActivityLevel) {
			case FishDatas.FishData.ActivityLevel.none:
				AudioManager.Instance.PlayFishActivitySound(this, 0, true);
				break;
			case FishDatas.FishData.ActivityLevel.calm:
				activityParticles[0].SetActive(true);
				AudioManager.Instance.PlayFishActivitySound(this, 1, true);
				break;
			case FishDatas.FishData.ActivityLevel.medium:
				activityParticles[1].SetActive(true);
				AudioManager.Instance.PlayFishActivitySound(this, 2, true);
				break;
			case FishDatas.FishData.ActivityLevel.active:
				activityParticles[2].SetActive(true);
				AudioManager.Instance.PlayFishActivitySound(this, 3, true);
				break;
			default:
				break;
		}
	}

	private void SetStrafePosition(FishDatas.FishData.MovementDirection movementDirection) {
		switch (movementDirection) {
			case FishDatas.FishData.MovementDirection.none:
				break;
			case FishDatas.FishData.MovementDirection.left:
				Strafe?.Invoke(movementDirection);
				transform.position = new Vector3(GameManager.Instance.LevelController.LeftStrafeTransform.position.x,transform.position.y,transform.position.z);
				break;
			case FishDatas.FishData.MovementDirection.right:
				Strafe?.Invoke(movementDirection);
				transform.position = new Vector3(GameManager.Instance.LevelController.RightStrafeTransform.position.x, transform.position.y, transform.position.z);
				break;
			default:
				break;
		}
	}

	private float DistanceAsPercentage() {
		float totalLength = reelStart - reelEnd;
		float distanceAlongLength = transform.position.z - reelStart;
		return -(distanceAlongLength / totalLength)/2;
	}

	#endregion
}
