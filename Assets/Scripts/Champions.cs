using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Champions : MonoBehaviour
{	
	// Stores all data
	Dictionary<string, BasicInformation> champions = new Dictionary<string, BasicInformation>();


	private static Champions _instance;

	public static Champions main { get { return _instance; } }


	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
			Initialize();
		}
	}
	
	// Get data for champion
	public BasicInformation GetChampion(string name)
	{
		BasicInformation newInfo = new BasicInformation();
		newInfo = champions[name];
		return newInfo;	
	}
	
	// Create hero data
	void Initialize ()
	{
		
		champions.Add("Nexus", new BasicInformation
		{
			//Base Stats;
			baseDamage = 0,
			baseHealth = 5000,
			baseMana = 0,
			speed = 0,
			baseArmor = 0,
			baseMagicResist = 0,
			baseHealthRegen = 0,
			baseManaRegen = 0,
			baseAttackSpeed = 0,
			expWorth = 0,

			//Stats PerLevel;	
			damagePerLevel = 0,
			healthPerLevel = 0,
			manaPerLevel = 0,
			armorPerLevel =0,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0,
			manaRegenPerLevel = 0
		});

		champions.Add("Tower", new BasicInformation
		{
			//Base Stats;
			baseDamage = 50,
			baseHealth = 380,
			baseMana = 230,
			speed = 120,
			baseArmor = 11,
			baseMagicResist = 30,
			baseHealthRegen = 5.5f,
			baseManaRegen = 6.25f,
			baseAttackSpeed = 2,
			expWorth = 0,

			//Stats PerLevel;	
			damagePerLevel = 3,
			healthPerLevel = 80,
			manaPerLevel = 50,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.6f,
			manaRegenPerLevel = 0.6f

		});

		champions.Add("Melee Minion", new BasicInformation
		{
			//Base Stats;
			baseDamage = 10,
			baseHealth = 380,
			baseMana = 230,
			speed = 120,
			baseArmor = 11,
			baseMagicResist = 30,
			baseHealthRegen = 5.5f,
			baseManaRegen = 6.25f,
			baseAttackSpeed = 4,
			expWorth = 42,

			//Stats PerLevel;	
			damagePerLevel = 3,
			healthPerLevel = 80,
			manaPerLevel = 50,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.6f,
			manaRegenPerLevel = 0.6f

		});

		champions.Add("Ahri", new BasicInformation
		{
            //Skills
            skills = new Type[] { typeof(AhriQ), typeof(AhriW), typeof(AhriE), typeof(AhriR) },

            //Base Stats;
            baseDamage = 50,
			baseHealth = 380,
			baseMana = 230,
			speed = 330,
			baseArmor = 11,
			baseMagicResist = 30,
			baseHealthRegen = 5.5f,
			baseManaRegen = 6.25f,
			baseAttackSpeed = 2,
			expWorth = 42,
				
			//Stats PerLevel;	
			damagePerLevel = 3,
			healthPerLevel = 80,
			manaPerLevel = 50,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.6f,
			manaRegenPerLevel = 0.6f
				
		});
		
		champions.Add("Akali", new BasicInformation
		{
			//Base Stats;
			baseDamage = 53,
			baseHealth = 445,
			baseMana = 200,
			speed = 350,
			baseArmor = 16.5f,
			baseMagicResist = 30,
			baseHealthRegen = 7.25f,
			baseManaRegen = 50,
			baseAttackSpeed = 1.3f,
			
			expWorth = 42,

			//Stats PerLevel;	
			damagePerLevel = 3.2f,
			healthPerLevel = 85,
			manaPerLevel = 0,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 1.25f,
			healthRegenPerLevel = 0.65f,
			manaRegenPerLevel = 0	
				
		});
		
		champions.Add("Alistar", new BasicInformation
		{
			//Base Stats;
			baseDamage = 55.03f,
			baseHealth = 442,
			baseMana = 215,
			speed = 330,
			baseArmor = 14.5f,
			baseMagicResist = 30,
			baseHealthRegen = 7.25f,
			baseManaRegen = 6.45f,
			baseAttackSpeed = 2,
			expWorth = 42,

			//Stats PerLevel;	
			damagePerLevel = 3.62f,
			healthPerLevel = 102,
			manaPerLevel = 38,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 1.25f,
			healthRegenPerLevel = 0.85f,
			manaRegenPerLevel = 0.45f
			
				
		});
		
		champions.Add("Anivia", new BasicInformation
		{
			//Base Stats;
			baseDamage = 48,
			baseHealth = 350,
			baseMana = 257,
			speed = 325,
			baseArmor = 10.5f,
			baseMagicResist = 30,
			baseHealthRegen = 4.65f,
			baseManaRegen = 7f,
			baseAttackSpeed = 1.8f,
			expWorth = 42,

			//Stats PerLevel;	
			damagePerLevel = 3.2f,
			healthPerLevel = 70,
			manaPerLevel = 53,
			armorPerLevel = 4f,
			magicResistPerLevel = 0f,
			healthRegenPerLevel = 0.55f,
			manaRegenPerLevel = 0.6f
	
		});
		
		champions.Add("Annie", new BasicInformation
		{
			//Base Stats;
			baseDamage = 48,
			baseHealth = 384,
			baseMana = 250,
			speed = 335,
			baseArmor = 12.5f,
			baseMagicResist = 30,
			baseHealthRegen = 4.5f,
			baseManaRegen = 6.9f,
			
			expWorth = 42,

			//Stats PerLevel;	
			damagePerLevel = 2.625f,
			healthPerLevel = 76,
			manaPerLevel = 50,
			armorPerLevel = 4f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.55f,
			manaRegenPerLevel = 0.6f
	
		});
		
		champions.Add("Ashe", new BasicInformation
		{
			//Base Stats;
			baseDamage = 46.3f,
			baseHealth = 395,
			baseMana = 173,
			speed = 325,
			baseArmor = 11.5f,
			baseMagicResist = 30,
			baseHealthRegen = 4.5f,
			baseManaRegen = 6.3f,
			
			expWorth = 42,

			//Stats PerLevel;	
			damagePerLevel = 2.85f,
			healthPerLevel = 79,
			manaPerLevel = 35,
			armorPerLevel = 3.4f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.55f,
			manaRegenPerLevel = 0.4f
			
		});
		
		champions.Add("Blitzcrank", new BasicInformation
		{
			//Base Stats;
			baseDamage = 55.66f,
			baseHealth = 423,
			baseMana = 260,
			speed = 325,
			baseArmor = 14.5f,
			baseMagicResist = 30,
			baseHealthRegen = 7.25f,
			baseManaRegen = 6.6f,
			
			expWorth = 42,

			//Stats PerLevel;	
			damagePerLevel = 3.5f,
			healthPerLevel = 95,
			manaPerLevel = 40,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 1.25f,
			healthRegenPerLevel = 0.75f,
			manaRegenPerLevel = 0.5f
			
		});
		
		champions.Add("Brand", new BasicInformation
		{
			//Base Stats;
			baseDamage = 51.66f,
			baseHealth = 380,
			baseMana = 250,
			speed = 340,
			baseArmor = 12,
			baseMagicResist = 30,
			baseHealthRegen = 4.5f,
			baseManaRegen = 7f,
			

			//Stats PerLevel;	
			damagePerLevel = 3f,
			healthPerLevel = 76,
			manaPerLevel = 45,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.55f,
			manaRegenPerLevel = 0.6f
			
		});
		
		champions.Add("Caitlyn", new BasicInformation
		{
			//Base Stats;
			baseDamage = 47f,
			baseHealth = 390,
			baseMana = 255,
			speed = 325,
			baseArmor = 13,
			baseMagicResist = 30,
			baseHealthRegen = 4.75f,
			baseManaRegen = 6.5f,
			

			//Stats PerLevel;	
			damagePerLevel = 3f,
			healthPerLevel = 80,
			manaPerLevel = 35,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.55f,
			manaRegenPerLevel = 0.55f
			
		});
		
		champions.Add("Cassiopeia", new BasicInformation
		{
			//Base Stats;
			baseDamage = 47,
			baseHealth = 380,
			baseMana = 250,
			speed = 335,
			baseArmor = 11.5f,
			baseMagicResist = 30,
			baseHealthRegen = 4.85f,
			baseManaRegen = 7.1f,
				
			//Stats PerLevel;	
						damagePerLevel = 3.2f,
			healthPerLevel = 75,
			manaPerLevel = 50,
			armorPerLevel = 4f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.5f,
			manaRegenPerLevel = 0.75f
			
		});
		
		champions.Add("Cho'Gath", new BasicInformation
		{
			//Base Stats;
			baseDamage = 54.1f,
			baseHealth = 440,
			baseMana = 205,
			speed = 345,
			baseArmor = 19f,
			baseMagicResist = 30,
			baseHealthRegen = 7.5f,
			baseManaRegen = 6.45f,
				
			//Stats PerLevel;	
						damagePerLevel = 4.2f,
			healthPerLevel = 80,
			manaPerLevel = 40,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 1.25f,
			healthRegenPerLevel = 0.85f,
			manaRegenPerLevel = 0.45f
			
		});
		
		champions.Add("Corki", new BasicInformation
		{
			//Base Stats;
			baseDamage = 48.2f,
			baseHealth = 375,
			baseMana = 243,
			speed = 325,
			baseArmor = 13.5f,
			baseMagicResist = 30,
			baseHealthRegen = 4.5f,
			baseManaRegen = 6.5f,
				
			//Stats PerLevel;	
						damagePerLevel = 3f,
			healthPerLevel = 82,
			manaPerLevel = 37,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.55f,
			manaRegenPerLevel = 0.55f
			
		});
		
		champions.Add("Darius", new BasicInformation
		{
			//Base Stats;
			baseDamage = 50f,
			baseHealth = 426,
			baseMana = 200,
			speed = 340,
			baseArmor = 20f,
			baseMagicResist = 30,
			baseHealthRegen = 8.25f,
			baseManaRegen = 6f,
				
			//Stats PerLevel;	
						damagePerLevel = 3.5f,
			healthPerLevel =93,
			manaPerLevel = 37.5f,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.95f,
			manaRegenPerLevel = 0.35f
			
		});
		
		champions.Add("Diana", new BasicInformation
		{
			//Base Stats;
			baseDamage = 48f,
			baseHealth = 438,
			baseMana = 230,
			speed = 345,
			baseArmor = 16f,
			baseMagicResist = 30,
			baseHealthRegen = 6f,
			baseManaRegen = 7f,
				
			//Stats PerLevel;	
						damagePerLevel = 3f,
			healthPerLevel = 90,
			manaPerLevel = 40,
			armorPerLevel = 3.6f,
			magicResistPerLevel = 1.25f,
			healthRegenPerLevel = 0.85f,
			manaRegenPerLevel = 0.6f
			
		});
		
		champions.Add("Dr.Mundo", new BasicInformation
		{
			//Base Stats;
			baseDamage = 56.23f,
			baseHealth = 433,
			baseMana = 0,
			speed = 345,
			baseArmor = 17,
			baseMagicResist = 30,
			baseHealthRegen = 6.5f,
			baseManaRegen = 0,
				
			//Stats PerLevel;	
						damagePerLevel = 3f,
			healthPerLevel = 89,
			manaPerLevel = 0,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 1.25f,
			healthRegenPerLevel = 0.75f,
			manaRegenPerLevel = 0.4f
			
		});
		
		champions.Add("Draven", new BasicInformation
		{
			//Base Stats;
			baseDamage = 46.5f,
			baseHealth =420,
			baseMana = 240,
			speed = 330,
			baseArmor = 16f,
			baseMagicResist = 30,
			baseHealthRegen = 5f,
			baseManaRegen = 6.95f,
				
			//Stats PerLevel;	
						damagePerLevel = 3.5f,
			healthPerLevel = 82,
			manaPerLevel = 42,
			armorPerLevel = 3.3f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.7f,
			manaRegenPerLevel = 0.65f
			
		});
		
		champions.Add("Elise", new BasicInformation
		{
			//Base Stats;
			baseDamage = 47.5f,
			baseHealth = 395,
			baseMana = 240,
			speed = 335,
			baseArmor = 12.5f,
			baseMagicResist = 30,
			baseHealthRegen = 4.7f,
			baseManaRegen = 6.8f,
				
			//Stats PerLevel;	
						damagePerLevel = 3f,
			healthPerLevel = 80,
			manaPerLevel = 50,
			armorPerLevel = 3.35f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.6f,
			manaRegenPerLevel = 0.65f
			
		});
		
		champions.Add("Evelynn", new BasicInformation
		{
			//Base Stats;
			baseDamage = 48f,
			baseHealth = 380,
			baseMana = 190,
			speed = 340,
			baseArmor = 12f,
			baseMagicResist = 30,
			baseHealthRegen = 8.9f,
			baseManaRegen = 7.1f,
				
			//Stats PerLevel;	
						damagePerLevel = 3.5f,
			healthPerLevel = 90,
			manaPerLevel = 45,
			armorPerLevel = 4f,
			magicResistPerLevel = 1.25f,
			healthRegenPerLevel = 0.55f,
			manaRegenPerLevel = 0.6f
			
		});
		
		champions.Add("Ezreal", new BasicInformation
		{
			//Base Stats;
			baseDamage = 47.2f,
			baseHealth = 350,
			baseMana = 235,
			speed = 330,
			baseArmor = 12f,
			baseMagicResist = 30,
			baseHealthRegen = 5.5f,
			baseManaRegen = 7,
				
			//Stats PerLevel;	
						damagePerLevel = 3f,
			healthPerLevel = 80,
			manaPerLevel = 45,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.55f,
			manaRegenPerLevel = 0.65f
			
		});
		
		champions.Add("Fiddlesticks", new BasicInformation
		{
			//Base Stats;
			baseDamage = 45.95f,
			baseHealth = 390,
			baseMana = 251,
			speed = 335,
			baseArmor = 11f,
			baseMagicResist = 30,
			baseHealthRegen = 4.6f,
			baseManaRegen = 6.0f,
				
			//Stats PerLevel;	
						damagePerLevel = 2.625f,
			healthPerLevel = 80,
			manaPerLevel = 59,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.6f,
			manaRegenPerLevel = 0.65f
			
		});
		
		champions.Add("Fiora", new BasicInformation
		{
			//Base Stats;
			baseDamage = 54.5f,
			baseHealth = 450,
			baseMana = 220,
			speed = 350,
			baseArmor = 15f,
			baseMagicResist = 30,
			baseHealthRegen = 5.5f,
			baseManaRegen = 6.75f,
				
			//Stats PerLevel;	
						damagePerLevel = 3.2f,
			healthPerLevel = 85,
			manaPerLevel = 40,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 1.25f,
			healthRegenPerLevel = 0.8f,
			manaRegenPerLevel = 0.5f
			
		});
		
		champions.Add("Fizz", new BasicInformation
		{
			//Base Stats;
			baseDamage = 53f,
			baseHealth = 414,
			baseMana = 200,
			speed = 335,
			baseArmor = 12.7f,
			baseMagicResist = 30,
			baseHealthRegen = 7f,
			baseManaRegen = 6.1f,
				
			//Stats PerLevel;	
						damagePerLevel = 3f,
			healthPerLevel = 86,
			manaPerLevel = 40,
			armorPerLevel = 3.4f,
			magicResistPerLevel = 1.25f,
			healthRegenPerLevel = 0.7f,
			manaRegenPerLevel = 0.45f
			
		});
		
		champions.Add("Galio", new BasicInformation
		{
			//Base Stats;
			baseDamage = 56.3f,
			baseHealth = 435,
			baseMana = 235,
			speed = 335,
			baseArmor = 17f,
			baseMagicResist = 30,
			baseHealthRegen = 7.45f,
			baseManaRegen = 7f,
				
			//Stats PerLevel;	
						damagePerLevel = 3.375f,
			healthPerLevel = 85,
			manaPerLevel = 50,
			armorPerLevel = 3.5f,
			magicResistPerLevel = 0,
			healthRegenPerLevel = 0.75f,
			manaRegenPerLevel = 0.7f
			
		});
		
		champions.Add("Gankplank", new BasicInformation
		{
			//Base Stats;
			baseDamage = 54f,
			baseHealth = 495,
			baseMana = 215,
			speed = 345,
			baseArmor = 16.5f,
			baseMagicResist = 30,
			baseHealthRegen = 4.25f,
			baseManaRegen = 6.5f,
				
			//Stats PerLevel;	
						damagePerLevel = 3f,
			healthPerLevel = 81,
			manaPerLevel = 40,
			armorPerLevel = 3.3f,
			magicResistPerLevel = 1.25f,
			healthRegenPerLevel = 0.75f,
			manaRegenPerLevel = 0.7f
			
		});
		
		champions.Add("Garen", new BasicInformation
		{
			//Base Stats;
			baseDamage = 52f,
			baseHealth = 455,
			baseMana = 0,
			speed = 345,
			baseArmor = 22f,
			baseMagicResist = 30,
			baseHealthRegen = 7.45f,
			baseManaRegen = 0f,
				
			//Stats PerLevel;	
						damagePerLevel = 3.5f,
			healthPerLevel = 96,
			manaPerLevel = 0,
			armorPerLevel = 2.7f,
			magicResistPerLevel = 1.25f,
			healthRegenPerLevel = 0.75f,
			manaRegenPerLevel = 0f
			
		});
	}
}