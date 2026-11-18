using UnityEngine;

public abstract class GameUIItem : MonoBehaviour { 
    #region Public Properties
    [SerializeField] public abstract UIType ButtonType { get; set; }
    #endregion

    public bool AllowUIPress(UIType childType)
    {
        return UIManager.Instance.DeferButtonPressIfNeeded(childType);
    }
}