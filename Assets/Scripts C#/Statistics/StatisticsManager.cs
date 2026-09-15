using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance;

    // Estadísticas de número de acciones
    public int eatCount;
    public int sleepCount;
    public int showerCount;
    public int toiletCount;
    public int tvCount;

    // Estadísticas de tiempo acumulado por acción
    public float eatTime;
    public float sleepTime;
    public float showerTime;
    public float toiletTime;
    public float tvTime;

    // Estadísticas de evolución de necesidades
    public List<float> hungerHistory = new();
    public List<float> energyHistory = new();
    public List<float> hygieneHistory = new();
    public List<float> funHistory = new();
    public List<float> bladderHistory = new();

    CharacterNeeds needs;

    float timer;

    void Start()
    {
        needs = FindFirstObjectByType<CharacterNeeds>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 2f)
        {
            timer = 0;

            hungerHistory.Add(needs.hunger);
            energyHistory.Add(needs.energy);
            hygieneHistory.Add(needs.hygiene);
            funHistory.Add(needs.fun);
            bladderHistory.Add(needs.bladder);
        }
    }


    void Awake()
    {
        Instance = this;
    }

    public void PrintStatistics()
    {
        Debug.Log(
            $"===== ESTADÍSTICAS =====\n" +
            $"Comer: {eatCount}\n" +
            $"Dormir: {sleepCount}\n" +
            $"Ducha: {showerCount}\n" +
            $"Baño: {toiletCount}\n" +
            $"TV: {tvCount}"
        );
    }
}