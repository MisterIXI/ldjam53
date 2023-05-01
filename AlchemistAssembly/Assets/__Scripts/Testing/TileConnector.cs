using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileConnector : MonoBehaviour
{
    [SerializeField] private GameObject startTile;
    [SerializeField] private GameObject endTile;
    [SerializeField]private Transform curved;
    [SerializeField]private Transform straight;
    // Start is called before the first frame update
    void Start()
    {
        if (startTile != null && endTile != null)
        {
                Vector2 direction;
                direction = transform.position - startTile.transform.position;
                Debug.Log(direction);
                if(direction.x < 0)
                {
                    Debug.Log("X < change and Track Curved - 60");
                    switchTracks();
                    transform.rotation = Quaternion.identity;
                    transform.RotateAround(endTile.transform.position, Vector3.up, -60);
                }else{
                    Debug.Log("X < change and Track Curved + 60");
                    switchTracks();
                    transform.rotation = Quaternion.identity;
                    transform.RotateAround(endTile.transform.position, Vector3.up, 60);
                }
        }
        else if (startTile != null && endTile == null)
        {

        }
        else
        {

        }
    }
    private void switchTracks()
    {
        curved.gameObject.SetActive(true);
        straight.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
