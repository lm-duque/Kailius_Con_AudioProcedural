using UnityEngine;

public class SimpleAudioTest : MonoBehaviour
{
    private SimpleProceduralAudio audioGen;
    
    void Start()
    {
        audioGen = FindObjectOfType<SimpleProceduralAudio>();
        
        if (audioGen == null)
        {
            Debug.LogError("No se encontró SimpleProceduralAudio!");
        }
        else
        {
            Debug.Log("SimpleProceduralAudio encontrado y listo");
        }
    }
    
    void Update()
    {
        // Presiona J para sonido de salto
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Tecla J - Sonido de salto");
            if (audioGen != null)
            {
                audioGen.PlayJumpSound();
            }
        }
        
        // Presiona A para sonido de ataque  
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Tecla A - Sonido de ataque");
            if (audioGen != null)
            {
                audioGen.PlayAttackSound();
            }
        }
        
        // Presiona C para sonido de moneda
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Tecla C - Sonido de moneda");
            if (audioGen != null)
            {
                audioGen.PlayCoinSound();
            }
        }
    }
}