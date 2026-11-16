using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageController : Singleton<LanguageController> {


	[SerializeField]
	private LanguageType language;

	public LanguageType Language {
		get => language;
		set => language = value;
	}

}
