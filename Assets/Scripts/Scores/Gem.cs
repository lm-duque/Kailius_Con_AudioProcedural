using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour 
{
    public int gemValue = 1;
    public int scoreValue = 15;
    public GameObject sonido; // Mantener por compatibilidad
    
    // Referencia al generador de audio procedural
    private SimpleProceduralAudio audioGenerator;
    
    void Start()
    {
        // Encontrar el generador de audio procedural
        audioGenerator = FindObjectOfType<SimpleProceduralAudio>();
        
        if (audioGenerator == null)
        {
            Debug.LogWarning("No se encontró SimpleProceduralAudio para Gem!");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            ScoreManager.instance.ChangeScore(scoreValue);
            ScoreManager.instance.ChangeScoreGem(gemValue);
            
            // NUEVO: Sonido procedural de gema
            if (audioGenerator != null)
            {
                Debug.Log("Reproduciendo sonido procedural de gema");
                audioGenerator.PlayGemSound();
            }
            else
            {
                Debug.Log("Usando sonido original de gema");
                Instantiate(sonido);
            }
            
            // Destruir la gema después de recogerla
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Water")) 
        {
            Destroy(gameObject);
        }
    }
}