using UnityEngine;
using UnityEngine.AI;

public class SleepAction : UtilityAction
{
    public SleepAction()
    {
        actionName = "SleepAction";
    }   

    public override float GetUtilityScore(GameObject agent)
    {
        if (!(target is Bed)) return 0f;

        float score = EvaluateConsiderations(agent, 1f);
        Debug.Log($"[Utility] Action={GetActionName()} Target={(target!=null?target.name:"null")} Base={score:F2}");
        return score;
    }

    public override float GetDuration()
    {
        return 20f;
    }

    public override string GetActionName()
    {
        return "SleepAction";
    }

    public override string GetActionText()
    {
        return "Durmiendo...";
    }

    public override System.Type GetTargetType()
    {
        return (typeof(Bed));
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