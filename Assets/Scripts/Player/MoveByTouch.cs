using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveByTouch : MonoBehaviour {

    public float moveSpeed;
    public float jumpHeight;

    public GameObject row;
    public GameObject menu;
    public GameObject stats;
    public GameObject ControlesMoviles;

    public Joystick joystick;
    public ParticleSystem dust;

    private bool canJump;
    private bool canDoubleJump;
    private bool rotacionA = false;
    private bool rotacionD = false;


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (ControlesMoviles.active) {
            canJump = GetComponentInChildren<PlayerControllerUP>().getJump();
            canDoubleJump = GetComponentInChildren<PlayerControllerUP>().getDoubleJump();

            if (joystick.Horizontal < -0.1) {
                CreateDust();
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed * (-joystick.Horizontal), GetComponent<Rigidbody2D>().velocity.y);
                gameObject.GetComponent<Animator>().SetBool("moving", true);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

                rotacionA = true;
                if (rotacionD == true) {
                    row.transform.Rotate(0f, 180f, 0f);
                    rotacionD = false;
                }
            }


            if (joystick.Horizontal > 0.1) {
                CreateDust();
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * (joystick.Horizontal), GetComponent<Rigidbody2D>().velocity.y);
                gameObject.GetComponent<Animator>().SetBool("moving", true);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;

                rotacionD = true;
                if (rotacionA == true) {
                    row.transform.Rotate(0f, 180f, 0f);
                    rotacionA = false;
                }
            }

            // Quitar animacion de correr
            if (joystick.Horizontal == 0) {
                gameObject.GetComponent<Animator>().SetBool("moving", false);
            }
        }
    }

    public void saltar() {
        Debug.Log("¡Método saltar() ejecutado!");
        
        if (ControlesMoviles.active) {
            Debug.Log("Controles móviles activos");
            
            if (canJump) {
                Debug.Log("¡Saltando normal!");
                
                // NUEVO: Agregar sonido procedural de salto (CORREGIDO)
                SimpleProceduralAudio audioGen = FindObjectOfType<SimpleProceduralAudio>();
                if (audioGen != null) {
                    Debug.Log("Reproduciendo sonido de salto normal");
                    audioGen.PlayJumpSound();
                } else {
                    Debug.Log("ERROR: No se encontró SimpleProceduralAudio");
                }
                
                CreateDust();
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
                GetComponentInChildren<PlayerControllerUP>().setJump(false);
            } else if (canDoubleJump) {
                Debug.Log("¡Doble salto!");
                
                // NUEVO: Sonido también para doble salto (CORREGIDO)
                SimpleProceduralAudio audioGen = FindObjectOfType<SimpleProceduralAudio>();
                if (audioGen != null) {
                    Debug.Log("Reproduciendo sonido de doble salto");
                    audioGen.PlayJumpSound();
                } else {
                    Debug.Log("ERROR: No se encontró SimpleProceduralAudio para doble salto");
                }
                
                CreateDust();
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
                GetComponentInChildren<PlayerControllerUP>().setDoubleJump(false);
                canDoubleJump = false;
            } else {
                Debug.Log("No se puede saltar - canJump: " + canJump + ", canDoubleJump: " + canDoubleJump);
            }
        } else {
            Debug.Log("Controles móviles NO están activos");
        }
    }

    public void settings() {
        if (ControlesMoviles.active) {
            if (!menu.active) {
                stats.SetActive(false);
                menu.SetActive(true);
                Time.timeScale = 0;
            } else {
                menu.SetActive(false);
                stats.SetActive(true);
                Time.timeScale = 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (ControlesMoviles.active) {
            if (collision.gameObject.CompareTag("Chest")) {
                collision.gameObject.GetComponent<Chest>().openChest();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (ControlesMoviles.active) {
            if (collision.gameObject.CompareTag("Instructor")) {
                collision.gameObject.GetComponent<DialogueManager>().DialogueMobile();
            }
        }
    }

    void CreateDust() {
        dust.Play();
    }
}