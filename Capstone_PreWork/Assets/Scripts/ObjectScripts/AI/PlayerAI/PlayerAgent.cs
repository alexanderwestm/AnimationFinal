using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum playerAIAction
{
    INVALID_ACTION = -1,
    MELEE,
    MELEE_CHARGED,
    BOW,
    BOW_CHARGED,
    DODGE,
    REPOSITION,
    SPECIAL,
    NUM_ACTIONS
}

public class PlayerAgent : MonoBehaviour
{
    NavMeshAgent agent;  
    PlayerInputInterpreter interpreter;
    bool isActing;
    PlayerDecisionTree decider;
    SpearmanAttack attackHolder;
    bool isRepositioning;
    [SerializeField] float ArriveRadius;
    Animator anim;
    // Start is called before the first frame update

    private void Awake()
    {
 
    }

    void Start()
    {
        TryGetComponent(out anim);
        TryGetComponent(out agent);
        TryGetComponent(out attackHolder);
        TryGetComponent(out decider);
        TryGetComponent(out interpreter);
        agent.angularSpeed = 0;
        agent.speed *= 2;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);
        attackHolder.isAIControlled = true;
        attackHolder.target = decider.target;
        transform.LookAt(decider.target.transform, Vector3.up);
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        if (!isActing)
        {
            playerAIAction decision = decider.Decide();
            Debug.Log(decision);
            switch (decision)
            {
                case playerAIAction.BOW:
                    {
                        StartCoroutine(BowBasic());
                        break;
                    }
                case playerAIAction.BOW_CHARGED:
                    {
                        StartCoroutine(BowCharged());
                        break;
                    }
                case playerAIAction.MELEE:
                    {
                        StartCoroutine(MeleeBasic());
                        break;
                    }
                case playerAIAction.MELEE_CHARGED:
                    {
                        StartCoroutine(MeleeCharged());
                        break;
                    }
                case playerAIAction.REPOSITION:
                    {
                        Reposition();
                        break;
                    }
                case playerAIAction.DODGE:
                    {
                        break;
                    }
            }
        }
    }

    IEnumerator BowBasic()
    {
        if (attackHolder.GetCurrentWeapon() != SpearmanAttack.Weapons.BOW)
        {
            interpreter.InterpretKeyboardInput('Q', KeyState.DOWN);
        }
        isActing = true;
        // left mouse down
        interpreter.InterpretMouseInput('0', KeyState.DOWN);
        yield return new WaitForSeconds(0.2f);
        // left mouse up
        interpreter.InterpretMouseInput('0', KeyState.UP);
        isActing = false;
    }
    
    IEnumerator BowCharged()
    {
        if (attackHolder.GetCurrentWeapon() != SpearmanAttack.Weapons.BOW)
        {
            interpreter.InterpretKeyboardInput('Q', KeyState.DOWN);
        }
        isActing = true;
        // left mouse down
        interpreter.InterpretMouseInput('0', KeyState.DOWN);
        yield return new WaitForSecondsRealtime(3.5f);
        // left mouse up
        interpreter.InterpretMouseInput('0', KeyState.UP);
        isActing = false;
    }

    IEnumerator MeleeBasic()
    {
        if (attackHolder.GetCurrentWeapon() != SpearmanAttack.Weapons.STAFF)
        {

        }
        isActing = true;
        // left mouse down
        interpreter.InterpretKeyboardInput('0', KeyState.DOWN);
        yield return new WaitForSecondsRealtime(0.2f);
        // left mouse up
        interpreter.InterpretKeyboardInput('0', KeyState.UP);
        isActing = false;
    }

    IEnumerator MeleeCharged()
    {
        if (attackHolder.GetCurrentWeapon() != SpearmanAttack.Weapons.STAFF)
        {

        }
        isActing = true;
        // left mouse down
        interpreter.InterpretKeyboardInput('0', KeyState.DOWN);
        yield return new WaitForSecondsRealtime(3.5f);
        // left mouse up
        interpreter.InterpretKeyboardInput('0', KeyState.UP);
        isActing = false;
    }
    IEnumerator Dodge()
    {
        isActing = true;
        Vector3 direction = decider.GetTargetHitbox().transform.position - transform.position;
        //if we have a defensive ability, use that to tank the hit
        
        //if we have a dodge ability, use that away from the hitbox's path

        
        //if the attack is above us, jump and dash to the side

        if (Mathf.Abs(direction.y) > 3.5f)
        {
            char dir;
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                interpreter.rotatedMove = -transform.right;
                dir = 'A';
            }
            else
            {
                interpreter.rotatedMove = transform.right;
                dir = 'D';
            }

            //give the interpreter a direction

            //then dash
            interpreter.InterpretKeyboardInput('h', KeyState.DOWN);
        }

        //otherwises, jump and dash back
        {
            ButtonEvent move = new ButtonEvent(0, 'S', KeyState.DOWN);
            ButtonEvent jump = new ButtonEvent(0, '_', KeyState.DOWN);

            //then dash
            interpreter.rotatedMove = Vector3.zero;
            interpreter.InterpretKeyboardInput('h', KeyState.DOWN);
        }
        yield return new WaitForEndOfFrame();
        isActing = false;
    }
    public bool IsRepositioning()
    {
        if ((transform.position - agent.destination).sqrMagnitude < 2)
        {
            isRepositioning = false;
        }
        return isRepositioning;
    }
    
    void Reposition()
    {
        agent.SetDestination(decider.repositionTarget);
        isRepositioning = true;
    }
}
