using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineChartUI : MonoBehaviour
{
    public RectTransform graphContainer;
    public RectTransform pointPrefab;

    private List<float> currentValues;
    private Color currentColor;

    public void SetData(List<float> values, Color color)
    {
        currentValues = values;
        currentColor = color;

        DrawGraph();
    }

    void DrawGraph()
    {
        if (currentValues == null || currentValues.Count < 2)
            return;

        // Limpiar gráfico anterior
        ClearChart();

        float width = graphContainer.rect.width;
        float height = graphContainer.rect.height;

        float xSpacing = width / Mathf.Max(currentValues.Count - 1, 1);

        RectTransform previousPoint = null;

        for (int i = 0; i < currentValues.Count; i++)
        {
            RectTransform point = Instantiate(pointPrefab, graphContainer);

            point.anchoredPosition = new Vector2(
                i * xSpacing,
                currentValues[i] / 100f * height
            );

            point.GetComponent<Image>().color = currentColor;

            previousPoint = point;
        }
    }


    public void ClearChart()
    {
        for (int i = graphContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(graphContainer.GetChild(i).gameObject);
        }
    }
}