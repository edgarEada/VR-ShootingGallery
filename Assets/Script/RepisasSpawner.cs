using UnityEngine;
using System.Collections;

public class RepisasSpawner : MonoBehaviour
{
    public GameObject prefabObjetivo;
    public Transform repisaArriba;
    public Transform repisaMedio;
    public Transform repisaAbajo;

    void Start()
    {
        // Llamamos corutinas para cada repisa
        StartCoroutine(SpawnEnRepisa(repisaArriba, 3f, 1f));   // rápido, cada 1 segundo (30 pts)
        StartCoroutine(SpawnEnRepisa(repisaMedio, 1.5f, 2f));  // normal, cada 2 segundos (20 pts)
        StartCoroutine(SpawnEnRepisa(repisaAbajo, 0.5f, 3f));  // lento, cada 3 segundos (10 pts)
    }

    IEnumerator SpawnEnRepisa(Transform repisa, float velocidad, float intervalo)
    {
        while (true)
        {
            // 1. Primero definimos la rotación usando el transform del prefab
            Quaternion rotacionDePie = prefabObjetivo.transform.rotation; 

            // 2. Ahora usamos esa variable para instanciar el objeto
            GameObject obj = Instantiate(prefabObjetivo, repisa.position, rotacionDePie);

            // 3. Añadimos el script de movimiento
            MovimientoObjetivo mover = obj.AddComponent<MovimientoObjetivo>();
            mover.velocidad = velocidad;

            // 4. Buscamos el script en el objeto instanciado O en cualquiera de sus hijos
            Target scriptTarget = obj.GetComponentInChildren<Target>();

            if (scriptTarget != null)
            {
                if (velocidad >= 3f)
                {
                    scriptTarget.scoreValue = 30;
                }
                else if (velocidad >= 1.5f)
                {
                    scriptTarget.scoreValue = 20;
                }
                else
                {
                    scriptTarget.scoreValue = 10;
                }
            }
            else
            {
                // Esta línea de ingeniería te salvará horas de depuración
                Debug.LogWarning("Error: El Spawner no encontró el script 'Target' en el prefab " + obj.name);
            }

            yield return new WaitForSeconds(intervalo);
        }
    }
}