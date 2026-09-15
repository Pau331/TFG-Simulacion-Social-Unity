using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public Transform interactionPoint;

    public abstract void Interact(GameObject agent);
}