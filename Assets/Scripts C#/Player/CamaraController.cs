using UnityEngine;

public class CamaraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Movimiento")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 120f;

    [Header("Distancia cámara")]
    public float cameraDistance = 25f;

    [Header("Altura")]
    public float minYAngle = 20f;
    public float maxYAngle = 80f;

    private float rotX = 60f;
    private float rotY = 45f;


    void Update()
    {
        if (target == null) return;

        MapMovement();
        Rotation();

        UpdateCamera();
    }


    void MapMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 forward = Quaternion.Euler(0, rotY, 0) * Vector3.forward;
        Vector3 right = Quaternion.Euler(0, rotY, 0) * Vector3.right;

        Vector3 dir = (forward * v + right * h).normalized;

        target.position += dir * moveSpeed * Time.deltaTime;
    }


    void Rotation()
    {
        // Mantén pulsado el botón derecho del ratón para rotar
        if (Input.GetMouseButton(1))
        {
            rotY += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            rotX -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, minYAngle, maxYAngle);
        }
    }


    void UpdateCamera()
    {
        Quaternion rotation = Quaternion.Euler(rotX, rotY, 0);

        Vector3 direction = rotation * new Vector3(0, 0, -cameraDistance);

        transform.position = target.position + direction;

        transform.LookAt(target.position);
    }
}