using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Listen For Bait Attached")]
public class ListenForBaitAttached : ListenEvent
{
    private bool hasBaitAttached = false;
    public override IEnumerator StartListening()
    {
        hasBaitAttached = false;
        GameManager.Instance.LevelController.OnBaitAttached += Satisfy;
        yield return CheckBaitAttached();
    }

    private IEnumerator CheckBaitAttached()
    {
        while (!hasBaitAttached)
        {
            yield return null;
        }
        GameManager.Instance.LevelController.OnBaitAttached -= Satisfy;
        OnEventTriggered?.Invoke();
    }

    private void Satisfy()
    {
        hasBaitAttached = true;
    }
}