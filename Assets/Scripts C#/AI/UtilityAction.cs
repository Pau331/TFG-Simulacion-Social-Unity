using System.Collections.Generic;
using UnityEngine;

public abstract class UtilityAction
{
    public string actionName;
    public InteractableObject target;

    public abstract System.Type GetTargetType();

    public List<IConsideration> considerations = new();

    protected float EvaluateConsiderations(GameObject agent, float personalityModifier = 1f)
    {
        if (considerations == null || considerations.Count == 0)
            return 1f;

        float sum = 0f;
        float weightSum = 0f;
        foreach (var c in considerations)
        {
            try
            {
                float v = Mathf.Clamp01(c.Evaluate(agent, target));

                // El modificador de personalidad solo afecta a la consideración
                // de la necesidad, nunca a la distancia u otras consideraciones.
                if (c is INeedConsideration)
                    v = Mathf.Clamp01(v * personalityModifier);
                    Debug.Log($"[Utility] Consideration={c.GetType().Name} Value={v:F2}");

                float w = c.GetWeight();
                sum += v * w;
                weightSum += w;
            }
            catch { /* Ignorar errores. */ }
        }

        if (weightSum <= 0) return 1f;
        return sum / weightSum;
    }
    

    public abstract void Execute(GameObject agent);
    public abstract float GetUtilityScore(GameObject agent);
    public abstract float GetDuration();
    public abstract string GetActionName();
    public abstract string GetActionText();
}