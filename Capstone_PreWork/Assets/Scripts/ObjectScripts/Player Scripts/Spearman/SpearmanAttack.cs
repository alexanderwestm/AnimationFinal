using UnityEngine;
using System.Collections.Generic;

public class SpearmanAttack : CharacterSkillSet
{
    [SerializeField] BasicAttack basicAttack;

    [SerializeField] AudioClip[] basicAttackSounds;

    [SerializeField] SpinAttack chargedAttack;
    [SerializeField] AudioClip[] chargedAttackSounds;


    [SerializeField] RangedAttack rangedAttack;
    [SerializeField] AudioClip[] rangedAttackSounds;

    [SerializeField] ChargedRangedAttack chargedRangedAttack;
    [SerializeField] AudioClip[] chargedRangedAttackSounds;

    [SerializeField] float chargeTimeForStaff;
    [SerializeField] float chargeTimeForBow;
    EventManager eventManager;
    SpearmanState stateHolder;

    float chargeTime;

    CommandLogger logger;

    [SerializeField] GameObject boStaff;
    [SerializeField] GameObject bow;
    [SerializeField] GameObject bowString;
    Animator anim;
    AudioSource audioSource;
    public Vector3 rangedDirection { get; protected set; }
    public GameObject target;
    public bool isAIControlled;
    Camera cam;
    void Awake()
    {
        gameObject.GetComponent<CharacterState>().skillSet = this;
        logger = gameObject.GetComponent<CommandLogger>();
        TryGetComponent(out anim);
        equippedWeapons = new List<Weapons>() { Weapons.STAFF, Weapons.BOW };
        currentWeaponNum = 0;
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;
    }
   
    // Start is called before the first frame update
    void Start()
    {
        stateHolder = GetComponent<SpearmanState>();
        
        basicAttack.Init(gameObject);
        rangedAttack.Init(gameObject);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Input.GetKey(KeyCode.Comma))
        {
            anim.SetTrigger("BecomeLit");
        }

        if (logger == null)
        {
            gameObject.GetComponent<CommandLogger>();
            
        }
        if (!isAIControlled)
        {
            rangedDirection = cam.transform.forward + Vector3.up / 5;
            rangedDirection.Normalize();
        }
        else
        {
            rangedDirection = target.transform.position - transform.position + Vector3.up * 10;
            rangedDirection = rangedDirection.normalized;
        }

        bool bothDeactive = boStaff.activeSelf == false && bow.activeSelf == false;

        if (currentWeaponNum == 0 && (boStaff.activeSelf == false || bothDeactive))
        {
            boStaff.SetActive(true);
            bow.SetActive(false);
            bowString.SetActive(false);
            anim.SetBool("bowEquipped", false);
        }
        else if (currentWeaponNum == 1 && (boStaff.activeSelf == true || bothDeactive))
        {
            boStaff.SetActive(false);
            bow.SetActive(true);
            bowString.SetActive(true);
            anim.SetBool("bowEquipped", true);
        }

    }

    public override void HandleButtonEvent(char input, KeyState state)
    {
        if (state == KeyState.DOWN)
        {
            switch (input)
            {
                default:
                {
                    Debug.Log("SpearmanAttack button not valid");
                    break;
                }
            }
        }
    }

    public override void HandleMouseEvent(char input, KeyState state)
    {
        CharacterState.CharacterStates currentState = stateHolder.currentState;
        //can't do any of these while dashing
        if (currentState != CharacterState.CharacterStates.DASHING)
        {
            //can't start a new attack during an ongoing one
            if (stateHolder.GetSuperState() != CharacterState.CharacterStates.ATTACKING)
            {
                if (input == '0' && state == KeyState.DOWN)
                {
                    /*
                     *    // check what weapon we are using first
                     *
                     *    if(equippedWeapons[currentWeaponNum] == Weapons.STAFF)
                     *    {
                     *        // then check if we have charged long enough
                     *        if (Time.time - chargeTime > chargeTimeForStaff)
                     *        {
                     *            logger.AddCommandAndExecute(chargedAttack);
                     *            chargedAttack = new SpinAttack(chargedAttack);
                     *            int index = Random.Range(0, chargedRangedAttackSounds.Length);
                     *            audioSource.clip = chargedAttackSounds[index];
                     *            audioSource.Play();
                     *        }
                     *    }
                     *        // otherwise use our basic attack
                     *        else
                     *        {
                     *            logger.AddCommandAndExecute(basicAttack);
                     *            basicAttack = new BasicAttack(basicAttack);
                     *             int index = Random.Range(0, basicAttackSounds.Length);
                     *            audioSource.clip = basicAttackSounds[index];
                     *            audioSource.Play();
                     *
                     *
                     *    }
                     *    }
                     *    else if(equippedWeapons[currentWeaponNum] == Weapons.BOW)
                     *    {
                     *        // then check if we have charged long enough
                     *        if(Time.time - chargeTime > chargeTimeForBow)
                     *        {
                     *            logger.AddCommandAndExecute(chargedRangedAttack);
                     *            chargedRangedAttack = new ChargedRangedAttack(chargedRangedAttack);
                     *            int index = Random.Range(0, chargedRangedAttackSounds.Length);
                     *            audioSource.clip = chargedRangedAttackSounds[index];
                     *            audioSource.Play();
                     *    }
                     *        // otherwise use our basic attack
                     *        else
                     *        {
                     *            logger.AddCommandAndExecute(rangedAttack);
                     *            rangedAttack = new RangedAttack(rangedAttack);
                     *            int index = Random.Range(0, rangedAttackSounds.Length);
                     *            audioSource.clip = rangedAttackSounds[index];
                     *            audioSource.Play();
                     *    }
                     *    }
                     *
                     *    chargeTime = 0;
                     *}
                     *if we can't charge, just basic attack on mouse release
                     *else
                     *if (input == '0' && state == KeyState.DOWN)
                    */
                    
                    if (equippedWeapons[currentWeaponNum] == Weapons.STAFF && basicAttack.canExecute)
                    {
                        logger.AddCommandAndExecute(basicAttack);
                        basicAttack = new BasicAttack(basicAttack);
                        int index = Random.Range(0, basicAttackSounds.Length);
                        audioSource.clip = basicAttackSounds[index];
                        audioSource.Play();
                    }
                    else if (equippedWeapons[currentWeaponNum] == Weapons.BOW && rangedAttack.canExecute)
                    {
                        logger.AddCommandAndExecute(rangedAttack);
                        rangedAttack = new RangedAttack(rangedAttack);
                        int index = Random.Range(0, rangedAttackSounds.Length);
                        audioSource.clip = rangedAttackSounds[index];
                        audioSource.Play();
                    }
                    
                }
            }

        }
    }

    public override void SetDefaultState()
    {
        stateHolder.SetState(CharacterState.CharacterStates.IDLE);
    }

    public Weapons GetCurrentWeapon()
    {
        return equippedWeapons[currentWeaponNum];
    }

    public bool CheckChargedCooldown(bool melee)
    {
        return false;
    }

    public void ResetCooldown()
    {
        basicAttack.timeRan = -basicAttack.cooldown;
        rangedAttack.timeRan = -rangedAttack.cooldown;
    }
}
