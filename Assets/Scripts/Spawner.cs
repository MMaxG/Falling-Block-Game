using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject fallingBlockPrefab;
    public Vector2 secondsBetweenSpawnsMinMax;
    float nextSpawnTime;
    public Vector2 spawnSizeMinMax;
    public float spawnAngleMax;

    GameManager gameManager;
    bool gameStarted = false;
    float startTime;

    Vector2 screenHalfSizeWorldUnits;

    // Start is called before the first frame update
    void Start() {
        screenHalfSizeWorldUnits = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        // Check to see if its time to start spawning obstacles
        if (Mathf.Floor(gameManager.backgroundTime) == startTime && startTime != 0) {
            gameStarted = true;
        }

        // Check if countdown is done, game is started and start spawning objects
        if (gameStarted) {
            if (gameManager.backgroundTime > nextSpawnTime) {

                // Time between spawns is based on current difficulty percent using Linear Interpolation (between min & max by difficulty)
                float secondsBetweenSpawns = Mathf.Lerp(secondsBetweenSpawnsMinMax.y, secondsBetweenSpawnsMinMax.x, Difficulty.GetDifficultyPercent(gameManager.backgroundTime));

                nextSpawnTime = gameManager.backgroundTime + secondsBetweenSpawns;

                float spawnAngle = Random.Range(-spawnAngleMax, spawnAngleMax);
                float spawnSize = Random.Range(spawnSizeMinMax.x, spawnSizeMinMax.y);
                Vector2 spawnPosition = new Vector2(Random.Range(-screenHalfSizeWorldUnits.x, screenHalfSizeWorldUnits.x), screenHalfSizeWorldUnits.y + spawnSize);
                GameObject newBlock = (GameObject)Instantiate(fallingBlockPrefab, spawnPosition, Quaternion.Euler(Vector3.forward * spawnAngle));
                newBlock.tag = "Falling Block";
                newBlock.transform.localScale = Vector2.one * spawnSize;
                newBlock.transform.parent = gameManager.spawner.transform;
            }
        }
    }

    public void Activate(float delay) {
        startTime = Mathf.Floor(gameManager.backgroundTime) + delay;
        nextSpawnTime = gameManager.backgroundTime + Mathf.Lerp(secondsBetweenSpawnsMinMax.y, secondsBetweenSpawnsMinMax.x, Difficulty.GetDifficultyPercent(gameManager.backgroundTime));
    }

    public void Deactivate() {
        gameStarted = false;
        GameObject[] fallingBlocks = GameObject.FindGameObjectsWithTag("Falling Block");
        

        foreach (GameObject block in fallingBlocks) {
            Destroy(block);
        }
    }
}
