using UnityEngine;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(UIOverrideData))]
    public class UIOverrideNode : BaseNode
    {
        public UIOverrideData UOData => (UIOverrideData)NodeData;

        public UIOverrideNode()
        {
        }

        public UIOverrideNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/DialogueNodeStyleSheet", "UI Functionality Override", _data)
        {
            AddGenericPorts();
        }

        public override BaseData Save()
        {
            UIOverrideData data = new UIOverrideData();
            SaveData(data);
            data.overrideEvent.Value = UOData.overrideEvent.Value;
            data.buttonToOverride.Value = UOData.buttonToOverride.Value;
            return data;
        }
    }
}
