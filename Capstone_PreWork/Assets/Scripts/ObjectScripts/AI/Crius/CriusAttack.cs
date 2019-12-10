using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CriusAttack : CharacterSkillSet
{
    [SerializeField] ChainSwing swingLow;
    [SerializeField] SlamDown slamDown;
    [SerializeField] FrostBreath frostBreath;
    [SerializeField] StairAttack stairAttack;
    CriusState stateHolder;

    CommandLogger logger;
    // Start is called before the first frame update
    private void Awake()
    {
        logger = GetComponent<CommandLogger>();
    }

    void Start()
    {
        TryGetComponent(out stateHolder);
        swingLow.Init(gameObject);
        slamDown.Init(gameObject);
        frostBreath.Init(gameObject);
        stairAttack.Init(gameObject);
    }

    public override void HandleButtonEvent(char input, KeyState state)
    {
        if (stateHolder.GetState() == CriusState.CriusStates.IDLE)
        {
            switch (input)
            {
                case 'L':
                {
                    if (swingLow.canExecute)
                    {
                        logger.AddCommandAndExecute(swingLow);
                        swingLow = new ChainSwing(swingLow);
                    }
                    break;
                }
                case 'S':
                {
                    if (slamDown.canExecute)
                    {
                        logger.AddCommandAndExecute(slamDown);
                        slamDown = new SlamDown(slamDown);
                    }
                    break;
                }
                case 'H':
                {
                    if (stairAttack.canExecute)
                    {
                        logger.AddCommandAndExecute(stairAttack);
                        stairAttack = new StairAttack(stairAttack);
                    }
                    break;
                }
                case 'F':
                {
                    if (frostBreath.canExecute)
                    {
                        Debug.Log("Frost Breath");
                        logger.AddCommandAndExecute(frostBreath);
                        frostBreath = new FrostBreath(frostBreath);
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }

    public override void HandleMouseEvent(char input, KeyState state)
    {
        throw new System.NotImplementedException();
    }

    public override void SetDefaultState()
    {
        stateHolder.SetState(CriusState.CriusStates.IDLE);
    }

}
