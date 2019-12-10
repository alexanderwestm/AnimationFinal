using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    int tickCount;
    float maxHealth = 0;
    List<Transform> children;
    Health targetHealth;
    [SerializeField] string targetTag;

    float lastHealth = -1;

    // Start is called before the first frame update
    void Start()
    {
        children = new List<Transform>();
        tickCount = transform.childCount;
        for (int i = 0; i < tickCount; ++i)
        {
            children.Add(transform.GetChild(i));
        }
    }

    void Awake()
    {
        if (GameObject.FindGameObjectWithTag(targetTag))
        {
            if (GameObject.FindGameObjectWithTag(targetTag).TryGetComponent(out targetHealth))
            {
                maxHealth = targetHealth.maxHealth;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetHealth == null)
        {
            if (GameObject.FindGameObjectWithTag(targetTag))
            {
                if (GameObject.FindGameObjectWithTag(targetTag).TryGetComponent(out targetHealth))
                {
                    maxHealth = targetHealth.maxHealth;
                }
            }
        }
        else
        {
            if(lastHealth != targetHealth.currentHealth)
            {
                float percent = 1 - targetHealth.currentHealth / maxHealth;
                int numActiveTicks = (int)(tickCount * percent);

                foreach (Transform t in children)
                {
                    t.gameObject.SetActive(true);
                }
                for (int i = 0; i < numActiveTicks; ++i)
                {
                    children[i].gameObject.SetActive(false);
                }
            }
            lastHealth = targetHealth.currentHealth;
        }
    }
}
