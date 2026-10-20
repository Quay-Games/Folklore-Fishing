using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(LevelStateData))]
    public class LevelStateNode : BaseNode
    {
        private LevelStateData levelStateData = new LevelStateData();
        public LevelStateData LevelStateData
        {
            get => levelStateData;
            set => levelStateData = value;
        }

        public LevelStateNode()
        {

        }
        public LevelStateNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/NodeStyleSheet", "Level State Change", _data)
        {

            if (_data != null)
            {
                LevelStateData.LevelStateValues.Value = (_data as LevelStateData).LevelStateValues.Value;
                LoadValueInToField();
            }

            AddGenericPorts();

            MakeMainContainer();
        }

        private void MakeMainContainer()
        {
            EnumField enumField = GetNewGenericEnum(levelStateData.LevelStateValues);

            mainContainer.Add(enumField);
        }

        public override void LoadValueInToField()
        {
            if (LevelStateData.LevelStateValues.EnumField != null)
                LevelStateData.LevelStateValues.EnumField.SetValueWithoutNotify(LevelStateData.LevelStateValues.Value);
        }

        public override BaseData Save()
        {
            LevelStateData data = new LevelStateData();
            SaveData(data);
            data.LevelStateValues.Value = LevelStateData.LevelStateValues.Value;
            return data;
        }
    }
}
