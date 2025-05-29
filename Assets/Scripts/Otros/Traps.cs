using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour 
{
    public int damage = 100;
    
    [Header("Configuración de Sonido")]
    public TrapType trapType = TrapType.Spikes; // Tipo de trampa para diferentes sonidos
    
    // Referencia al generador de audio procedural
    private SimpleProceduralAudio audioGenerator;
    
    public enum TrapType
    {
        Spikes,    // Púas - sonido punzante
        Water,     // Agua - sonido de splash
        Fire,      // Fuego - sonido de combustión
        Electric,  // Eléctrico - sonido de choque
        Poison     // Veneno - sonido tóxico
    }
    
    void Start()
    {
        // Encontrar el generador de audio procedural
        audioGenerator = FindObjectOfType<SimpleProceduralAudio>();
        
        if (audioGenerator == null)
        {
            Debug.LogWarning("No se encontró SimpleProceduralAudio para Traps!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.name == "Player") 
        {
            // NUEVO: Reproducir sonido procedural según el tipo de trampa
            if (audioGenerator != null)
            {
                PlayTrapSound();
            }
            
            // Aplicar daño
            collision.gameObject.GetComponentInParent<Stats>().takeTrueDamage(damage);

            if(collision.gameObject.GetComponentInParent<Stats>().health > 0) 
            {
                // Si el jugador sobrevive, hacer respawn
                Debug.Log("Jugador sobrevive - haciendo respawn");
                collision.gameObject.GetComponentInParent<PlayerController>().reSpawn();
            }
            else
            {
                // Si el jugador muere por la trampa
                Debug.Log("Jugador muerto por trampa");
            }
        }
    }
    
    void PlayTrapSound()
    {
        switch (trapType)
        {
            case TrapType.Spikes:
                Debug.Log("Reproduciendo sonido de púas");
                audioGenerator.PlaySpikesSound();
                break;
                
            case TrapType.Water:
                Debug.Log("Reproduciendo sonido de agua");
                audioGenerator.PlayWaterSplashSound();
                break;
                
            case TrapType.Fire:
                Debug.Log("Reproduciendo sonido de fuego");
                audioGenerator.PlayFireSound();
                break;
                
            case TrapType.Electric:
                Debug.Log("Reproduciendo sonido eléctrico");
                audioGenerator.PlayElectricSound();
                break;
                
            case TrapType.Poison:
                Debug.Log("Reproduciendo sonido de veneno");
                audioGenerator.PlayPoisonSound();
                break;
        }
    }
}