using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDarkPotion : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Rotate()
    {
        transform.RotateAround(transform.position, transform.up, Time.fixedDeltaTime * 90f);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
    }
}
