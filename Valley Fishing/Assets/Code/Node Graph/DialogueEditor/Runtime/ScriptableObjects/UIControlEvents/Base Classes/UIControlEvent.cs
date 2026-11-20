using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public abstract class UIControlEvent : ScriptableObject
{
    //the bool returned here signifies wether the intended action should be allowed to happen after this
    public abstract bool ActionInstead();
}
