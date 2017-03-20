using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/*
 * Responsible for spawning enemies at the beginning of the level
 */
public class Spawner : MonoBehaviour {
    public GameObject[] spawnPoints;
    public GameObject[] spawner;
    public int maxSpawnPerWave;
    public UnityEvent finishEvent;
    public UnityEvent startEvent;

    ArrayList curList = new ArrayList();

    int spawnCounter = 0;
    bool triggered = false;
    bool finished = false;

    public void spawn() {
        int waveCounter = 0;
        if (spawnCounter == 0) {
            startEvent.Invoke();
        }
        while (waveCounter < maxSpawnPerWave) {
            if (spawnCounter < spawner.Length) {
                GameObject spawn = spawner[spawnCounter];
                spawnCounter += 1;
                Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].GetComponent<Transform>().position;
                GameObject newEnemy = Instantiate(spawn, new Vector2(spawnPoint.x + Random.Range(-2,2), spawnPoint.y + Random.Range(-2,2)), Quaternion.identity);
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

    public ArrayList getCurArray() {
        return curList;
    }

  
}
