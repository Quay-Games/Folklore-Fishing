using System.Collections.Generic;
using UnityEngine;

public class HelpManager : Singleton<HelpManager>
{
    [SerializeField] public bool CaptainReelHelpActive = false;
    private bool waitingForCatch = false;
    private bool currentlyReeling = false;
    private int secondsToListen = 3;
    private float failTime = 0;
    private Dictionary<ReelSpeed, ActivityLevel> reelSpeedToActivityLevel = new Dictionary<ReelSpeed, ActivityLevel>()
    {
        { ReelSpeed.Slow, ActivityLevel.calm },
        { ReelSpeed.Medium, ActivityLevel.medium },
        { ReelSpeed.Fast, ActivityLevel.active }
    };
    private Dictionary<ActivityLevel, ReelSpeed> activityLevelToReelSpeed = new Dictionary<ActivityLevel, ReelSpeed>()
    {
        { ActivityLevel.calm, ReelSpeed.Slow },
        { ActivityLevel.medium, ReelSpeed.Medium },
        { ActivityLevel.active, ReelSpeed.Fast }
    };

    void Update()
    {
        if(CaptainReelHelpActive)
        {
            if(!waitingForCatch && !currentlyReeling)
            {
                waitingForCatch = true;
            }

            if(currentlyReeling)
            {
                //a voice line is playing or the player is not reeling 
                if (AudioManager.Instance.VoiceLineInProgress || GameManager.Instance.InputController.ReelLevel == 0)
                {
                    failTime = 0;
                }
                //we dont do anything if theyre reeling at the right speed
                else if (reelSpeedToActivityLevel[GetCurrentReelSpeed()] == GameManager.Instance.CurrentFish.CurrentActivityLevel)
                {
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
                        if (GetCurrentReelSpeed() < GetCurrentFishReelSpeed())
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
                }
            } else
            {
                waitingForCatch = false;
            }
        }
    }

    public void toggleReelHelp(bool activate, int seconds = 0)
    {
        CaptainReelHelpActive = activate;
        if (CaptainReelHelpActive)
        {
            secondsToListen = seconds;
            GameManager.Instance.LevelController.OnFishBit += ActivateReelHelp;
            GameManager.Instance.CurrentFish.OnActivityLevelChange += ResetReelValues;

        }
        else
        {
            GameManager.Instance.LevelController.OnFishBit -= ActivateReelHelp;
            GameManager.Instance.CurrentFish.OnActivityLevelChange -= ResetReelValues;
        }
    }

    private void ActivateReelHelp ()
    {
        currentlyReeling = true;
        ResetReelValues();
    }

    private void ResetReelValues()
    {
        failTime = 0;
    }

    private ReelSpeed GetCurrentReelSpeed ()
    {
        switch (GameManager.Instance.InputController.ReelLevel)
        {
            case 1:
                return ReelSpeed.Slow;
            case 2:
                return ReelSpeed.Medium;
            case 3:
                return ReelSpeed.Fast;
            default:
                return ReelSpeed.Any;
        }
    }

    private ReelSpeed GetCurrentFishReelSpeed()
    {
        return activityLevelToReelSpeed[GameManager.Instance.CurrentFish.CurrentActivityLevel];
    }
}
