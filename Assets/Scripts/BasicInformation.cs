using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BasicInformation
{
	//Skills
	public Type[] skills = new Type[4];


	//Base Stats;
	public int gold;
	public int expWorth;
	public float speed;
	public float armor;
	public float magicResist;
	public float abilityPower;
	public float healthRegen;
	public float manaRegen;
	public float attackSpeed;
	
	//Armor Damage Stats;
	public float effectiveHealthAR;
	public int enemyAD;
	public float damageReductionAR;
	public float realDamageAR;
	public float armorPenFlat;
	public float armorPenPc;
	internal float armorPenPcFactor;
	
	//MagicResist Damage Stats;
	public float effectiveHealthMR;
	public int enemyAPDamage;
	internal float damageReductionMR;
	public float realDamageMR;
	internal float magicPenFlat;
	internal float magicPenPc;
	internal float magicPenPcFactor;
	
	//PerLevel Stats;
	internal float damagePerLevel;
	internal float healthPerLevel;
	internal float manaPerLevel;
	internal float armorPerLevel;
	internal float magicResistPerLevel;
	internal float healthRegenPerLevel;
	internal float manaRegenPerLevel;
	
	//Base stats;
	internal float baseDamage;
	internal float baseSpeed;
	internal float baseHealth;
	internal float baseArmor;
	internal float baseMagicResist;
	internal float baseAbilityPower;
	internal float baseMana;
	internal float baseHealthRegen;
	internal float baseManaRegen;
	internal float baseAttackSpeed;

}
