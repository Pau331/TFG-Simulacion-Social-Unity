using UnityEngine;

public class BladderConsideration : INeedConsideration
{
    public float Evaluate(GameObject agent, InteractableObject target)
    {
        var needs = GameManager.Instance.simNeeds;
        if (needs != null)
        {
            // Cuanto más necesite ir al baño, mayor será el valor de utilidad
            float normalizedBladder = (100 - needs.bladder) / 100f;
            return normalizedBladder;
        }
        return 0f;
    }

    public float GetWeight()
    {
        return 0.8f;
    }
    
    public string GetName()
    {
        return "BladderConsideration";
    }
}