using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(LevelStateData))]
    public class LevelStateNode : BaseNode
    {
        public LevelStateData LevelStateData => (LevelStateData)NodeData;

        public LevelStateNode()
        {

        }
        public LevelStateNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/NodeStyleSheet", "Level State Change", _data)
        {

            AddGenericPorts();
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
