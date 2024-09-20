using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScreenFade : MonoBehaviour
{
    Animator animator;
    public int sceneToSwitch = 0;
    public GameObject cutScene;
    public bool playOutro = false;
    public float outroTime = 8f;
    private float outroTimer = 0f;
    private bool outroTimeOut = false;
    public GameObject player;


    private void Awake(){
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    void Start(){
        animator.SetTrigger("fadeInLong");
        cutScene.GetComponent<Animator>().SetBool("playIntro", true);
    }

    void Update(){
        if(playOutro && !outroTimeOut){
            if(outroTimer <= outroTime){
                outroTimer += Time.deltaTime;
            }
            else{
                animator.SetTrigger("fadeOut");
                outroTimeOut = true;
            }
        }
    }


    public void FadeToScene(int sceneNo){
        sceneToSwitch = sceneNo;
        animator.SetTrigger("fadeOutLong");
    }

    public void FadeIn(){
        animator.SetTrigger("fadeIn");
    }

    public void FadeOut(){
        animator.SetTrigger("fadeOut");
    }

    public void PlayOutro(){
        playOutro = true;
    }

    public void OnFadeLComplete(){
        SceneManager.LoadScene(sceneToSwitch);
    }

    public void OnFadeComplete(){
        if(playOutro){
            cutScene.SetActive(true);
            cutScene.GetComponent<Animator>().SetBool("playOutro", true);
            player.SetActive(false);
        }
        else{
            cutScene.SetActive(false);
            player.SetActive(true);
        }
    }
}
