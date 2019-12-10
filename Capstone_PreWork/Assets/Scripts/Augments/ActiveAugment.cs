using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAugment : MonoBehaviour
{
    public AugmentDataScriptableObject data;
    // every thing has a cooldown
    // every frame the ability is recharged by some percentage of this cooldown
    // aka charge += % * dt <= cooldown

    [SerializeField] bool adjustedValues = false;
    bool recharging;
    float rechargeAmount;
    [SerializeField] float rechargePercent;

    public float maxCharge;
    public float currentCharge;
    [SerializeField] ParticleSystem healParticles;
    [SerializeField] GameObject healCircle;
    [SerializeField] GameObject shieldBubble;
    [SerializeField] ParticleSystem blinkEnter;
    [SerializeField] ParticleSystem blinkExit;
    [SerializeField] GameObject buffCircle;

    CharacterState characterState;
    CharacterSkillSet skillSet;

    [SerializeField] GameObject thornyBubble;
    [SerializeField] GameObject healBubble;
    GameObject thornyInstance;
    GameObject healInstance;

    [SerializeField] GameObject shieldPrefab;
    GameObject shieldInstance;


    // Start is called before the first frame update
    void Start()
    {
        characterState = GetComponent<CharacterState>();
        skillSet = GetComponent<CharacterSkillSet>();
        SetStartValues();
    }

    private void Update()
    {
        if (thornyBubble == null)
        {
            thornyBubble = Resources.Load("Prefabs/Bubble_Thorny") as GameObject;
        }
        if (healBubble == null)
        {
            healBubble = Resources.Load("Prefabs/Bubble_Heal") as GameObject;
        }
        if (shieldPrefab == null)
        {
            shieldPrefab = Resources.Load("Prefabs/Shield") as GameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (characterState.currentState != CharacterState.CharacterStates.USING_ABILITY)
        {
            currentCharge += data.activeVariables["rechargeAmount"] * Time.fixedDeltaTime;
            if (recharging && currentCharge > rechargeAmount)
            {
                recharging = false;
            }
            if (currentCharge > maxCharge)
            {
                currentCharge = maxCharge;
            }
        }
    }

    public void SetStartValues()
    {
        currentCharge = maxCharge = data.activeVariables["maxCharge"];
        rechargeAmount = rechargePercent * data.activeVariables["maxCharge"];
        FindObjectOfType<EnergyBar>().SetMaxCharge(maxCharge);
    }

    public void UseAbility(bool state)
    {
        if (state && !recharging && CheckActivatable(data.activeVariables["depletionAmount"]) && characterState.currentState != CharacterState.CharacterStates.STUNNED)
        {
            switch (data.type)
            {
                case AugmentType.SOUL:
                {
                    // healing buff
                    // depletion time
                    // refresh time
                    // heal amount
                    // range
                    // cannot move or do anything else
                    if (state)
                    {
                        UpdateCharge();

                        // check a range for the Clone layer
                        Collider[] hitCollider = Physics.OverlapSphere(transform.position, data.activeVariables["range"], LayerMask.GetMask("Player", "Clone"));
                        foreach (Collider col in hitCollider)
                        {
                            Health health = col.gameObject.GetComponent<Health>();
                            health.SetHealth(health.currentHealth + data.activeVariables["healAmount"] * Time.fixedDeltaTime);
                            if (healParticles)
                            {
                                healParticles.Play();
                            }
                            healCircle.SetActive(true);
                            healCircle.transform.localScale = new Vector3(data.activeVariables["range"], healCircle.transform.localScale.y, data.activeVariables["range"]);
                        }
                        characterState.SetState(CharacterState.CharacterStates.USING_ABILITY);
                    }
                    else
                    {
                        characterState.SetState(CharacterState.CharacterStates.IDLE);
                        healCircle.SetActive(false);
                    }

                    break;
                }
                case AugmentType.SOUL_METAL:
                {
                    // healing bubble
                    // depletion time
                    // refresh time
                    // heal amount
                    // range
                    // shield amount
                    // if shield broken disrupt

                    UpdateCharge();
                    characterState.SetState(CharacterState.CharacterStates.USING_ABILITY);

                    // check in a radius for Clone layer
                    Collider[] hitCollider = Physics.OverlapSphere(transform.position, data.activeVariables["range"], LayerMask.GetMask("Player", "Clone"));
                    foreach (Collider col in hitCollider)
                    {
                        Health health = col.gameObject.GetComponent<Health>();
                        health.SetHealth(health.currentHealth + data.activeVariables["healAmount"] * Time.fixedDeltaTime);
                        if (healParticles)
                        {
                            healParticles.Play();
                        }
                    }

                    // so you can't just spam press the button
                    if (!adjustedValues && healInstance == null)
                    {
                        adjustedValues = true;
                        Bubble bubble = CreateBubble(healBubble).GetComponent<Bubble>();
                        healInstance = bubble.gameObject;
                        bubble.health = data.activeVariables["health"];
                        if (healParticles)
                        {
                            healParticles.Play();
                        }
                    }
                    break;
                }
                case AugmentType.SOUL_MIST:
                {
                    // healing blink
                    // healing amount
                    // blink distance
                    blinkEnter.Play();
                    if (healParticles)
                    {
                        healParticles.Play();
                    }
                    UpdateCharge();
                    Health health;
                    RaycastHit[] hits = Blink(LayerMask.GetMask("Clone"), LayerMask.GetMask("Player", "Clone"));
                    foreach (RaycastHit hit in hits)
                    {
                        health = hit.collider.transform.root.gameObject.GetComponent<Health>();
                        health.SetHealth(health.currentHealth + data.activeVariables["healAmount"]);
                    }

                    blinkExit.Play();
                    break;
                }
                case AugmentType.SOUL_BLOOD:
                {
                    // damage buff
                    // 
                    // buff amount (%)
                    UpdateCharge();

                    buffCircle.transform.localScale = new Vector3(data.activeVariables["range"], buffCircle.transform.localScale.y, data.activeVariables["range"]);

                    StartCoroutine(DamageBuff(data.activeVariables["damageModifier"], data.activeVariables["activeTime"]));
                    break;
                }
                case AugmentType.METAL:
                {
                    // shield
                    // shield amount
                    // stun time

                    UpdateCharge();
                    MetalShield(false);
                    shieldBubble.SetActive(true);
                    break;
                }
                case AugmentType.METAL_METAL:
                {
                    // multi shield
                    // shield amount
                    // stun time
                    // range
                    // secondary shield
                    UpdateCharge();
                    MetalShield(true);
                    shieldBubble.SetActive(true);
                    break;
                }
                case AugmentType.METAL_BLOOD:
                {
                    // thorny bubble
                    // depletion time
                    // refresh time
                    // damage reflected
                    // damage done on break
                    UpdateCharge();
                    Bubble bubbleScript = CreateBubble(thornyBubble).GetComponent<Bubble>();
                    thornyInstance = bubbleScript.gameObject;
                    bubbleScript.damageBack = data.activeVariables["damageBack"];
                    bubbleScript.breakDamageBack = data.activeVariables["breakDamage"];
                    bubbleScript.health = data.activeVariables["health"];
                    EventManager.GetInstance().AddListener(bubbleScript, EventType.DAMAGE_EVENT);
                    break;
                }
                case AugmentType.METAL_MIST:
                {
                    // blink but do damage
                    // distance
                    // width
                    // padding
                    // damage amount
                    blinkEnter.Play();
                    UpdateCharge();
                    RaycastHit[] hits = Blink(LayerMask.GetMask("Enemy"), LayerMask.GetMask("Player", "Clone", "Enemy"));
                    List<GameObject> objs = new List<GameObject>();
                    foreach (RaycastHit hit in hits)
                    {
                        if(!objs.Contains(hit.transform.root.gameObject))
                        {
                            Health health = hit.transform.root.GetComponent<Health>();
                            health.SetHealth(health.currentHealth - data.activeVariables["damageAmount"]);
                            objs.Add(hit.transform.root.gameObject);
                        }
                    }
                    blinkExit.Play();
                    break;
                }
                case AugmentType.MIST:
                {
                    // blink
                    // distance warped
                    blinkEnter.Play();
                    UpdateCharge();
                    Blink(0, LayerMask.GetMask("Player", "Clone"));
                    blinkExit.Play();
                    break;
                }
                case AugmentType.MIST_MIST:
                {
                    // shroud blink
                    // distance warped
                    // time intangible for
                    // range
                    // ally invisibility time

                    // same as mist just with the extra intangible/ally stealth
                    blinkEnter.Play();
                    UpdateCharge();
                    Blink(0, LayerMask.GetMask("Player", "Clone"));
                    StartCoroutine(ShroudBlinkIntangible(data.activeVariables["intangibleTime"]));
                    Collider[] colliders = Physics.OverlapSphere(transform.position, data.activeVariables["range"], 1 << 10);
                    StartCoroutine(ShroudBlinkInvisible(colliders, data.activeVariables["invisibleTime"]));
                    blinkExit.Play();
                    break;
                }
                case AugmentType.MIST_BLOOD:
                {
                    blinkEnter.Play();
                    UpdateCharge();
                    Blink(0, LayerMask.GetMask("Player", "Clone"));
                    StartCoroutine(Strength(data.activeVariables["damageModifier"], data.activeVariables["activeTime"]));
                    blinkExit.Play();
                    break;
                }
                case AugmentType.BLOOD:
                {
                    // strength && betterStrength
                    // damage modifier
                    // time active
                    UpdateCharge();
                    StartCoroutine(Strength(data.activeVariables["damageModifier"], data.activeVariables["activeTime"]));
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        else if (!recharging)
        {
            recharging = true;
            switch (data.type)
            {
                case AugmentType.SOUL:
                {
                    characterState.SetState(CharacterState.CharacterStates.IDLE);
                    healCircle.SetActive(false);
                    break;
                }
                case AugmentType.SOUL_METAL:
                {
                    characterState.SetState(CharacterState.CharacterStates.IDLE);
                    adjustedValues = false;
                    if (healInstance != null)
                    {
                        Destroy(healInstance);
                    }
                    break;
                }
                case AugmentType.METAL:
                {
                    shieldBubble.SetActive(false);
                    break;
                }
                case AugmentType.METAL_METAL:
                {
                    shieldBubble.SetActive(false);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }

    private bool CheckActivatable(float depletionAmount)
    {
        if (data.activeVariables["overTime"] != 0)
        {
            return currentCharge - depletionAmount * Time.fixedDeltaTime >= 0;
        }
        return currentCharge - depletionAmount >= 0;
    }

    private void UpdateCharge()
    {
        float depletionAmount = data.activeVariables["depletionAmount"];
        if (data.activeVariables["overTime"] != 0)
        {
            depletionAmount *= Time.fixedDeltaTime;
        }
        currentCharge -= depletionAmount;
        // overloading charge?
        // shouldn't be possible anyways?
        if (currentCharge < 0)
        {
            currentCharge = 0;
        }
    }

    private RaycastHit[] Blink(int collideLayer, int ignoreLayer)
    {
        Vector3 blinkDirection = transform.forward;
        float distance = data.activeVariables["blinkDistance"];
        Vector3 blinkVector = blinkDirection * distance;
        RaycastHit hit;

        // raycast in direction ignoring the player layer and clone layer
        Physics.Raycast(transform.position, blinkDirection, out hit, distance, ~ignoreLayer);
        // if we've collided with something we need to adjust blink distance
        if (hit.collider != null)
        {
            distance = hit.distance - data.activeVariables["padding"];
            blinkVector = blinkDirection * distance;
        }

        float halfWidths = data.activeVariables["width"] * .5f;
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, new Vector3(halfWidths, halfWidths, 1), blinkDirection, Quaternion.identity, distance, collideLayer);
        transform.position += blinkVector;
        return hits;
    }

    private GameObject MetalShield(bool clonesToo)
    {
        // create the shield
        GameObject shield = Instantiate(shieldPrefab, transform.position + transform.forward * data.activeVariables["placeDistance"], Quaternion.identity);
        float size = data.activeVariables["size"];
        shield.transform.localScale = new Vector3(size, size, shield.transform.localScale.z);
        shield.transform.forward = transform.forward;
        Shield shieldScript = shield.GetComponent<Shield>();
        shieldScript.health = data.activeVariables["health"];
        shieldScript.affectsClones = clonesToo;
        shieldScript.distance = data.activeVariables["effectDistance"];
        shieldScript.modifier = data.activeVariables["shieldModifier"];
        return shield;
    }

    private GameObject CreateBubble(GameObject bubblePrefab)
    {
        GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
        float range = data.activeVariables["bubbleSize"];
        bubble.transform.localScale = new Vector3(range, range, range);
        return bubble;
    }

    IEnumerator Strength(float mod, float time)
    {
        skillSet.damageModifier += mod;
        adjustedValues = true;
        yield return new WaitForSecondsRealtime(time);
        skillSet.damageModifier -= mod;
        adjustedValues = false;
    }

    IEnumerator Stun(float time)
    {
        characterState.SetState(CharacterState.CharacterStates.STUNNED);
        yield return new WaitForSecondsRealtime(time);
        characterState.SetState(CharacterState.CharacterStates.IDLE);
    }

    IEnumerator DamageBuff(float modifier, float time)
    {
        if (gameObject.tag == "Player")
        {
            GetComponent<CharacterSkillSet>().damageModifier += modifier;
        }
        buffCircle.GetComponent<BuffCircle>().buffAmount = modifier;
        buffCircle.SetActive(true);

        yield return new WaitForSecondsRealtime(time);

        buffCircle.SetActive(false);
        if (gameObject.tag == "Player")
        {
            GetComponent<CharacterSkillSet>().damageModifier -= modifier;
        }
    }

    IEnumerator ShroudBlinkIntangible(float time)
    {
        characterState.isInvisible = true;
        float tempValue = skillSet.damageTakenModifier;
        skillSet.damageTakenModifier -= tempValue;
        yield return new WaitForSecondsRealtime(time);
        skillSet.damageTakenModifier += tempValue;
        characterState.isInvisible = false;
    }

    IEnumerator ShroudBlinkInvisible(Collider[] colliders, float time)
    {
        foreach (Collider col in colliders)
        {
            col.GetComponent<CharacterState>().isInvisible = true;
        }
        yield return new WaitForSecondsRealtime(time);
        foreach (Collider col in colliders)
        {
            col.GetComponent<CharacterState>().isInvisible = false;
        }
    }

    public void ResetCharge()
    {
        currentCharge = maxCharge;
        recharging = false;
    }

    private void OnDrawGizmos()
    {
        if (tag == "Player")
        {
            Gizmos.DrawWireSphere(transform.position, data.activeVariables["range"]);
        }
    }
}