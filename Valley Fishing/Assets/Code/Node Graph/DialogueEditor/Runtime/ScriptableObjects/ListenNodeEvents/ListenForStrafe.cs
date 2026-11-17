using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Listen For Strafe")]
public class ListenForStrafe : ListenEvent
{
    private bool hasStrafed = false;
    public override IEnumerator StartListening()
    {
        hasStrafed = false;
        GameManager.Instance.CurrentFish.OnStrafe += Satisfy;
        yield return CheckStrafe();
    }

    private IEnumerator CheckStrafe()
    {
        while (!hasStrafed)
        {
            yield return null;
        }
        GameManager.Instance.CurrentFish.OnStrafe -= Satisfy;
        OnEventTriggered?.Invoke();
    }

    private void Satisfy()
    {
        hasStrafed = true;
    }
}