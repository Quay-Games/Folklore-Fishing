using FMODUnity;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class Container_EventReference : UIContainer
{
    public EventReference Value;

    public Container_EventReference(string newLabel = "") : base(newLabel)
    {
    }

    private class EventReferenceHolder : ScriptableObject
    {
        public EventReference VoiceEvent;
    }

    public override void CreateUI(VisualElement mainContainer)
    {
        Box boxContainer = new Box();
        boxContainer.AddToClassList("EventReferenceBox");

        Label label = new Label(Label.Length > 0 ? Label : "Voice Event");
        boxContainer.Add(label);

        EventReferenceHolder eventReferenceHolder = ScriptableObject.CreateInstance<EventReferenceHolder>();
        eventReferenceHolder.VoiceEvent = Value;

        SerializedObject serializedObject = new SerializedObject(eventReferenceHolder);

        SerializedProperty property = serializedObject.FindProperty("VoiceEvent");

        PropertyField propertyField = new PropertyField(property);
        propertyField.label = "";

        propertyField.Bind(serializedObject);

        propertyField.TrackPropertyValue(property, changedProperty =>
        {
            serializedObject.ApplyModifiedProperties();

            Value = eventReferenceHolder.VoiceEvent;
            Debug.Log("Hi");
        });

        boxContainer.Add(propertyField);
        mainContainer.Add(boxContainer);
    }
}