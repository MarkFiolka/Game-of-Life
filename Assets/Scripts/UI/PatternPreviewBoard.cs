using UnityEngine;
using UnityEngine.UIElements;

public class PatternPreviewBoard
{
    private readonly VisualElement previewArea;

    public PatternPreviewBoard(VisualElement previewArea)
    {
        this.previewArea = previewArea;
    }

    public void ShowPattern(Pattern pattern)
    {
        previewArea.Clear();

        if (pattern == null || pattern.cells == null || pattern.cells.Length == 0)
            return;

        Vector2Int center = pattern.GetCenter();
        float cellSize = 20f;

        var gridContainer = new VisualElement();
        gridContainer.style.flexDirection = FlexDirection.Column;
        gridContainer.style.alignItems = Align.Center;
        gridContainer.style.justifyContent = Justify.Center;

        Vector2Int min = pattern.cells[0];
        Vector2Int max = pattern.cells[0];
        foreach (var cell in pattern.cells)
        {
            min = Vector2Int.Min(min, cell);
            max = Vector2Int.Max(max, cell);
        }

        int width = max.x - min.x + 1;
        int height = max.y - min.y + 1;

        for (int y = height - 1; y >= 0; y--)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;

            for (int x = 0; x < width; x++)
            {
                var cell = new VisualElement();
                cell.style.width = cellSize;
                cell.style.height = cellSize;
                cell.style.marginRight = 1;
                cell.style.marginBottom = 1;

                Vector2Int worldPos = new Vector2Int(min.x + x, min.y + y);
                bool isAlive = System.Array.Exists(pattern.cells, c => c == worldPos);
                cell.style.backgroundColor = isAlive ? Color.white : Color.clear;

                row.Add(cell);
            }

            gridContainer.Add(row);
        }

        previewArea.Add(gridContainer);
    }
}