using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour 
{
    // Referencia al generador de audio procedural
    private SimpleProceduralAudio audioGenerator;
    
    void Start()
    {
        // Encontrar el generador de audio procedural
        audioGenerator = FindObjectOfType<SimpleProceduralAudio>();
        
        if (audioGenerator == null)
        {
            Debug.LogWarning("No se encontró SimpleProceduralAudio para Menu!");
        }
    }

    public void PlayGame() 
    {
        // Sonido de iniciar juego (épico y motivacional)
        if (audioGenerator != null)
        {
            Debug.Log("Reproduciendo sonido de Play Game");
            audioGenerator.PlayStartGameSound();
        }
        
        PlayerPrefs.SetInt("AttackDamage", 0);
        PlayerPrefs.SetInt("Defense", 0);
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("ScoreCoins", 0);
        PlayerPrefs.SetInt("ScoreGems", 0);
        PlayerPrefs.SetInt("ScoreStars", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResumeGame() 
    {
        // Sonido de reanudar (positivo y suave)
        if (audioGenerator != null)
        {
            Debug.Log("Reproduciendo sonido de Resume Game");
            audioGenerator.PlayResumeSound();
        }
        
        Time.timeScale = 1;
    }

    public void QuitGame() 
    {
        // Sonido de salir (descendente y final)
        if (audioGenerator != null)
        {
            Debug.Log("Reproduciendo sonido de Quit Game");
            audioGenerator.PlayQuitSound();
        }
        
        Application.Quit();
    }

    public void PlayAgain() 
    {
        // Sonido de jugar otra vez (renovador y energético)
        if (audioGenerator != null)
        {
            Debug.Log("Reproduciendo sonido de Play Again");
            audioGenerator.PlayAgainSound();
        }
        
        SceneManager.LoadScene("Menu");
    }
}