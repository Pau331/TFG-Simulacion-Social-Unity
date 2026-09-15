using UnityEngine;

public class Shower : InteractableObject
{
    public float hygieneGain = 100f;

    void Awake()
    {
        hygieneGain = 100f;
    }

    public override void Interact(GameObject agent)
    {
        var needs = GameManager.Instance.simNeeds;
        Debug.Log("ANTES DE DUCHARSE: " + needs.hygiene);
        needs.hygiene = Mathf.Clamp(needs.hygiene + hygieneGain, 0, 100);
        Debug.Log("DESPUES DE DUCHARSE: " + needs.hygiene);
        Debug.Log($"Duchándose en la ducha -> +{hygieneGain} higiene");
    }
}