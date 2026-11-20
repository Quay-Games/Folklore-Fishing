using UnityEngine.UIElements;

//every class derived from this will need this serlializable attribute
[System.Serializable]
public abstract class UIContainer
{
    public string Label;
    public UIContainer(string newLabel)
    {
        Label = newLabel;
    }
    public abstract void CreateUI(VisualElement mainContainer);
}