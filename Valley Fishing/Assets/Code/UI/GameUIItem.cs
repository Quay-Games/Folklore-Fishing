using Unity.VisualScripting;
using UnityEngine;

public abstract class GameUIItem : MonoBehaviour { 
    #region Public Properties
    [SerializeField] public abstract UIType ButtonType { get; set; }
    private void OnEnable()
    {
        UIManager.Instance?.NotifyOfOpenClose(ButtonType, true);
    }
    private void OnDisable()
    {
        UIManager.Instance?.NotifyOfOpenClose(ButtonType, false);
    }
    #endregion
    //TODO: NOT ALL USAGES WRITTEN

    public bool AllowUIPress(UIType childType)
    {
        return UIManager.Instance.DeferButtonPressIfNeeded(childType);
    }
}