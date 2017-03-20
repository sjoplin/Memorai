using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SamuraiController : MonoBehaviour {
    public int health = 30;

    Animator heart;
    Rigidbody2D rig;
    Animator animator;
    Collider2D col;
    CameraFuncs cam;
    public float maxRunSpeed = 1000f;
    public float runAccel = 100f;
    float runSpeedMod = 1.0f;
    float jumpVelocity = 25f;

    bool runningStop = false;
    bool dead = false;
    GameManager manager;


    public bool dashOnStart = true;
	// Use this for initialization
	void Start () {
        rig = gameObject.GetComponent<Rigidbody2D>();

        if (dashOnStart)  rig.velocity = new Vector2(50, rig.velocity.y); //Inital push to make him run onto screen when level starts

        animator = gameObject.GetComponent<Animator>();
        col = gameObject.GetComponent<CapsuleCollider2D>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFuncs>();
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        heart = GameObject.FindGameObjectWithTag("Heart").GetComponent<Animator>();
    }
	
	
	void Update () {
        //For Debug Purposes, Restarts the level. REMOVE FROM FINAL PRODUCT
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //Checks for hitting the ground
        RaycastHit2D groundHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + ((-col.bounds.size.y / 2) - 0.1f)), Vector2.down, 0.01f);
        RaycastHit2D leftHit = Physics2D.Raycast(new Vector2(transform.position.x - (col.bounds.size.x / 2) - 0.1f, transform.position.y), Vector2.left, 0.1f);
        RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(transform.position.x + (col.bounds.size.x / 2) + 0.1f, transform.position.y), Vector2.right, 0.1f);

        //Handles flipping the sprite
        if (rig.velocity.x > 0.05 && !animator.GetBool("Hurt")) {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y);
        }
        if (rig.velocity.x < -0.05 && !animator.GetBool("Hurt")) {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }

        //Animation triggers being set
        if (animator.GetBool("jumping")) animator.SetBool("jumping", false);
        if (health <= 0) animator.SetBool("Dead", true);
        animator.SetBool("Up", Input.GetAxis("Vertical") > 0);
        animator.SetBool("Down", Input.GetAxis("Vertical") < 0);
        dead = animator.GetBool("Dead");
        animator.SetBool("falling", Mathf.Abs(rig.velocity.y) > 0.05);
        runSpeedMod = Mathf.Abs(rig.velocity.x/maxRunSpeed);
        animator.SetFloat("runSpeedMod", runSpeedMod);
        animator.SetBool("grounded", groundHit);
        heart.SetInteger("Health", health);
        


        //Handles flipping the Samurai in the direction of his velocity
        if (Mathf.Abs(rig.velocity.x) > 1) {
            animator.SetBool("running", true);
        } else {
            animator.SetBool("running", false);
        }

        //Handles Movement
        float axis = Input.GetAxis("Horizontal");
        if (axis < 0 && !runningStop && !dead) {
            if (rig.velocity.x > -maxRunSpeed && !animator.GetBool("Hurt") && (!leftHit || leftHit.collider.gameObject.tag == "enemy") ) {
                rig.velocity = new Vector2(rig.velocity.x - runAccel, rig.velocity.y);
            }
        } else if (axis > 0 && !animator.GetBool("Hurt") && !runningStop && !dead && (!rightHit || rightHit.collider.gameObject.tag == "enemy")) {
            if (rig.velocity.x < maxRunSpeed && (!rightHit || rightHit.collider.gameObject.tag == "enemy")) {
                rig.velocity = new Vector2(rig.velocity.x + runAccel, rig.velocity.y);
            }
        }

        //Handles Attacking
        if (Input.GetButtonDown("Fire1") && !dead) {
            animator.SetBool("slash",true);
            if (Mathf.Abs(rig.velocity.x) > 2) {
                runningStop = true;
            }
        }
        if (Input.GetButtonUp("Fire1")) {
            if (runningStop) runningStop = false;
        }

        //Handles Dodging
        if (Input.GetButtonDown("Fire2") && !animator.GetBool("Down") && groundHit && !animator.GetBool("Hurt") && !animator.GetBool("dodge") && !dead) {
            animator.SetBool("dodge", true);
            float dodgeDir = 1;
            //dodgeDir = Mathf.Sign(Input.GetAxis("Fire2"));

            if (Mathf.Abs(rig.velocity.x) < 15) {
                rig.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 40 * dodgeDir, rig.velocity.y);
            } else {
                rig.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 50 * dodgeDir, rig.velocity.y);
            }
        }

        //Handles Jumping
        if (Input.GetButtonDown("Jump") && !dead && !animator.GetBool("slash")) {
            animator.SetBool("jumping", true);
            if (groundHit) {
                rig.velocity = new Vector2(rig.velocity.x * 1, jumpVelocity);
            }
        }

        //Handles Death
        if (dead) {
            StartCoroutine(manager.deathRestart());
        }

    }

    public void downwardDive() {
        rig.velocity = Vector2.zero;
    }

    public void endAnimator(string param) {
        animator.SetBool(param, false);
    }
    public GameObject downwardDust;
    public void createField() {
        //Instantiate(downwardDust, GameObject.FindGameObjectWithTag("sword").GetComponent<Transform>().position, Quaternion.identity);
        Instantiate(downwardDust, transform.position + (Vector3.down * 2), Quaternion.identity);
        StartCoroutine(cam.shakeScreen(0.4f));
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "gate") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (other.gameObject.tag == "enemy" && !animator.GetBool("dodge") && !animator.GetBool("Hurt") && !dead) {
            bool enemyHurt = false;
            if (other.gameObject.transform.root.gameObject.GetComponent<Animator>()) {
                enemyHurt = other.gameObject.transform.root.gameObject.GetComponent<Animator>().GetBool("Hurt");
            } else {
                enemyHurt = false;
            }
            if (!enemyHurt){
                if (!cam.getShaking()) StartCoroutine(cam.shakeScreen());
                animator.SetBool("Hurt", true);
                heart.SetTrigger("Hurt");
                health -= 10;
                manager.resetMult();
                if (health > 0) rig.velocity = new Vector2(Mathf.Sign(transform.position.x - other.transform.position.x) * 40, rig.velocity.y);
            }        
        }
    }
}
