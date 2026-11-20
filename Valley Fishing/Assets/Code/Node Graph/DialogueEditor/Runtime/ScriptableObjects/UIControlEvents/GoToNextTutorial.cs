using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "UIControl/Go To Next Tutorial")]

public class GoToNextTutorial : UIControlEvent
{
    //this will be changed when we make the progression manager
    [SerializeField] SceneName scene;
    public override bool ActionInstead()
    {
        SceneManager.LoadSceneAsync((int)scene);
        return false;
    }
}