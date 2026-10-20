using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.UIElements;
#endif

public class DialogueContainerValues {
}



[System.Serializable]
public class LanguageGeneric<T> {
	public LanguageType LanguageType;
	public T LanguageGenericType;
}

[System.Serializable]
public class Container_ListenEvent {
	public ListenEvent ListenEvent;
}

//values
[System.Serializable]
public class Container_String {
	public string Value;
}

[System.Serializable]
public class Container_Int {
	public int Value;
}

[System.Serializable]
public class Container_Float {
	public float Value;
}

[System.Serializable]
public class Container_EventReference{
	public EventInstance Value;
}

//enums
[System.Serializable]
public class Container_ChoiceStateType {
#if UNITY_EDITOR
	public EnumField EnumField;
#endif
	public ChoiceStateType Value = ChoiceStateType.Hide;
}

[System.Serializable]
public class Container_EndNodeType {
#if UNITY_EDITOR
	public EnumField EnumField;
#endif
	public EndNodeType Value = EndNodeType.End;
}

[System.Serializable]
public class Container_StringEventModifierType {
#if UNITY_EDITOR
	public EnumField EnumField;
#endif
	public StringEventModifierType Value = StringEventModifierType.SetTrue;
}

[System.Serializable]
public class Container_StringEventConditionType {
#if UNITY_EDITOR
	public EnumField EnumField;
#endif
	public StringEventConditionType Value = StringEventConditionType.True;
}

[System.Serializable]
public class Container_Enum<T> where T : System.Enum
{
#if UNITY_EDITOR
    public EnumField EnumField;
#endif
    public T Value;
}

    //events
    [System.Serializable]
public class EventData_StringModifier {
	public Container_String StringEventText = new Container_String();
	public Container_String StringEventValue = new Container_String();
	public Container_StringEventModifierType StringEventModifierType = new Container_StringEventModifierType();
}

[System.Serializable]
public class EventData_StringCondition {
	public Container_String StringEventText = new Container_String();
	public Container_String StringEventValue = new Container_String();
	public Container_StringEventConditionType StringEventConditionType = new Container_StringEventConditionType();
}

[System.Serializable]
public class EventData_EventName {
	public Container_String StringEventValue = new Container_String();
}