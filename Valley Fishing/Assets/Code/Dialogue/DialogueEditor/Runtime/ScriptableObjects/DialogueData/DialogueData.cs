using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//does this class actually get properly used?
[System.Serializable]
public class DialogueData : BaseData {

	public List<DialogueData_BaseContainer> BaseContainers {
		get;
		set;
	} = new List<DialogueData_BaseContainer>();

	public List<DialogueData_Name> DialogueData_Names = new List<DialogueData_Name>();
	public List<DialogueData_Text> DialogueData_Texts = new List<DialogueData_Text>();
	public List<DialogueData_Port> DialogueData_Ports = new List<DialogueData_Port>();

    public override void Run(IGraphRunnerRuntime runtime)
    {
        BaseContainers = new List<DialogueData_BaseContainer>();
        BaseContainers.AddRange(DialogueData_Names);
        BaseContainers.AddRange(DialogueData_Texts);

        BaseContainers.Sort(delegate (DialogueData_BaseContainer x, DialogueData_BaseContainer y) {
            return x.ID.Value.CompareTo(y.ID.Value);
        });

        for (int i = 1; i < BaseContainers.Count; i++)
        {
            if (BaseContainers[i] is DialogueData_Name)
            {
                DialogueData_Name tmp = BaseContainers[i] as DialogueData_Name;
                runtime.SetSpeakerName(tmp.CharacterName.Value);
            }
            if (BaseContainers[i] is DialogueData_Text)
            {
                DialogueData_Text tmp = BaseContainers[i] as DialogueData_Text;
                //dialogueText.text = tmp.Text.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType;
                break;
            }
        }

        //im not sure if this is necessary but im keeping it for now
        runtime.SetBaseContainers(BaseContainers);
    }
}

[System.Serializable]
public class DialogueData_BaseContainer {
	public Container_Int ID = new Container_Int();
}

[System.Serializable]
public class DialogueData_Name : DialogueData_BaseContainer {
	public Container_String CharacterName = new Container_String();
}

[System.Serializable]
public class DialogueData_Text : DialogueData_BaseContainer {
#if UNITY_EDITOR
	public TextField TextField {
		get;
		set;
	}
#endif
	public Container_String GuidID = new Container_String();
	public List<LanguageGeneric<string>> Text = new List<LanguageGeneric<string>>();
}

[System.Serializable]
public class DialogueData_Port : DialogueData_BaseContainer {
	public string PortGuid;
	public string InputGuid;
	public string OutputGuid;
}
