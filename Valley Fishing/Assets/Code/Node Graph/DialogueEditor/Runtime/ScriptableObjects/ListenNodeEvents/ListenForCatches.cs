using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Listen for Catches")]
public class ListenForCatches : ListenEvent
{
    [SerializeField] private int numCatches;
    private int numCaughtToDate = 0;

    public override IEnumerator StartListening()
    {
        GameManager.Instance.LevelController.OnFishCaught += IncrementCatches;
        numCaughtToDate = 0;
        yield return CheckNumCatches();
    }

    private IEnumerator CheckNumCatches()
    {
        while (numCaughtToDate < numCatches)
        {
            yield return null;
        }
        GameManager.Instance.LevelController.OnFishCaught -= IncrementCatches;
        OnEventTriggered?.Invoke();
    }

    private void IncrementCatches()
    {
        numCaughtToDate++;
    }
}