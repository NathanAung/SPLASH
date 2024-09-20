using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditExit : MonoBehaviour
{
    float test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        test = Input.GetAxis("Cancel");
        if (test != 0)
        {
            SceneManager.LoadScene("Title");
        }
    }
}
