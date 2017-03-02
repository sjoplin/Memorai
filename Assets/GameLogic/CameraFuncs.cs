using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFuncs : MonoBehaviour {
    bool shaking = false;
    public float shakeStr = 0.5f;
    Vector3 origPos;
	// Use this for initialization
	void Start () {
        origPos = transform.position;
	}
	
    public bool getShaking() {
        return shaking;
    }
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F1)) {
            print(Screen.width + " , " + Screen.height);
        }
	}
    public void shakeOnce() {
        transform.position = new Vector3(Random.Range(origPos.x - shakeStr, origPos.x + shakeStr),
                Random.Range(origPos.y - shakeStr, origPos.y + shakeStr),
                transform.position.z);
    }

    public void endShake() {
        transform.position = origPos;
    }
    public IEnumerator shakeScreen() {
        shaking = true;
        origPos = transform.position;
        float deltaT = 0;
        while (deltaT < 0.05f) {
            transform.position = new Vector3(Random.Range(origPos.x - shakeStr, origPos.x + shakeStr),
                Random.Range(origPos.y - shakeStr, origPos.y + shakeStr),
                transform.position.z);
            yield return new WaitForSeconds(0.03f);
            deltaT += Time.deltaTime;
        }
        transform.position = origPos;
        shaking = false;
    }

    public IEnumerator shakeScreen(int customShake) {
        shaking = true;
        Vector3 origPos = transform.position;
        float deltaT = 0;
        while (deltaT < 0.05f) {
            transform.position = new Vector3(Random.Range(origPos.x - customShake, origPos.x + customShake),
                Random.Range(origPos.y - customShake, origPos.y + customShake),
                transform.position.z);
            yield return new WaitForSeconds(0.03f);
            deltaT += Time.deltaTime;
        }
        transform.position = origPos;
        shaking = false;
    }
}
