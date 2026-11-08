using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class InputManager : Singleton<InputManager>
{

	#region Input Swithching
	public InputDevice CurrentDevice { get; set; }
	public bool InputRecieved;
	public bool InputSwitched;
	public event System.Action OnInputSwitched;
	public List<string> LastButtons = new List<string>();

	public override void Awake() {
		base.Awake();
		InputSystem.onEvent += OnAnyInputEvent;
	}

	void OnDestroy() {
		if (Instance == this)
			InputSystem.onEvent -= OnAnyInputEvent;
	}

	void OnAnyInputEvent(InputEventPtr eventPtr, InputDevice device) {
		if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) {
			return;
		}

		foreach (var control in eventPtr.EnumerateChangedControls(device)) {
			if (control is ButtonControl button && button.isPressed) {
				if (LastButtons.Count == 3) {
					LastButtons.RemoveAt(0);
				}
				LastButtons.Add(button.name);
			}
		}
		if (device == CurrentDevice) {			
		InputSwitched = false;
		this.InputRecieved = false;
		return;
		}
		this.InputSwitched = true;
		this.InputRecieved = true;
		CurrentDevice = device;		

		OnInputSwitched?.Invoke();
	}

	#endregion


	#region Public Methods

	public void SelectButton(GameObject button) {
		StartCoroutine(SelectButtonAfterOneFrame(button));
	}

	#endregion


	#region Private Methods

	private static IEnumerator SelectButtonAfterOneFrame(GameObject button) {
		yield return new WaitForEndOfFrame();
		GameManager.Instance.EventSystem.SetSelectedGameObject(button);
	}

	#endregion

}
