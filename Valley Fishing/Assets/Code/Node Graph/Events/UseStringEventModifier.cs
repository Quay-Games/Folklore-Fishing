using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseStringEventModifier : MonoBehaviour
{
	public bool ModifierBoolCheck(StringEventModifierType stringEventModifierType) {
		switch (stringEventModifierType) {
			case StringEventModifierType.SetTrue:
				return true;
			case StringEventModifierType.SetFalse:
				return false;
			default:
				return false;
		}
	}

	public string ModifierFloatCheck(string inputValue, StringEventModifierType stringEventModifierType) {
		switch (stringEventModifierType) {
			case StringEventModifierType.Equals:
				return inputValue;
			case StringEventModifierType.Add:
				return inputValue;
			case StringEventModifierType.Subtract:
				return "-" + inputValue;
			default:
				return "";
		}
	}
}
