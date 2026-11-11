using UnityEngine;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(CatchOverrideData))]
    public class CatchOverrideNode : BaseNode
    {
        public CatchOverrideData CatchOverrideData => (CatchOverrideData)NodeData;

        public CatchOverrideNode()
        {
        }

        public CatchOverrideNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/DialogueNodeStyleSheet", "Catch Override", _data)
        {
            AddGenericPorts();
        }

        public override BaseData Save()
        {
            CatchOverrideData data = new CatchOverrideData();
            SaveData(data);
            data.FishToCatch = CatchOverrideData.FishToCatch;

            return data;
        }
    }
}
