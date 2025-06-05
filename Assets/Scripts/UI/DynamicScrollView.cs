using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DynamicScrollView : MonoBehaviour
{
    private ScrollView scrollView;
    private ScrollLogic scrollLogic;
    private PatternList patternList;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var (scroll, previewArea, useButton) = ScrollUIBuilder.BuildUI(root);
        scrollView = scroll;

        patternList = GetComponent<PatternList>();
        if (patternList == null)
        {
            Debug.LogWarning("No Pattern List found!");
            return;
        }

        var previewBoard = new PatternPreviewBoard(previewArea);
        scrollLogic = new ScrollLogic(scrollView, patternList, previewBoard);
        scrollLogic.PopulateButtons();
        scrollLogic.CenterInitialScroll();
        
        useButton.clickable.clicked += () =>
        {
            Pattern selectedPattern = scrollLogic.GetSelectedPattern();
            if (selectedPattern != null)
            {
                Debug.Log("Selected Pattern: " + selectedPattern.name);
                PatternReceiver.Instance.UsePattern(selectedPattern);
                SceneManager.LoadScene("Game of Life");
                SceneManager.UnloadScene("Game of Life UI");
            }
        };
    }

    void LateUpdate()
    {
        scrollLogic.UpdateScroll(Input.GetAxis("Mouse ScrollWheel"));
    }
}