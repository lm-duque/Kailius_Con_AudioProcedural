using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour 
{
    public int shieldValue = 10;
    public GameObject sonido; // Mantener por compatibilidad
    
    // Referencia al generador de audio procedural
    private SimpleProceduralAudio audioGenerator;
    
    void Start()
    {
        // Encontrar el generador de audio procedural
        audioGenerator = FindObjectOfType<SimpleProceduralAudio>();
        
        if (audioGenerator == null)
        {
            Debug.LogWarning("No se encontró SimpleProceduralAudio para Shield!");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            Stats.instance.addDefense(shieldValue);
            
            // NUEVO: Sonido procedural de escudo
            if (audioGenerator != null)
            {
                Debug.Log("Reproduciendo sonido procedural de escudo");
                audioGenerator.PlayShieldSound();
            }
            else
            {
                Debug.Log("Usando sonido original de escudo");
                Instantiate(sonido);
            }
            
            // Destruir el escudo después de recogerlo
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Water")) 
        {
            Destroy(gameObject);
        }
    }
}