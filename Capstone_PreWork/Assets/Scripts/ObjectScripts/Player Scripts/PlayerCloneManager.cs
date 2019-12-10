using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerCloneManager : MonoBehaviour
{
    private static PlayerCloneManager _instance;
    public static PlayerCloneManager Instance { get { return _instance; } private set { } }

    GameObject prefab;
    Vector3 spawnPosition;

    public List<Vector3> spawnPositions;
    public List<Quaternion> spawnRotations;
    public List<int> spawnWeapons;

    List<GameObject> clones;
    List<CommandLogger> cloneLoggers;

    public int cloneCount;
    private Text counterText; 

    // Start is called before the first frame update
    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }

      

        prefab = Resources.Load("Prefabs/Warden") as GameObject;

        clones = new List<GameObject>();
        cloneLoggers = new List<CommandLogger>();
    }

    private void Update()
    {
        if (counterText == null)
        {
            if (GameObject.Find("Ghost_Display_Text") != null)
            {
                counterText = GameObject.Find("Ghost_Display_Text").GetComponent<Text>();
            }
        }
   

    }

    public void SpawnClone(GameObject clonedFrom)
    {
        GameObject obj = Instantiate(prefab, spawnPositions[spawnPositions.Count - 1], spawnRotations[spawnRotations.Count - 1]);
        DisplayCloneCount(); 
        obj.tag = "PlayerClone";
        obj.GetComponent<CommandLogger>().SetLoggerInformation(clonedFrom.GetComponent<CommandLogger>());

        //obj.GetComponent<CharacterSkillSet>().currentWeaponNum = spawnWeaponNum;

        obj.GetComponent<PassiveAugment>().data = clonedFrom.GetComponent<PassiveAugment>().data;
        obj.GetComponent<ActiveAugment>().data = clonedFrom.GetComponent<ActiveAugment>().data;

        clones.Add(obj);
        cloneLoggers.Add(obj.GetComponent<CommandLogger>());
        ResetClones();
    }

    public void ResetClones()
    {
        foreach(CommandLogger logger in cloneLoggers)
        {
            logger.SetToStart();
            logger.CancelLastCommand();
        }

        for(int i = 0; i < clones.Count; ++i)
        {
            GameObject obj = clones[i];
            obj.GetComponent<Rigidbody>().isKinematic = true;
            obj.GetComponent<Rigidbody>().useGravity = false;
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            obj.transform.position = spawnPositions[i];
            obj.transform.rotation = spawnRotations[i];
            obj.GetComponent<CharacterSkillSet>().currentWeaponNum = spawnWeapons[i];

            //StartCoroutine(PausePlayback(obj));
        }
    }

    public void StartClones()
    {
        foreach(CommandLogger logger in cloneLoggers)
        {
            logger.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            logger.gameObject.GetComponent<Rigidbody>().useGravity = true;
            logger.StartPlayback();

            logger.gameObject.GetComponent<ActiveAugment>().ResetCharge();

            //logger.gameObject.GetComponent<Animator>().enabled = true;
        }
    }

    public void DisplayCloneCount() 
    {
        cloneCount++;
        counterText.text = cloneCount.ToString(); 
    }

    // enough time for idle to start but have them wait to play until you go through boss wall
    IEnumerator PausePlayback(GameObject obj)
    {
        yield return new WaitForSecondsRealtime(1f);
        obj.GetComponent<Animator>().enabled = false;
    }
}
