using UnityEngine.UIElements;

public static class ScrollUIBuilder
{
    public static ScrollView BuildScrollUI(VisualElement root)
    {
        root.style.flexDirection = FlexDirection.Row;

        var leftSpacer = new VisualElement();
        leftSpacer.style.flexGrow = 8;

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

        rightContainer.Add(scrollView);
        root.Add(leftSpacer);
        root.Add(rightContainer);

        return scrollView;
    }
}