using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCircle : MonoBehaviour
{
    List<GameObject> gameObjects;
    public float buffAmount;

    private void Awake()
    {
        gameObjects = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerClone" || other.tag == "Player")
        {
            if (other.gameObject.GetComponent<CharacterSkillSet>() != null)
            {
                other.gameObject.GetComponent<CharacterSkillSet>().damageModifier += buffAmount;
            }
            gameObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!gameObjects.Contains(other.gameObject) && other.tag == "PlayerClone" || other.tag == "Player")
        {
            if (other.gameObject.GetComponent<CharacterSkillSet>() != null)
            {
                other.gameObject.GetComponent<CharacterSkillSet>().damageModifier += buffAmount;
            }
            gameObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(gameObjects.Contains(other.gameObject))
        {
            other.gameObject.GetComponent<CharacterSkillSet>().damageModifier -= buffAmount;
            gameObjects.Remove(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerClone" || collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<CharacterSkillSet>() != null)
            {
                collision.gameObject.GetComponent<CharacterSkillSet>().damageModifier += buffAmount;
            }
            gameObjects.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (gameObjects.Contains(collision.gameObject))
        {
            collision.gameObject.GetComponent<CharacterSkillSet>().damageModifier -= buffAmount;
            gameObjects.Remove(collision.gameObject);
        }
    }

    private void OnDisable()
    {
        foreach(GameObject obj in gameObjects)
        {
            obj.GetComponent<CharacterSkillSet>().damageModifier -= buffAmount;
        }
        gameObjects.Clear();
    }
}
