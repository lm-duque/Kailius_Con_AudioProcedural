using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public int health = 500;
    public float timeDestroy = 1.5f;
    public Animator animator;
    public GameObject sonidoMuerte; // Mantener por compatibilidad

    public GameObject coins;
    public GameObject hearts;
    public GameObject sword;
    public GameObject shield;

    public int maxCoins = 5;
    public int maxHearts = 3;
    public int maxSwords = 2;
    public int maxShields = 2;
    
    [Header("Configuración de Ataque")]
    public int damageToPlayer = 50; // Daño que hace al jugador
    public float attackCooldown = 2f; // Tiempo entre ataques
    private float lastAttackTime = 0f;
    
    [Header("Tipo de Enemigo")]
    public EnemyType enemyType = EnemyType.Human; // NUEVO: Tipo de enemigo
    
    // Referencia al generador de audio procedural
    private SimpleProceduralAudio audioGenerator;
    
    public enum EnemyType
    {
        Human,      // Knight, soldados - sonido metálico
        Undead,     // Skeleton, zombies - sonido de huesos
        Slime,      // Slimes - sonido gelatinoso
        Flying,     // Bee, pájaros - sonido de alas
        Beast,      // Lobos, osos - rugido
        Armor,      // Robots, armaduras - sonido metálico pesado
        Magic,      // Criaturas mágicas - sonido místico
        Giant       // Gigantes - sonido masivo
    }
    
    void Start()
    {
        // Encontrar el generador de audio procedural
        audioGenerator = FindObjectOfType<SimpleProceduralAudio>();
        
        if (audioGenerator == null)
        {
            Debug.LogWarning("No se encontró SimpleProceduralAudio para Enemy!");
        }
    }

    public void TakeDamage(int damage) 
    {
        this.health -= damage;

        // NUEVO: Reproducir sonido diferente según tipo de enemigo cuando es golpeado
        if (audioGenerator != null)
        {
            Debug.Log($"Enemigo {enemyType} recibe daño");
            audioGenerator.PlayEnemyHitSound(enemyType);
        }

        // Play animacion de herida
        animator.SetTrigger("hurt");

        if(health <= 0) 
        {
            Die();
        }
    }

    void Die() 
    {
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;

        // Dropear items
        dropItems();

        // Play animacion de muerto
        animator.SetBool("isDead", true);

        // Añadir puntuacion
        ScoreManager.instance.ChangeScore(100);

        // Destruir al enemigo
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // NUEVO: Sonido procedural de muerte según tipo de enemigo
        if (audioGenerator != null)
        {
            Debug.Log($"Reproduciendo sonido de muerte de enemigo {enemyType}");
            audioGenerator.PlayEnemySpecificDeathSound(enemyType);
        }
        else
        {
            Debug.Log("Usando sonido original de muerte de enemigo");
            Instantiate(sonidoMuerte);
        }
        
        Object.Destroy(gameObject, timeDestroy);
    }
    
    // Detectar colisión con el jugador para hacer daño
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time >= lastAttackTime + attackCooldown)
        {
            // Hacer daño al jugador
            Stats playerStats = other.GetComponentInChildren<Stats>();
            if (playerStats != null)
            {
                Debug.Log($"Enemigo {enemyType} ataca al jugador con {damageToPlayer} de daño");
                playerStats.takeDamage(damageToPlayer);
                lastAttackTime = Time.time;
                
                // Opcional: animación de ataque del enemigo
                if (animator != null)
                {
                    animator.SetTrigger("attack");
                }
            }
        }
    }
    
    // También detectar colisión física (si el enemigo tiene Rigidbody2D)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= lastAttackTime + attackCooldown)
        {
            // Hacer daño al jugador
            Stats playerStats = collision.gameObject.GetComponentInChildren<Stats>();
            if (playerStats != null)
            {
                Debug.Log($"Enemigo {enemyType} colisiona con jugador - {damageToPlayer} de daño");
                playerStats.takeDamage(damageToPlayer);
                lastAttackTime = Time.time;
                
                // Opcional: empujar al jugador
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                    playerRb.AddForce(pushDirection * 300f);
                }
            }
        }
    }

    void dropItems() 
    {
        int numCoins = Random.Range(1, maxCoins);
        int numHearts = Random.Range(0, maxHearts);
        int numSwords = Random.Range(0, maxSwords);
        int numShields = Random.Range(0, maxShields);

        for (int i = 0; i < numCoins; i++) 
        {
            Instantiate(coins, new Vector3(gameObject.transform.position.x - 1.0f, gameObject.transform.position.y + 2.0f, gameObject.transform.position.z), Quaternion.identity);
        }

        for (int i = 0; i < numHearts; i++) 
        {
            Instantiate(hearts, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2.0f, gameObject.transform.position.z), Quaternion.identity);
        }

        for (int i = 0; i < numSwords; i++) 
        {
            Instantiate(sword, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2.0f, gameObject.transform.position.z), Quaternion.identity);
        }

        for (int i = 0; i < numShields; i++) 
        {
            Instantiate(shield, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2.0f, gameObject.transform.position.z), Quaternion.identity);
        }
    }
}