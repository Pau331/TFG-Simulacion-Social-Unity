using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionProgressUI : MonoBehaviour
{
    public Slider slider;
    public TMP_Text actionTxt;

    void Start()
    {
        slider.value = 0f;
        actionTxt.text = "";
        gameObject.SetActive(false);
    }

    public void SetProgress(float value)
    {
        slider.value = value;
    }

    public void SetText(string text)
    {
        actionTxt.text = text;
    }

    public void Show(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
