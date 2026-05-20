using UnityEngine;
using System.Collections;

public class SpawnerMoverVelocidades : MonoBehaviour
{
    public GameObject prefab;        // Prefab a instanciar
    public float destinoX = 3.97f;   // Coordenada X donde se destruyen
    public float intervalo = 2f;     // Tiempo entre cada nueva instancia
    public int repeticionesPorPosicion = 3; // Cuántos objetos por ciclo

    // Posiciones iniciales
    public Vector3[] posicionesIniciales = new Vector3[]
    {
        new Vector3(2.14f, 1.381f, -4.12f),
        new Vector3(2.14f, 1.775f, -4.12f),
        new Vector3(2.14f, 0.971f, -4.12f)
    };

    void Start()
    {
        StartCoroutine(SpawnerLoop());
    }

    IEnumerator SpawnerLoop()
    {
        Quaternion rotacion = prefab.transform.rotation;
        // Vector3 escala = new Vector3(1, 1, 1); // Ya no forzamos la escala

        while (true) // bucle infinito
        {
            foreach (Vector3 pos in posicionesIniciales)
            {
                for (int j = 0; j < repeticionesPorPosicion; j++)
                {
                    GameObject instancia = Instantiate(prefab, pos, rotacion);

                    // 1. Asignar velocidad y puntos según la altura Y
                    float velocidad = 2f; // normal por defecto
                    int puntos = 20;      // 20 puntos por defecto

                    if (Mathf.Approximately(pos.y, 1.775f))
                    {
                        velocidad = 4f; // rápido
                        puntos = 30;    // Mayor puntuación
                    }
                    else if (Mathf.Approximately(pos.y, 0.971f))
                    {
                        velocidad = 1f; // lento
                        puntos = 10;    // Menor puntuación
                    }

                    // 2. Buscar el script Target (en el padre o en el hijo) y asignar puntos
                    Target scriptTarget = instancia.GetComponentInChildren<Target>();
                    
                    if (scriptTarget != null)
                    {
                        scriptTarget.scoreValue = puntos;
                    }
                    else
                    {
                        Debug.LogWarning("Error: El Spawner no encontró el script 'Target' en el prefab " + instancia.name);
                    }

                    StartCoroutine(MoverHastaX(instancia, velocidad));
                }
            }

            yield return new WaitForSeconds(intervalo);
        }
    }

    IEnumerator MoverHastaX(GameObject objeto, float velocidad)
    {
        // 3. ACTUALIZACIÓN CRÍTICA: También usamos GetComponentInChildren aquí 
        // para que la lógica de movimiento detecte correctamente si la diana fue golpeada
        Target scriptTarget = objeto.GetComponentInChildren<Target>();

        // Añadimos la condición: && (scriptTarget == null || !scriptTarget.wasHit)
        while (objeto != null && objeto.transform.position.x < destinoX && (scriptTarget == null || !scriptTarget.wasHit))
        {
            objeto.transform.position += Vector3.right * velocidad * Time.deltaTime;
            yield return null;
        }

        // Si el objeto llegó al destino (no fue golpeado), lo destruimos
        // Si fue golpeado, dejamos que el Target.cs lo destruya después de volar
        if (objeto != null && scriptTarget != null && !scriptTarget.wasHit)
        {
            Destroy(objeto);
        }
    }
}