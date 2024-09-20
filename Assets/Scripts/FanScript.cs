using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanScript : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public GameObject player;
    public Vector2 wind;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D playerRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (player.layer == LayerMask.NameToLayer("Gas"))
        {
            playerRB.AddRelativeForce(wind*Time.deltaTime*10);
        }
    }
}
