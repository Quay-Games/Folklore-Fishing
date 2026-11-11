using UnityEngine;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(SceneChangeData))]
    public class SceneChangeNode : BaseNode
    {
        public SceneChangeData SceneChangeData => (SceneChangeData)NodeData;

        public SceneChangeNode()

        {
        }

        public SceneChangeNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/DialogueNodeStyleSheet", "Scene Change", _data)
        {
            AddGenericPorts();
        }

        public override BaseData Save()
        {
            SceneChangeData data = new SceneChangeData();
            SaveData(data);
            data.SceneValues = SceneChangeData.SceneValues;

            return data;
        }
    }
}
