using UnityEngine;
using System.Collections;
using Pathfinding;

public class UnitController : MonoBehaviour
{
    public float waypointReachedThreshold;
    public float speed;
    public Transform target;

    public float agroRange;
    public float attackRange;
    public float keepTargetRange;
    public int attackDamage;
    public float attackSpeed;

    private Path path;
    private Seeker seeker;
    private CharacterController controller;
    private Attackable attackable;
    private int currentWaypoint = 0;
    private bool calculatingPath = false;
    private float attackTimer;
    private GameObject targetObject;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
        attackable = GetComponent<Attackable>();
    }

    public void Update()
    {
        if (path == null && target != null && !calculatingPath)
        {
            calculatingPath = true;
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    public void FixedUpdate()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.fixedDeltaTime;
        }

        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            path = null;
            return;
        }

        if (targetObject != null)
        {
            var distance = Vector3.Distance(transform.position, targetObject.transform.position);

            if (distance > keepTargetRange)
            {
                targetObject = null;
            }
        }

        if (targetObject == null)
        {
            LookForTrouble();
        }

        if (targetObject != null)
        {
            var distance = Vector3.Distance(transform.position, targetObject.transform.position);
            var hitRange = targetObject.GetComponent<Attackable>().hitRange;

            if (distance <= attackRange + hitRange)
            {
                Attack(targetObject);
                return;
            }

            var targetDir = (targetObject.transform.position - transform.position).normalized;
            controller.SimpleMove(targetDir * speed * Time.fixedDeltaTime);
            return;
        }

        var dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        controller.SimpleMove(dir * speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < waypointReachedThreshold)
        {
            currentWaypoint++;
        }
    }

    public void OnPathComplete(Path path)
    {
        Debug.Log("Path complete");
        this.currentWaypoint = 0;
        this.path = path;
        this.calculatingPath = false;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void LookForTrouble()
    {
        var colliders = Physics.OverlapSphere(transform.position, agroRange);

        GameObject newTarget = null;
        float targetDistance = 1000f;

        int i = 0;
        while (i < colliders.Length)
        {
            var attackable = colliders[i].GetComponent<Attackable>();

            if (attackable != null && attackable.team != this.attackable.team)
            {
                var distance = Vector3.Distance(transform.position, colliders[i].transform.position);

                if (distance < targetDistance)
                {
                    newTarget = colliders[i].gameObject;
                    targetDistance = distance;
                }
            }

            i++;
        }

        targetObject = newTarget;
    }

    private void Attack(GameObject target)
    {
        if (attackTimer > 0f)
            return;

        target.SendMessage("Hit", attackDamage);
        attackTimer = attackSpeed;
    }
}
