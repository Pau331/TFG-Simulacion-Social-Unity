using UnityEngine;

public class Bed : InteractableObject
{
    [SerializeField] private Transform sleepPoint;
    public float energyGain = 100f;
    public Transform SleepPoint => sleepPoint;

    void Awake()
    {
        // Asegurar el valor por defecto en tiempo de ejecución
        energyGain = 100f;
    }

    public override void Interact(GameObject agent)
    {
        var needs = GameManager.Instance.simNeeds;
        Debug.Log("ANTES DE DORMIR: " + needs.energy);
        needs.energy = Mathf.Clamp(needs.energy + energyGain, 0, 100);
        Debug.Log("DESPUES DE DORMIR: " + needs.energy);
        Debug.Log($"Durmiendo en la cama -> +{energyGain} energía");
    }
}