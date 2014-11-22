using UnityEngine;
using System.Collections;
using Pathfinding;


public class UnitController : MonoBehaviour
{
    public float waypointReachedThreshold;
    public float speed;
    public Transform target;
    public bool leavesCorpse = true;

    public float agroRange;
    public float attackRange;
    public float keepTargetRange;
    public int attackDamage;
    public float attackSpeed;
    public int coinValue;

    public string walkAnimation;
    public string attackAnimation;
    public string deathAnimation;

    public ParticleSystem attackParticleSystem;

    private Path path;
    private Seeker seeker;
    private CharacterController controller;
    private Attackable attackable;
    private int currentWaypoint = 0;
    private bool calculatingPath = false;
    private float attackTimer;
    private GameObject targetObject;
    private Animation _animation;
    private GameController gameController;
    private bool stunned = false;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
        attackable = GetComponent<Attackable>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        _animation = GetComponentInChildren<Animation>();
        _animation[walkAnimation].wrapMode = WrapMode.Loop;
        _animation[attackAnimation].wrapMode = WrapMode.Once;
        _animation[deathAnimation].wrapMode = WrapMode.ClampForever;

    }

    public void Update()
    {
        if (attackable.dead)
            return;

        if (path == null && target != null && !calculatingPath && targetObject == null)
        {
            calculatingPath = true;
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    public void FixedUpdate()
    {
        if (attackable.dead || stunned)
            return;

        if (attackTimer > 0f)
        {
            attackTimer -= Time.fixedDeltaTime;
        }

        if (targetObject != null)
        {
            var distance = Vector3.Distance(transform.position, targetObject.transform.position);

            if (distance > keepTargetRange || targetObject.GetComponent<Attackable>().dead)
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
            path = null;
            var distance = Vector3.Distance(transform.position, targetObject.transform.position);
            var hitRange = targetObject.GetComponent<Attackable>().hitRange;

            if (distance <= attackRange + hitRange)
            {
                transform.LookAt(targetObject.transform.position);
                Attack(targetObject);
                return;
            }

            var targetDir = (targetObject.transform.position - transform.position).normalized;

            MoveDir(targetDir);

            return;
        }

        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            path = null;
            return;
        }

        var dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        MoveDir(dir);

        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < waypointReachedThreshold)
        {
            currentWaypoint++;
        }
    }

    private void MoveDir(Vector3 dir)
    {
        transform.LookAt(transform.position + dir);
        controller.SimpleMove(dir * speed * Time.fixedDeltaTime);
    }

    public void OnPathComplete(Path path)
    {
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

            if (attackable != null &&
                !attackable.dead &&
                attackable.team != this.attackable.team &&
                (this.attackable.canAttack & attackable.unitType) > 0)
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

        if (attackParticleSystem != null)
            attackParticleSystem.Play();

        _animation.CrossFade(attackAnimation);
        _animation.CrossFadeQueued(walkAnimation);
        target.SendMessage("Hit", attackDamage);
        gameObject.SendMessage("OnAttack", target, SendMessageOptions.DontRequireReceiver);
        attackTimer = attackSpeed;
    }

    public void OnDeath()
    {
        if (leavesCorpse)
            gameController.PutCorpseAt(transform.position);

        gameController.AddCoinsToOtherTeam(attackable.team, coinValue);
        _animation.CrossFade(deathAnimation);
        controller.enabled = false;
        Destroy(gameObject, _animation[deathAnimation].length);
    }

    public void OnDestroy()
    {
        gameController.DecrementUnits(attackable.team);
    }

    public GameObject GetTargetObject()
    {
        return targetObject;
    }

    public void SetWalkAnimationSpeed(float speed)
    {
        _animation[walkAnimation].speed = speed;
    }

    public void Stun(float duration)
    {
        stunned = true;
        StartCoroutine(UnStun(duration));
    }

    private IEnumerator UnStun(float duration)
    {
        yield return new WaitForSeconds(duration);
        stunned = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Unit")
            return;
        Debug.Log("Collision!!!");
        var attackable = collision.gameObject.GetComponent<Attackable>();
        if ((this.attackable.unitType & attackable.unitType) == 0)
        {
            Physics.IgnoreCollision(collision.collider, controller);
        }
    }
}
