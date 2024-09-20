using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    UIScreenFade screenFade;

    private void Awake(){
        screenFade = GameObject.FindGameObjectWithTag("ScreenFade").GetComponent<UIScreenFade>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        screenFade.FadeToScene(SceneManager.GetActiveScene().buildIndex);
    }
}