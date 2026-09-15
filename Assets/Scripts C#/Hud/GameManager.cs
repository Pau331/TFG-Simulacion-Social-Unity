using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CharacterNeeds simNeeds;

    private void Awake()
    {
        Instance = this;
    }
}