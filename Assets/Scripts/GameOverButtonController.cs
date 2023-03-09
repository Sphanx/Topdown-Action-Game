using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtonController : MonoBehaviour
{
    private Scene scene;
    private void Awake() {
        scene = SceneManager.GetActiveScene();
    }
    private void RestartLevel(){
        SceneManager.LoadScene(scene.buildIndex);
    }
}
