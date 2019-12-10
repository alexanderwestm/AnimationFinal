using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Ability : Command
{
    public float cooldown;
    [SerializeField] protected float damage;
    [SerializeField] protected bool cancelable;
    [SerializeField] protected GameObject hitBox;

    public bool canExecute { get { return CheckCooldown(); } set { } }

    protected Coroutine action;
    protected CharacterSkillSet holder;
    protected CharacterState state;
    protected Rigidbody rb;

    public Ability(GameObject obj, float timeRan) : base(obj, timeRan)
    {
        holder = obj.GetComponent<CharacterSkillSet>();
        state = obj.GetComponent<CharacterState>();
        rb = obj.GetComponent<Rigidbody>();
    }

    public Ability(Ability other) : base(other.gameObject, other.timeRan)
    {
        cooldown = other.cooldown;
        damage = other.damage;
        cancelable = other.cancelable;
        hitBox = other.hitBox;
        canExecute = false;

        holder = other.holder;
        state = other.state;
        rb = other.rb;
    }

    public override void Execute()
    {
        if (hitBox)
        {
            if (hitBox.TryGetComponent(out HitboxScript script))
            {
                script.Reset(damage);
            }
            else if (hitBox.TryGetComponent(out EnemyHitbox enemyScript))
            {
                enemyScript.Reset(damage);
            }
        }
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }

    public override bool Cancel()
    {
        if(action != null)
        {
            holder.StopCoroutine(action);
            holder.SetDefaultState();
            return true;
        }
       
        return false;
    }

    public override void Init(GameObject obj)
    {
        base.Init(obj);
        holder = gameObject.GetComponent<CharacterSkillSet>();
        state = gameObject.GetComponent<CharacterState>();
        rb = gameObject.GetComponent<Rigidbody>();

        if (hitBox != null)
        {
            // check for the new hitobject
            string hitboxName = hitBox.name;
            Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>(true);
            // check each of the attack objects to see if they're the same
            // Child(0) is AttackObjects on Warden Prefab
            foreach(Transform trans in childTransforms)
            {
                if(trans.name == hitboxName)
                {
                    hitBox = trans.gameObject;
                    break;
                }
            }
        }
    }

    public virtual bool CheckCooldown()
    {
        if (state.currentState == CharacterState.CharacterStates.STUNNED && state.currentState != CharacterState.CharacterStates.USING_ABILITY)
            return false;
        return GameTimer.GlobalTimer.time - timeRan > cooldown;
    }
}
