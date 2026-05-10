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
        StartCoroutine(SpawnEnRepisa(repisaArriba, 3f, 1f));   // rápido, cada 1 segundo
        StartCoroutine(SpawnEnRepisa(repisaMedio, 1.5f, 2f));  // normal, cada 2 segundos
        StartCoroutine(SpawnEnRepisa(repisaAbajo, 0.5f, 3f));  // lento, cada 3 segundos
    }

    IEnumerator SpawnEnRepisa(Transform repisa, float velocidad, float intervalo)
    {
        while (true)
        {
            // 1. Primero definimos la rotación usando el transform del prefab
            Quaternion rotacionDePie = prefabObjetivo.transform.rotation; 

            // 2. Ahora usamos esa variable para instanciar el objeto
            GameObject obj = Instantiate(prefabObjetivo, repisa.position, rotacionDePie);

            // El resto del código se mantiene igual
            MovimientoObjetivo mover = obj.AddComponent<MovimientoObjetivo>();
            mover.velocidad = velocidad;

            yield return new WaitForSeconds(intervalo);
        }
    }
}
