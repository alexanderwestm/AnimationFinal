using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayback : MonoBehaviour
{
    CommandLogger logger;
    List<Command> commands;
    bool solving;
    int currentCommandIndex = 0;
    public bool isSpawned = false;
    public GameObject objectClonedFrom;

    private void Update()
    {
        if (logger == null)
        {
            logger = GetComponent<CommandLogger>();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F1) || isSpawned)
            {
                //logger.SetLoggerInformation(gameObject, objectClonedFrom.GetComponent<CommandLogger>());
                isSpawned = false;
            }
        }
    }
}
