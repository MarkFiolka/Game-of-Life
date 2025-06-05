using UnityEngine;
using UnityEngine.UIElements;

public class DynamicScrollView : MonoBehaviour
{
    private ScrollView scrollView;
    private ScrollLogic scrollLogic;
    private PatternList patternList;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        scrollView = ScrollUIBuilder.BuildScrollUI(root);

        patternList = GetComponent<PatternList>();
        if (patternList == null)
        {
            Debug.LogWarning("No Pattern List found!");
            return;
        }

        scrollLogic = new ScrollLogic(scrollView, patternList);
        scrollLogic.PopulateButtons();
        scrollLogic.CenterInitialScroll();
    }

    void LateUpdate()
    {
        scrollLogic.UpdateScroll(Input.GetAxis("Mouse ScrollWheel"));
    }
}