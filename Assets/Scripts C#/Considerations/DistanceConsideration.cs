using UnityEngine;

public class DistanceConsideration : IConsideration
{
    public float Evaluate(GameObject agent, InteractableObject target)
    {
        if (target == null) return 0f;
        Vector3 targetPosition;

        // Utilizar el punto de interacción si está definido
        if (target.interactionPoint != null)
            targetPosition = target.interactionPoint.position;
        else
            targetPosition = target.transform.position;

        float distance = Vector3.Distance(
            agent.transform.position,
            targetPosition
        );

        return 1f / (1f + distance); // cercano -> cerca de 1, lejano -> cerca de 0
    }

    public float GetWeight()
    {
        return 0.2f; // Peso de la consideración de distancia
    }
    
    public string GetName()
    {
        return "DistanceConsideration";
    }
}
