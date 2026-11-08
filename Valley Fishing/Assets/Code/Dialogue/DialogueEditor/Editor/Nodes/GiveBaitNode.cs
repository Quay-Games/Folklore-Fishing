using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(GiveBaitData))]
    public class GiveBaitNode : BaseNode
    {
        public GiveBaitData GiveBaitData => (GiveBaitData)NodeData;

        public GiveBaitNode()
        {

        }
        public GiveBaitNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/NodeStyleSheet", "Give Bait", _data)
        {
            MakeMainContainer();

            AddGenericPorts();
        }

        private void MakeMainContainer()
        {
            IntegerField amountField = GetNewIntegerField(GiveBaitData.amount);
            amountField.label = "Amount";

            mainContainer.Add(amountField);
        }

        public override BaseData Save()
        {
            GiveBaitData data = new GiveBaitData();
            SaveData(data);
            data.BaitValues.Value = GiveBaitData.BaitValues.Value;
            data.amount.Value = GiveBaitData.amount.Value;
            return data;
        }
    }
}
