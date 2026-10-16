using Project.DialogueEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(ListenData))]
    public class ListenNode : BaseNode
    {

        ListenData listenData = new ListenData();

        public ListenNode()
        {
            //an empty constructor is necessary to appear creatable in the graph view
        }

        public ListenNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
        : base(_position, _editorWindow, _graphView, "USS/Nodes/EventNodeStyleSheet", "ListenNode", _data)
        {

            //if theres data to initialize with, do that, if not use default values
            if (_data != null)
            {
                listenData = _data as ListenData;
            }

            AddInputPort("Input", Port.Capacity.Multi);
            AddOutputPort("Continue");
            AddScriptableEvent();
        }

        public void AddScriptableEvent()
        {

            Box boxContainer = new Box();
            boxContainer.AddToClassList("EventBox");

            ObjectField objectField;
            if (listenData.listenEventContainer != null)
            {
                objectField = GetNewObjectField_ListenEvent(listenData.listenEventContainer, "EventObject");
                objectField.userData = listenData.listenEventContainer; // tag so we can find/update it later
            }
            else
            {
                //initialize new event container and assign it to data
                Container_ListenEvent newContainer = new Container_ListenEvent();
                objectField = GetNewObjectField_ListenEvent(newContainer, "EventObject");
                objectField.userData = newContainer;
                listenData.listenEventContainer = newContainer;
            }
            boxContainer.Add(objectField);
            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }
        public override BaseData Save()
        {
            ListenData nodeData = new ListenData();
            SaveData(nodeData);
            nodeData.listenEventContainer = listenData.listenEventContainer;
            return nodeData;
        }
    }
}