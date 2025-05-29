using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour 
{
    public int coinValue = 1;
    public int scoreValue = 10;
    public GameObject sonido; // Mantener por compatibilidad
    
    // Referencia al generador de audio procedural
    private SimpleProceduralAudio audioGenerator;
    
    void Start()
    {
        // Encontrar el generador de audio procedural
        audioGenerator = FindObjectOfType<SimpleProceduralAudio>();
        
        if (audioGenerator == null)
        {
            Debug.LogWarning("No se encontró SimpleProceduralAudio para Coin!");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.CompareTag("Player")) 
        {
            ScoreManager.instance.ChangeScore(scoreValue);
            ScoreManager.instance.ChangeScoreCoin(coinValue);
            
            // NUEVO: Sonido procedural de moneda
            if (audioGenerator != null)
            {
                Debug.Log("Reproduciendo sonido procedural de moneda");
                audioGenerator.PlayCoinSound();
            }
            else
            {
                Debug.Log("Usando sonido original de moneda");
                Instantiate(sonido);
            }
            
            // Destruir la moneda después de recogerla
            Destroy(gameObject);
        }
        
        if(collision.gameObject.CompareTag("Water")) 
        {
            Destroy(gameObject);
        }
    }
}