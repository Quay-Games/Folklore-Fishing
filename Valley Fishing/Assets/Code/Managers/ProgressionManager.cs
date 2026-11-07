using UnityEngine;

public class ProgressionManager : Singleton<ProgressionManager>
{
	[field:SerializeField] public int LocationsUnlocked { get; set; } = 1;
}
