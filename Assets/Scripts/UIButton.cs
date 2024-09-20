using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public Button button1;
    public string sceneName;
    //UIScreenFade screenFade;
    // Start is called before the first frame update
    void Start()
    {
        button1.onClick.AddListener(TaskOnClick);
        //screenFade = GameObject.FindGameObjectWithTag("ScreenFade").GetComponent<UIScreenFade>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TaskOnClick() 
    {
        SceneManager.LoadScene(sceneName);
    }
}
