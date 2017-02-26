using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {
    public float delayTime = 3;
    public float fadeTime = 0.01f;
    // Use this for initialization

    public bool triggerSpawn = false;

	void Start () {
        StartCoroutine(fadeInFadeOut());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator fadeInFadeOut() {
        Image img = GetComponent<Image>();
        Color col = img.color;
        col.a = 0;
        img.color = col;
        while (col.a < 1) {
            col.a += fadeTime;
            img.color = col;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(delayTime);


        while (col.a > 0) {
            col.a -= fadeTime;
            img.color = col;
            yield return new WaitForSeconds(0.01f);
        }
        if (triggerSpawn == true) {
            GameObject.FindGameObjectWithTag("spawner").GetComponent<Spawner>().spawn();
        }
        Destroy(gameObject);
    }
}
