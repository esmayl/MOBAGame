using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Basic Information", menuName = "Data/Basic Information", order = 1)]
public class BasicInformation : ScriptableObject
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
	public float armorPenPcFactor;
	
	//MagicResist Damage Stats;
	public float effectiveHealthMR;
	public int enemyAPDamage;
	public float damageReductionMR;
	public float realDamageMR;
	public float magicPenFlat;
	public float magicPenPc;
	public float magicPenPcFactor;
	
	//PerLevel Stats;
	public float damagePerLevel;
	public float healthPerLevel;
	public float manaPerLevel;
	public float armorPerLevel;
	public float magicResistPerLevel;
	public float healthRegenPerLevel;
	public float manaRegenPerLevel;
	
	//Base stats;
	public float baseDamage;
	public float baseSpeed;
	public float baseHealth;
	public float baseArmor;
	public float baseMagicResist;
	public float baseAbilityPower;
	public float baseMana;
	public float baseHealthRegen;
	public float baseManaRegen;
	public float baseAttackSpeed;

}
