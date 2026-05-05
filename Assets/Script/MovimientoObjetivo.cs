using UnityEngine;

public class MovimientoObjetivo : MonoBehaviour
{
    public float velocidad = 1f;

    void Update()
    {
        // Ejemplo: mover hacia la derecha
        transform.Translate(Vector3.right * velocidad * Time.deltaTime);
    }
}
