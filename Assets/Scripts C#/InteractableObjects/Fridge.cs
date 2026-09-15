using UnityEngine;

public class Fridge : InteractableObject
{
    public float hungerGain = 80f;

    void Awake()
    {
        hungerGain = 80f;
    }

    public override void Interact(GameObject agent)
    {
        var needs = GameManager.Instance.simNeeds;
        Debug.Log("ANTES DE COMER: " + needs.hunger);
        needs.hunger = Mathf.Clamp(needs.hunger + hungerGain, 0, 100);
        Debug.Log("DESPUES DE COMER: " + needs.hunger);
        Debug.Log($"Comiendo de la nevera -> +{hungerGain} hambre");
    }
}