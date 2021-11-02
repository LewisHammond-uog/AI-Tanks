using UnityEditor;
using UnityEngine.UIElements;

namespace BehaviourTreeEditorWindow
{
    public class InspectorView : VisualElement
    {
        //Factory that allows unity to see this Element
        public new class  UxmlFactory : UxmlFactory<InspectorView, InspectorView.UxmlTraits> {}

        private Editor nodeInspector;

        public void UpdateSelection(NodeView nodeView)
        {
            //Clear previous elements
            Clear();
            UnityEngine.Object.DestroyImmediate(nodeInspector);
        
            //Create an editor (inspector) of the properties expose on the node
            nodeInspector = Editor.CreateEditor(nodeView.node);
            IMGUIContainer container = new IMGUIContainer(nodeInspector.OnInspectorGUI);
            Add(container);
        }
    }
}
