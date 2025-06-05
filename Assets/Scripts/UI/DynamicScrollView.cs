using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DynamicScrollView : MonoBehaviour
{
    private ScrollView scrollView;
    private List<Button> buttons = new List<Button>();

    private float scrollVelocity = 0f;
    private float currentScrollTargetY = 0f;
    private bool isUserScrolling = false;
    private float smoothTime = 0.15f;

    private const int TotalButtons = 111;
    private const int VisibleCount = 9;
    private const float ButtonHeight = 104f;
    private const float LoopTriggerDistance = ButtonHeight * TotalButtons * 0.5f;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.style.flexDirection = FlexDirection.Row;

        var leftSpacer = new VisualElement();
        leftSpacer.style.flexGrow = 8;

        var rightContainer = new VisualElement();
        rightContainer.style.flexGrow = 4;
        rightContainer.style.flexDirection = FlexDirection.Column;

        scrollView = new ScrollView();
        scrollView.style.flexGrow = 1;
        scrollView.style.marginTop = 10;
        scrollView.style.marginBottom = 10;
        scrollView.style.marginLeft = 10;
        scrollView.style.marginRight = 10;
        scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;

        rightContainer.Add(scrollView);
        root.Add(leftSpacer);
        root.Add(rightContainer);

        PopulateButtons();
        CenterScroll();
    }

    void LateUpdate()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(wheelInput) > 0.01f)
        {
            float delta = -wheelInput * 200f;
            currentScrollTargetY = scrollView.scrollOffset.y + delta;
            isUserScrolling = true;
        }

        float currentY = scrollView.scrollOffset.y;
        float newY = Mathf.SmoothDamp(currentY, currentScrollTargetY, ref scrollVelocity, smoothTime);
        scrollView.scrollOffset = new Vector2(0, newY);

        if (isUserScrolling && Mathf.Abs(newY - currentScrollTargetY) < 0.1f)
        {
            scrollView.scrollOffset = new Vector2(0, currentScrollTargetY);
            scrollVelocity = 0f;
            isUserScrolling = false;
        }

        HandleLooping();
    }

    private void PopulateButtons()
    {
        scrollView.Clear();
        buttons.Clear();

        for (int i = -TotalButtons; i < TotalButtons * 2; i++)
        {
            int index = (i + TotalButtons) % TotalButtons;
            var button = new Button();
            button.style.marginBottom = 4;
            button.style.width = Length.Percent(70);
            button.style.height = 100;
            button.style.alignSelf = Align.FlexEnd;

            var label = new Label($"Item {index + 1}");
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.fontSize = 100;
            label.style.flexGrow = 1;
            label.style.unityFontStyleAndWeight = FontStyle.Bold;

            button.Add(label);
            scrollView.Add(button);
            buttons.Add(button);
        }
    }

    private void CenterScroll()
    {
        float centerY = TotalButtons * ButtonHeight;
        scrollView.scrollOffset = new Vector2(0, centerY);
        currentScrollTargetY = centerY;
    }

    private void HandleLooping()
    {
        float offsetY = scrollView.scrollOffset.y;
        float loopHeight = TotalButtons * ButtonHeight;

        if (offsetY < ButtonHeight)
        {
            scrollView.scrollOffset += new Vector2(0, loopHeight);
            currentScrollTargetY += loopHeight;
        }
        else if (offsetY > loopHeight * 2)
        {
            scrollView.scrollOffset -= new Vector2(0, loopHeight);
            currentScrollTargetY -= loopHeight;
        }
    }
}
