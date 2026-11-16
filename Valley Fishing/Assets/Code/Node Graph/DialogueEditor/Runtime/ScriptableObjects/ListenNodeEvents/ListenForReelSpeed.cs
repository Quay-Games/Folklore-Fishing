using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Listen For Reel Speed")]
public class ListenForReelSpeed : ListenEvent
{
    enum ReelSpeed
    {
        Slow,
        Medium,
        Fast,
        Any
    }
    [SerializeField] private ReelSpeed reelSpeed;
    [SerializeField] private int secondsToListen;
    private float practiceTime = 0;
    private float failTime = 0;
    private bool satisfied = false;

    public override IEnumerator StartListening()
    {
        practiceTime = 0;
        failTime = 0;
        satisfied = false;
        yield return CountReelInput();
    }

    private IEnumerator CountReelInput()
    {

        while (!satisfied)
        {
            //a voice line is playing or the player is not reeling 
            if (AudioManager.Instance.VoiceLineInProgress || GameManager.Instance.InputController.ReelLevel == 0)
            {
                practiceTime = 0;
                failTime = 0;
            } else if (GameManager.Instance.InputController.ReelLevel == (int) reelSpeed + 1 || reelSpeed == ReelSpeed.Any)
            {
                if (practiceTime < secondsToListen)
                {
                    practiceTime += Time.deltaTime;
                }
                else
                {
                    satisfied = true;
                }
                failTime = 0;
            }
            else
            {
                if (failTime < secondsToListen)
                {
                    failTime += Time.deltaTime;
                }
                else
                {
                    if (GameManager.Instance.InputController.ReelLevel < (int)reelSpeed + 1)
                    {
                        AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.tooSlowPrompt);
                    }
                    else
                    {
                        AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.tooFastPrompt);
                    }
                    //make sure the timer resets after a voice line plays
                    failTime = 0;
                }
                practiceTime = 0;
            }
            yield return null;
        }

        OnEventTriggered?.Invoke();
    }
}
