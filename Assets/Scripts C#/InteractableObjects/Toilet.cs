using UnityEngine;

public class Toilet : InteractableObject
{
    [SerializeField] private Transform sitPoint;
    public float bladderGain = 80f;
    public Transform SitPoint => sitPoint;

    void Awake()
    {
        bladderGain = 80f;
    }

    public override void Interact(GameObject agent)
    {
        var needs = GameManager.Instance.simNeeds;
        Debug.Log("ANTES DE USAR EL BAÑO: " + needs.bladder);
        needs.bladder = Mathf.Clamp(needs.bladder + bladderGain, 0, 100);
        Debug.Log("DESPUES DE USAR EL BAÑO: " + needs.bladder);
        Debug.Log($"Usando el baño -> +{bladderGain} vejiga");
    }
}