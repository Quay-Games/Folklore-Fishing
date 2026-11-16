using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(GiveItemIfNotHadData))]
    public class GiveItemIfNotHadNode : BaseNode
    {
        public GiveItemIfNotHadData GiveItemIfNotHadData => (GiveItemIfNotHadData)NodeData;

        public GiveItemIfNotHadNode()
        {

        }
        public GiveItemIfNotHadNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/NodeStyleSheet", "Give Item If Not Had Node", _data)
        {
            AddGenericPorts();
        }
        public override BaseData Save() {
            GiveItemIfNotHadData data = new GiveItemIfNotHadData();
            SaveData(data);
            data.ChosenItem.selectedMemberName = GiveItemIfNotHadData.ChosenItem.selectedMemberName;
            data.ChosenItem.listCategory = GiveItemIfNotHadData.ChosenItem.listCategory;
            data.amount.Value = GiveItemIfNotHadData.amount.Value;
            return data;
        }
    }
}
