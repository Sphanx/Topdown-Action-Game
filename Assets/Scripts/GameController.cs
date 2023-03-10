using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameOverController gameOverController;
    public playerController playerController;

    void Update()
    {
        if(playerController.isPlayerDead){
            gameOverController.GameOver();
        }
    }
}
