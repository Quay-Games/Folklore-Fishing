using UnityEngine;

public interface IDialogueRuntime
{
    MonoBehaviour Runner { get; }
    void Continue(int option);
    void EndDialogue();
    BaseData GetNextNode(BaseData node);
    BaseData GetNodeByGuid(string guid);
}