using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject GameOverScreen;
    private bool gameOver;
    public Health phealth;


    IEnumerator MyFunction()
        {
            yield return new WaitForSeconds(1);
            Time.timeScale = 0;
            GameOverScreen.SetActive(true);
            gameOver = true;
        }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (phealth.health <=0)
        {
            StartCoroutine("MyFunction");
        }
        else
        {
            Time.timeScale = 1;
            GameOverScreen.SetActive(false);
            gameOver = false;

        }

    
    }
}
