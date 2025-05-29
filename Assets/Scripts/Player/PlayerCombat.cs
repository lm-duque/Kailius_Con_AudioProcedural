using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public GameObject sonido; // Mantener por compatibilidad, pero no se usará
    
    public float attactRate = 0.6f;
    public float attackRange = 1f;
    private float nextAttactTime = 0.5f;
    
    // Referencia al generador de audio procedural simple
    private SimpleProceduralAudio audioGenerator;

    void Start()
    {
        // Encontrar el generador de audio procedural
        audioGenerator = FindObjectOfType<SimpleProceduralAudio>();
        
        if (audioGenerator == null)
        {
            Debug.LogWarning("No se encontró SimpleProceduralAudio en la escena!");
        }
        else
        {
            Debug.Log("SimpleProceduralAudio encontrado correctamente para PlayerCombat");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Hace que no se pueda spamear
        if(Time.time >= nextAttactTime)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Tecla F presionada - Ejecutando Attack()");
                Attack();
                nextAttactTime = Time.time + attactRate / attackRange;
            }
        }
    }

    void Attack()
{
    Debug.Log("¡Método Attack() ejecutado!");
    
    // Play una animacion
    animator.SetTrigger("attack");

    // Detectar enemigos ANTES de reproducir sonido
    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
    bool hitEnemy = false;
    
    // Resta el daño
    foreach(Collider2D enemy in hitEnemies)
    {
        enemy.GetComponent<Enemy>().TakeDamage(Stats.instance.getAttackDamage());
        
        if(GetComponent<Stats>().getPower() != 4)
            GetComponent<Stats>().takePower(1);
        
        hitEnemy = true; // Marcamos que golpeamos enemigo
        break;
    }
    
    // NUEVO: Sonido diferente según si golpeaste enemigo o aire
    if (audioGenerator != null)
    {
        if (hitEnemy)
        {
            Debug.Log("Reproduciendo sonido de golpe a enemigo");
            audioGenerator.PlayHitEnemySound();
        }
        else
        {
            Debug.Log("Reproduciendo sonido de ataque al aire");
            audioGenerator.PlaySwordSlashSound(); // El sonido actual de espada al aire
        }
    }
    else
    {
        Debug.Log("audioGenerator es null, usando sonido original");
        Instantiate(sonido);
    }
}

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void AttackButton()
    {
        // Hace que no se pueda spamear
        if (Time.time >= nextAttactTime)
        {
            Debug.Log("AttackButton presionado");
            Attack();
            nextAttactTime = Time.time + attactRate / attackRange;
        }
    }
}