using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


struct DecisionNode
{
    public DecisionNode(bool isDefault = true)
    {
        toTake = playerAIAction.INVALID_ACTION;
        getDecision = null;
        children = new DecisionNode[2];
    }
    public playerAIAction toTake;
    public delegate bool Decide();
    public Decide getDecision;
    public DecisionNode[] children;
}
public class PlayerDecisionTree : MonoBehaviour
{
    [SerializeField] float meleedDistanceSquared = 9;
    [SerializeField] float playerNeighborhoodRadiusSquared = 100;
    [SerializeField] float hitboxDodgeRadiusSquared = 9;

    PlayerAgent holder;
    SpearmanAttack skillHolder;
    public GameObject target { get; protected set; }
    CharacterState targetStateHolder;
    Rigidbody rb;
    PlayerInputInterpreter interpreter;
    PlayerHealth health;
    DecisionNode root;

    PlayerHealth[] players;
    List<GameObject> currentNeighborhood;

    public Vector3 repositionTarget { get; protected set; }
    private void Awake()
    {
        TryGetComponent(out holder);
        TryGetComponent(out skillHolder);
        TryGetComponent(out health);
        players = FindObjectsOfType<PlayerHealth>();
        target = FindObjectOfType<BossInputInterpreter>().gameObject;

        if (!target)
        {
            Debug.LogError("No boss found!");
        }
        else
        {
            target.TryGetComponent(out targetStateHolder);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        //leaf nodes that only hold a decision
        DecisionNode resultNodeBow = new DecisionNode(true);
        resultNodeBow.toTake = playerAIAction.BOW;


        DecisionNode resultNodeStaff = new DecisionNode(true);
        resultNodeStaff.toTake = playerAIAction.MELEE;


        DecisionNode resultNodeDodge = new DecisionNode(true);
        resultNodeDodge.toTake = playerAIAction.DODGE;

        DecisionNode resultNodeRepos = new DecisionNode(true);
        resultNodeRepos.toTake = playerAIAction.REPOSITION;

        DecisionNode resultNodeSpecial = new DecisionNode(true);
        resultNodeSpecial.toTake = playerAIAction.SPECIAL;


        root = new DecisionNode(true);
        root.getDecision = IsAboutToBeHit;

        DecisionNode allyNear = new DecisionNode(true);
        allyNear.getDecision = IsAllyPlayerNear;

        DecisionNode isLowHealth = new DecisionNode(true);
        isLowHealth.getDecision = IsLowHealth;

        DecisionNode isRepos = new DecisionNode(true);
        isRepos.getDecision = IsRepositioning;

        DecisionNode inMelee = new DecisionNode(true);
        inMelee.getDecision = IsInMeleeRange;

        DecisionNode allyIsHealing = new DecisionNode(true);
        allyIsHealing.getDecision = NearbyPlayerHasHealingPool;

        DecisionNode canHeal = new DecisionNode(true);
        canHeal.getDecision = CanIHeal;

        DecisionNode isGettingHealed = new DecisionNode(true);
        isGettingHealed.getDecision = IsBeingHealed;

        root.children[0] = resultNodeDodge;
        root.children[1] = allyNear;

        allyNear.children[0] = isLowHealth;
        allyNear.children[1] = inMelee;


        isLowHealth.children[0] = allyIsHealing;
        isLowHealth.children[1] = isRepos;

        allyIsHealing.children[0] = isGettingHealed;
        allyIsHealing.children[1] = isRepos;

        isGettingHealed.children[0] = resultNodeBow;
        isGettingHealed.children[1] = isRepos;

        isRepos.children[0] = resultNodeBow;
        isRepos.children[1] = resultNodeRepos;

        inMelee.children[0] = resultNodeStaff;
        inMelee.children[1] = resultNodeBow;



        //TODO: no ally nearby logic
    }

    // Update is called once per frame
    void Update()
    {
        if (!health)
        {
            TryGetComponent(out health);
        }
    }

    public GameObject GetTargetHitbox()
    {
        return targetStateHolder.currentHitbox;
    }

    public playerAIAction Decide()
    {
        return FollowTree(root);
    }
    playerAIAction FollowTree(DecisionNode node)
    {
        if (node.toTake == playerAIAction.INVALID_ACTION)
        {
            if (node.getDecision())
            {
                return FollowTree(node.children[0]);
            }
            else
            {
                return FollowTree(node.children[1]);
            }
        }
        else
        {
            return node.toTake;
        }
    }


    bool IsAboutToBeHit()
    {
        if (EnemyIsAttacking() && targetStateHolder.currentHitbox != null)
        {
            if (GetDistanceSquared(targetStateHolder.currentHitbox) < hitboxDodgeRadiusSquared)
            {
                return true;
            }
        }
        return false;
    }

    bool EnemyIsAttacking()
    {
        CriusState enemyState = (CriusState)targetStateHolder;
        return (enemyState.GetState() != CriusState.CriusStates.IDLE);
    }

    float GetDistanceSquared(GameObject to)
    {
        return (new Vector2(transform.position.x, transform.position.z) - new Vector2(to.transform.position.x, to.transform.position.z)).sqrMagnitude;
    }

    bool EnemyHitboxIsNear()
    {
        return false;
    }

    bool IsAllyPlayerNear()
    {
        Debug.Log("CHECKING FOR ALLY PLAYERS");
        currentNeighborhood = new List<GameObject>();
        bool found = false;
        Debug.Log(players.Length);
        foreach (PlayerHealth p in players)
        {
            if (p.gameObject != gameObject)
            {
                if ((new Vector2(p.transform.position.x, p.transform.position.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude < playerNeighborhoodRadiusSquared)
                {
                    
                    found = true;
                    currentNeighborhood.Add(p.gameObject);
                    if (!holder.IsRepositioning())
                    {
                        repositionTarget = GetRandomPositionOnMesh();
                        Debug.Log("ALLY IS NEAR< REPOSITIONING");
                    }
                }
            }
        }

        return found;
    }

    bool IsInMeleeRange()
    {
        return (GetDistanceSquared(target) < meleedDistanceSquared);
    }

    bool IsRepositioning()
    {
        return holder.IsRepositioning();
    }

    bool IsRangedChargedReady()
    {
        return skillHolder.CheckChargedCooldown(false);
    }

    bool IsMeleeChargedReady()
    {
        return skillHolder.CheckChargedCooldown(true);
    }

    bool IsLowHealth()
    {
        return health.currentHealth <= 25;
    }

    bool NearbyPlayerHasHealingPool()
    {
        foreach (GameObject g in currentNeighborhood)
        {
            //check if they have a pool down

            //if they do, travel to it
        }
        return false;
    }

    bool CanIHeal()
    {
        //check if I have a healing ability
        return false;
    }

    bool IsBeingHealed()
    {
        //check if we're in the active radius of a healing pool
        return false;
    }


    Vector3 GetRandomPositionOnMesh()
    {
        Vector3 pos = Random.onUnitSphere;
        pos *= Random.Range(20, 60);
        NavMesh.SamplePosition(pos, out NavMeshHit hit, 20, 1);
        if (hit.hit)
        {
            return hit.position;
        }
        else
        {
            return GetRandomPositionOnMesh();
        }
    }
}
