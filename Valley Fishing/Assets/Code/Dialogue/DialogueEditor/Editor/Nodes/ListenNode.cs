using Project.DialogueEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[NodeDataType(typeof(ListenData))]
public class ListenNode : BaseNode {
    //[System.Serializable]
    //public class ListenData : BaseData
    //{
    //    public List<Container_ListenEventSO> Container_ListenEventSOs = new List<Container_ListenEventSO>();
    //}

    ListenData listenData = new ListenData();

	//public ListenData ListenData {
	//	get => listenData;
	//	set => listenData = value;
	//}

	public ListenNode() {

	}

	public ListenNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
	: base(_position, _editorWindow, _graphView, "USS/Nodes/EventNodeStyleSheet", "ListenNode", _data){

		//if theres data to initialize with, do that, if not use default values
		if (_data != null)
		{
			
            NodeGuid = _data.NodeGuid;

            foreach (Container_ListenEvent item in (_data as ListenData).listenEvents)
            {
                this.AddScriptableEvent(item);
            }

			//is this even a worthwhile thing to do? do these data objects actually get used?
			listenData = _data as ListenData;

            //virtual in base class and not overridden, currently the call does nothing
            //this.LoadValueInToField();
        } else
		{
			//TODO GRIFF: might not be necessary anymore
            SetPosition(new Rect(_position, defaultNodeSize));
            nodeGuid = Guid.NewGuid().ToString();
        }

		AddInputPort("Input", Port.Capacity.Multi);
		AddOutputPort("Continue");
		AddScriptableEvent();
	}

	public void AddScriptableEvent(Container_ListenEvent paramidaEventScriptableObjectData = null) {

		// Try to reuse an existing empty box before creating a new one
		Box existingEmptyBox = FindEmptyEventBox();

		if (paramidaEventScriptableObjectData != null && existingEmptyBox != null) {
			ObjectField existingObjectField = existingEmptyBox.Q<ObjectField>();
			Container_ListenEvent existingData = existingObjectField?.userData as Container_ListenEvent;

			if (existingObjectField != null && existingData != null) {
				existingData.ListenEvent = paramidaEventScriptableObjectData.ListenEvent;
				existingObjectField.SetValueWithoutNotify(existingData.ListenEvent);
				return;
			}
		}

		// No reusable box found — create a new one as before
		Container_ListenEvent tmpListenEventSO = new Container_ListenEvent();

		if (paramidaEventScriptableObjectData != null) {
			tmpListenEventSO.ListenEvent = paramidaEventScriptableObjectData.ListenEvent;
		}
		listenData.listenEvents.Add(tmpListenEventSO);

		// Container of all object.
		Box boxContainer = new Box();
		boxContainer.AddToClassList("EventBox");

		// Scriptable Object Event.
		ObjectField objectField = GetNewObjectField_ListenEvent(tmpListenEventSO, "EventObject");
		objectField.userData = tmpListenEventSO; // tag so we can find/update it later

		// Add it to the box
		boxContainer.Add(objectField);

		mainContainer.Add(boxContainer);
		RefreshExpandedState();
	}

	private Box FindEmptyEventBox() {
		foreach (VisualElement child in mainContainer.Children()) {
			if (child is Box box && box.ClassListContains("EventBox")) {
				ObjectField field = box.Q<ObjectField>();
				if (field != null && field.value == null) {
					return box;
				}
			}
		}
		return null;
	}
	//this function generates some listen data to give out to the scriptable object
    public override BaseData Save()
    {
        ListenData nodeData = new ListenData();
        SaveData(nodeData);

        // Save Dialogue Event
        foreach (Container_ListenEvent dialogueEvent in listenData.listenEvents)
        {
            nodeData.listenEvents.Add(dialogueEvent);
        }

        return nodeData;
    }
}
