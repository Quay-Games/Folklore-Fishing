using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ResponseData : BranchingTypeData
{
	public List<ResponseData_Text> ResponseData_Texts = new List<ResponseData_Text>();

    public override void Run(IGraphRunnerRuntime runtime)
    {
        List<ResponseData_Text> tmp = new List<ResponseData_Text>(ResponseData_Texts);
        List<string> texts = new List<string>();
        for (int i = 0; i < tmp.Count; i++)
        {
            texts.Add(tmp[i].Text.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
        }
    }
}

[System.Serializable]
public class ResponseData_Text {
	public Container_Int ID = new Container_Int();
	public Container_String GuidID = new Container_String();
	public List<LanguageGeneric<string>> Text = new List<LanguageGeneric<string>>();

#if UNITY_EDITOR
	public TextField TextField { get; set; }
#endif
}
