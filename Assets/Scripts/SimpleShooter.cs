using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SimpleShooter : MonoBehaviour
{
    public Transform muzzle;
    public float range = 50f;
    public LayerMask targetLayer; // Para que el rayo solo choque con objetivos
    public AudioSource audioSource;
    public AudioClip fireSound;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void OnEnable() => grabInteractable.activated.AddListener(OnFire);
    void OnDisable() => grabInteractable.activated.RemoveListener(OnFire);

    void OnFire(ActivateEventArgs args)
    {
        Shoot();
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range, targetLayer))
        {
        Debug.Log("Impacto en: " + hit.transform.name);
        
        // Cambia esto en SimpleShooter.cs
Target target = hit.transform.GetComponentInParent<Target>(); // Busca en el objeto golpeado o sus padres

        if (target != null)
        {
            target.OnHit(hit.point, muzzle.forward);
        }
        
        if (target != null)
        {
            // CAMBIA ESTA LÍNEA:
            // Pasamos hit.point (donde chocó el rayo) 
            // y muzzle.forward (hacia dónde apuntaba el arma)
            target.OnHit(hit.point, muzzle.forward); 
        }
    }
        // Reproducir sonido de disparo
        if (audioSource && fireSound) audioSource.PlayOneShot(fireSound);

        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range, targetLayer))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                // Ahora le enviamos al objetivo el punto de impacto y la dirección
                target.OnHit(hit.point, muzzle.forward); 
            }
        }
    }
}