using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    private Scene scene;

    public void GameOver(){
        gameObject.SetActive(true);
        scene = SceneManager.GetActiveScene();
    }
    public void RestartLevel(){
        SceneManager.LoadScene(scene.buildIndex);
        Debug.Log("yeniden başladı");
    }
}
