using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Listen For Bite")]
public class ListenForBite : ListenEvent
{
    private bool hasBitten = false;
    public override IEnumerator StartListening()
    {
        hasBitten = false;
        GameManager.Instance.LevelController.OnFishBit += Satisfy;
        yield return CheckBite();
    }

    private IEnumerator CheckBite()
    {
        while (!hasBitten)
        {
            yield return null;
        }
        GameManager.Instance.LevelController.OnFishBit -= Satisfy;
        OnEventTriggered?.Invoke();
    }

    private void Satisfy()
    {
        hasBitten = true;
    }
}