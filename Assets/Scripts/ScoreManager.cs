using UnityEngine;
using TMPro; // Necesitas TextMeshPro para el texto

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TMPro.TextMeshProUGUI scoreText; // El UGUI es clave si usas Canvas
    [SerializeField] private int currentScore = 0;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void AddPoints(int amount)
    {
        currentScore += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "Puntos: " + currentScore;
    }
}
