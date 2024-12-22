using UnityEngine;

public class PositionOnScreenEdge : MonoBehaviour
{
    public float horizontalOffset = 0.5f; // El offset en el eje horizontal
    public bool alignToLeftEdge = true; // Ajustar a la izquierda o derecha de la pantalla

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        PositionObjectOnScreenEdge();
    }

    void PositionObjectOnScreenEdge()
    {
        // Obtiene las coordenadas del borde de la pantalla
        Vector3 edgePosition;
        if (alignToLeftEdge)
        {
            edgePosition =
                mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, Mathf.Abs(mainCamera.transform.position.z)));
        }
        else
        {
            edgePosition =
                mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, Mathf.Abs(mainCamera.transform.position.z)));
        }

        // Aplica el offset horizontal
        edgePosition.x += alignToLeftEdge ? horizontalOffset : -horizontalOffset;

        // Establece la posición del objeto
        transform.position = new Vector3(edgePosition.x, transform.position.y, transform.position.z);
    }
}