using TMPro;
using UnityEngine;

public class StatisticsUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Dropdown needsDropdown;
    public GameObject contentPanel;
    public PieChartUI pieChart;
    public LineChartUI lineChart;

    public TMP_Text eatText;
    public TMP_Text sleepText;
    public TMP_Text showerText;
    public TMP_Text toiletText;
    public TMP_Text tvText;

    private bool isPanelActive;

    void Start()
    {
        contentPanel.SetActive(false);
        isPanelActive = false;

    }

    void Update()
    {
        if(isPanelActive)
        {
            UpdateUI();
        }
    }

    public void TogglePanel()
    {
        isPanelActive = !isPanelActive;
        contentPanel.SetActive(isPanelActive);

        if (isPanelActive)
        {
            UpdateUI();
        }
     
    }

    public void UpdateUI()
    {
        if (StatisticsManager.Instance == null) return;

        var stats = StatisticsManager.Instance;

        eatText.text = $"Comer: {stats.eatCount} \n";
        sleepText.text = $"Dormir: {stats.sleepCount} \n";
        showerText.text = $"Ducha: {stats.showerCount} \n";
        toiletText.text = $"Baño: {stats.toiletCount} \n";
        tvText.text = $"TV: {stats.tvCount} \n";

        // Update the pie chart
        if (pieChart != null)
        {
            pieChart.UpdateChart(
                StatisticsManager.Instance.eatTime,
                StatisticsManager.Instance.sleepTime,
                StatisticsManager.Instance.showerTime,
                StatisticsManager.Instance.toiletTime,
                StatisticsManager.Instance.tvTime
            );
        }

        ChangeNeedChart(needsDropdown.value);

    }

    void ChangeNeedChart(int option)
    {
        if (!isPanelActive)
            return;

        switch (option)
        {
            case 0:
                lineChart.SetData(StatisticsManager.Instance.hungerHistory, Color.red);
                break;

            case 1:
                lineChart.SetData(StatisticsManager.Instance.energyHistory, Color.blue);
                break;

            case 2:
                lineChart.SetData(StatisticsManager.Instance.hygieneHistory, Color.green);
                break;

            case 3:
                lineChart.SetData(StatisticsManager.Instance.funHistory, new Color(1f, 0.5f, 0f)); // Orange
                break;
            case 4:
                lineChart.SetData(StatisticsManager.Instance.bladderHistory, new Color(0.5f, 0f, 0.5f)); // Purple
                break;
        }
    }
}