using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleWeapons : Command
{
    private CharacterSkillSet skillSet;
    public CycleWeapons(CharacterSkillSet skillSet, GameObject obj, float timeRan):base(obj, timeRan)
    {
        this.skillSet = skillSet;
    }

    public override void Execute()
    {
        skillSet.currentWeaponNum++;
        skillSet.currentWeaponNum = skillSet.currentWeaponNum % skillSet.equippedWeapons.Count;
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(GameObject obj)
    {
        base.Init(obj);
        skillSet = obj.GetComponent<CharacterSkillSet>();
    }
}
