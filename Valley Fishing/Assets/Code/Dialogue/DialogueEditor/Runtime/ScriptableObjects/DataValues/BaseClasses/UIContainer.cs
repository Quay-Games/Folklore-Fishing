using UnityEngine.UIElements;

public abstract class UIContainer
{
    public string Label;
    public UIContainer(string newLabel)
    {
        Label = newLabel;
    }
    public abstract void CreateUI(VisualElement mainContainer);
}