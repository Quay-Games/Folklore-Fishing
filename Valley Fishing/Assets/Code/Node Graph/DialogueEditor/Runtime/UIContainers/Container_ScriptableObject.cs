using FMODUnity;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Container_ScriptableObject<T> : UIContainer where T : ScriptableObject
{
    public T Value;

    public Container_ScriptableObject(string newLabel = "") : base(newLabel)
    {
    }

    public override void CreateUI(VisualElement mainContainer)
    {
        Box boxContainer = new Box();
        boxContainer.AddToClassList("EventBox");

        ObjectField objectField = new ObjectField()
        {
            objectType = typeof(T),
            allowSceneObjects = false,
        };

        // When we change the variable from graph view.
        objectField.RegisterValueChangedCallback(value => {
            Value = value.newValue as T;
        });
        objectField.SetValueWithoutNotify(Value);

        boxContainer.Add(objectField);
        mainContainer.Add(boxContainer);
    }
}
