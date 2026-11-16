using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[System.Serializable]
public class Container_Int : UIContainer
{
    public int Value;

    public Container_Int(string newLabel = "") : base(newLabel) { }

    public override void CreateUI(VisualElement mainContainer)
    {
        Box boxContainer = new Box();
        boxContainer.AddToClassList("IntBox");
        IntegerField intField = new IntegerField
        {
            label = Label.Length > 0 ? Label : "Int Field"
        };
        intField.SetValueWithoutNotify(Value);

        intField.RegisterValueChangedCallback(eventVal =>
        {
            Value = eventVal.newValue;
        });

        boxContainer.Add(intField);
        mainContainer.Add(boxContainer);
    }
}