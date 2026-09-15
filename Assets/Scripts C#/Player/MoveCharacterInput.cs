using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MoveCharacterInput : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Camera mainCamera;
    public NPCBrain npcBrain;

    void Update()
    {
        Debug.Log("Agent activo: " + agent.isActiveAndEnabled);
        Debug.Log("Está en NavMesh: " + agent.isOnNavMesh);
        if (!Input.GetMouseButtonDown(0))
            return;

        // No mover al personaje si se está colocando un objeto
        ObjectPlacementManager placement =
            Object.FindFirstObjectByType<ObjectPlacementManager>();

        if (placement != null && placement.IsPlacing())
        {
            return;
        }

        // No mover al personaje si se ha pulsado sobre la UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // No mover al personaje si está en modo eliminación de objetos
        if (placement != null && placement.IsDeleting())
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            npcBrain.CancelCurrentAction();
            agent.SetDestination(hit.point);
        }
    }
}