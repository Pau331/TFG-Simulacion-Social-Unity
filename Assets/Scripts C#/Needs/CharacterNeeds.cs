using UnityEngine;

public class CharacterNeeds : MonoBehaviour
{
    [Range(0, 100)] public float hunger = 100;
    [Range(0, 100)] public float energy = 100;
    [Range(0, 100)] public float bladder = 100;
    [Range(0, 100)] public float hygiene = 100;
    [Range(0, 100)] public float fun = 100;

    void Update()
    {
        hunger -= Time.deltaTime * 0.85f;
        energy -= Time.deltaTime * 0.5f;
        fun -= Time.deltaTime * 0.8f;
        hygiene -= Time.deltaTime * 0.6f;
        bladder -= Time.deltaTime * 0.7f;

        ClampValues();
    }

    void ClampValues()
    {
        hunger = Mathf.Clamp(hunger, 0, 100);
        energy = Mathf.Clamp(energy, 0, 100);
        fun = Mathf.Clamp(fun, 0, 100);
        hygiene = Mathf.Clamp(hygiene, 0, 100);
        bladder = Mathf.Clamp(bladder, 0, 100);
    }
}
