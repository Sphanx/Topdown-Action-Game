using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI score;
    // Start is called before the first frame update
    void Start()
    {
        score.SetText("0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int score){
        this.score.SetText(score.ToString());
    }
}
