using UnityEngine;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(ShowHideUIData))]
    public class ShowHideUiNode : BaseNode
    {
        public ShowHideUIData DialogueData => (ShowHideUIData)NodeData;

        public ShowHideUiNode()
        {
        }

        public ShowHideUiNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/DialogueNodeStyleSheet", "Show/Hide UI", _data)
        {
            AddGenericPorts();
        }

        public override BaseData Save()
        {
            ShowHideUIData data = new ShowHideUIData();
            SaveData(data);
            data.uiType.Value = DialogueData.uiType.Value ;
            data.showContainer.Value = DialogueData.showContainer.Value;

            return data;
        }
    }
}
