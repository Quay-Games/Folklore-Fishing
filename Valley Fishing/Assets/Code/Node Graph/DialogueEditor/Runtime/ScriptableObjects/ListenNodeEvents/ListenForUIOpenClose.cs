using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Listen For UI Open or Close")]
public class ListenForUIOpenClose : ListenEvent
{
    [SerializeField] private UIType UiType;
    [SerializeField] private bool Open;
    private bool satisfied = false;

    public override IEnumerator StartListening()
    {
        satisfied = false;
        yield return CheckUIStatus();
    }

    private IEnumerator CheckUIStatus()
    {

        UIManager.Instance.UIOpenClosed += Satisfy;
        while (!satisfied)
        {
            yield return null;
        }
        UIManager.Instance.UIOpenClosed -= Satisfy;

        OnEventTriggered?.Invoke();
    }

    private void Satisfy(UIType listenType, bool isOpen)
    {
        if(listenType == UiType && isOpen)
        {
            satisfied = true;
        }
    }
}
