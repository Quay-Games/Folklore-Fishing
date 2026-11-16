using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.DialogueEditor {
	public class UpdateLanguageType {
		public void UpdateLanguage() {
			//List<DialogueContainer> dialogueContainers = CSVHelper.FindAllObjectsFromResources<DialogueContainer>();
			//List<DialogueContainer> dialogueContainers = CSVHelper.FindAllDialogueContainers();
			//foreach (DialogueContainer dialogueContainer in dialogueContainers) {
			//	foreach (DialogueNodeData dialogueNodeData in dialogueContainer.DialogueNodeDatas) {
			//		dialogueNodeData.TextLanguages = UpdateLanguageGeneric(dialogueNodeData.TextLanguages);
			//		foreach (DialogueNodePort dialogueNodePort in dialogueNodeData.DialogueNodePorts) {
			//			dialogueNodePort.TextLanguages = UpdateLanguageGeneric(dialogueNodePort.TextLanguages);
			//		}
			//	}
			//}
		}

		//private List<LanguageGeneric<T>> UpdateLanguageGeneric<T>(List<LanguageGeneric<T>> languageGenerics) {
			//List<LanguageGeneric<T>> tmps = new List<LanguageGeneric<T>>();
			//foreach (LanguageType languageType in (LanguageType[])Enum.GetValues(typeof(LanguageType))) {
			//	tmps.Add(new LanguageGeneric<T> {
			//		LanguageType = languageType
			//	});
			//}

			//foreach (LanguageGeneric<T> languageGeneric in languageGenerics) {
			//	if (tmps.Find(language => language.LanguageType == languageGeneric.LanguageType) != null) {
			//		tmps.Find(language => language.LanguageType == languageGeneric.LanguageType).LanguageGenericType = languageGeneric.LanguageGenericType;
			//	}
			//}

			//return tmps;
		//}

	}
}
