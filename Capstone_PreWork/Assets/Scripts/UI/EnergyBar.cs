using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    int tickCount;
    float maxCharge;
    List<Transform> children;
    ActiveAugment targetAugment;
    [SerializeField] string targetTag;

    float lastCharge;

    private void Start()
    {
        maxCharge = 0;
        children = new List<Transform>();
        tickCount = transform.childCount;
        for (int i = 0; i < tickCount; ++i)
        {
            children.Add(transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetAugment == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                targetAugment = GameObject.FindGameObjectWithTag("Player").GetComponent<ActiveAugment>();
                maxCharge = targetAugment.maxCharge;
            }
        }
        else
        {
            if (maxCharge != 0 && lastCharge != targetAugment.currentCharge)
            {
                float percent = 1 - targetAugment.currentCharge / maxCharge;
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
            lastCharge = targetAugment.currentCharge;
        }
    }

    public void SetMaxCharge(float max)
    {
        maxCharge = max;
    }
}
