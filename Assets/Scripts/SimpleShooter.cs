using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;

public class SimpleShooter : MonoBehaviour
{
    [Header("Referencias Base")]
    public Transform muzzle;
    public float range = 50f;
    public LayerMask targetLayer; // Filtro para colisiones

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip fireSound;

    [Header("Configuración de Munición")]
    public int maxAmmo = 12;
    private int currentAmmo;
    public TMP_Text ammoText; // Asigna el texto 'AmmoText' del HUD aquí

    [Header("Puntero Láser")]
    public LineRenderer laserLine; // Componente para dibujar la línea
    private bool laserEnabled = true;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        
        // Cargar la configuración guardada del láser (1 = Activo, 0 = Inactivo)
        laserEnabled = PlayerPrefs.GetInt("UseLaser", 1) == 1;
        if (laserLine != null) 
        {
            laserLine.gameObject.SetActive(laserEnabled);
        }
    }

    void OnEnable() => grabInteractable.activated.AddListener(OnFire);
    void OnDisable() => grabInteractable.activated.RemoveListener(OnFire);

    void Update()
    {
        // El láser se actualiza frame a frame si está activado en las configuraciones
        if (laserEnabled && laserLine != null && muzzle != null)
        {
            RenderizarLaser();
        }
    }

    void OnFire(ActivateEventArgs args)
    {
        Shoot();
    }

    void Shoot()
    {
        // 1. Verificación de munición
        if (currentAmmo <= 0)
        {
            // Opcional: Sonido de click seco si se queda sin balas
            return;
        }

        // 2. Descontar bala y actualizar la UI del jugador
        currentAmmo--;
        UpdateAmmoUI();

        // 3. Reproducir audio de disparo
        if (audioSource != null && fireSound != null) 
        {
            audioSource.PlayOneShot(fireSound);
        }

        // 4. Lógica de impacto (Raycast)
        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range, targetLayer))
        {
            Debug.Log("Impacto en: " + hit.transform.name);
            
            // Buscamos el script en el padre debido a la nueva estructura de la diana
            Target target = hit.transform.GetComponentInParent<Target>(); 

            if (target != null)
            {
                target.OnHit(hit.point, muzzle.forward);
            }
        }
    }

    void RenderizarLaser()
    {
        if (muzzle == null || laserLine == null) return;

        laserLine.SetPosition(0, muzzle.position); // Comienzo en la punta del cañón
        RaycastHit hit;
        
        // CORRECCIÓN: Usamos 'targetLayer' para que el láser visual ignore la pistola/jugador
        // y solo colisione con objetivos o el entorno permitido.
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range, targetLayer))
        {
            // Si choca, la línea termina en el impacto
            laserLine.SetPosition(1, hit.point);
        }
        else
        {
            // Si no choca, se extiende hasta el rango máximo en línea recta hacia adelante
            laserLine.SetPosition(1, muzzle.position + (muzzle.forward * range));
        }
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = "Balas: " + currentAmmo + " / " + maxAmmo;
        }
    }

    // Método público que invocarás desde tu menú de opciones con el UI Toggle
    public void ToggleLaser(bool state)
    {
        laserEnabled = state;
        if (laserLine != null) 
        {
            laserLine.gameObject.SetActive(laserEnabled);
        }
        
        // Persistencia de datos para la configuración
        PlayerPrefs.SetInt("UseLaser", state ? 1 : 0);
        PlayerPrefs.Save();
    }
}