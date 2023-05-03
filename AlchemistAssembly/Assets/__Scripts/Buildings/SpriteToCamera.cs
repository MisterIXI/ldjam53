using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToCamera : MonoBehaviour
{
    private new Camera camera;
    // [SerializeField] public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Awake()
    {
        camera = Camera.main;
    }
    private void Update() {
        if(camera != null){
        transform.eulerAngles = camera.transform.eulerAngles;
        }
    }
    // Update is called once per frame
    
}
