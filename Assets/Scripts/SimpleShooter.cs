using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SimpleShooter : MonoBehaviour
{
    public Transform muzzle;
    public float range = 50f;
    public LayerMask targetLayer; // Para que el rayo solo choque con objetivos

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
            
            // Buscamos si el objeto tiene el script de Target
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.OnHit();
            }
        }
    }
}