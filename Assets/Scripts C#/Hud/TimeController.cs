using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private const float DEFAULT_FIXED_DELTA = 0.02f;

    public Image x1Button;
    public Image x2Button;
    private Color32 activeColor = new Color32(200, 200, 200, 255); 
    private Color32 inactiveColor = Color.white;

    void Start()
    {
        SetNormalSpeed();
    }

    public void SetNormalSpeed()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = DEFAULT_FIXED_DELTA * Time.timeScale;
        x1Button.color = activeColor;
        x2Button.color = inactiveColor;
    }

    public void SetFastSpeed()
    {
        Time.timeScale = 2f;
        Time.fixedDeltaTime = DEFAULT_FIXED_DELTA * Time.timeScale;
        x1Button.color = inactiveColor;
        x2Button.color = activeColor;
    }


}