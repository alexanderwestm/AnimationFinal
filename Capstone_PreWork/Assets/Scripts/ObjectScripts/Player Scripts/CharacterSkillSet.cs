using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class CharacterSkillSet : MonoBehaviour
{
    // damage modifier will be 1 normally
    // if it's 1.5 character takes 50% more damage
    // if it's .5 character takes 50% less damage
    public float damageTakenModifier = 1;

    // damage modifier will be 1 normally
    // if it's 1.5 character deals 50% more damage
    // if it's .5 character deals 50% less damage
    public float damageModifier = 1;

    public enum Weapons
    {
        INVALID = -1,
        STAFF,
        BOW,
        NUM_WEAPONS
    }

    public List<Weapons> equippedWeapons { get; protected set; }

    public int currentWeaponNum;

    public PassiveAugment passiveAugment; //{ get; protected set; }
    public ActiveAugment activeAugment;

    //public ActiveAugment activeAugment{ get; protected set; }

    public abstract void HandleButtonEvent(char input, KeyState state);

    public abstract void HandleMouseEvent(char input, KeyState state);

    public abstract void SetDefaultState();

    // Increments the weapon slot index
    public virtual void ChangeWeapon(GameObject obj)
    {
        CommandLogger logger = obj.GetComponent<CommandLogger>();
        logger.AddCommandAndExecute(new CycleWeapons(this, obj, GameTimer.GlobalTimer.time));
    }
}
