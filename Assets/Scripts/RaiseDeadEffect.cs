using UnityEngine;
using System.Collections;

public class RaiseDeadEffect : MonoBehaviour
{
    public float displayTime;
    public float rotationSpeed;

    private float displayTimer = 0f;
    private GameController gameController;

    public void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    public void Update()
    {
        if (gameController.gameState != GameState.Playing)
            return;

        transform.Rotate(0f, Time.deltaTime * rotationSpeed, 0f);

        displayTimer += Time.deltaTime;

        if (displayTimer >= displayTime)
        {
            Destroy(gameObject);
        }
    }
}
