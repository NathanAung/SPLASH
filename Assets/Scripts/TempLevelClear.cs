using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLevelClear : MonoBehaviour
{
    public GameObject text;
    public bool textActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(!textActive){
            text.SetActive(true);
            textActive = true;
        }
    }
}
