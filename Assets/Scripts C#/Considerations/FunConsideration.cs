using UnityEngine;

public class FunConsideration : INeedConsideration
{
    public float Evaluate(GameObject agent, InteractableObject target)
    {
        var needs = GameManager.Instance.simNeeds;
        if (needs != null)
        {
            // Cuanto más aburrido esté, mayor será el valor de utilidad
            float normalizedFun = (100 - needs.fun) / 100f;
            return normalizedFun;
        }
        return 0f;
    }

    public float GetWeight()
    {
        return 0.8f;
    }

    public string GetName()
    {
        return "FunConsideration";
    }
}