using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider {

		private DialogueEditorWindow editorWindow;
		private DialogueGraphView graphView;

		private Texture2D iconImage;

		public void Configure(DialogueEditorWindow _editorWindow, DialogueGraphView _graphView) {
			editorWindow = _editorWindow;
			graphView = _graphView;

			iconImage = new Texture2D(1, 1);
			iconImage.SetPixel(0, 0, new Color(0, 0, 0, 0));
			iconImage.Apply();
		}

		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {

			List<SearchTreeEntry> tree = new List<SearchTreeEntry> {
			new SearchTreeGroupEntry(new GUIContent("Dialogue Editor"),0),
			new SearchTreeGroupEntry(new GUIContent("Dialogue Node"),1),

			//This can be made generic, just look at the nodes folder and create instances of the class for each file
			//that isnt base node. just make sure each of these classes has an empty constructor
			AddNodeSearch("Branch Node", new BranchNode()),
			AddNodeSearch("Dialogue Node", new DialogueNode()),
			AddNodeSearch("Responce Node", new ResponceNode()),
			AddNodeSearch("Event Node", new EventNode()),
			AddNodeSearch("Condition Node", new ConditionNode()),
			AddNodeSearch("Start Node", new StartNode()),
			AddNodeSearch("End Node", new EndNode()),
			AddNodeSearch("ListenNode", new ListenNode()),
		};

			return tree;
		}

		private SearchTreeEntry AddNodeSearch(string _name, BaseNode _baseNode) {
			SearchTreeEntry tmp = new SearchTreeEntry(new GUIContent(_name, iconImage)) {
				level = 2,
				userData = _baseNode
			};
			return tmp;
		}

		public bool OnSelectEntry(SearchTreeEntry _searchTreeEntry, SearchWindowContext _context) {

			Vector2 mousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent, _context.screenMousePosition - editorWindow.position.position);
			Vector2 graphMousePosition = graphView.contentViewContainer.WorldToLocal(mousePosition);
			return CreateNodeOfType(_searchTreeEntry, graphMousePosition);
		}

		//this can be made generic, Im not entirely sure how at the moment, but it would be a check
		//agaisnt the type of the class
		private bool CreateNodeOfType(SearchTreeEntry _searchTreeEntry, Vector2 _pos) {
			return graphView.CreateEmptyNodeOfType(_searchTreeEntry.userData as BaseNode, _pos);
			//switch (_searchTreeEntry.userData) {
			//	case StartNode node:
			//		graphView.AddElement(graphView.CreateStartNode(_pos));
			//		return true;
			//	case DialogueNode node:
			//		graphView.AddElement(graphView.CreateNPCDialogueNode(_pos,true));
			//		return true;
			//	case ResponceNode node:
			//		graphView.AddElement(graphView.CreateResponceNode(_pos,true));
			//		return true;
			//	case EventNode node:
			//		graphView.AddElement(graphView.CreateNPCEventNode(_pos,true));
			//		return true;
			//	case ConditionNode node:
			//		graphView.AddElement(graphView.CreateNPCConditionNode(_pos, true));
			//		return true;
			//	case EndNode node:
			//		graphView.AddElement(graphView.CreateEndNode(_pos));
			//		return true;
			//	case BranchNode node:
			//		graphView.AddElement(graphView.CreateBranchNode(_pos));
			//		return true;
			//	case ListenNode node:
			//		graphView.AddElement(graphView.CreateListenNode(_pos));
			//		return true;
			//	default:
			//		break;
			//}
			//return false;
		}

	}
}
