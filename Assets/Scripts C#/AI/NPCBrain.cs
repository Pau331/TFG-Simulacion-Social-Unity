using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class NPCBrain : MonoBehaviour
{
    private NPCPersonality personality;
    private NavMeshAgent navAgent;
    private NPCAnimator npcAnimator;
    public ActionProgressUI progressUI;
    public List<UtilityAction> actions = new();

    private UtilityAction currentAction;
    private float decisionInterval = 5f; // tiempo entre decisiones
    private float timer;
    private bool isActing;

    public float interactRadius = 1f; // distancia a la que se considera "en posición" para interactuar
    public float arrivalTimeout = 10f; // tiempo para llegar al punto de interacción

    private Vector3 lastNavMeshPosition; // distancia del último punto en el NavMesh

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        personality = GetComponent<NPCPersonality>();
        npcAnimator = GetComponent<NPCAnimator>();

        StartCoroutine(InitPersonality());

        // Crear instancias de acciones y añadir consideraciones de distancia
        var eat = new EatAction();
        eat.considerations.Add(new HungerConsideration());
        eat.considerations.Add(new DistanceConsideration());

        var sleep = new SleepAction();
        sleep.considerations.Add(new EnergyConsideration());    
        sleep.considerations.Add(new DistanceConsideration());

        var shower = new ShowerAction();
        shower.considerations.Add(new HygieneConsideration());
        shower.considerations.Add(new DistanceConsideration());

        var urinate = new UrinateAction();
        urinate.considerations.Add(new BladderConsideration());
        urinate.considerations.Add(new DistanceConsideration());

        var watch = new WatchTVAction();
        watch.considerations.Add(new FunConsideration());
        watch.considerations.Add(new DistanceConsideration());

        actions.Add(eat);
        actions.Add(sleep);
        actions.Add(shower);
        actions.Add(urinate);
        actions.Add(watch);

    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isActing)
            return;

        if (timer >= decisionInterval)
        {
            timer = 0;
            DecideAction();
        }
    }

    void DecideAction()
    {
        UtilityAction bestAction = null;
        InteractableObject bestTarget = null;

        float bestScore = -1f;

        var objects = FindObjectsByType<InteractableObject>(FindObjectsSortMode.None);


        foreach (var action in actions)
        {
            foreach (var obj in objects)
            {
                if (action.GetTargetType() != obj.GetType() || IsPreviewObject(obj))
                    continue;

                action.target = obj;

                float score = action.GetUtilityScore(gameObject);

                Debug.Log(action.GetActionName() + " - " + obj.name + ": " + score.ToString("F2"));

                if (score > bestScore)
                {
                    bestScore = score;
                    bestAction = action;
                    bestTarget = obj;
                }
            }
        }

        if (bestAction != null)
        {
            Debug.Log("BEST ACTION: " + bestAction.actionName+ " - " + bestTarget.name + " with score: " + bestScore);
            bestAction.target = bestTarget;
            currentAction = bestAction;

            // Determinar punto de llegada del objetivo
            Vector3 reachPoint = GetReachPoint(bestTarget);

            // Ajustar stoppingDistance para que el agente se detenga cerca del punto de interacción y no intente meterse dentro del objeto
            if (navAgent != null && navAgent.isActiveAndEnabled && navAgent.isOnNavMesh)
            {
                navAgent.stoppingDistance = interactRadius;
                navAgent.isStopped = false;
                navAgent.SetDestination(reachPoint);
            }
            else
            {
                Debug.LogWarning("No se puede mover: NavMeshAgent no está activo o no está sobre el NavMesh.");

                currentAction = null;
                isActing = false;
                return;
            }

            isActing = true;
            
            StartCoroutine(PerformAction(currentAction));
        }
    }

    IEnumerator PerformAction(UtilityAction action)
    {
        // Esperar a llegar al punto de interacción
        float arrivalTimer = 0f;
        Vector3 reachPoint = GetReachPoint(action.target);

        while (arrivalTimer < arrivalTimeout)
        {
            reachPoint = GetReachPoint(action.target);

            bool arrived;

            if (action is SleepAction)
            {
                // Para dormir necesitamos llegar cerca del interactionPoint
                arrived = !navAgent.pathPending &&
                          navAgent.hasPath &&
                          navAgent.remainingDistance <= navAgent.stoppingDistance + 0.2f;
            }
            else
            {
                arrived = IsAtInteractionPosition(reachPoint);
            }

            if (arrived)
                break;

            arrivalTimer += Time.deltaTime;
            yield return null;
        }

        // Comprobar si hemos llegado al punto de interacción
        bool finalArrived;

        if (action is SleepAction)
        {
            finalArrived = !navAgent.pathPending &&
                           navAgent.hasPath &&
                           navAgent.remainingDistance <= navAgent.stoppingDistance + 0.2f;
        }
        else
        {
            finalArrived = IsAtInteractionPosition(reachPoint);
        }

        if (!finalArrived)
        {
            Debug.Log("Failed to reach interaction point for action: " + action.GetActionText());

            currentAction = null;
            isActing = false;

            if (navAgent != null &&
                navAgent.isActiveAndEnabled &&
                navAgent.isOnNavMesh)
            {
                navAgent.ResetPath();
            }

            yield break;
        }

        // Parar el agente
        if (navAgent != null &&
            navAgent.isActiveAndEnabled &&
            navAgent.isOnNavMesh)
        {
            navAgent.isStopped = true;
            navAgent.ResetPath();
        }

        // Girar hacia el punto de interacción
        bool isEating = action is EatAction;
        bool isSitting = action is WatchTVAction || action is UrinateAction;

        if (isEating && action.target.interactionPoint != null)
        {
            transform.rotation = action.target.interactionPoint.rotation;
        }

        // Sentarse: colocar al NPC en el SittingPoint
        if (isSitting)
        {
            Transform sitPoint = null;

            // Si es en la TV
            if (action.target is TV tv)
            {
                sitPoint = tv.SitPoint;
            }

            // Si es en el sofá
            if (action.target is Toilet toilet)
            {
                sitPoint = toilet.SitPoint;
            }

            if (sitPoint != null)
            {
                lastNavMeshPosition = transform.position;
                navAgent.enabled = false;

                transform.position = sitPoint.position;
                transform.rotation = sitPoint.rotation;
            }
        }

        // Dormir: colocar al NPC en el SleepPoint
        bool isSleeping = action is SleepAction;

        if (isSleeping)
        {
            Bed bed = action.target as Bed;

            if (bed != null && bed.SleepPoint != null)
            {
                lastNavMeshPosition = transform.position;
                navAgent.enabled = false;

                transform.position = bed.SleepPoint.position;
                transform.rotation = bed.SleepPoint.rotation;
            }
            else
            {
                Debug.LogWarning("La cama no tiene SleepPoint asignado.");
            }
        }

        // Animaciones
        if (npcAnimator != null)
        {
            npcAnimator.SetEating(isEating);
            npcAnimator.SetSleeping(isSleeping);
            npcAnimator.SetSitting(isSitting);
        }

        // Mostrar barra de progreso y esperar la duración de la acción
        float duration = action.GetDuration();
        float elapsed = 0f;

        if (progressUI != null)
        {
            progressUI.Show(true);
            progressUI.SetText(action.GetActionText());
        }

        while (elapsed < duration)
        {
            

            elapsed += Time.deltaTime;

            if (progressUI != null)
                progressUI.SetProgress(elapsed / duration);

            yield return null;
        }

        // Ejecutar la interacción si la acción ha terminado
        if (action.target != null && elapsed >= duration)
        {
            action.target.Interact(gameObject);
            RegisterStatistics(action, duration);
        }

        // Terminar de sentarse
        if (isSitting)
        {
            if (npcAnimator != null)
                npcAnimator.SetSitting(false);

            navAgent.enabled = true;

            if (navAgent.isOnNavMesh)
            {
                navAgent.isStopped = false;
                navAgent.ResetPath();
            }
        }

        // Terminar de dormir
        if (isSleeping)
        {
            Bed bed = action.target as Bed;

            if (npcAnimator != null)
                npcAnimator.SetSleeping(false);

            if (bed != null && bed.interactionPoint != null)
            {
                transform.position = bed.interactionPoint.position;

                Vector3 direction = bed.transform.position - transform.position;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.001f)
                    transform.rotation = Quaternion.LookRotation(direction);
            }

            navAgent.enabled = true;

            if (navAgent.isOnNavMesh)
            {
                navAgent.isStopped = false;
                navAgent.ResetPath();
            }
        }
        else if (!isSitting)
        {
            // Limpiar camino para la siguiente acción
            if (navAgent != null &&
                navAgent.isActiveAndEnabled &&
                navAgent.isOnNavMesh)
            {
                navAgent.ResetPath();
                navAgent.isStopped = false;
            }

            if (npcAnimator != null)
            {
                npcAnimator.SetEating(false);
                npcAnimator.SetSleeping(false);
                npcAnimator.SetSitting(false);
            }
        }

        progressUI.Show(false);

        currentAction = null;
        isActing = false;
    }


    Vector3 GetReachPoint(InteractableObject target)
    {
        if (target == null)
            return transform.position;

        // Si existe un punto de interacción explícito, utilizarlo siempre.
        if (target.interactionPoint != null)
            return target.interactionPoint.position;

        // Último recurso: posición del objeto.
        return target.transform.position;
    }

    bool IsAtInteractionPosition(Vector3 reachPoint)
    {
        float dist = Vector3.Distance(transform.position, reachPoint);
        if (dist <= Mathf.Max(interactRadius, 0.1f))
            return true;

        // También considerar el navAgent.remainingDistance si está disponible
        if (navAgent != null && !navAgent.pathPending && navAgent.hasPath)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance + 0.1f)
                return true;
        }

        return false;
    }

    public void CancelCurrentAction()
    {
        StopAllCoroutines();

        currentAction = null;
        isActing = false;

        // Detener animaciones
        if (npcAnimator != null)
        {
            npcAnimator.SetEating(false);
            npcAnimator.SetSleeping(false);
            npcAnimator.SetSitting(false);
        }

        // Reactivar y colocar el agente sobre el último punto en el NavMesh
        if (navAgent != null)
        {
            if (!navAgent.enabled)
            {
                transform.position = lastNavMeshPosition;
                navAgent.enabled = true;
            }
            navAgent.isStopped = false;
            navAgent.ResetPath();
        }

        if (progressUI != null)
            progressUI.Show(false);
    }

    IEnumerator InitPersonality()
    {
        yield return null; 

        float baseSpeed = navAgent.speed;

        switch (personality.activity)
        {
            case ActivityTrait.Active:
                navAgent.speed = baseSpeed * 1.5f;
                break;

            case ActivityTrait.Lazy:
                navAgent.speed = baseSpeed * 0.8f;
                break;

            default:
                navAgent.speed = baseSpeed;
                break;
        }

        Debug.Log("FINAL SPEED: " + navAgent.speed);
    }

    void RegisterStatistics(UtilityAction action, float duration)
    {
        switch (action)
        {
            case EatAction:
                StatisticsManager.Instance.eatCount++;
                StatisticsManager.Instance.eatTime += duration;
                break;

            case SleepAction:
                StatisticsManager.Instance.sleepCount++;
                StatisticsManager.Instance.sleepTime += duration;
                break;

            case ShowerAction:
                StatisticsManager.Instance.showerCount++;
                StatisticsManager.Instance.showerTime += duration;
                break;

            case UrinateAction:
                StatisticsManager.Instance.toiletCount++;
                StatisticsManager.Instance.toiletTime += duration;
                break;

            case WatchTVAction:
                StatisticsManager.Instance.tvCount++;
                StatisticsManager.Instance.tvTime += duration;
                break;
        }

        StatisticsManager.Instance.PrintStatistics();
    }

    // Método que comprueba si el objeto es PreviewObject o no, mirando la capa del objeto y sus padres
    bool IsPreviewObject(InteractableObject obj)
    {
        if (obj == null)
            return true;

        int previewLayer = LayerMask.NameToLayer("PreviewObject");

        if (previewLayer == -1)
            return false;

        Transform current = obj.transform;

        while (current != null)
        {
            if (current.gameObject.layer == previewLayer)
                return true;

            current = current.parent;
        }

        return false;
    }
}