using UnityEngine;

public class MovimientoObjetivo : MonoBehaviour
{
    public float velocidad = 1f;
    private Target scriptTarget;

    void Start() {
        scriptTarget = GetComponent<Target>();
    }

    void Update()
    {
        // Si ya fue golpeado, dejamos de mover por transform
        if (scriptTarget != null && scriptTarget.wasHit) return;

        transform.Translate(Vector3.right * velocidad * Time.deltaTime);
    }
}