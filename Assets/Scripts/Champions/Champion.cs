using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.VFX;

public class Champion : MonoBehaviour
{
	public BasicInformation bi;
	public bool dead = false;
	public Team team;

	public delegate void LevelUpEvent();
	public delegate void DieEvent();

	public event DieEvent championDeath;
	public event LevelUpEvent levelUp;

	public Animator anim;

	int[] expPerLevel = new int[]
	{
		0,
		280,
		660,
		1140,
		1720,
		2400,
		3180,
		4060,
		5040,
		6120,
		7300,
		8580,
		9960,
		11440,
		13020,
		14700,
		16480,
		18360,
	};

	public int hp = 0;
	public int mp = 0;
	public int exp = 0;
	public int level = 1;
	public float attackSpeed;
	public int damage;
	public int speed;
	public int gold;

	public void Init()
	{
		//bi = Champions.main.GetChampion(gameObject.name);
		anim = transform.GetChild(0).GetComponent<Animator>();

		UpdateStats();
	}

	public bool ChangeHp(int dmg,Champion owner)
	{
		if (hp - dmg <= 0) 
		{ 
			hp = 0;

			GetComponent<HealthBar>().UpdateHpBar(1 / ((bi.baseHealth + bi.healthPerLevel * level) / hp));

			if (GetComponent<Tower>()) 
			{
				Die();
				return true; 
			}

			owner.GainGold(bi.gold); 
			Die(); 
			return true; 
		}
		else
		{
			hp -= dmg;

			GetComponent<HealthBar>().UpdateHpBar(1 / ((bi.baseHealth + bi.healthPerLevel * level) / hp));
			return false;
		}
	}

	public void GainExp(int expToGain)
	{
		if(expPerLevel[level] <= exp + expToGain)
		{
			LevelUp();
		}

		exp += expToGain;
	}

	public void GainGold(int goldToGain)
    {

    }

	public void LevelUp()
	{
		level++;
		levelUp?.Invoke();
		UpdateStats();
	}

	public float LevelProgress()
    {
		return (float)exp / (float)expPerLevel[level];
    }

	public void Die()
	{

		GetComponent<Collider>().enabled = false;

		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(false);
		}

		championDeath?.Invoke();
	}

	public void UpdateStats()
	{
		hp = (int)(bi.baseHealth + bi.healthPerLevel * level);
		mp = (int)(bi.baseMana + bi.manaPerLevel * level);
		damage = (int)(bi.baseDamage + bi.damagePerLevel * level);
		attackSpeed = bi.baseAttackSpeed / level;
		speed = (int)bi.baseSpeed * level;

		//stats updaten per level;
		bi.armor = bi.baseArmor + bi.armorPerLevel * level;
		bi.magicResist = bi.baseMagicResist + bi.magicResistPerLevel * level;
		bi.healthRegen = bi.baseHealthRegen + bi.healthRegenPerLevel * level;
		bi.manaRegen = bi.baseManaRegen + bi.manaRegenPerLevel * level;
		bi.expWorth = bi.expWorth * level;

		//Armor Calculation;
		bi.armorPenPcFactor = 1 - bi.armorPenPc/100;	
		bi.effectiveHealthAR = (((bi.armor*bi.armorPenPcFactor) - bi.armorPenFlat) /100 + 1) * (float)hp;	
		bi.damageReductionAR = (float)hp/bi.effectiveHealthAR;
		bi.realDamageAR = bi.damageReductionAR;
		//*bi.enemyAD


		//Magic Resist Calculation;
		bi.magicPenPcFactor = 1 - bi.magicPenPc/100;
		bi.effectiveHealthMR = ((bi.magicResist*bi.magicPenPcFactor) /100 + 1) *(float)hp;
		bi.damageReductionMR = (float)hp/bi.effectiveHealthMR;
		bi.realDamageMR = bi.damageReductionMR;	
	}

	public static Transform GetClosestEnemy(Vector3 thisPos, Collider[] enemies, Collider thisCollider, Team thisTeam)
	{
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Champion tempChampion;

		for (int i = 0; i < enemies.Length; i++)
		{
			if (enemies[i].Equals(thisCollider))
			{
				continue;
			}
			tempChampion = enemies[i].GetComponent<Champion>();

			if(tempChampion.team == thisTeam) { continue; }

			Vector3 directionToTarget = enemies[i].transform.position - thisPos;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if (dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = enemies[i].transform;
			}
		}

		return bestTarget;
	}

	public static bool CheckIfEnemy(Transform hit,Team thisTeam)
	{
		Champion temp = hit.transform.GetComponent<Champion>();

		if (temp)
        {
			if(temp.team != thisTeam)
            {
				return true;
            }
        }

		return false;
	}
}

