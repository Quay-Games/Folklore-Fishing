using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(GiveBaitData))]
    public class GiveBaitNode : BaseNode
    {
        private GiveBaitData giveBaitData = new GiveBaitData();
        // a bit of a hack because the integer field doesnt hold its own UI reference, im still learning about using these
        private IntegerField amountField;
        public GiveBaitData GiveBaitData
        {
            get => giveBaitData;
            set => giveBaitData = value;
        }

        public GiveBaitNode()
        {

        }
        public GiveBaitNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/NodeStyleSheet", "Give Bait", _data)
        {
            MakeMainContainer();

            if (_data != null)
            {
                GiveBaitData.BaitValues.Value = (_data as GiveBaitData).BaitValues.Value;
                GiveBaitData.amount.Value = (_data as GiveBaitData).amount.Value;
                LoadValueInToField();
            }

            AddGenericPorts();
        }

        private void MakeMainContainer()
        {
            EnumField enumField = GetNewGenericEnum(giveBaitData.BaitValues);
            enumField.label = "Bait Type";
            amountField = GetNewIntegerField(giveBaitData.amount);
            amountField.label = "Amount";

            mainContainer.Add(enumField);
            mainContainer.Add(amountField);
        }

        public override void LoadValueInToField()
        {
            if (GiveBaitData.BaitValues.EnumField != null)
            {
                GiveBaitData.BaitValues.EnumField.SetValueWithoutNotify(GiveBaitData.BaitValues.Value);
            }

            if(GiveBaitData.amount != null)
            {
                amountField.SetValueWithoutNotify(GiveBaitData.amount.Value);
            }
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
