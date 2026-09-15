using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterCreationUI : MonoBehaviour
{
    [Header("Activity")]
    public Toggle activeToggle;
    public Toggle lazyToggle;

    [Header("Food")]
    public Toggle gluttonToggle;
    public Toggle moderateToggle;

    [Header("Cleanliness")]
    public Toggle cleanFreakToggle;
    public Toggle carelessToggle;

    [Header("Leisure")]
    public Toggle addictedToggle;
    public Toggle indifferentToggle;

    public void StartGame()
    {
        // Activity
        if (activeToggle.isOn)
            SimCreationData.Activity = ActivityTrait.Active;
        else if (lazyToggle.isOn)
            SimCreationData.Activity = ActivityTrait.Lazy;
        else
            SimCreationData.Activity = ActivityTrait.None;

        // Food
        if (gluttonToggle.isOn)
            SimCreationData.Food = FoodTrait.Glutton;
        else if (moderateToggle.isOn)
            SimCreationData.Food = FoodTrait.Moderate;
        else
            SimCreationData.Food = FoodTrait.None;      

        // Cleanliness
        if (cleanFreakToggle.isOn)
            SimCreationData.Cleanliness = CleanlinessTrait.CleanFreak;
        else if (carelessToggle.isOn)
            SimCreationData.Cleanliness = CleanlinessTrait.Careless;
        else
            SimCreationData.Cleanliness = CleanlinessTrait.None;

        // Leisure
        if (addictedToggle.isOn)
            SimCreationData.Leisure = LeisureTrait.Addicted;
        else if (indifferentToggle.isOn)
            SimCreationData.Leisure = LeisureTrait.Indifferent;
        else
            SimCreationData.Leisure = LeisureTrait.None;

        SceneManager.LoadScene("GameScene");
    }
}