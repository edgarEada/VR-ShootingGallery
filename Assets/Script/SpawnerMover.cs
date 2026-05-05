using UnityEngine;
using System.Collections;

public class SpawnerMover : MonoBehaviour
{
    public GameObject prefab;        // Prefab a instanciar
    public float velocidad = 2f;     // Velocidad de movimiento
    public float destinoX = 3.97f;   // Coordenada X donde se destruyen
    public float intervalo = 2f;     // Tiempo entre cada nueva instancia
    public int repeticionesPorPosicion = 3; // Cuántos objetos por ciclo

    // Posiciones iniciales configurables desde el Inspector
    public Vector3[] posicionesIniciales = new Vector3[]
    {
        new Vector3(2.14f, 1.381f, -4.12f),
        new Vector3(2.14f, 1.775f, -4.12f),
        new Vector3(2.14f, 0.971f, -4.12f)
    };

    void Start()
    {
        // Lanzamos corutina que repite infinitamente
        StartCoroutine(SpawnerLoop());
    }

    IEnumerator SpawnerLoop()
    {
        Quaternion rotacion = Quaternion.Euler(-90f, 0f, 0f);
        Vector3 escala = new Vector3(0.3f, 0.05f, 0.3f);

        while (true) // bucle infinito
        {
            foreach (Vector3 pos in posicionesIniciales)
            {
                for (int j = 0; j < repeticionesPorPosicion; j++)
                {
                    GameObject instancia = Instantiate(prefab, pos, rotacion);
                    instancia.transform.localScale = escala;
                    StartCoroutine(MoverHastaX(instancia));
                }
            }

            yield return new WaitForSeconds(intervalo); // esperar antes de repetir
        }
    }

    IEnumerator MoverHastaX(GameObject objeto)
    {
        while (objeto != null && objeto.transform.position.x < destinoX)
        {
            objeto.transform.position += Vector3.right * velocidad * Time.deltaTime;
            yield return null;
        }

        if (objeto != null)
        {
            Destroy(objeto);
        }
    }
}
