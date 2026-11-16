using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
//This is a class where one the user may pick from multiple types of enum, related by a category enum
public class Container_EnumSelect<T> : UIContainer where T : System.Enum
{
	public T listCategory;
    public List<string> enumTypeNames = new List<string>();
    public string selectedMemberName;
    //we dont store the value directly, as it couldnt be serialized, so when we want to access it we read from the name
    //that is stored alongside it
    public Enum Value
    {
        get
        {
            if (string.IsNullOrEmpty(selectedMemberName))
                return null;

            Type choiceType = GetChoiceType();
            if (choiceType == null || !Enum.IsDefined(choiceType, selectedMemberName))
                return null;

            return (Enum)Enum.Parse(choiceType, selectedMemberName);
        }
        set => selectedMemberName = value?.ToString();
    }

    public Container_EnumSelect(List<Type> enumList, string newLabel = "", T category = default(T)) : base(newLabel)
	{
		listCategory = category;
        foreach (Type enumType in enumList)
            enumTypeNames.Add(enumType.AssemblyQualifiedName);
    }
	public override void CreateUI(VisualElement mainContainer)
	{
		Box boxContainer = new Box();
		boxContainer.AddToClassList("EnumBox");
		EnumField categoryField = new EnumField
		{
			label = Label.Length > 0 ? Label : "Category"
		};
		EnumField specificField = new EnumField
        {
            label = "Specific Choice"
        };

        if (!Enum.IsDefined(typeof(T), listCategory))
        {
            var categories = (T[])Enum.GetValues(typeof(T));
            if (categories.Length > 0)
                listCategory = categories[0];
        }

        void InitSpecificField()
        {
            Type defaultChoiceType = GetChoiceType();
            var defaultChoiceValue = Enum.Parse(defaultChoiceType, Enum.GetNames(defaultChoiceType)[0]);


            if (Value == null ||
                Value.GetType() != defaultChoiceType ||
                !Enum.IsDefined(defaultChoiceType, Value))
            {
                Value = (Enum)defaultChoiceValue;
            }

            specificField.Init((Enum)Value);
        }

        categoryField.Init(listCategory);
        InitSpecificField();

        categoryField.RegisterValueChangedCallback(evt =>
        {
            listCategory = (T)evt.newValue;
            Value = null;
            InitSpecificField();
        });

        specificField.RegisterValueChangedCallback(evt =>
        {
            Value = evt.newValue;
        });

        boxContainer.Add(categoryField);
		boxContainer.Add(specificField);
        mainContainer.Add(boxContainer);
	}

    //A helper function that gets the type currently chosen via the top enum selector, worth noting that
    //this doesnt get the actual enum value of the category, but the enum at the same index in the types list
    //as they each correlate to a category in the list
    private Type GetChoiceType()
    {
        return Type.GetType(enumTypeNames[Convert.ToInt32(listCategory)]);
    }
}