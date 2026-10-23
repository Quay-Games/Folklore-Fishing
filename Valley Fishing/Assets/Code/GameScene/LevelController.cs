using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelController : AbstractState<LevelController.State> {

	#region Serialized Fields
	
	[SerializeField] private EventSystem eventSystem;
	[SerializeField] private EventReference levelMusic;
	[SerializeField] private EventReference levelAmbience;
	[SerializeField] private Transform gameplayContainer;
	[SerializeField] public bool isTutorialLevel;

	#endregion


	#region Properties

	[field: SerializeField] public VoiceOverController VoiceOverController;
	[field: SerializeField]	public FishView FishView { get; set; }
	[field: SerializeField] public BaitView BaitView { get; set; }
	[field: SerializeField] public Transform LeftStrafeTransform { get; set; }
	[field: SerializeField] public Transform RightStrafeTransform { get; set; }
	[field: SerializeField] public Transform FishSpawnTransform { get; set; }
	public BaitDatas.BaitData CurrentBait { get => InventoryManager.Instance.CurrentBait; }
	//The equivalent of an event describing when the fish bites
	public Action OnFishSpawned { get; set; }
	public Action OnFishBit { get; set; }
	public Action OnFishCaught { get; set; }

	#endregion


	#region Abstract State

	public enum State {
		Default,
		Cutscene,
		Idle,
		AttatchBait,
		IdleWithBait,
		WaitingForBite,
		ReelingFish,
		FishCaught
	}

	protected override void EnterState(State state) {
		switch (state) {
			case State.Default:
				GameManager.Instance.LevelController = this;
				GameManager.Instance.EventSystem = eventSystem;
				AudioManager.Instance.PlayMusic(levelMusic);
				AudioManager.Instance.PlayAmbience(levelAmbience);
				if(isTutorialLevel)
                {
                    SetState(State.Cutscene);
                }
                else
                {
                    SetState(State.Idle);
                }
				break;
			case State.Cutscene:
				break;
			case State.Idle:
				SetState(State.AttatchBait);
				break;
			case State.AttatchBait:
				GameManager.Instance.LevelController = this;
				this.BaitView.EnableBaitUI(true);
				break;
			case State.IdleWithBait:
				break;
			case State.WaitingForBite:
				SpawnFish();
				break;
			case State.ReelingFish:
				GameManager.Instance.InputController.SetState(InputController.State.NotReeling);
				break;
			case State.FishCaught:	
				GameManager.Instance.EventController.FishCaught();
				this.FishView.EnableFishUI(true);
				break;
		}
	}
	#endregion


	#region Private Methods

	private IEnumerator WaitForBite() {
		yield return new WaitForSeconds(3); // TODO floating number??
		SetState(State.ReelingFish);
		OnFishBit?.Invoke();
		GameManager.Instance.CurrentFish.Initialize();
	}

	private void SpawnFish() {
		int fishIndex = 0;
		float randomValue = UnityEngine.Random.value;
		float cumulative = 0f;
;		for (int i = 0; i < this.CurrentBait.FishSpawnChances.Count; i++) {
			cumulative += this.CurrentBait.FishSpawnChances[i].SpawnChance;
			if (randomValue < cumulative) {
				fishIndex = (int)this.CurrentBait.FishSpawnChances[i].FishName;
				break;
			}
		}
		Fish fishInstance = Instantiate(GameManager.Instance.FishPrefab, this.FishSpawnTransform.position, Quaternion.identity);
		fishInstance.FishData = InventoryManager.Instance.FishDatas.Datas[fishIndex];
		fishInstance.transform.parent = gameplayContainer;
		fishInstance.IsTutorial = this.CurrentBait.IsTutorial;
		InventoryManager.Instance.CurrentBait = null;
		GameManager.Instance.CurrentFish = fishInstance;
		GameManager.Instance.CurrentFish.OnCatch += NotifyOfCaughtFish;
		this.OnFishSpawned?.Invoke();
		StartCoroutine(WaitForBite());
	}

	#endregion

	//You may think, surely this isnt necessary, why dont people just subscribe to the Fish's event? Well, such places would be able
	//to do that for single fish, but for successive fish they would lose the listener, because it wont know to attach a new listener
	//to the new fish, but the level controller knows when new fish spawn and despawn, so it can take care of that responsibility
	public void NotifyOfCaughtFish()
	{
		GameManager.Instance.CurrentFish.OnCatch -= NotifyOfCaughtFish;
		OnFishCaught?.Invoke();
	}
}
