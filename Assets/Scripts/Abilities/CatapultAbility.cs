using UnityEngine;
using System.Collections;

public class CatapultAbility : MonoBehaviour
{
    public GameObject catapultStonePrefab;
    public Vector3 launchOffset;
    public float launchForce;
    public float launchAngle;
    public float launchDelay;

    public void OnAttack(GameObject target)
    {
        StartCoroutine(Launch(target));
    }

    private IEnumerator Launch(GameObject target)
    {
        yield return new WaitForSeconds(launchDelay);

        var stone = Instantiate(
                catapultStonePrefab,
                transform.position + launchOffset,
                Quaternion.identity) as GameObject;

        var launchVector = transform.TransformDirection(Vector3.forward);
        launchVector = Quaternion.Euler(0, 0, launchAngle) * launchVector;
        stone.SendMessage("SetSender", gameObject);
        stone.rigidbody.AddForce(launchVector * launchForce);
    }
}
