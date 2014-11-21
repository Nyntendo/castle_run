using UnityEngine;
using System.Collections;

public class RaiseDeadEffect : MonoBehaviour
{
    public float displayTime;
    public float rotationSpeed;

    private float displayTimer = 0f;

    public void Update()
    {
        transform.Rotate(0f, Time.deltaTime * rotationSpeed, 0f);

        displayTimer += Time.deltaTime;

        if (displayTimer >= displayTime)
        {
            Destroy(gameObject);
        }
    }
}
