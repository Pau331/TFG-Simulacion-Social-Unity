using UnityEngine;

public class TV : InteractableObject
{
    [SerializeField] private Transform sitPoint;
    public float funGain = 80f;
    public Transform SitPoint => sitPoint;

    void Awake()
    {
        funGain = 80f;
    }

    public override void Interact(GameObject agent)
    {
        var needs = GameManager.Instance.simNeeds;
        Debug.Log("ANTES DE VER LA TV: " + needs.fun);
        needs.fun = Mathf.Clamp(needs.fun + funGain, 0, 100);
        Debug.Log("DESPUES DE VER LA TV: " + needs.fun);
        Debug.Log($"Viendo la televisión -> +{funGain} diversión");
    }
}