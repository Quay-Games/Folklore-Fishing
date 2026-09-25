using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * This class deals with what is seen in the search window for the node graph. It should automatically populate data from
 * Node types we create. 
 */
namespace Project.DialogueEditor {
	public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider {

		private DialogueEditorWindow editorWindow;
		private DialogueGraphView graphView;

		private Texture2D iconImage;
		//where nodes should sit
		private const string NodeFolderPath = "Assets/Code/Dialogue/DialogueEditor/Editor/Nodes";

		public void Configure(DialogueEditorWindow _editorWindow, DialogueGraphView _graphView) {
			editorWindow = _editorWindow;
			graphView = _graphView;

			iconImage = new Texture2D(1, 1);
			iconImage.SetPixel(0, 0, new Color(0, 0, 0, 0));
			iconImage.Apply();
		}

		/*
		 * This function will create a list of all the nodes that exist in the Nodes folder and adds them to
		 * the search window.
		 */
		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {

			List<SearchTreeEntry> tree = new List<SearchTreeEntry> {
			new SearchTreeGroupEntry(new GUIContent("Dialogue Editor"),0),
			new SearchTreeGroupEntry(new GUIContent("Dialogue Node"),1),
		};

			foreach (BaseNode node in GetSearchableNodes()) {
				tree.Add(AddNodeSearch(GetNodeSearchName(node.GetType().Name), node));
			}

			return tree;
		}

		/* 
		 * This function is called when a node type is clicked in the menu, it will create a node of that type where
		 * the user clicked.
		 */
        public bool OnSelectEntry(SearchTreeEntry _searchTreeEntry, SearchWindowContext _context)
        {

            Vector2 mousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent, _context.screenMousePosition - editorWindow.position.position);
            Vector2 graphMousePosition = graphView.contentViewContainer.WorldToLocal(mousePosition);
            return CreateNodeOfType(_searchTreeEntry, graphMousePosition);
        }

        /*
 * This function gets a list of all the nodes in the nodes folder that extend from BaseNode
 */
        private List<BaseNode> GetSearchableNodes()
        {
            List<BaseNode> nodes = new List<BaseNode>();
            string[] scriptGuids = AssetDatabase.FindAssets("t:MonoScript", new[] { NodeFolderPath });

            foreach (string scriptGuid in scriptGuids)
            {
                string scriptPath = AssetDatabase.GUIDToAssetPath(scriptGuid);
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
                Type nodeType = script != null ? script.GetClass() : null;

                if (!IsSearchableNodeType(nodeType))
                {
                    continue;
                }

                try
                {
                    if (Activator.CreateInstance(nodeType) is BaseNode node)
                    {
                        nodes.Add(node);
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogWarning($"Could not add {nodeType.Name} to the dialogue node search window. Make sure it has an empty constructor. {exception.Message}");
                }
            }

            nodes.Sort((first, second) => string.Compare(GetNodeSearchName(first.GetType().Name), GetNodeSearchName(second.GetType().Name), StringComparison.Ordinal));
            return nodes;
        }

        //a function checking a type is not null, not abstract, and is a subclass of BaseNode
        private bool IsSearchableNodeType(Type nodeType)
        {
            return nodeType != null
                && nodeType != typeof(BaseNode)
                && !nodeType.IsAbstract
                && typeof(BaseNode).IsAssignableFrom(nodeType);
        }

        private SearchTreeEntry AddNodeSearch(string _name, BaseNode _baseNode)
        {
            SearchTreeEntry tmp = new SearchTreeEntry(new GUIContent(_name, iconImage))
            {
                level = 2,
                userData = _baseNode
            };
            return tmp;
        }

        /*
		 * This function basically takes a type name that contains the word Node and returns a string
		 * with a space before the word Node
		 */
        private string GetNodeSearchName(string typeName) {
			const string nodeSuffix = "Node";

			if (typeName.EndsWith(nodeSuffix, StringComparison.Ordinal)) {
				typeName = typeName.Substring(0, typeName.Length - nodeSuffix.Length);
			}

			return $"{AddSpacesToPascalCase(typeName)} Node";
		}

        /*
		 * This function takes a string in pascal case and adds spaces before each capital letter, except
		 * for the first letter
		 */
        private string AddSpacesToPascalCase(string text) {
			StringBuilder result = new StringBuilder();

			for (int i = 0; i < text.Length; i++) {
				char currentCharacter = text[i];
				bool shouldAddSpace = i > 0
					&& char.IsUpper(currentCharacter)
					&& (!char.IsUpper(text[i - 1]) || (i + 1 < text.Length && char.IsLower(text[i + 1])));

				if (shouldAddSpace) {
					result.Append(' ');
				}

				result.Append(currentCharacter);
			}

			return result.ToString();
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
