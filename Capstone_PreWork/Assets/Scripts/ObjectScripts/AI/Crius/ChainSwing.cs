using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ChainSwing : Ability
{
    [SerializeField] float duration;
    [SerializeField] GameObject hitbox2;
    List<Collider> hitboxes;
    Collider hitboxCol;
    CriusState holderState;
    public ChainSwing(GameObject obj, float timeRan) : base(obj, timeRan)
    {
        hitboxes = new List<Collider>();
        RecursiveFind(hitBox);
        RecursiveFind(hitbox2);

        foreach (Collider c in hitboxes)
        {
            c.enabled = false;
        }

    }

    public ChainSwing(ChainSwing other) : base(other)
    {
        if (holderState == null)
        {
            holderState = holder.GetComponent<CriusState>();
        }
        duration = other.duration;
        damage = other.damage;
        hitboxes = other.hitboxes;
        holderState = other.holderState;
        hitbox2 = other.hitbox2;
        RecursiveFind(hitBox);
        RecursiveFind(hitbox2);
        foreach (Collider c in hitboxes)
        {
            c.enabled = false;
        }
    }

    public override void Execute()
    {
        base.Execute();
        action = holder.StartCoroutine(ChainSwingLow());
    }

    IEnumerator ChainSwingLow()
    {
        if (hitboxes == null)
        {
            hitboxes = new List<Collider>();
            RecursiveFind(hitBox);
            RecursiveFind(hitbox2);
        }
        holder.gameObject.GetComponent<Animator>().SetTrigger("SwingLow");


        hitbox2.GetComponent<EnemyHitbox>().Reset(damage);

        timeRan = GameTimer.GlobalTimer.time;
        yield return new WaitForSecondsRealtime(duration * 0.3f);

        if (holderState == null)
        {
            holderState = holder.GetComponent<CriusState>();
        }
        holderState.SetState(CriusState.CriusStates.CHAIN_SWING);

        foreach (Collider c in hitboxes)
        {
            c.enabled = true;
        }
        yield return new WaitForSecondsRealtime(duration * 0.7f);

        foreach (Collider c in hitboxes)
        {
            c.enabled = false;
        }
        timeRan = GameTimer.GlobalTimer.time;
        holderState.SetState(CriusState.CriusStates.IDLE);
    }

    void RecursiveFind(GameObject box)
    {
        Collider col;
        if (box.TryGetComponent(out col))
        {
            hitboxes.Add(col);
            col.enabled = false;
        }
        for (int i = 0; i < box.transform.childCount; ++i)
        {
            if (box.transform.GetChild(i).TryGetComponent(out EnemyHitbox script))
            {
                RecursiveFind(box.transform.GetChild(i).gameObject);
            }
        }
    }
}