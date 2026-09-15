using UnityEngine;

public class DistanceConsideration : IConsideration
{
    [SerializeField] private float maxDistance = 10f;
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

        float normalizedDistance = Mathf.Clamp01(distance / maxDistance);
        return 1f - normalizedDistance;
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
