using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor
{
    [NodeDataType(typeof(PlayMusicData))]
    public class PlayMusicNode : BaseNode
    {

        public PlayMusicData PlayMusicData => (PlayMusicData)NodeData;

        public PlayMusicNode()
        {
        }
        public PlayMusicNode(Vector2 _position, NodeGraphEditorWindow _editorWindow, NodeGraphView _graphView, BaseData _data = null)
            : base(_position, _editorWindow, _graphView, "USS/Nodes/NodeStyleSheet", "Play Music", _data)
        {
            AddGenericPorts();
        }

        public override BaseData Save()
        {
            PlayMusicData data = new PlayMusicData();
            SaveData(data);
            data.MusicTrackHolder.Value = PlayMusicData.MusicTrackHolder.Value;
            return data;
        }
    }
}
