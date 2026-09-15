using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public CharacterNeeds sim;
    public Slider hungerBar;
    public Slider energyBar;
    public Slider bladderBar;
    public Slider hygieneBar;
    public Slider funBar;
    
    public TextMeshProUGUI estadoTexto;
    public Image image;
    public Sprite simSprite;

    // Cambiar color de las barras
    public Image hungerFill;
    public Image energyFill;
    public Image bladderFill;
    public Image hygieneFill;
    public Image funFill;

    void Update()
    { 
        // Actualizar UI
        hungerBar.value = sim.hunger;
        energyBar.value = sim.energy; 
        bladderBar.value = sim.bladder;
        hygieneBar.value = sim.hygiene;
        funBar.value = sim.fun;
        image.sprite = simSprite;

        // Cambiar color de las barras
        UpdateColorBar(hungerBar, hungerFill);
        UpdateColorBar(energyBar, energyFill);
        UpdateColorBar(bladderBar, bladderFill);
        UpdateColorBar(hygieneBar, hygieneFill);
        UpdateColorBar(funBar, funFill);
        UpdateState();
    }

    void UpdateState()
    {
        float media =
            (sim.hunger * 0.25f +
             sim.energy * 0.25f +
             sim.fun * 0.15f +
             sim.hygiene * 0.15f +
             sim.bladder * 0.2f);

        if (media >= 75)
            estadoTexto.text = "Contento";
        else if (media >= 50)
            estadoTexto.text = "Normal";
        else
            estadoTexto.text = "Incómodo";
    }

    void UpdateColorBar(Slider slider, Image fill)
    {
        if (slider.value <= 25)
        {
            fill.color = Color.red;
        }
        else if (slider.value <= 60)
        {
            fill.color = new Color(1f, 0.5f, 0f); // naranja
        }
        else
        {
            fill.color = Color.green;
        }
    }

}