using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterInit : MonoBehaviour
{
    [SerializeField] List<string> playerUniqueScripts;
    [SerializeField] List<string> playerObjectsToLoad;
    [SerializeField] List<string> cloneUniqueScripts;
    [SerializeField] List<string> cloneObjectsToLoad;

    private void Awake()
    {
        LoadData();
    }

    private void Update()
    {
        LoadData();
    }

    public void LoadData()
    {
        Type type;
        if (gameObject.tag == "Player" || gameObject.tag == "Enemy")
        {
            for (int i = 0; i < playerUniqueScripts.Count; ++i)
            {
                type = Type.GetType(playerUniqueScripts[i]);
                if (!gameObject.GetComponent(type))
                {
                    gameObject.AddComponent(type);
                    // store a serialized version of the data in the script itself
                }
            }

            foreach (string objectName in playerObjectsToLoad)
            {
                foreach (Transform child in transform)
                {
                    if (child.name == objectName)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }

            // set to player layer
            gameObject.layer = 9;
            Destroy(this);
        }
        else if (gameObject.tag == "PlayerClone" || gameObject.tag == "EnemyClone")
        {
            for (int i = 0; i < cloneUniqueScripts.Count; ++i)
            {
                type = Type.GetType(cloneUniqueScripts[i]);
                if (!gameObject.GetComponent(type))
                {
                    gameObject.AddComponent(type);
                    // store a serialized version of the data in the script itself
                }
            }

            foreach (string objectName in cloneObjectsToLoad)
            {
                foreach (Transform child in transform)
                {
                    if (child.name == objectName)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }
            SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            Material ghostMat = (Material)Resources.Load("Materials/SpookyGhost");
            foreach(SkinnedMeshRenderer renderer in renderers)
            {
                renderer.material = ghostMat;
            }
            gameObject.layer = 10;
            Destroy(this);
        }
    }
}
