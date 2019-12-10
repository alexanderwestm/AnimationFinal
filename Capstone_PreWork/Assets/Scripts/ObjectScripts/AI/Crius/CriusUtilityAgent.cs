using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CriusUtilityAgent : MonoBehaviour
{
    GameObject head;
    [SerializeField] float minTimeBetweenActions = 1f;
    float timeSinceLastAction;
    BasicEnemy healthScript;
    PlayerHealth[] players;
    EventManager eventManager;
    CriusState state;
    GameObject target;
    Vector3 potentialTarget;
    Vector3[] targets;
    int lastChoiceIndex;
    public bool isActive = false;
    public enum Action
    {
        INVALID_ACTION = -1,
        SWING_LOW,
        SWING_HIGH,
        SLAM,
        FROST_BREATH,
        ICICLE_THROW,
        NUM_ACTIONS
    }

    void Awake()
    {
        TryGetComponent(out state);
        TryGetComponent(out healthScript);
        head = GameObject.Find("crius:skel_head");
        eventManager = EventManager.GetInstance();
        players = FindObjectsOfType<PlayerHealth>();
        targets = new Vector3[(int)Action.NUM_ACTIONS];
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      
        players = FindObjectsOfType<PlayerHealth>();
        if (!target)
        {
            target = players[0].gameObject;
        }

        if (state.GetSuperState() != CharacterState.CharacterStates.ATTACKING)
        {
            Quaternion q = Quaternion.LookRotation(targets[lastChoiceIndex] - transform.position, Vector3.up);
            q = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * 3);
            transform.rotation = new Quaternion(0, q.y, 0, q.w);

            timeSinceLastAction += Time.deltaTime;
            if (isActive)
            {
                RequestDecision();
            }
        }
    }

    void RequestDecision()
    {
        {
            Action decision = MakeDecision();
            char action;
            if (timeSinceLastAction >= minTimeBetweenActions)
            {
                switch (decision)
                {
                    case Action.SWING_LOW:
                    {
                        action = 'L';
                        break;
                    }
                    case Action.SWING_HIGH:
                    {
                        action = 'H';
                        break;
                    }
                    case Action.SLAM:
                    {
                        action = 'S';
                        break;
                    }
                    case Action.ICICLE_THROW:
                    {
                        action = 'I';
                        break;
                    }
                    case Action.FROST_BREATH:
                    {
                        action = 'F';
                        break;
                    }
                    default:
                    {
                        action = ' ';
                        break;
                    }
                }
                CriusEvent newEvent = new CriusEvent(0, action);
                eventManager.QueueEvent(newEvent);
                timeSinceLastAction = 0;
            }
        }
    }

    Action MakeDecision()
    {
        float[] scores = new float[(int)Action.NUM_ACTIONS];

        scores[0] = TestEachQuarterCircle(25, 37.5f) / players.Length;
        targets[0] = potentialTarget;

        scores[1] = GetPlayersOnStairs(3f)*1.01f / players.Length;
        targets[1] = transform.position - Vector3.forward;

        scores[2] = TestEachQuarterCircle(0, 25) / players.Length;
        targets[2] = potentialTarget;

        scores[3] = TestEachQuarterCircle(37.5f, 100);
        targets[3] = potentialTarget;
    

        int highest = 0;

        for (int i = 0; i < scores.Length; ++i)
        {
            if (scores[i] > scores[highest])
            {
                highest = i;
            }
            else if (scores[i] == scores[highest] && scores[highest] != 0)
            {
                float rand = Random.Range(0, 100);
                if (rand > 50)
                {
                    highest = i;
                }
            }
        }
        lastChoiceIndex = highest;
        
        return (Action)highest;
    }


    int TestConeOnPlayer(float angle)
    {
        int max = 0;
        int maxIndex = 0;
        for (int i = 0; i < players.Length; ++i)
        {
            int current = TestCone(head.transform.position, players[i].transform.position, 45);
            if (current > max)
            {
                max = current;
                maxIndex = i;
            }
        }
        potentialTarget = players[maxIndex].transform.position;
        return max;
    }

    int TestCone(Vector3 origin, Vector3 target, float angle)
    {
        int totalCount = 0;
        
        Vector3 direction = target - origin;
        float dist = direction.magnitude;
        direction.Normalize();
        //O = A * tan(angle)
        float radius = dist * Mathf.Tan(angle / 57.296f);

        //approximate our cone as a sphere

        //sphere 1
        List<Collider> cols = new List<Collider>();
        cols.AddRange(Physics.OverlapSphere(origin + direction * (dist), radius, LayerMask.GetMask("Player", "Clone")));
        totalCount += cols.Count;

        return totalCount;
    }

    int GetNumPlayersInQuarterCircle(float innerRadius, float outerRadius, Vector2 axis)
    {
        Vector3 avgPos = Vector3.zero;
        int count = 0;
        foreach (PlayerHealth p in players)
        {

            if (Mathf.Sign(axis.x) == Mathf.Sign(p.transform.position.x) && Mathf.Sign(axis.y) == Mathf.Sign(p.transform.position.z))
            {
                float sqRad = (p.transform.position - transform.position).sqrMagnitude;

                if (sqRad > innerRadius * innerRadius && sqRad < outerRadius * outerRadius)
                {
                    count++;
                    avgPos += p.transform.position;
                }
            }
        }

        if (count > 0)
        {
            potentialTarget = avgPos / count;
        }
        return count;
    }

    int GetPlayersOnStairs(float height)
    {
        int count = 0;
        foreach (PlayerHealth p in players)
        {
            if (Physics.Raycast(p.transform.position, Vector3.down, out RaycastHit hit, 10) && hit.collider.gameObject.name == "Stairs")
            {
                count++;
            }
        }

        return count;
    }

    GameObject GetLowestHealthPlayer()
    {
        float lowest = 1000;
        int lowestIndex = 0;
        for (int i = 0; i < players.Length; ++i)
        {
            if (players[i].currentHealth < lowest)
            {
                lowest = players[i].currentHealth;
                lowestIndex = i;
            }
        }
        return players[lowestIndex].gameObject;
    }

    int TestEachQuarterCircle(float innerRadius, float outerRadius)
    {
        Vector2 XZ = new Vector2(1, 1);
        Vector2 minusXZ = new Vector2(-1, 1);
        Vector2 XminusZ = new Vector2(1, -1);
        Vector2 minusXminusZ = new Vector2(-1, -1);

        int max = 0;
        int a = GetNumPlayersInQuarterCircle(innerRadius, outerRadius, XZ);
        Vector3 xzTarget = potentialTarget;

        int b = GetNumPlayersInQuarterCircle(innerRadius, outerRadius, minusXZ);
        Vector3 minusxzTarget = potentialTarget;

        int c = GetNumPlayersInQuarterCircle(innerRadius, outerRadius, XminusZ);
        Vector3 xminuszTarget = potentialTarget;

        int d = GetNumPlayersInQuarterCircle(innerRadius, outerRadius, minusXminusZ);
        Vector3 minusxminuszTarget = potentialTarget;

        if (a > max)
        {
            max = a;
            potentialTarget = xzTarget;
        }
        if (b > max)
        {
            max = b;
            potentialTarget = minusxzTarget;
        }
        if (c > max)
        {
            max = c;
            potentialTarget = xminuszTarget;
        }
        if (d > max)
        {
            max = d;
            potentialTarget = minusxminuszTarget;
        }

        return max;

    }

}
