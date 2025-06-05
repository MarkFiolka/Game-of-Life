using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScrollLogic
{
    private readonly ScrollView scrollView;
    private readonly PatternList patternList;
    private readonly List<Button> buttons = new();
    private float currentScrollTargetY = 0f;
    private float scrollVelocity = 0f;
    private bool isUserScrolling = false;
    private const float ButtonHeight = 104f;
    private int objCount = 0;
    private Button selectedButton;

    public ScrollLogic(ScrollView scrollView, PatternList patternList)
    {
        this.scrollView = scrollView;
        this.patternList = patternList;
        this.objCount = patternList.getObjectCount();
    }

    public void PopulateButtons()
    {
        scrollView.Clear();
        buttons.Clear();

        for (int i = -objCount; i < objCount * 2; i++)
        {
            int index = (i + objCount) % objCount;
            var pattern = patternList.getObjectAtIndex(index);

            var button = new Button();
            button.style.marginBottom = 4;
            button.style.width = Length.Percent(70);
            button.style.height = 100;
            button.style.alignSelf = Align.FlexEnd;

            var label = new Label(pattern.name);
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.fontSize = 50;
            label.style.flexGrow = 1;
            label.style.unityFontStyleAndWeight = FontStyle.Bold;

            button.Add(label);
            button.clicked += () => CenterButton(button);

            scrollView.Add(button);
            buttons.Add(button);
        }
    }

    public void CenterInitialScroll()
    {
        float centerY = objCount * ButtonHeight;
        scrollView.scrollOffset = new Vector2(0, centerY);
        currentScrollTargetY = centerY;
    }

    public void UpdateScroll(float scrollDelta)
    {
        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            float delta = -scrollDelta * 200f;
            currentScrollTargetY = scrollView.scrollOffset.y + delta;
            isUserScrolling = true;
        }

        float currentY = scrollView.scrollOffset.y;
        float newY = Mathf.SmoothDamp(currentY, currentScrollTargetY, ref scrollVelocity, 0.15f);
        scrollView.scrollOffset = new Vector2(0, newY);

        if (isUserScrolling && Mathf.Abs(newY - currentScrollTargetY) < 0.1f)
        {
            scrollView.scrollOffset = new Vector2(0, currentScrollTargetY);
            scrollVelocity = 0f;
            isUserScrolling = false;
        }

        HandleLooping();
        UpdateVisuals();
    }

    private void HandleLooping()
    {
        float offsetY = scrollView.scrollOffset.y;
        float loopHeight = objCount * ButtonHeight;

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

    private void UpdateVisuals()
    {
        float centerY = scrollView.worldBound.center.y;
        float topY = scrollView.worldBound.yMin;
        float bottomY = scrollView.worldBound.yMax;

        float minDistance = float.MaxValue;
        Button closest = null;

        foreach (var button in buttons)
        {
            float buttonY = button.worldBound.center.y;
            float normalizedY = Mathf.InverseLerp(topY, bottomY, buttonY);

            float curveOffset = -Mathf.Sin(normalizedY * Mathf.PI) * 100f;
            button.style.translate = new Translate(curveOffset, 0, 0);

            float distance = Mathf.Abs(centerY - buttonY);
            float t = Mathf.Clamp01(distance / 400f);
            button.style.opacity = Mathf.Lerp(1f, 0.3f, t);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = button;
            }
        }

        if (selectedButton != null)
            selectedButton.RemoveFromClassList("selected");

        if (closest != null)
        {
            closest.AddToClassList("selected");
            selectedButton = closest;
        }
    }

    private void CenterButton(VisualElement button)
    {
        float containerHeight = scrollView.worldBound.height;
        float buttonHeight = button.resolvedStyle.height;
        float buttonTop = button.layout.y;

        float targetOffset = buttonTop - (containerHeight - buttonHeight) / 2f;
        currentScrollTargetY = targetOffset;
        isUserScrolling = true;
    }
}
