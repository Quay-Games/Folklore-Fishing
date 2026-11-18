using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonVoiceOverComponent : GameUIItem, IPointerEnterHandler, ISelectHandler, IMoveHandler, ISubmitHandler
{
	#region Serialised Fields
	[SerializeField] public UIType buttonType;
    [SerializeField] protected EventReference HoverButtonSoundEventReference;
	[SerializeField] protected UnityEvent hoverEvent;
	[SerializeField] protected UnityEvent clickEvent;
	//im just gonna keep this one separate so we dont unlink all the buttons in the scene
    public override UIType ButtonType { get => buttonType; set => buttonType = value; }

    #endregion


    #region Properties

    public Action<UIType, ButtonVoiceOverComponent> SelectAction { get; set; }
	private Button ButtonReference { get; set; }
	public bool TutorialTriggered { get; set; }
	public Button Button {
		get {
			if (this.ButtonReference == null)
				this.ButtonReference = GetComponent<Button>();

			return this.ButtonReference;
		}
	}

    #endregion

    #region Mono Behaviors
    public void Start()
    {
        UIManager.Instance.Register(this);
    }

    public void OnDestroy()
    {
        UIManager.Instance.Deregister(this);
    }
    #endregion

    public virtual void OnPointerEnter(PointerEventData eventData) {
		GameManager.Instance.LastSelectedButton = gameObject;
		DoHoverEffect(); 
	}

	public virtual void OnSelect(BaseEventData eventData) {
		if(!base.AllowUIPress(ButtonType))
		{
			//this is the case where the button's action has been taken over;
			//may not exactly work like this, as im not sure what the hover effect is doing on there.
			return;
		}
		if (GameManager.Instance.InputController.SelectionManuallySet) {
			GameManager.Instance.InputController.SelectionManuallySet = false;
			return;
		}
		GameManager.Instance.LastSelectedButton = gameObject;
		SelectAction?.Invoke(buttonType,this);
		if (this.TutorialTriggered) {
			this.TutorialTriggered = false;
			return;
		}
		DoHoverEffect();
	}

	public virtual void DoHoverEffect() {
		if (!HoverButtonSoundEventReference.IsNull) {
			AudioManager.Instance.PlayVoiceOver(HoverButtonSoundEventReference);			
		}
		hoverEvent?.Invoke();
	}

	public void OnMove(AxisEventData eventData) {
		if (eventData.moveDir == MoveDirection.Down) {
			if (this.Button.navigation.selectOnDown == null) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			}else if(!this.Button.navigation.selectOnDown.gameObject.activeSelf) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			}
		}
		if (eventData.moveDir == MoveDirection.Up) {
			if (this.Button.navigation.selectOnUp == null) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			} else if (!this.Button.navigation.selectOnUp.gameObject.activeSelf) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			}
		}
		if (eventData.moveDir == MoveDirection.Left) {
			if (this.Button.navigation.selectOnLeft == null) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			} else if (!this.Button.navigation.selectOnLeft.gameObject.activeSelf) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			}
		}
		if (eventData.moveDir == MoveDirection.Right) {
			if (this.Button.navigation.selectOnRight == null) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			} else if (!this.Button.navigation.selectOnRight.gameObject.activeSelf) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			}
		}
	}

	public virtual void OnSubmit(BaseEventData eventData) {
		ButtonClicked(!InputManager.Instance.InputSwitched);
	}

	public virtual bool ButtonClicked(bool buttonInteractable) {
		clickEvent?.Invoke();
		return buttonInteractable;
	}
}
