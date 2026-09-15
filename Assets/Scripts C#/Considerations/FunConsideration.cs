using UnityEngine;

public class FunConsideration : INeedConsideration
{
    public float Evaluate(GameObject agent, InteractableObject target)
    {
        var needs = GameManager.Instance.simNeeds;
        if (needs != null)
        {
            // Cuanto más aburrido esté, mayor será el valor de utilidad
            float deficit = (100 - needs.fun) / 100f;
            return deficit * deficit;
        }
        return 0f;
    }

    public float GetWeight()
    {
        return 0.9f;
    }

    public string GetName()
    {
        return "FunConsideration";
    }
}