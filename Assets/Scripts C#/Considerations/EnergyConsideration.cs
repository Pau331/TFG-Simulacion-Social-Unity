using UnityEngine;

public class EnergyConsideration : INeedConsideration
{
    public float Evaluate(GameObject agent, InteractableObject target)
    {
        var needs = GameManager.Instance.simNeeds;
        if (needs != null)
        {
            // Cuanta más energía tenga, menor será el valor de utilidad
            float deficit = (100 - needs.energy) / 100f;
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
        return "EnergyConsideration";
    }
}