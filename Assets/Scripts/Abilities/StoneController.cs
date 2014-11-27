using UnityEngine;
using System.Collections;

public class StoneController : MonoBehaviour
{
    public float radius;
    public int damage;
    public float activateTime;
    public float destroyTime;
    public GameObject impactEffect;

    private bool active = false;
    private GameObject sender;
    private Team team;

    public void Start()
    {
        collider.isTrigger = true;
        StartCoroutine(Activate());
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!active || collision.gameObject == sender)
            return;

        var colliders = Physics.OverlapSphere(transform.position, radius);

        int i = 0;
        while (i < colliders.Length)
        {
            if (colliders[i].tag == "Unit")
            {
                if (colliders[i].gameObject.GetComponent<Attackable>().team != team)
                    colliders[i].gameObject.SendMessage("Hit", damage);
            }

            i++;
        }

        var effect = Instantiate(impactEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(effect, destroyTime);
        active = false;
        Destroy(gameObject, destroyTime);
    }

    public void SetSender(GameObject sender)
    {
        this.sender = sender;
        this.team = sender.GetComponent<Attackable>().team;
    }

    private IEnumerator Activate()
    {
        yield return new WaitForSeconds(activateTime);
        active = true;
        collider.isTrigger = false;
    }
}
