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

	public Skill[] skillPrefabs;
	public SkillState[] skills;
	GameObject[] skillInstances;

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
		anim = GetComponent<Animator>();

		UpdateStats();


		//if(skillPrefabs.Length <= 0)
		//{
		//	return;
		//}

		//skills = new SkillState[4];

		////Instantiate all skillPrefabs
		//skillInstances = new GameObject[skillPrefabs.Length];
		//int i = 0;

		//foreach (Skill g in skillPrefabs)
		//{
		//	skillInstances[i] = Instantiate(g.skillPrefab);
		//	i++;
		//}

		//skills[0] = new AhriQ(gameObject, this, anim, skillInstances[0]);
		//skills[1] = new AhriW(gameObject, this, anim, skillInstances[1]);
		//skills[2] = new AhriE(gameObject, this, anim, skillInstances[2]);
		//skills[3] = new AhriR(gameObject, this, anim, skillInstances[3]);
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

	//void OnGUI ()
	//{


	//	healthString = bi.currentHp.ToString();
	//	armorString = bi.armor.ToString();	
	//	magicResistString = bi.magicResist.ToString();	
	//	damageString = bi.damage.ToString();	
	//	ArmorPenPercentString = bi.armorPenPc.ToString();
	//	ArmorPenFlatString = bi.armorPenFlat.ToString();	

	//	GUI.Window(0,new Rect(10,10,200,300),DoMyWindow,"Stats");


	//}

	//void DoMyWindow(int windowID)
	//{
	//	GUI.Label(new Rect(20, 30, 200, 20), "Health: ");
	//	GUI.Label(new Rect(20, 50, 200, 20), "Armor: ");
	//	GUI.Label(new Rect(20, 70, 200, 20), "MagicResist: ");
	//	GUI.Label(new Rect(20, 90, 200, 20), "AD: ");
	//	GUI.Label(new Rect(20, 110, 200, 20), "ArmorPen Percent: ");
	//	GUI.Label(new Rect(20, 130, 200, 20), "ArmorPen Flat: ");
	//	GUI.Label(new Rect(20, 150, 200, 20), "Damage: " + Mathf.Round(bi.realDamageAR));

	//	GUI.Label(new Rect(160, 30, 40, 20), healthString);
	//	GUI.Label(new Rect(160, 50, 40, 20), armorString);
	//	GUI.Label(new Rect(160, 70, 40, 20), magicResistString);
	//	GUI.Label(new Rect(160, 90, 40, 20), damageString);
	//	GUI.Label(new Rect(160, 110, 40, 20), ArmorPenPercentString);
	//	GUI.Label(new Rect(160, 130, 40, 20), ArmorPenFlatString);
	//}


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

			Vector3 directionToTarget = enemies[i].transform.position - thisPos;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if (dSqrToTarget < closestDistanceSqr && tempChampion.team != thisTeam)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = enemies[i].transform;
			}
		}

		//if (bestTarget)
		//{
		//    Debug.Log("Closest: " + bestTarget.name);
		//}

		return bestTarget;
	}
}

