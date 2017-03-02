using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/*
 * Responsible for spawning enemies at the beginning of the level
 */
public class Spawner : MonoBehaviour {
    public GameObject[] spawner;
    public int maxSpawnPerWave;
    public UnityEvent finishEvent;

    ArrayList curList = new ArrayList();

    int spawnCounter = 0;
    bool triggered = false;
    bool finished = false;

    public void spawn() {
        int waveCounter = 0;
        while (waveCounter < maxSpawnPerWave) {
            if (spawnCounter < spawner.Length) {
                GameObject spawn = spawner[spawnCounter];
                spawnCounter += 1;
                GameObject newEnemy = Instantiate(spawn, new Vector2(transform.position.x + (Random.Range(-20, 20)), transform.position.y), Quaternion.identity);
                curList.Add(newEnemy);
            }
            waveCounter += 1;
        }
        triggered = true;
    }

    public void removeCurrent(GameObject item) {
        curList.Remove(item);
    }
    void Update() {
        if (curList.Count <= 0 && spawnCounter < spawner.Length && triggered) {
            spawn();
        }
        if (spawnCounter >= spawner.Length && curList.Count == 0 && !finished) {
            finishEvent.Invoke();
            finished = true;
        }
    }

  
}
