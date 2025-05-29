using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour 
{
    // Referencia al generador de audio procedural
    private SimpleProceduralAudio audioGenerator;
    private bool hasTriggered = false; // Evitar múltiples activaciones
    
    void Start()
    {
        // Encontrar el generador de audio procedural
        audioGenerator = FindObjectOfType<SimpleProceduralAudio>();
        
        if (audioGenerator == null)
        {
            Debug.LogWarning("No se encontró SimpleProceduralAudio para Portal!");
        }
    }

    private void OnTriggerEnter2D(Collider2D plyr) 
    {
        if (plyr.gameObject.tag == "Player" && !hasTriggered) 
        {
            hasTriggered = true; // Evitar múltiples activaciones
            
            // NUEVO: Sonido procedural de portal mágico
            if (audioGenerator != null)
            {
                Debug.Log("Reproduciendo sonido de portal mágico");
                audioGenerator.PlayPortalSound();
            }
            
            // Pequeño delay para que se escuche el sonido antes de cambiar escena
            StartCoroutine(LoadSceneAfterSound());
        }
    }
    
    private IEnumerator LoadSceneAfterSound()
    {
        // Esperar un poco para que se reproduzca el sonido del portal
        yield return new WaitForSeconds(0.8f);
        
        // Cargar la siguiente escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}