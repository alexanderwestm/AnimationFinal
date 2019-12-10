using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StairAttack : Ability
{
    [SerializeField] float duration;
    [SerializeField] GameObject hitbox2;
    List<Collider> hitboxes;
    Collider hitboxCol;
    CriusState holderState;
    public StairAttack(GameObject obj, float timeRan) : base(obj, timeRan)
    {
        hitboxes = new List<Collider>();
        RecursiveFind(hitBox);
        RecursiveFind(hitbox2);

        foreach (Collider c in hitboxes)
        {
            c.enabled = false;
        }
    }

    public StairAttack(StairAttack other) : base(other)
    {
        duration = other.duration;
        damage = other.damage;
        hitbox2 = other.hitbox2;
        hitboxes = other.hitboxes;
        holderState = other.holderState;
        foreach (Collider c in hitboxes)
        {
            c.enabled = false;
        }
    }

    public override void Execute()
    {
        if (holderState == null)
        {
            holderState = holder.GetComponent<CriusState>();
        }
        hitbox2.GetComponent<EnemyHitbox>().Reset(damage);
        base.Execute();
        action = holder.StartCoroutine(StairAttackLow());
    }

    IEnumerator StairAttackLow()
    {
        if (hitboxes == null)
        {
            hitboxes = new List<Collider>();
            RecursiveFind(hitBox);
            RecursiveFind(hitbox2);
        }
        holder.gameObject.GetComponent<Animator>().SetTrigger("StairAttack");


        timeRan = GameTimer.GlobalTimer.time;
        yield return new WaitForSecondsRealtime(duration * 0.2f);
        holderState.SetState(CriusState.CriusStates.SWING_HIGH);

        foreach (Collider c in hitboxes)
        {
            c.enabled = true;
        }
        yield return new WaitForSecondsRealtime(duration * 0.8f);

        foreach (Collider c in hitboxes)
        {
            c.enabled = false;
        }

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