using UnityEngine.UIElements;

[System.Serializable]
public class Container_Bool : UIContainer
{
    public bool Value;

    public Container_Bool(string newLabel = "") : base(newLabel) {}

    public override void CreateUI(VisualElement mainContainer)
    {
        Box boxContainer = new Box();
        boxContainer.AddToClassList("BoolBox");
        Toggle boolField = new Toggle
        {
            label = Label.Length > 0 ? Label : "Bool Field"
        };
        boolField.SetValueWithoutNotify(Value);

        boolField.RegisterValueChangedCallback(eventVal =>
        {
            Value = eventVal.newValue;
        });

        boxContainer.Add(boolField);
        mainContainer.Add(boxContainer);
    }
}