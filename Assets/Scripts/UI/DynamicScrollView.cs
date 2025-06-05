using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DynamicScrollView : MonoBehaviour
{
    private ScrollView scrollView;
    private List<Button> buttonList = new List<Button>();

    private float scrollVelocity = 0f;
    private float? targetScrollOffsetY = null;
    private float currentScrollTargetY = 0f;
    private bool isUserScrolling = false;
    private float smoothTime = 0.15f;

    private const int TotalButtons = 111;
    private const int VisibleCount = 9;
    private const float ButtonHeight = 104f;
    private float lastScrollY = 0f;

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

        PopulateScrollView();
    }

    private void PopulateScrollView()
    {
        scrollView.Clear();
        buttonList.Clear();

        for (int j = -VisibleCount; j < TotalButtons + VisibleCount; j++)
        {
            int i = (j + TotalButtons) % TotalButtons;

            var button = new Button();
            button.style.marginBottom = 4;
            button.style.width = Length.Percent(70);
            button.style.height = 100;
            button.style.alignSelf = Align.FlexEnd;

            var label = new Label($"Item {i + 1}");
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.fontSize = 100;
            label.style.flexGrow = 1;
            label.style.unityFontStyleAndWeight = FontStyle.Bold;

            button.Add(label);
            button.clicked += () => CenterButtonInScrollView(button);

            scrollView.Add(button);
            buttonList.Add(button);
        }

        currentScrollTargetY = scrollView.scrollOffset.y;
    }

    void LateUpdate()
    {
        float centerY = scrollView.worldBound.center.y;
        float topY = scrollView.worldBound.yMin;
        float bottomY = scrollView.worldBound.yMax;

        // Mouse wheel scroll input
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(wheelInput) > 0.01f)
        {
            float delta = -wheelInput * 200f;
            currentScrollTargetY = scrollView.scrollOffset.y + delta;
            isUserScrolling = true;
            targetScrollOffsetY = null;
        }

        if (targetScrollOffsetY.HasValue)
        {
            currentScrollTargetY = targetScrollOffsetY.Value;
            isUserScrolling = false;
        }

        float currentY = scrollView.scrollOffset.y;
        float newY = Mathf.SmoothDamp(currentY, currentScrollTargetY, ref scrollVelocity, smoothTime);
        scrollView.scrollOffset = new Vector2(0, newY);

        if ((isUserScrolling || targetScrollOffsetY.HasValue) && Mathf.Abs(newY - currentScrollTargetY) < 0.1f)
        {
            scrollView.scrollOffset = new Vector2(0, currentScrollTargetY);
            scrollVelocity = 0f;

            if (targetScrollOffsetY.HasValue)
                targetScrollOffsetY = null;

            isUserScrolling = false;
        }

        HandleLooping();

        foreach (var button in buttonList)
        {
            float buttonY = button.worldBound.center.y;
            float normalizedY = Mathf.InverseLerp(topY, bottomY, buttonY);

            float curveOffset = -Mathf.Sin(normalizedY * Mathf.PI) * 100f;
            button.style.translate = new Translate(curveOffset, 0, 0);

            float distance = Mathf.Abs(centerY - buttonY);
            float t = Mathf.Clamp01(distance / 400f);
            button.style.opacity = Mathf.Lerp(1f, 0.3f, t);
        }
    }


    private void HandleLooping()
    {
        float offsetY = scrollView.scrollOffset.y;
        float loopHeight = TotalButtons * ButtonHeight;

        bool isScrollingUp = offsetY < lastScrollY;
        bool isScrollingDown = offsetY > lastScrollY;

        // Track scroll direction
        lastScrollY = offsetY;

        float upperWrapThreshold = loopHeight * 1.25f;
        float lowerWrapThreshold = -loopHeight * 0.25f;

        if (isScrollingDown && offsetY > upperWrapThreshold)
        {
            scrollView.scrollOffset -= new Vector2(0, loopHeight);
            currentScrollTargetY -= loopHeight;
        }
        else if (isScrollingUp && offsetY < lowerWrapThreshold)
        {
            scrollView.scrollOffset += new Vector2(0, loopHeight);
            currentScrollTargetY += loopHeight;
        }
    }

    private void CenterButtonInScrollView(VisualElement button)
    {
        float containerHeight = scrollView.worldBound.height;
        float buttonHeight = button.resolvedStyle.height;
        float buttonTop = button.layout.y;

        float targetOffset = buttonTop - (containerHeight - buttonHeight) / 2f;
        targetScrollOffsetY = targetOffset;
    }
}
