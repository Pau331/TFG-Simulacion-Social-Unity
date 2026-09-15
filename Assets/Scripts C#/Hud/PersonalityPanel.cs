using TMPro;
using UnityEngine;

public class PersonalityPanel : MonoBehaviour
{
    [Header("Texts")]
    public TMP_Text activityText;
    public TMP_Text foodText;
    public TMP_Text cleanlinessText;
    public TMP_Text leisureText;

    void Start()
    {
        UpdatePersonalityPanel();
    }

    void UpdatePersonalityPanel()
    {
        // Activity
        switch (SimCreationData.Activity)
        {
            case ActivityTrait.Active:
                activityText.text = "Activo";
                break;

            case ActivityTrait.Lazy:
                activityText.text = "Perezoso";
                break;

            default:
                activityText.text = "Ninguno";
                break;
        }

        // Food
        switch (SimCreationData.Food)
        {
            case FoodTrait.Glutton:
                foodText.text = "Glotón";
                break;

            case FoodTrait.Moderate:
                foodText.text = "Moderado";
                break;

            default:
                foodText.text = "Ninguno";
                break;
        }

        // Cleanliness
        switch (SimCreationData.Cleanliness)
        {
            case CleanlinessTrait.CleanFreak:
                cleanlinessText.text = "Maniático";
                break;

            case CleanlinessTrait.Careless:
                cleanlinessText.text = "Descuidado";
                break;

            default:
                cleanlinessText.text = "Ninguno";
                break;
        }

        // Leisure
        switch (SimCreationData.Leisure)
        {
            case LeisureTrait.Addicted:
                leisureText.text = "Adicto";
                break;

            case LeisureTrait.Indifferent:
                leisureText.text = "Indiferente";
                break;

            default:
                leisureText.text = "Ninguno";
                break;
        }
    }
}