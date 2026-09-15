using UnityEngine;

public class HungerConsideration : INeedConsideration
{
    public float Evaluate(GameObject agent, InteractableObject target)
    {
        var needs = GameManager.Instance.simNeeds;
        if (needs != null)
        {
            // Cuanto más hambre tenga, mayor será el valor de utilidad
            float deficit = (100 - needs.hunger) / 100f;
            return deficit * deficit;
        }
        return 0f;
    }

    public float GetWeight()
    {
        return 0.8f;
    }

    public string GetName()
    {
        return "HungerConsideration";
    }
}