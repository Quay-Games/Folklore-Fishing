using System.Collections;
using UnityEngine;

public class Seagull : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField] private InventionDatas inventionDatas;

	#endregion


	#region Properties

	private float WarningTime { get; set; }
	private float FailTime { get; set; }

	#endregion


	#region Public Methods

	public void Initialize(float intervalRate, float warningTime, float failTime) {
		this.WarningTime = warningTime;
		this.FailTime = failTime;
		InvokeRepeating("SeagullAttack", intervalRate, intervalRate + 4 * this.WarningTime);
	}

	#endregion


	#region Private Methods
	private void SeagullAttack() {
		StartCoroutine(PerformSeagullAttack());
	}

	private IEnumerator PerformSeagullAttack() {
		for (int i = 0; i < 3; i++) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.SeagullWarning);
			yield return new WaitForSeconds(this.WarningTime);
		}
		float currentTime = 0;
		while (currentTime < GameManager.Instance.EventController.SeagullAvoidanceThreshold) {
			currentTime += Time.deltaTime;
			yield return new();
			if (GameManager.Instance.EventController.Ducking) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.SeagullMiss);
				yield break;
			}
		}
		if (InventionManager.Instance.InventionAttatched(InventionName.Scarecrow)) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.SeagullMiss);
			InventionManager.Instance.InventionDestroyed(InventionName.Scarecrow);
			yield break;
		}
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.SeagullAttack);
		GameManager.Instance.CurrentFish.HitBySeagull = true;
		yield return new WaitForSeconds(this.FailTime);
		GameManager.Instance.CurrentFish.HitBySeagull = false;
	}

	#endregion

}
