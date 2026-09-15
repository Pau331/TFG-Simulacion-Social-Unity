using UnityEngine;

public interface IConsideration
{
    float Evaluate(GameObject agent, InteractableObject target);
    float GetWeight();
    string GetName();
}