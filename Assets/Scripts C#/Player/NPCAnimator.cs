using UnityEngine;
using UnityEngine.AI;

public class NPCAnimator : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        if (agent == null)
            Debug.LogError("NPCAnimator: No se encuentra NavMeshAgent en PJ.");

        if (animator == null)
            Debug.LogError("NPCAnimator: No se encuentra Animator en los hijos de PJ.");
    }

    void Update()
    {
        if (agent == null || animator == null)
            return;

        bool eating = animator.GetBool("Eating");
        bool sleeping = animator.GetBool("Sleeping");
        bool sitting = animator.GetBool("Sitting");

        bool moving =
            agent.hasPath &&
            !agent.isStopped &&
            agent.remainingDistance > agent.stoppingDistance + 0.1f &&
            agent.velocity.sqrMagnitude > 0.01f;


        animator.SetBool("Moving", moving && !eating && !sleeping && !sitting);
    }

    public void SetEating(bool eating)
    {
        if (animator == null)
            return;
        animator.SetBool("Eating", eating);
    }

    public void SetSleeping(bool sleeping)
    {
        if (animator == null)
            return;
        animator.SetBool("Sleeping", sleeping);
    }
    
    public void SetSitting(bool sitting)
    {
        if (animator == null)
            return;
        animator.SetBool("Sitting", sitting);
    }
}