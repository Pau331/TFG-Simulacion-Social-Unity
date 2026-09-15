using UnityEngine;
using UnityEngine.UI;

public class PieChartUI : MonoBehaviour
{
    public Image eatSlice;
    public Image sleepSlice;
    public Image showerSlice;
    public Image toiletSlice;
    public Image tvSlice;


    public void UpdateChart(float eat, float sleep, float shower, float toilet, float tv)
    {
        float total = eat + sleep + shower + toilet + tv;

        if (total <= 0f) return;

        float eatPct = eat / total;
        float sleepPct = sleep / total;
        float showerPct = shower / total;
        float toiletPct = toilet / total;
        float tvPct = tv / total;

        float fill = 0f;

        fill = SetSlice(eatSlice, fill, eatPct);
        fill = SetSlice(sleepSlice, fill, sleepPct);
        fill = SetSlice(showerSlice, fill, showerPct);
        fill = SetSlice(toiletSlice, fill, toiletPct);
        fill = SetSlice(tvSlice, fill, tvPct);
    }

    private float SetSlice(Image img, float start, float amount)
    {
        img.fillAmount = amount;
        img.transform.localRotation = Quaternion.Euler(0, 0, -start * 360f);
        return start + amount;
    }
}