using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FrostBreath : Ability
{
    [SerializeField] float duration;
    CriusState holderState;
    [SerializeField] ParticleSystem frostBreath;
    public FrostBreath(GameObject obj, float timeRan) : base(obj, timeRan)
    {
    }

    public FrostBreath(FrostBreath other) : base(other)
    {
        duration = other.duration;
        damage = other.damage;
        holderState = other.holderState;
        frostBreath = other.frostBreath;
    }

    public override void Execute()
    {
        if (holderState == null)
        {
            holderState = holder.GetComponent<CriusState>();
        }
        base.Execute();
        hitBox.GetComponent<EnemyHitbox>().Reset(damage);
        action = holder.StartCoroutine(BreatheFrost());
    }   

    IEnumerator BreatheFrost()
    {
        holder.gameObject.GetComponent<Animator>().SetTrigger("FrostBreath");

        timeRan = GameTimer.GlobalTimer.time;
        yield return new WaitForSecondsRealtime(duration * 0.6f);
        hitBox.SetActive(true);
        frostBreath.Play();
        holderState.SetState(CriusState.CriusStates.FROST_BEAM);
        float waitTime = duration * 0.3f;
        float timeWaited = 0;
        while (waitTime > timeWaited)
        {
            timeWaited += Time.deltaTime;
            yield return new WaitForEndOfFrame();

            Vector3 maxScale = new Vector3(40,20,40);
            hitBox.transform.localScale = maxScale * timeWaited / waitTime;
        }
        yield return new WaitForSecondsRealtime(duration * 0.1f);
        hitBox.SetActive(false);
        frostBreath.Stop();

        holderState.SetState(CriusState.CriusStates.IDLE);
    }

}