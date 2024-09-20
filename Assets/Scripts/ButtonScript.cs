using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameObject door;
    public GameObject player;
    public Animator buttonAnimator;
    public Animator doorAnimator;
    public bool isDoor = true;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject door = GetComponent<GameObject>();
        GameObject player = GetComponent<GameObject>();
        
        Animator buttonAnimation = GetComponent<Animator>();
        Animator doorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (player.layer == LayerMask.NameToLayer("Solid")) 
        {
            buttonAnimator.SetTrigger("pressed");
            if (isDoor) {
                doorAnimator.SetTrigger("pressed");
            }
            else door.SetActive(false);
        }
    }
}
