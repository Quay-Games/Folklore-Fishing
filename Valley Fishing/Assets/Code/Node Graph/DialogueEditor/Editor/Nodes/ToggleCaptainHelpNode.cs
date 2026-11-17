using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(ToggleCaptainHelpData))]
    public class ToggleCaptainHelpNode : BaseNode
    {
        public ToggleCaptainHelpData ToggleCaptainHelpData => (ToggleCaptainHelpData)NodeData;

        public ToggleCaptainHelpNode()
        {

        }
        public ToggleCaptainHelpNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/NodeStyleSheet", "Toggle Captain Help", _data)
        {
            AddGenericPorts();
        }

        public override BaseData Save()
        {
            ToggleCaptainHelpData data = new ToggleCaptainHelpData();
            SaveData(data);
            data.ToggleCaptainHelp.Value = ToggleCaptainHelpData.ToggleCaptainHelp.Value;
            data.Seconds.Value = ToggleCaptainHelpData.Seconds.Value;
            return data;
        }
    }
}
