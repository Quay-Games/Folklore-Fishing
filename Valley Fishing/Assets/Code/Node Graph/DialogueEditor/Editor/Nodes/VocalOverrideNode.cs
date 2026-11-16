using UnityEngine;

namespace Project.DialogueEditor {
    [NodeDataType(typeof(VocalOverrideData))]
    public class VocalOverrideNode : BaseNode {
        public VocalOverrideData DialogueData => (VocalOverrideData)NodeData;

        public VocalOverrideNode() {
        }

        public VocalOverrideNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/DialogueNodeStyleSheet", "Voice Line Override", _data) {
            AddGenericPorts();
        }

        public override BaseData Save() {
            VocalOverrideData data = new VocalOverrideData();
            SaveData(data);
            data.VoiceEvent = DialogueData.VoiceEvent;
            data.VoiceEventScenario = DialogueData.VoiceEventScenario;
            data.ProgressGraph = DialogueData.ProgressGraph;

            return data;
        }
    }
}
