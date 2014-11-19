using UnityEngine;
using System.Collections;

public class ZombieAbility : MonoBehaviour
{
    public float infectChance;
    public float infectDelay;
    public GameObject zombiePrefab;

    private UnitController unitController;
    private Attackable attackable;

    public void Start()
    {
        unitController = GetComponent<UnitController>();
        attackable = GetComponent<Attackable>();
    }

    public void OnAttack(GameObject target)
    {
        if (target.tag == "Unit" && Random.value < infectChance)
        {
            StartCoroutine(Infect(target));
        }
    }

    private IEnumerator Infect(GameObject target)
    {
        yield return new WaitForSeconds(infectDelay);

        if (target == null)
            yield break;

        var pos =  target.transform.position;
        var rot = target.transform.rotation;
        Destroy(target);
        var zombie = Instantiate(zombiePrefab, pos, rot) as GameObject;
        zombie.SendMessage("SetTarget", unitController.target);
        zombie.SendMessage("SetTeam", attackable.team);
    }
}
