using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    public Transform cameraPos;
    private Vector3 spawnPos;
    public GameObject slimePrefab;
    float spawnPosX;
    private int enemyCount;
    public int numberOfEnemies;
    
    private void Start() {
        //spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 10f));

    }
    private void Update() {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(enemyCount<= numberOfEnemies){
        SpawnEnemy();
        }
    }

    void SpawnEnemy(){
        Instantiate(slimePrefab, GenerateSpawnPosition1(), slimePrefab.transform.rotation);
        Instantiate(slimePrefab, GenerateSpawnPosition2(), slimePrefab.transform.rotation);
        Instantiate(slimePrefab, GenerateSpawnPosition3(), slimePrefab.transform.rotation);

    }
    Vector2 GenerateSpawnPosition1(){
        float cameraXright = Random.Range(cameraPos.position.x + 11, 17);
        float cameraYup = Random.Range(cameraPos.position.y + 7f, 32);
        Vector2 randomPos = new Vector2(spawnPosX, cameraYup);
        return randomPos;
    }
    Vector2 GenerateSpawnPosition2(){
        float cameraYup = Random.Range(cameraPos.position.y + 7f, 32);
        spawnPosX = Random.Range(-41, 17);
        Vector2 randomPos = new Vector2(spawnPosX, cameraYup);
        return randomPos;
    }
        Vector2 GenerateSpawnPosition3(){
        float cameraYdown = Random.Range(cameraPos.position.y - 7, -7);
        spawnPosX = Random.Range(-41, 17);
        Vector2 randomPos = new Vector2(spawnPosX, cameraYdown);
        return randomPos;
    }    
}
