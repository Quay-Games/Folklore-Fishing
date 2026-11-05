using UnityEngine.UIElements;

[System.Serializable]
public class Container_Enum<T> : UIContainer where T : System.Enum 
{
    public T Value;
    public Container_Enum(string newLabel = "") : base(newLabel) { }
    public override void CreateUI(VisualElement mainContainer)
    {
        Box boxContainer = new Box();
        boxContainer.AddToClassList("EnumBox");
        EnumField enumField = new EnumField
        {
            label = Label.Length > 0 ? Label : "Enum Field"
        };
        enumField.SetValueWithoutNotify(Value);

        enumField.RegisterValueChangedCallback(eventVal =>
        {
            Value = (T)eventVal.newValue;
        });

        boxContainer.Add(enumField);
        mainContainer.Add(boxContainer);
    }
}