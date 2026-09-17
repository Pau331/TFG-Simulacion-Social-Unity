using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance;

    [Header("Exportación entre partidas")]
    [Tooltip("Nombre de la configuración que se está probando ahora mismo (ej. 'Maniatico_x1.4'). Cámbialo antes de cada tanda de partidas.")]
    public string currentConfigLabel = "Neutra";
    [Tooltip("Tecla para volcar los resultados de ESTA partida a un CSV en disco, antes de cerrar/reiniciar.")]
    public KeyCode exportKey = KeyCode.F9;

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

    // Estadísticas de nivel de necesidad al inicio de cada acción (al LLEGAR, tras viajar)
    public List<float> hungerAtStart = new();
    public List<float> energyAtStart = new();
    public List<float> hygieneAtStart = new();
    public List<float> funAtStart = new();
    public List<float> bladderAtStart = new();

    public List<float> hungerAtDecision = new();
    public List<float> energyAtDecision = new();
    public List<float> hygieneAtDecision = new();
    public List<float> funAtDecision = new();
    public List<float> bladderAtDecision = new();

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

        if (Input.GetKeyDown(exportKey))
        {
            ExportRunToCSV();
        }
    }


    void Awake()
    {
        Instance = this;
    }

    public float GetAverage(List<float> values)
    {
        if (values == null || values.Count == 0)
            return 0f;

        float sum = 0f;
        foreach (var v in values)
            sum += v;

        return sum / values.Count;
    }

    public void PrintStatistics()
    {
        Debug.Log(
            $"===== ESTADÍSTICAS =====\n" +
            $"Comer: {eatCount} (al llegar: {GetAverage(hungerAtStart):F1} ± {GetStdDev(hungerAtStart):F1} | al decidir: {GetAverage(hungerAtDecision):F1})\n" +
            $"Dormir: {sleepCount} (al llegar: {GetAverage(energyAtStart):F1} ± {GetStdDev(energyAtStart):F1} | al decidir: {GetAverage(energyAtDecision):F1})\n" +
            $"Ducha: {showerCount} (al llegar: {GetAverage(hygieneAtStart):F1} ± {GetStdDev(hygieneAtStart):F1} | al decidir: {GetAverage(hygieneAtDecision):F1})\n" +
            $"Baño: {toiletCount} (al llegar: {GetAverage(bladderAtStart):F1} ± {GetStdDev(bladderAtStart):F1} | al decidir: {GetAverage(bladderAtDecision):F1})\n" +
            $"TV: {tvCount} (al llegar: {GetAverage(funAtStart):F1} ± {GetStdDev(funAtStart):F1} | al decidir: {GetAverage(funAtDecision):F1})"
        );
    }

    public void ExportRunToCSV()
    {
        string path = Path.Combine(Application.persistentDataPath, "resultados_simulacion.csv");

        try
        {
            bool fileExists = File.Exists(path);

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                if (!fileExists)
                {
                    writer.WriteLine(
                        "Configuracion;ShowerCount;HygieneAtStartMedia;HygieneAtStartDesvIntra;HygieneAtDecisionMedia;" +
                        "TVCount;FunAtStartMedia;FunAtStartDesvIntra;FunAtDecisionMedia;" +
                        "EatCount;HungerAtStartMedia;HungerAtStartDesvIntra;HungerAtDecisionMedia;" +
                        "SleepCount;EnergyAtStartMedia;EnergyAtStartDesvIntra;EnergyAtDecisionMedia;" +
                        "ToiletCount;BladderAtStartMedia;BladderAtStartDesvIntra;BladderAtDecisionMedia"
                    );
                }

                writer.WriteLine(
                    $"{currentConfigLabel};{showerCount};{GetAverage(hygieneAtStart):F2};{GetStdDev(hygieneAtStart):F2};{GetAverage(hygieneAtDecision):F2};" +
                    $"{tvCount};{GetAverage(funAtStart):F2};{GetStdDev(funAtStart):F2};{GetAverage(funAtDecision):F2};" +
                    $"{eatCount};{GetAverage(hungerAtStart):F2};{GetStdDev(hungerAtStart):F2};{GetAverage(hungerAtDecision):F2};" +
                    $"{sleepCount};{GetAverage(energyAtStart):F2};{GetStdDev(energyAtStart):F2};{GetAverage(energyAtDecision):F2};" +
                    $"{toiletCount};{GetAverage(bladderAtStart):F2};{GetStdDev(bladderAtStart):F2};{GetAverage(bladderAtDecision):F2}"
                );
            }

            Debug.Log($"[Export] Fila añadida para configuración '{currentConfigLabel}'. CSV en: {path}");
        }
        catch (IOException)
        {
            Debug.LogWarning(
                $"[Export] No se pudo escribir en '{path}' porque el archivo está abierto en otro programa " +
                "(probablemente Excel). Ciérralo y vuelve a pulsar la tecla de exportación."
            );
        }
    }

    public float GetStdDev(List<float> values)
    {
        if (values == null || values.Count < 2)
            return 0f;

        float mean = GetAverage(values);
        float sumSquares = 0f;

        foreach (var v in values)
            sumSquares += (v - mean) * (v - mean);

        return Mathf.Sqrt(sumSquares / (values.Count - 1));
    }

    public void RegisterLevel(UtilityAction a, float level)

    {

        switch (a)

        {

            case ShowerAction: hygieneAtStart.Add(level); break;

            case WatchTVAction: funAtStart.Add(level); break;

            case EatAction: hungerAtStart.Add(level); break;

            case SleepAction: energyAtStart.Add(level); break;

            case UrinateAction: bladderAtStart.Add(level); break;

        }

    }

    public void RegisterDecisionLevel(UtilityAction a, float level)
    {
        switch (a)
        {
            case ShowerAction: hygieneAtDecision.Add(level); break;
            case WatchTVAction: funAtDecision.Add(level); break;
            case EatAction: hungerAtDecision.Add(level); break;
            case SleepAction: energyAtDecision.Add(level); break;
            case UrinateAction: bladderAtDecision.Add(level); break;
        }
    }
}