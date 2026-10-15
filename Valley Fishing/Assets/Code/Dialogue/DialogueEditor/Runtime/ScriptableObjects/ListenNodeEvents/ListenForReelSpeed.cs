using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Listen For Reel Speed")]
public class ListenForReelSpeed : ListenEvent
{
    enum ReelSpeed
    {
        Slow,
        Medium,
        Fast
    }
    [SerializeField] private ReelSpeed reelSpeed;

    public override IEnumerator StartListening()
    {
        float currentTime = 0;
        while (currentTime < 5)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        OnEventTriggered?.Invoke();
    }
}
