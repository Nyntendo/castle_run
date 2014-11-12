using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float leftStop;
    public float rightStop;
    public float scrollSpeed;
    public float scrollZoneWidth;


    void Update () {
        if (Input.mousePosition.x < scrollZoneWidth && transform.position.x > leftStop)
        {
            transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x > (Screen.width - scrollZoneWidth) && transform.position.x < rightStop)
        {
            transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
        }
    }
}
