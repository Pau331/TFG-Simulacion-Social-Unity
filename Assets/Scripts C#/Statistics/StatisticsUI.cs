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

        eatText.text = "Comer: " + StatisticsManager.Instance.eatCount;
        sleepText.text = "Dormir: " + StatisticsManager.Instance.sleepCount;
        showerText.text = "Ducha: " + StatisticsManager.Instance.showerCount;
        toiletText.text = "Baño: " + StatisticsManager.Instance.toiletCount;
        tvText.text = "TV: " + StatisticsManager.Instance.tvCount;

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
                lineChart.SetData(StatisticsManager.Instance.funHistory, Color.orange);
                break;
            case 4:
                lineChart.SetData(StatisticsManager.Instance.bladderHistory, Color.purple);
                break;
        }
    }
}