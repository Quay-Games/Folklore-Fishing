using System.Collections.Generic;
using UnityEngine;

public interface IDialogueRuntime
{
    MonoBehaviour GetMonoBehaviour();
    void StopCoroutines();
    void SetBaseContainers(List<DialogueData_BaseContainer> containers);
    void SetSpeakerName(string name);
}