using UnityEngine;
using UnityEngine.UIElements;

public static class ScrollUIBuilder
{
    public static (ScrollView scrollView, VisualElement previewArea, Button useButton) BuildUI(VisualElement root)
    {
        root.style.flexDirection = FlexDirection.Row;

        var previewArea = new VisualElement();
        previewArea.style.width = new Length(60, LengthUnit.Percent);
        previewArea.style.height = new Length(100, LengthUnit.Percent);
        previewArea.style.backgroundColor = new Color(0, 0, 0, 0);
        previewArea.style.alignItems = Align.Center;
        previewArea.style.justifyContent = Justify.Center;

        var rightContainer = new VisualElement();
        rightContainer.style.flexGrow = 4;
        rightContainer.style.flexDirection = FlexDirection.Column;

        var scrollView = new ScrollView();
        scrollView.style.flexGrow = 1;
        scrollView.style.marginTop = 10;
        scrollView.style.marginBottom = 10;
        scrollView.style.marginLeft = 10;
        scrollView.style.marginRight = 10;
        scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;

        var useButton = new Button(() => {}) { text = "Use" };
        useButton.style.position = Position.Absolute;
        useButton.style.right = 10;
        useButton.style.top = new Length(50, LengthUnit.Percent);
        useButton.style.translate = new Translate(0, -20);
        useButton.style.width = 100;
        useButton.style.height = 40;

        root.Add(previewArea);
        root.Add(rightContainer);
        root.Add(useButton);

        rightContainer.Add(scrollView);

        return (scrollView, previewArea, useButton);
    }
}