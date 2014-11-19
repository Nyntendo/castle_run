using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float leftStop;
    public float rightStop;
    public float scrollSpeed;
    public float scrollZoneWidth;
    public float swipeScrollSpeed;
    public float swipeScrollFriction;

    private Vector3 swipeScrollVelocity = Vector3.zero;


    void Update () {
        if (Input.mousePosition.x < scrollZoneWidth && transform.position.x > leftStop)
        {
            transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x > (Screen.width - scrollZoneWidth) && transform.position.x < rightStop)
        {
            transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            var delta = Input.GetTouch(0).deltaPosition;

            if (delta.x < 0)
            {
                swipeScrollVelocity = Vector3.left * swipeScrollSpeed * delta.x;
            }

            if (delta.x > 0)
            {
                swipeScrollVelocity = Vector3.left * swipeScrollSpeed * delta.x;
            }
        }

        if (Vector3.Magnitude(swipeScrollVelocity) > 0f)
        {
            if (swipeScrollVelocity.x > 0f &&transform.position.x < rightStop)
                transform.position += swipeScrollVelocity;

            if (swipeScrollVelocity.x < 0f && transform.position.x > leftStop)
                transform.position += swipeScrollVelocity;

            swipeScrollVelocity *= swipeScrollFriction;

            if (Vector3.Magnitude(swipeScrollVelocity) < 0.1f)
            {
                swipeScrollVelocity = Vector3.zero;
            }
        }
    }
}
