using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.AI.Navigation;

public class ObjectPlacementManager : MonoBehaviour
{
    [Header("Navegación")]
    public NavMeshSurface navMeshSurface;

    [Header("Referencias")]
    public NPCBrain npcBrain;

    [Header("Referencias")]
    public Camera mainCamera;

    [Header("Configuración")]
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;

    public GameObject buildPanel;

    [Header("Colores de previsualización")]
    public Color validColor = Color.green;
    public Color invalidColor = Color.red;

    private GameObject selectedPrefab;
    private GameObject previewObject;

    private bool isPlacing = false;
    private bool canPlace = false;
    private bool isDeleting = false;

    private Material[] originalMaterials;

    [SerializeField] private float rotationAmount = 90f;
    [SerializeField] private TMP_Text deleteButtonText;

    void Awake()
    {
       if(buildPanel != null)
        {
            buildPanel.SetActive(false);
        }
    }

    public void ToggleBuildPanel()
    {
        if (buildPanel == null)
            return;
        
        buildPanel.SetActive(!buildPanel.activeSelf);
    }


    void Update()
    {
        // MODO ELIMINACIÓN

        if (isDeleting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryDeleteObject();
            }
            return;
        }

        // MODO CONSTRUCCIÓN
        if (!isPlacing)
            return;

        UpdatePreview();

        // Rotar objeto con R
        if(Input.GetKeyDown(KeyCode.R))
        {
            RotatePreview();
        }

        // Clic izquierdo: colocar
        if (Input.GetMouseButtonDown(0))
        {
            if (canPlace)
            {
                PlaceObject();
            }
        }

        // Clic derecho: cancelar
        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
        }

        
    }

    public void ToggleDeleteMode()
    {
        isDeleting = !isDeleting;
        if (isDeleting)
        {
            // Desactivar el panel de construcción
            if (isPlacing)
            {
                CancelPlacement();
            }

            deleteButtonText.text = "CANCELAR";

            Debug.Log("Modo eliminación activado");
        }
        else
        {
            deleteButtonText.text = "ELIMINAR";
            Debug.Log("Modo eliminación desactivado");
        }
    }

    void TryDeleteObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        Transform current = hit.collider.transform;

        int placedLayer = LayerMask.NameToLayer("PlacedObject");

        if (placedLayer == -1)
            return;

        // Buscar hacia arriba el objeto colocado
        while (current != null)
        {
            if (current.gameObject.layer == placedLayer)
            {
                GameObject objectToDelete = current.gameObject;
                InteractableObject interactable = objectToDelete.GetComponent<InteractableObject>();

                if (npcBrain != null && interactable != null && npcBrain.CurrentTarget == interactable)
                {
                    Debug.Log("No se puede eliminar: el personaje está usando este objeto (" + objectToDelete.name + ").");
                    return;
                }

                Debug.Log("Objeto eliminado: " + objectToDelete.name);

                Destroy(objectToDelete);
                navMeshSurface.BuildNavMesh(); 

                return;
            }

            current = current.parent;
        }
    }

    void RotatePreview()
    {
        if (previewObject == null)
        {
            return;
        }
        if (previewObject.GetComponent<RequiresZAxisRotation>() != null)
        {
            previewObject.transform.Rotate(Vector3.forward, rotationAmount);
        }
        else
        {
            previewObject.transform.Rotate(Vector3.up, rotationAmount);
        }

        UpdatePreview();
    }


    public void StartPlacement(GameObject prefab)
    {
        if (prefab == null)
            return;

        selectedPrefab = prefab;
        isPlacing = true;
        canPlace = false;

        // Crear objeto de previsualización
        previewObject = Instantiate(selectedPrefab);

        // Evitar que el objeto de previsualización sea utilizado por el agente
        int previewLayer = LayerMask.NameToLayer("PreviewObject");

        if (previewLayer != -1)
        {
            SetLayerRecursively(previewObject, previewLayer);
        }

        // Guardar materiales originales
        Renderer[] renderers =
            previewObject.GetComponentsInChildren<Renderer>();

        originalMaterials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].sharedMaterial;
        }

        // Inicialmente aparece en rojo hasta encontrar una posición válida
        SetPreviewColor(invalidColor);
    }


    void UpdatePreview()
    {
        if (previewObject == null)
            return;

        Ray ray =
            mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            100f,
            groundLayer))
        {
            // Buscar PlacementPoint
            Transform placementPoint =
                previewObject.transform.Find("PlacementPoint");

            if (placementPoint != null)
            {
                // Mantener el PlacementPoint sobre el punto
                // donde ha hecho clic el jugador
                Vector3 offset =
                    previewObject.transform.position -
                    placementPoint.position;

                previewObject.transform.position =
                    hit.point + offset;
            }
            else
            {
                previewObject.transform.position =
                    hit.point;
            }

            // Comprobar si puede colocarse
            canPlace = CheckPlacement();
           
            if (canPlace)
            {
                SetPreviewColor(validColor);
            }
            else
            {
                SetPreviewColor(invalidColor);
            }
        }
        else
        {
            // Si no estamos apuntando al suelo,
            // la posición no es válida.
            canPlace = false;

            SetPreviewColor(invalidColor);
        }
    }


    bool CheckPlacement()
    {
        if (previewObject == null)
            return false;

        Collider[] previewColliders =
            previewObject.GetComponentsInChildren<Collider>();

        int placedObjectLayer =
            LayerMask.NameToLayer("PlacedObject");

        if (placedObjectLayer == -1)
            return true;

        int layerMask = 1 << placedObjectLayer;

        foreach (Collider previewCollider in previewColliders)
        {
            Bounds bounds = previewCollider.bounds;

            Collider[] collisions = Physics.OverlapBox(
                bounds.center,
                bounds.extents,
                previewObject.transform.rotation,
                layerMask,
                QueryTriggerInteraction.Ignore
            );

            foreach (Collider collision in collisions)
            {
                // Ignorar los colliders del propio objeto
                if (collision.transform.IsChildOf(previewObject.transform))
                    continue;

                // Hay un objeto colocado ocupando el espacio
                return false;
            }
        }

        return true;
    }


    void SetPreviewColor(Color color)
    {
        if (previewObject == null)
            return;

        Renderer[] renderers =
            previewObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // Usamos material para crear una instancia
            // y no modificar el material original del prefab.
            Material material = renderer.material;

            material.color = color;
        }
    }


    void RestoreOriginalMaterials()
    {
        if (previewObject == null)
            return;

        Renderer[] renderers =
            previewObject.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            if (i < originalMaterials.Length)
            {
                renderers[i].sharedMaterial =
                    originalMaterials[i];
            }
        }
    }


    void PlaceObject()
    {
        if (previewObject == null || !canPlace)
            return;

        // Restaurar materiales originales
        RestoreOriginalMaterials();

        // Reactivar colliders
        Collider[] colliders =
            previewObject.GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        // Asignar la Layer PlacedObject
        int placedObjectLayer =
            LayerMask.NameToLayer("PlacedObject");

        if (placedObjectLayer != -1)
        {
            SetLayerRecursively(previewObject, placedObjectLayer);
        }

        // Cambiar nombre al objeto colocado
        string baseName = selectedPrefab.name.Replace("(Clone)", "").Trim();
        int objectNumber = GetNextObjectNumber(baseName);
        previewObject.name = baseName + " " + objectNumber;

        // Terminar colocación
        previewObject = null;
        selectedPrefab = null;
        isPlacing = false;
        canPlace = false;
        originalMaterials = null;
        
        //Recalcular navmesh
        navMeshSurface.BuildNavMesh();
    }


    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(
                child.gameObject,
                layer
            );
        }
    }


    void CancelPlacement()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        previewObject = null;
        selectedPrefab = null;
        isPlacing = false;
        canPlace = false;

        originalMaterials = null;
    }

    // Metodo para obtener el siguiente número de objeto 
    int GetNextObjectNumber(string baseName)
    {
        GameObject[] objects =
            FindObjectsByType<GameObject>(
                FindObjectsSortMode.None
            );

        int highestNumber = 0;

        foreach (GameObject obj in objects)
        {
            if (!obj.name.StartsWith(baseName + " "))
                continue;

            string suffix =
                obj.name.Substring(baseName.Length).Trim();

            if (int.TryParse(suffix, out int number))
            {
                if (number > highestNumber)
                    highestNumber = number;
            }
        }

        return highestNumber + 1;
    }

    public bool IsPlacing()
    {
        return isPlacing;
    }

    public bool IsDeleting()
    { 
        return isDeleting; 
    }  

}