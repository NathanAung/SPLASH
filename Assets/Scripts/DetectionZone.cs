using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent NoCollidersRemain;
    Collider2D col;
    public List<Collider2D> detectedCol = new List<Collider2D>();
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        detectedCol.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision){
        detectedCol.Remove(collision);
        
        // invoke event for cliff detection zone
        if(detectedCol.Count <= 0){
            NoCollidersRemain.Invoke();
        }
    }
}
