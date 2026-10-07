using System.Collections.Generic;
using UnityEngine;

public interface IGraphRunnerRuntime
{
    //A smattering of methods that are exposed to nodes for use, as some nodes need to access things
    //only avaialable to the Graph Runner, but we do it this way so these are the only things the
    //nodes know about
    MonoBehaviour GetMonoBehaviour();
    void StopCoroutines();
    void SetBaseContainers(List<DialogueData_BaseContainer> containers);
    void SetSpeakerName(string name);
}