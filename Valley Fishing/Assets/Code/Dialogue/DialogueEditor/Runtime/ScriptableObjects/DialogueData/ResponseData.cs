using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ResponseData : BaseData
{
	public string FirstOptionGuid;
	public string SecondOptionGuid;

	public List<ResponseData_Text> ResponceData_Texts = new List<ResponseData_Text>();
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
