using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WatchTVAction : UtilityAction
{
    public WatchTVAction()
    {
        actionName = "WatchTVAction";
    }
    public override float GetUtilityScore(GameObject agent)
    {
        if (!(target is TV)) return 0f;

        var needs = GameManager.Instance.simNeeds;
        if (needs != null && needs.fun >= 99f)
            return 0f;

        var personality = agent.GetComponent<NPCPersonality>();

        float personalityModifier = 1f;

        if (personality != null)
        {
            switch (personality.leisure)
            {
                case LeisureTrait.Addicted:
                    personalityModifier = 1.5f;
                    break;
                case LeisureTrait.Indifferent:
                    personalityModifier = 0.5f;
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
        return 9f;
    }

    public override string GetActionName()
    {
        return "WatchTVAction";
    }

    public override string GetActionText()
    {
        return "Viendo la televisión...";
    }

    public override System.Type GetTargetType()
    {
        return (typeof(TV));
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