using UnityEngine;

public class Target : MonoBehaviour
{
    public int scoreValue = 10;
    public float impactForce = 500f;
    public AudioClip hitSound;
    
    private Rigidbody rb;
    public bool wasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Nueva versión que recibe posición y dirección del proyectil
    public void OnHit(Vector3 hitPoint, Vector3 hitDirection)
    {
        if (wasHit) return; // Evita procesar el golpe dos veces
        wasHit = true;

        ScoreManager.instance.AddPoints(scoreValue);

        // Sonido de golpe (se reproduce en la posición del impacto)
        if (hitSound) AudioSource.PlayClipAtPoint(hitSound, hitPoint);

        // Activamos la física
        if (rb != null)
        {
            rb.isKinematic = false; 
            // Aplicamos una fuerza en el punto exacto donde chocó el rayo
            rb.AddForceAtPosition(hitDirection * impactForce, hitPoint);
        }

        // Destruir después de 3 segundos para no llenar la escena de basura
        Destroy(gameObject, 3f);
    }
}