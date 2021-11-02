using UnityEngine.UIElements;

namespace BehaviourTreeEditorWindow
{
    public class SplitView : TwoPaneSplitView
    {
        //Factory that allows unity to see this Element
        public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits>
        {
        }
    }
}