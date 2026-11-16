using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(
	fileName = "FMODEventDatabase",
	menuName = "FMOD/Event Database"
)]
public class FMODEventDatabase : ScriptableObject {
	public List<EventReference> Events = new();
}
