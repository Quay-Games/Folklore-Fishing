using UnityEngine;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(EnterShopData))]
    public class EnterShopNode : BaseNode
    {

        private EnterShopData ESData => (EnterShopData)NodeData;

        public EnterShopNode()
        {
        }

        public EnterShopNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
        : base(_position, _editorWindow, _graphView, "USS/Nodes/EventNodeStyleSheet", "Enter Shop", _data)
        {
            AddGenericPorts();
        }

        public override BaseData Save()
        {
            EnterShopData nodeData = new EnterShopData();
            SaveData(nodeData);
            nodeData.selectedShop.Value = ESData.selectedShop.Value;
            return nodeData;
        }
    }
}
