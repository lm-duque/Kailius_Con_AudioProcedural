using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpHeight;
  
    public GameObject row;
    public GameObject menu;
    public GameObject stats;
    public GameObject controllerMobile;
    public int damagePatrols = 25;

    private bool canJump;
    private bool canDoubleJump;
    private bool rotacionA = false;
    private bool rotacionD = false;

    private bool dead = false;
    private float attactRate = 0.6f;
    private float nextAttactTime = 0.5f;
    public ParticleSystem dust;

    void Start() {
        // NUEVO: Forzar que los controles móviles estén desactivados al inicio
        if (controllerMobile != null) {
            controllerMobile.SetActive(false);
            Debug.Log("ControllerMobile desactivado en Start()");
        }
    }

    void Update() {
        
        // NUEVO: Debug para ver el estado
        if (controllerMobile != null) {
            Debug.Log("ControllerMobile active: " + controllerMobile.activeSelf);
        }

        if(!dead && (controllerMobile == null || !controllerMobile.activeSelf)) {

            if(Input.GetKeyDown(KeyCode.Escape)) {
                if(!menu.activeSelf) {
                    stats.SetActive(false);
                    menu.SetActive(true);
                    Time.timeScale = 0;
                } else {
                    menu.SetActive(false);
                    stats.SetActive(true);
                    Time.timeScale = 1;
                }
            }

            canJump = GetComponentInChildren<PlayerControllerUP>().getJump();
            canDoubleJump = GetComponentInChildren<PlayerControllerUP>().getDoubleJump();

            if (Input.GetKeyDown(KeyCode.Space)) {
                if(canJump) {
                    CreateDust();
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
                    GetComponentInChildren<PlayerControllerUP>().setJump(false);
                } else if(canDoubleJump) {
                    CreateDust();
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
                    GetComponentInChildren<PlayerControllerUP>().setDoubleJump(false);
                    canDoubleJump = false;
                }
            }

            if (Input.GetKey(KeyCode.A)) {
                CreateDust();
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                gameObject.GetComponent<Animator>().SetBool("moving", true);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                
                rotacionA = true;
                if (rotacionD == true) {
                    row.transform.Rotate(0f, 180f, 0f);
                    rotacionD = false;
                }
            }

            if (Input.GetKey(KeyCode.D)) {
                CreateDust();
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                gameObject.GetComponent<Animator>().SetBool("moving", true);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;

                rotacionD = true;
                if(rotacionA == true) {
                    row.transform.Rotate(0f, 180f, 0f);
                    rotacionA = false;
                }
            }

            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
                gameObject.GetComponent<Animator>().SetBool("moving", false);
            }
        } else {
            // NUEVO: Debug cuando los controles están bloqueados
            Debug.Log("Controles bloqueados - Dead: " + dead + ", ControllerMobile active: " + (controllerMobile != null ? controllerMobile.activeSelf.ToString() : "null"));
        }
    }

    // Resto del código igual...
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Patrols") {
            if (Time.time >= nextAttactTime) {
                GetComponentInChildren<Stats>().takeDamage(damagePatrols);
                nextAttactTime = Time.time + attactRate;
            }
            GetComponent<Rigidbody2D>().AddForce(new Vector2(5, 5) * 2, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Coins")) {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Gems")) {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Stars")) {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Heart")) {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Sword")) {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Shield")) {
            Destroy(collision.gameObject);
        }
    }

    public void reSpawn() {
        transform.position = GameObject.FindWithTag("ReSpawn").transform.position;
    }

    public void destroy() {
        Object.Destroy(gameObject, 5.0f);
        dead = true;
    }

    public bool isDead() {
        return this.dead = true;
    }

    void CreateDust() {
        dust.Play();
    }
}