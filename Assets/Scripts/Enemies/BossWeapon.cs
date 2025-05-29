using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour 
{
    public int attackDamage = 100;

    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;
    
    [Header("Configuración de Sonido")]
    public BossType bossType = BossType.Knight; // Tipo de jefe para diferentes sonidos
    
    // Referencia al generador de audio procedural
    private SimpleProceduralAudio audioGenerator;
    
    public enum BossType
    {
        Knight,    // Caballero - sonido metálico pesado
        Beast,     // Bestia - rugido y garras
        Undead,    // No-muerto - sonido siniestro
        Magic,     // Mágico - sonido místico
        Giant,     // Gigante - sonido masivo
        Dragon     // Dragón - rugido épico
    }
    
    void Start()
    {
        // Encontrar el generador de audio procedural
        audioGenerator = FindObjectOfType<SimpleProceduralAudio>();
        
        if (audioGenerator == null)
        {
            Debug.LogWarning("No se encontró SimpleProceduralAudio para BossWeapon!");
        }
    }

    public void Attack() 
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null) 
        {
            // Hacer daño al jugador
            colInfo.gameObject.GetComponentInParent<Stats>().takeDamage(attackDamage);
            
            // NUEVO: Reproducir sonido procedural del boss atacando
            if (audioGenerator != null)
            {
                Debug.Log($"Boss {bossType} golpea al jugador");
                audioGenerator.PlayBossAttackSound(bossType);
            }
        } 
        else 
        {
            // Boss ataca al aire
            if (audioGenerator != null)
            {
                Debug.Log($"Boss {bossType} ataca al aire");
                audioGenerator.PlayBossSwingSound(bossType);
            }
        }
    }

    void OnDrawGizmosSelected() 
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }
}