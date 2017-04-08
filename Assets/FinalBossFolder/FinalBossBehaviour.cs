using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossBehaviour : MonoBehaviour {
    CameraFuncs cam;
    Animator animator;
    public EvilCloudFunctions clouds;
    public int health = 100;
    public int attackProbability = 1000;
    public float restTime = 10.0f;

    public GameObject lightning;
    public GameObject[] possibleSpawn;

	void Start () {
        animator = gameObject.GetComponent<Animator>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFuncs>();
	}


    
	void Update () {
        openingCameraShake();    
        if (health < 20) {
            attackProbability = 10;
        }
        if (!animator.GetBool("Vunerable") && !animator.GetBool("Awakening") && !animator.GetBool("Death") && Random.Range(0, attackProbability) == 0) {
            if (health > 30) {
                clouds.startSparks();
                spawnEnemies();
            } else {
                clouds.startSimulSparks();
                spawnEnemies();
            }
            StartCoroutine(cooldown());
        }
	}


    /*
     * Used for the opening camera shake as the boss appears.
     */
    bool awakening = true;
    float timeCounter = 0;
    void openingCameraShake() {
        if (!animator.GetBool("Awakening") && awakening) {
            cam.endShake();
            timeCounter = 0;
            awakening = false;
        } else if (animator.GetBool("Awakening")) {
            if (timeCounter >= 0.01f) {
                cam.shakeOnce();
                timeCounter = 0;
            }
            timeCounter += Time.deltaTime;
        }
    }

    public void hurt() {
        animator.SetBool("Vunerable", false);
        StopAllCoroutines();
        health -= 10;
        if (health <= 0) {
            death();
        }
    }

    void death() {
        animator.SetBool("Death", true);
        cam.shakeOnce();
    }

    void spawnEnemies() {
        GameObject[] cur = GameObject.FindGameObjectsWithTag("enemy");
        if (cur.Length <= 6) {
            if (Random.Range(0, 3) == 1) {
                Instantiate(possibleSpawn[Random.Range(0, possibleSpawn.Length - 1)], transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator cooldown() {
        if (animator.GetBool("Vunerable") == false) {
            yield return new WaitForSeconds(5);
            animator.SetBool("Vunerable", true);
            yield return new WaitForSeconds(restTime);
            if (animator.GetBool("Vunerable")) {
                animator.SetBool("Vunerable", false);
            }
        }
    }

}
