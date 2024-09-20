using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    public UIScreenFade screenFade;
    Animator animator;

    private void Awake(){
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CloseOutro(){
        screenFade.FadeToScene(0);
    }

    public void CloseIntro(){
        screenFade.FadeOut();
    }
}
