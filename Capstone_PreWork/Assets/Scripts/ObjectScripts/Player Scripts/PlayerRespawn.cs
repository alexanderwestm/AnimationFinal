using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : EventListener
{
    [SerializeField] GameObject playerObj;
    [SerializeField] Transform spawnTransform;
    Vector3 spawnPosition;

    [SerializeField] float timeToSpawn = 1.0f;
    float timer;
    bool respawning = false;

    [SerializeField] GameObject blackScreenObj;
    List<GameObject> deadCloneList;
    private void Awake()
    {
        deadCloneList = new List<GameObject>();
        spawnPosition = spawnTransform == null ? Vector3.zero : spawnTransform.position;
        if(playerObj == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
        }
        EventManager.GetInstance().AddListener(this, EventType.DEATH_EVENT);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Comma))
        {
            playerObj.GetComponent<Animator>().SetTrigger("BecomeLit");
        }
        if (spawnTransform == null)
        {
            if (GameObject.Find("PlayerSpawnPoint") != null)
            {
                spawnTransform = GameObject.Find("PlayerSpawnPoint").transform;
                spawnPosition = spawnTransform.position;
            }
        }

        if(blackScreenObj == null)
        {
            blackScreenObj = GameObject.Find("BlackScreen");
        }

        if (playerObj == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
        }

        if (respawning)
        {
            timer += Time.deltaTime;
            if(timer > timeToSpawn)
            {
                GameObject.Find("door").transform.position = GameObject.Find("open_point").transform.position;
                blackScreenObj.SetActive(false);
                playerObj.GetComponent<Health>().isDead = false;
                respawning = false;

                foreach (GameObject g in deadCloneList)
                {
                    g.SetActive(true);
                    g.GetComponent<Health>().Reset();
                }
                deadCloneList.Clear();
            }
        }
    }

    public void ResetPlayer()
    {
        respawning = true;
        playerObj.transform.position = spawnPosition;
        playerObj.GetComponent<CommandLogger>().loggingTransforms = false;
        playerObj.GetComponent<CommandLogger>().CancelCommands();
        playerObj.GetComponent<SpearmanState>().SetState(CharacterState.CharacterStates.IDLE);
        playerObj.GetComponent<Health>().Reset();
        playerObj.GetComponent<ActiveAugment>().currentCharge = playerObj.GetComponent<ActiveAugment>().maxCharge;
        playerObj.GetComponent<Animator>().SetTrigger("Reset");
        playerObj.GetComponent<Animator>().SetBool("Dead", false);

        playerObj.transform.Find("AttackObjects").Find("healing_circle").gameObject.SetActive(false);
        playerObj.transform.Find("AttackObjects").Find("damage_buff_circle").gameObject.SetActive(false);
    }

    public void ResetBoss()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        boss.GetComponent<CommandLogger>().SetToStart();
        boss.GetComponent<Health>().Reset();
    }

    public override void HandleEvent(Event incomingEvent)
    {
        if (((DeathEvent)incomingEvent).deadObject == playerObj)
        {
            StartCoroutine(DelayDie(3));
            playerObj.GetComponent<SpearmanState>().SetState(CharacterState.CharacterStates.STUNNED);

        }
        else if (((DeathEvent)incomingEvent).deadObject.tag == "PlayerClone")
        {
            GameObject obj = ((DeathEvent)incomingEvent).deadObject;
            deadCloneList.Add(obj);
            obj.SetActive(false);
        }
        else if (((DeathEvent)incomingEvent).deadObject.tag == "Boss")
        {
            GameObject obj = ((DeathEvent)incomingEvent).deadObject;
            obj.GetComponent<CriusAttack>().enabled = false;
            obj.GetComponent<BasicEnemy>().enabled = false;
            obj.GetComponent<CriusState>().enabled = false;
            obj.GetComponent<CommandLogger>().enabled = false;
            obj.GetComponent<BossInputInterpreter>().enabled = false;
            obj.GetComponent<EnemyHitbox>().enabled = false;
            obj.GetComponent<CriusUtilityAgent>().enabled = false;
            StartCoroutine(DelayReset(5));
            
        }
    }

    public void RemoveProjectiles()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject obj in objs)
        {
            Destroy(obj);
        }
    }

    public void StopAllParticles()
    {
        ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();
        foreach(ParticleSystem ps in particleSystems)
        {
            if(ps.isPlaying)
            {
                ps.Stop();
                ps.time = 0;
            }
        }
    }
    
    IEnumerator DelayDie(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        blackScreenObj.SetActive(true);
        respawning = true;
        timer = 0;
        ResetPlayer();
        ResetBoss();
        RemoveProjectiles();
        StopAllParticles();

        FindObjectOfType<BossWall>().enabled = true;
        FindObjectOfType<BossWall>().isInArena = false;
        PlayerCloneManager.Instance.SpawnClone(playerObj);
    }

    IEnumerator DelayReset(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        GameObject.FindObjectOfType<SceneLoader>().Reset();
    }
}
