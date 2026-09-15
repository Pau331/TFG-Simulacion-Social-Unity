using UnityEngine;
using UnityEngine.AI;

public class ShowerAction : UtilityAction
{
    public ShowerAction()
    {
        actionName = "ShowerAction";
    }

    public override float GetUtilityScore(GameObject agent)
    {
        if (!(target is Shower)) return 0f;

        var needs = GameManager.Instance.simNeeds;
        if (needs != null && needs.hygiene >= 99f)
            return 0f;

        var personality = agent.GetComponent<NPCPersonality>();

        float personalityModifier = 1f;

        if (personality != null)
        {
            switch (personality.cleanliness)
            {
                case CleanlinessTrait.CleanFreak:
                    personalityModifier = 1.4f;
                    break;
                case CleanlinessTrait.Careless:
                    personalityModifier = 0.6f;
                    break;
            }
        }
        float baseScore = EvaluateConsiderations(agent, 1f);          // sin personalidad
        float finalScore = EvaluateConsiderations(agent, personalityModifier); // con personalidad
        Debug.Log($"[Utility] {GetActionName()} Base={baseScore:F2} Personality={personalityModifier:F2} Final={finalScore:F2}");
        return finalScore;
    }

    public override float GetDuration()
    {
        return 10f;
    }

    public override string GetActionName()
    {
        return "ShowerAction";
    }

    public override string GetActionText()
    {
        return "Duchándose...";
    }

    public override System.Type GetTargetType()
    {
        return (typeof(Shower));
    }

    public override void Execute(GameObject agent)
    {
        var nav = agent.GetComponent<NavMeshAgent>();

        if (target != null)
        {
            nav.SetDestination(target.transform.position);
        }
    }
}