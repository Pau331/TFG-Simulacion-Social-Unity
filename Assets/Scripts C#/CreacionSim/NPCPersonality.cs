using UnityEngine;

public class NPCPersonality : MonoBehaviour
{
    public ActivityTrait activity;
    public FoodTrait food;
    public CleanlinessTrait cleanliness;
    public LeisureTrait leisure;

    void Start()
    {
        activity = SimCreationData.Activity;
        food = SimCreationData.Food;
        cleanliness = SimCreationData.Cleanliness;
        leisure = SimCreationData.Leisure;

        Debug.Log($"Activity: {activity}");
    }
}