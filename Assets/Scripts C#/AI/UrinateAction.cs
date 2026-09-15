using UnityEngine;
using UnityEngine.AI;

public class UrinateAction : UtilityAction
{
    
    public UrinateAction()
    {
        actionName = "UrinateAction";
    }

    public override float GetUtilityScore(GameObject agent)
    {
        if (!(target is Toilet)) return 0f;

        var needs = GameManager.Instance.simNeeds;
        if (needs != null && needs.bladder >= 99f)
            return 0f;

        float score = EvaluateConsiderations(agent, 1f);
        Debug.Log($"[Utility] Action={GetActionName()} Target={(target!=null?target.name:"null")} Base={score:F2}");
        return score;
    }

    public override float GetDuration()
    {
        return 7f;
    }

    public override string GetActionName()
    {
        return "UrinateAction";
    }

    public override string GetActionText()
    {
        return "Usando el baño...";
    }

    public override System.Type GetTargetType()
    {
        return(typeof(Toilet));
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