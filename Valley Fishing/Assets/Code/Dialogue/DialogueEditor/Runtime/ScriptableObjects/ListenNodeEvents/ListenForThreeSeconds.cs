using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Listen For Seconds")]
public class ListenForThreeSeconds : ListenEvent
{
	[SerializeField] private float listenTime;

	public override IEnumerator StartListening() {
		float currentTime = 0;
		while(currentTime < listenTime) {
			currentTime += Time.deltaTime;
			yield return null;
		}
		OnEventTriggered?.Invoke();
	}
}
