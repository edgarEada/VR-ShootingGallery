using UnityEngine;

public class Target : MonoBehaviour
{
    public int scoreValue = 10;
    
    public void OnHit()
    {
        // Sumamos puntos al manager
        ScoreManager.instance.AddPoints(scoreValue);
        
        // Aquí podrías poner un efecto de sonido o partículas
        Debug.Log("Objetivo destruido");
        
        // Por ahora, solo lo desactivamos
        gameObject.SetActive(false);
    }
}