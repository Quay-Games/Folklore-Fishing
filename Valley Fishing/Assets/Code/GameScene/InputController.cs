using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputController : AbstractState<InputController.State> {

	#region States

	public enum State {
		Default,
		ReelingLocked,
		NotReeling,
		CalmReeling,
		NormalReeling,
		FastReeling
	}

	protected override void EnterState(State state) {
		switch (state) {
			case State.Default:
				GameManager.Instance.InputController = this;
				this.InputTypes = new List<InputType> { keyboard, controller };
				StartCoroutine(EnableSceneSwitching());	
				//why is this set here?
				SetState(State.ReelingLocked);
				break;
			case State.ReelingLocked:
				controller.ReelSpeed = 0;
				controller.ReelInput = 0;
				keyboard.ReelSpeed = 0;
				controller.ReelInput = 0;
				this.ReelLevel = 0;
				break;
			case State.NotReeling:
				this.ReelLevel = 0;
				break;
			case State.CalmReeling:
				this.ReelLevel = 1;
				break;
			case State.NormalReeling:
				this.ReelLevel = 2;
				break;
			case State.FastReeling:
				this.ReelLevel = 3;
				break;
			default:
				break;
		}
	}

	protected override void UpdateState(State state) {
		switch (state) {
			case State.ReelingLocked:
				break;
			case State.NotReeling:
				if (InputManager.Instance.CurrentDevice is Gamepad) {
					ReelController();
				}
				if (InputManager.Instance.CurrentDevice is Keyboard) {
					ReelKeyboard();
				}
				break;
			case State.CalmReeling:
				if (InputManager.Instance.CurrentDevice is Gamepad) {
					ReelController();
				}
				if (InputManager.Instance.CurrentDevice is Keyboard) {
					ReelKeyboard();
				}
				break;
			case State.NormalReeling:
				if (InputManager.Instance.CurrentDevice is Gamepad) {
					ReelController();
				}
				if (InputManager.Instance.CurrentDevice is Keyboard) {
					ReelKeyboard();
				}
				break;
			case State.FastReeling:
				if (InputManager.Instance.CurrentDevice is Gamepad) {
					ReelController();
				}
				if (InputManager.Instance.CurrentDevice is Keyboard) {
					ReelKeyboard();
				} break;
			default:
				break;
		}
	}

	public enum LastKeyboardReelInput {
		none,
		left,
		right
	}

	public LastKeyboardReelInput lastKeyboardReelInput;

	#endregion


	#region Serialized Fields

	[Header("Variables")]

	[SerializeField] private Vector2 clickVibration;
	[SerializeField] private float contrllerReelCalculationTime;

	[Header("Components")]

	[SerializeField] private PlayerInput playerInput;
	[SerializeField] private GameObject blurObject;
	[SerializeField] private GameObject blackObject;
	[SerializeField] private PlayerArms playerArms;
	[SerializeField] private RodLineComponent rodLineComponent;

	[Header("ReelTypes")]

	[SerializeField] private InputType keyboard = new InputType();
	[SerializeField] private InputType controller = new InputType();

	#endregion


	#region ReelType

	[System.Serializable]
	public class InputType {

		public List<float> reelStateThresholds;
		public float HorizontalSpeed;
		public float ReelInput;
		public float ReelSpeed;
	}

	#endregion


	#region Properties

	private List<InputType> InputTypes { get; set; }
	public Action OnClick { get; set; }
	public Action OnSkip { get; set; }
	public Action OnPause { get; set; }
	public Action<int> OnStrafe { get; set; }
	public int StrafeDirection { get; set; }
	[field: SerializeField] public bool SelectionManuallySet { get; set; }
	public Vector2 HorizontalInput { get; set; }
	private Vector2 PreviousRightStickInput { get; set; }
	[field: SerializeField] public int ReelLevel { get; set; }
	private bool IsSwitchingScenes { get; set; } = false;
	private bool CanSwitchScenes { get; set; } = false;
	public float CurrentControllerReelResetTime { get; set; }
	public float TimeSinceLastKeyboardReel { get; set; }
	public bool IsSFXMuted { get; set; }

	#endregion


	#region Mono Behaviours

	public void Start() {
		if (Mouse.current != null) {
			InputSystem.DisableDevice(Mouse.current);
		}
	}

	#endregion


	#region Public Methods

	public void Click(InputAction.CallbackContext context) {
		if (context.performed) {
			OnClick?.Invoke();
		}
	}

	public void MoveHorizontalMouse(InputAction.CallbackContext context) {
		this.HorizontalInput = context.ReadValue<Vector2>() * keyboard.HorizontalSpeed;
		HadleStrafing();
	}

	public void ReelMouse(InputAction.CallbackContext context) {
		if (this.CurrentState == State.ReelingLocked) {
			return;
		}
		keyboard.ReelInput -= context.ReadValue<Vector2>().y;
	}

	public void ReelController(InputAction.CallbackContext context) {
		if (this.CurrentState == State.ReelingLocked) {
			return;
		}
		Vector2 currentStick = context.ReadValue<Vector2>();
		if (currentStick.magnitude < 0.5f) {
			return;
		}
		Vector2 from = this.PreviousRightStickInput.normalized;
		Vector2 to = currentStick.normalized;
		float angle = Vector2.SignedAngle(from, to);
		float rotationMagnitude = Mathf.Abs(angle);

		controller.ReelInput = controller.ReelInput + (rotationMagnitude / 360f);

		this.PreviousRightStickInput = currentStick;
		//TODO set previous magnitude
	}

	public void ReelRightArrow(InputAction.CallbackContext context) {
		if (this.CurrentState == State.ReelingLocked) {
			return;
		}
		if (lastKeyboardReelInput == LastKeyboardReelInput.right) {
			return;
		}
		if (context.performed) {
			SetKeyboardReelState();
			this.TimeSinceLastKeyboardReel = 0;
			lastKeyboardReelInput = LastKeyboardReelInput.right;
		}
	}

	public void ReelLeftArrow(InputAction.CallbackContext context) {
		if (this.CurrentState == State.ReelingLocked) {
			return;
		}
		if (lastKeyboardReelInput == LastKeyboardReelInput.left) {
			return;
		}
		if (context.performed) {
			SetKeyboardReelState();
			this.TimeSinceLastKeyboardReel = 0;
			lastKeyboardReelInput = LastKeyboardReelInput.left;
		}
	}

	public void MoveHorizontalController(InputAction.CallbackContext context) {
		this.HorizontalInput = context.ReadValue<Vector2>() * controller.HorizontalSpeed;
		HadleStrafing();
	}

	public void Pause(InputAction.CallbackContext context) {
		if (this.OnPause != null) {
			this.OnPause.Invoke();
		}
	}

	public void Shop(InputAction.CallbackContext context) {
		if (context.performed) {
			if (this.IsSwitchingScenes || !this.CanSwitchScenes) {
				return;
			}
			this.IsSwitchingScenes = true;
			if (SceneManager.GetActiveScene().name == "Shop") {
				SceneManager.LoadScene("Game");
			} else {
				SceneManager.LoadScene("Shop");
			}
		}
	}

	public void NorthGamepad(InputAction.CallbackContext context) {
		if (context.performed) {
			if (GameManager.Instance.ShopController != null) {
			}
		}
	}

	public void RT(InputAction.CallbackContext context) {
		if (context.performed) {
			if (GameManager.Instance.EventController != null) {
				GameManager.Instance.EventController.Duck();
			}
		}
	}

	public void Skip(InputAction.CallbackContext context) {
		if (context.performed) {
			if (!AudioManager.Instance.VoiceLineInProgress) {
				OnSkip?.Invoke();
			}
			AudioManager.Instance.SkipVoiceOver();
		}
	}

	public void RepeatLine(InputAction.CallbackContext context) {
		if (context.performed) {
			AudioManager.Instance.ReplayVoiceLine();
		}
	}

	public void SelectButton(GameObject button) {
		this.SelectionManuallySet = true;
		GameManager.Instance.EventSystem.SetSelectedGameObject(button);
	}

	public void ResetReelInput() {
		for (int i = 0; i < this.InputTypes.Count; i++) {
			this.InputTypes[i].ReelInput = 0;
		}
	}

	public void PauseReelSFX(float time) {
		StartCoroutine(RunPauseReelSFX(time));
	}

	#endregion


	#region Private Methods

	private IEnumerator RunPauseReelSFX(float time) {
		this.IsSFXMuted = true;
		float currentTime = 0;
		while (currentTime < time) {
			currentTime += Time.deltaTime;
			yield return null;
		}
		this.IsSFXMuted = false;
		playerArms.Reel();
	}

	private IEnumerator EnableSceneSwitching() {
		yield return new WaitForSeconds(1f);
		this.CanSwitchScenes = true;
	}

	private void ReelController() {		
		if (this.CurrentControllerReelResetTime < contrllerReelCalculationTime) {
			this.CurrentControllerReelResetTime += Time.deltaTime;
		} else {
			this.CurrentControllerReelResetTime = 0;
			controller.ReelSpeed = controller.ReelInput / contrllerReelCalculationTime;
			controller.ReelInput = 0;
		}
		SetControllerReelState();
	}

	private void ReelKeyboard() {
		this.TimeSinceLastKeyboardReel += Time.deltaTime;
		if(this.TimeSinceLastKeyboardReel > keyboard.reelStateThresholds[0]) {
			SetState(State.NotReeling);
		}
	}

	private void SetKeyboardReelState() {
		keyboard.ReelSpeed = this.TimeSinceLastKeyboardReel;
		if (keyboard.ReelSpeed < keyboard.reelStateThresholds[0] && keyboard.ReelSpeed > keyboard.reelStateThresholds[1]) {
			SetState(State.CalmReeling);
		}
		if (keyboard.ReelSpeed < keyboard.reelStateThresholds[1] && keyboard.ReelSpeed > keyboard.reelStateThresholds[2]) {
			SetState(State.NormalReeling);
		}
		if (keyboard.ReelSpeed < keyboard.reelStateThresholds[2]) {
			SetState(State.FastReeling);
		}
}


	private void SetControllerReelState() {
		if(controller.ReelSpeed == 0) {
			SetState(State.NotReeling);
		}
		if (controller.ReelSpeed < controller.reelStateThresholds[0] && controller.ReelInput > 0) {
			SetState(State.CalmReeling);
		}
		if (controller.ReelSpeed < controller.reelStateThresholds[1] && controller.ReelSpeed > controller.reelStateThresholds[0]) {
			SetState(State.NormalReeling);
		}
		if (controller.ReelSpeed > controller.reelStateThresholds[1]) {
			SetState(State.FastReeling);
		}
	}

	private void HadleStrafing() {
		int laststrafedirection = this.StrafeDirection;
		if (this.HorizontalInput.x > 0.3f) {
			this.StrafeDirection = 1;
		} else if (this.HorizontalInput.x < -0.3f) {
			this.StrafeDirection = -1;
		} else {
			this.StrafeDirection = 0;
		}
		if (laststrafedirection != this.StrafeDirection) {
			this.OnStrafe?.Invoke(StrafeDirection);
		}
	}

	#endregion

}
