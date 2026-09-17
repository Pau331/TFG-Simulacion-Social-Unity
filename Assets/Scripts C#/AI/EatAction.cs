using UnityEngine;
using UnityEngine.AI;

public class EatAction : UtilityAction
{
    public EatAction()
    {
        actionName = "EatAction";
    }

    public override float GetUtilityScore(GameObject agent)
    {
        if (!(target is Fridge)) return 0f;

        var needs = GameManager.Instance.simNeeds;
        if (needs != null && needs.hunger >= 99f)
            return 0f;

        var personality = agent.GetComponent<NPCPersonality>();

        float personalityModifier = 1f;
        if (personality != null)
        {
            switch (personality.food)
            {
                case FoodTrait.Glutton:
                    personalityModifier = 1.2f;
                    break;
                case FoodTrait.Moderate:
                    personalityModifier = 0.8f;
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
        return 5f;
    }

    public override string GetActionName()
    {
        return "EatAction";
    }

    public override string GetActionText()
    {
        return "Comiendo...";
    }

    public override System.Type GetTargetType()
    {
        return (typeof(Fridge));
    }

    public override float GetNeedValue(CharacterNeeds needs)
    {
        return needs != null ? needs.hunger : 0f;
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