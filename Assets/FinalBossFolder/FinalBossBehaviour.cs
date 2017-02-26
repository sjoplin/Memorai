using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossBehaviour : MonoBehaviour {
    CameraFuncs cam;
    Animator animator;

	void Start () {
        animator = gameObject.GetComponent<Animator>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFuncs>();
	}


    
	void Update () {
        openingCameraShake();    
        


	}


    /*
     * Used for the opening camera shake as the boss appears.
     */
    bool awakening = true;
    float timeCounter = 0;
    void openingCameraShake() {
        if (!animator.GetBool("Awakening") && awakening) {
            cam.endShake();
            awakening = false;
        } else if (animator.GetBool("Awakening")) {
            if (timeCounter >= 0.01f) {
                cam.shakeOnce();
                timeCounter = 0;
            }
            timeCounter += Time.deltaTime;
        }
    }


}
