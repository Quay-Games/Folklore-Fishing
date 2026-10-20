using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{

    #region Serialized Fields

    [SerializeField] private Image image;
    [SerializeField] private float fadeDuration;
	[SerializeField] private AnimationCurve fadeInCurve;
	[SerializeField] private AnimationCurve fadeOutCurve;

	#endregion


	#region Mono Behaviours

	public override void Awake() {
		base.Awake();
		SceneManager.sceneUnloaded += FadeToBlack;
		SceneManager.sceneLoaded += FadeToClear;
	}

	#endregion

	#region Public Methods

	public void FadeToBlack(Scene scene) {
        //StartCoroutine(FadeImageToBlack());
    }

    public void FadeToClear(Scene scene, LoadSceneMode loadSceneMode) {
       // StartCoroutine(FadeImageToClear());
    }

    #endregion


    #region Private Methods

    private IEnumerator FadeImageToBlack() {
        float elapsedTime = 0f;
        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = Color.black;

        while (elapsedTime < fadeDuration) {
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / fadeDuration;
			float curvedT = fadeOutCurve.Evaluate(t);
			image.color = Color.Lerp(startColor, endColor, curvedT);
			yield return null;
		}

        image.color = endColor;
    }

    private IEnumerator FadeImageToClear() {
        float elapsedTime = 0f;
        Color startColor = Color.black;
        Color endColor = new Color(0, 0, 0, 0);

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
			float t = elapsedTime / fadeDuration;
			float curvedT = fadeInCurve.Evaluate(t);
			image.color = Color.Lerp(startColor, endColor, curvedT);
            yield return null;
        }

        image.color = endColor;
    }

    #endregion

}
