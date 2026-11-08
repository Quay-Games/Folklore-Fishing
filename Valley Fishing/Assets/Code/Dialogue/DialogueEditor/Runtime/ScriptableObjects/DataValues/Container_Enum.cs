using UnityEngine.UIElements;

[System.Serializable]
public class Container_Enum<T> : UIContainer where T : System.Enum 
{
    public T Value;
    public Container_Enum(string newLabel = "") : base(newLabel) {
        
    }
    public override void CreateUI(VisualElement mainContainer)
    {
        Box boxContainer = new Box();
        boxContainer.AddToClassList("EnumBox");
        EnumField enumField = new EnumField
        {
           label = Label.Length > 0 ? Label : "Enum Field"
        };

        if (!System.Enum.IsDefined(typeof(T), Value))
        {
            var choices = (T[])System.Enum.GetValues(typeof(T));
            if (choices.Length > 0)
                Value = choices[0];
        }

        enumField.Init(Value);

        enumField.RegisterValueChangedCallback(eventVal =>
        {
            Value = (T)eventVal.newValue;
        });

        boxContainer.Add(enumField);
        mainContainer.Add(boxContainer);
    }
}