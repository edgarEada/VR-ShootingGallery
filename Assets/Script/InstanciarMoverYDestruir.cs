using UnityEngine;
using System.Collections;

public class InstanciarMoverDestruir : MonoBehaviour
{
    public GameObject prefab;       // Prefab a instanciar
    public float velocidad = 2f;    // Velocidad de movimiento
    public float destinoX = 3.97f;  // Coordenada X donde se destruyen

    void Start()
    {
        // Posiciones iniciales
        Vector3[] posicionesIniciales = new Vector3[]
        {
            new Vector3(2.14f, 1.381f, -4.12f),
            new Vector3(2.14f, 1.775f, -4.12f),
            new Vector3(2.14f, 0.971f, -4.12f)
        };

        Quaternion rotacion = Quaternion.Euler(-90f, 0f, 0f);
        Vector3 escala = new Vector3(0.3f, 0.05f, 0.3f);

        // Instanciamos varias veces en cada posición
        for (int i = 0; i < posicionesIniciales.Length; i++)
        {
            for (int j = 0; j < 3; j++) // número de repeticiones por posición
            {
                GameObject instancia = Instantiate(prefab, posicionesIniciales[i], rotacion);
                instancia.transform.localScale = escala;
                StartCoroutine(MoverHastaX(instancia));
            }
        }
    }

    IEnumerator MoverHastaX(GameObject objeto)
    {
        // Mientras no haya llegado al destino
        while (objeto != null && objeto.transform.position.x < destinoX)
        {
            objeto.transform.position += Vector3.right * velocidad * Time.deltaTime;
            yield return null;
        }

        // Al llegar al destino, destruir
        if (objeto != null)
        {
            Destroy(objeto);
        }
    }
}
