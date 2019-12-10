using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWall : MonoBehaviour
{
    public bool isInArena;
    public GameObject closed;
    public GameObject door;



 

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isInArena)
        {
            isInArena = true;
            PlayerCloneManager.Instance.spawnPositions.Add(other.transform.position);
            PlayerCloneManager.Instance.spawnRotations.Add(other.transform.rotation);
            PlayerCloneManager.Instance.spawnWeapons.Add(GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSkillSet>().currentWeaponNum);
            StartPlayer();
            StartBoss();
            PlayerCloneManager.Instance.StartClones();
            GameTimer.GlobalTimer.ResetTimer();
            enabled = false;

            door.transform.position = closed.transform.position; 
        }
       
    }

    public void StartPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        playerObj.GetComponent<CommandLogger>().RestartData(true);
        playerObj.GetComponent<SpearmanAttack>().ResetCooldown();
    }

    public void StartBoss()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        boss.GetComponent<CommandLogger>().StartPlayback();
        boss.GetComponent<CommandLogger>().RestartData(false);
    }
}
