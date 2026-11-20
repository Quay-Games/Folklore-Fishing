using UnityEngine;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(ListenData))]
    public class ListenNode : BaseNode
    {

        private ListenData ListenData => (ListenData)NodeData;

        public ListenNode()
        {
        }

        public ListenNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
        : base(_position, _editorWindow, _graphView, "USS/Nodes/EventNodeStyleSheet", "ListenNode", _data)
        {
            AddGenericPorts();
        }

        public override BaseData Save()
        {
            ListenData nodeData = new ListenData();
            SaveData(nodeData);
            nodeData.listenEventContainer.Value = ListenData.listenEventContainer.Value;
            return nodeData;
        }
    }
}
