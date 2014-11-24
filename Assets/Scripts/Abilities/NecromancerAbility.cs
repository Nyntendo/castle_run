using UnityEngine;
using System.Collections;

public class NecromancerAbility : MonoBehaviour
{
    public GameObject skeletonPrefab;
    public float cooldown;
    public float range;
    public float radius;
    public string raiseDeadAnimation;
    public float tryAgainDelay;
    public GameObject effectPrefab;

    private float cooldownTimer = 0f;
    private float tryAgainTimer = 0f;
    private Animation _animation;
    private UnitController unitController;
    private Attackable attackable;
    private GameController gameController;

    public void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        unitController = GetComponent<UnitController>();
        attackable = GetComponent<Attackable>();
        _animation = GetComponentInChildren<Animation>();
        _animation[raiseDeadAnimation].wrapMode = WrapMode.Once;
        _animation[raiseDeadAnimation].layer = 1;
    }

    public void Update()
    {
        if (gameController.gameState != GameState.Playing)
            return;

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (tryAgainTimer > 0f)
        {
            tryAgainTimer -= Time.deltaTime;
        }

        if (cooldownTimer <= 0f && tryAgainTimer <= 0f)
        {
            var colliders = Physics.OverlapSphere(transform.position, range);

            Transform target = null;
            int i = 0;
            while (i < colliders.Length)
            {
                if (colliders[i].tag == "Corpse")
                {
                    target = colliders[i].transform;
                    break;
                }
                i++;
            }

            if (target != null)
                CastAt(target);

            tryAgainTimer = tryAgainDelay;
        }
    }

    private void CastAt(Transform target)
    {
        _animation.CrossFade(raiseDeadAnimation);
        Instantiate(effectPrefab, target.position, Quaternion.identity);

        var colliders = Physics.OverlapSphere(target.position, radius);
        int i = 0;
        while (i < colliders.Length)
        {
            if (colliders[i].tag == "Corpse")
            {
                SpawnSkeletonAt(colliders[i].transform.position);
                Destroy(colliders[i].gameObject);
            }
            i++;
        }
        cooldownTimer = cooldown;
    }

    private void SpawnSkeletonAt(Vector3 position)
    {
        var skeleton = Instantiate(skeletonPrefab, position, Quaternion.identity) as GameObject;
        skeleton.SendMessage("SetTeam", attackable.team);
    }
}
