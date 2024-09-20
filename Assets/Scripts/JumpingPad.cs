using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPad : MonoBehaviour
{
    // Start is called before the first frame update
    public float jumpForce = 30f;
    Vector2 jumpVector;
    void Start()
    {
        jumpVector = new Vector2(0f, jumpForce);
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            collision.attachedRigidbody.AddForce(100 * jumpVector * Time.deltaTime);
        }
    }
}
