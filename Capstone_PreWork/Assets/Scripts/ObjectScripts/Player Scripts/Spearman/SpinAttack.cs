using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpinAttack : Ability
{
    [SerializeField] float duration;

    public SpinAttack(GameObject obj, float timeRan) : base(obj, timeRan)
    {

    }

    public SpinAttack(SpinAttack other) : base(other)
    {
        duration = other.duration;
    }

    public override void Execute()
    {
        base.Execute();
        if (canExecute)
        {
            ((SpearmanState)state).SetState(CharacterState.CharacterStates.ATTACKING);
            action = holder.StartCoroutine(Spin());
        }
    }

    IEnumerator Spin()
    {
        hitBox.SetActive(true);
        timeRan = GameTimer.GlobalTimer.time;
        yield return new WaitForSecondsRealtime(duration);
        hitBox.SetActive(false);
    }
}
