using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Open : MonoBehaviour
{
    public bool touching; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Spearman")
        {
            SpriteRenderer icon = GameObject.Find("Press_A").GetComponent<SpriteRenderer>();
            //Debug.Log(icon); 
            icon.enabled = true;

            touching = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Spearman")
        {
            SpriteRenderer icon = GameObject.Find("Press_A").GetComponent<SpriteRenderer>();
            icon.enabled = false;

            CloseMenu(); 

            touching = false;
        }
    }

    public void OpenMenu()
    {
        GameObject menu = GameObject.Find("Canvas_Menu");
        Transform menu_holder = menu.transform.GetChild(0);
        menu_holder.gameObject.SetActive(true); 
    }

    public void CloseMenu()
    {
        GameObject menu = GameObject.Find("Canvas_Menu");
        Transform menu_holder = menu.transform.GetChild(0);
        menu_holder.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterState>().SetState(CharacterState.CharacterStates.IDLE);
    }
}
