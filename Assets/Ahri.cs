using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ahri : ChampionSkills
{
    public Skill[] skillPrefabs;

    float deltaTime;

    public override void Init()
    {
        skills = new SkillState[skillPrefabs.Length];

        //Instantiate all skillPrefabs
        skillInstances = new GameObject[skillPrefabs.Length];

        thisChampion = GetComponent<Champion>();
        if (!thisChampion.anim)
        {
            thisChampion.Init();
        }

        int i = 0;

        foreach (Skill g in skillPrefabs)
        {
            skillInstances[i] = Instantiate(g.skillPrefab);
            i++;
        }

        skills[0] = new AhriQ(gameObject, thisChampion, thisChampion.anim, skillInstances[0]);
        skills[1] = new AhriW(gameObject, thisChampion, thisChampion.anim, skillInstances[1]);
        skills[2] = new AhriE(gameObject, thisChampion, thisChampion.anim, skillInstances[2]);
        skills[3] = new AhriR(gameObject, thisChampion, thisChampion.anim, skillInstances[3]);

        qIcon = GameObject.Find("Q").transform.GetChild(0).GetComponent<Image>();
        wIcon = GameObject.Find("W").transform.GetChild(0).GetComponent<Image>();
        eIcon = GameObject.Find("E").transform.GetChild(0).GetComponent<Image>();
        rIcon = GameObject.Find("R").transform.GetChild(0).GetComponent<Image>();


        qIcon.fillAmount = (skills[0].counter / skills[0].cooldown);
        wIcon.fillAmount = (skills[1].counter / skills[1].cooldown);
        eIcon.fillAmount = (skills[2].counter / skills[2].cooldown);
        rIcon.fillAmount = (skills[3].counter / skills[3].cooldown);

    }

    void Update()
    {
        deltaTime = Time.deltaTime;

        foreach (SkillState s in skills)
        {
            s.counter += Time.deltaTime;
        }

        qIcon.fillAmount = (skills[0].counter / skills[0].cooldown);
        wIcon.fillAmount = (skills[1].counter / skills[1].cooldown);
        eIcon.fillAmount = (skills[2].counter / skills[2].cooldown);
        rIcon.fillAmount = (skills[3].counter / skills[3].cooldown);
    }

    public override void Q()
    {
        skills[0].Execute(null, deltaTime);
    }

    public override void W()
    {
        skills[1].Execute(null, deltaTime);
    }

    public override void E()
    {
        skills[2].Execute(null, deltaTime);
    }

    public override void R()
    {
        skills[3].Execute(null, deltaTime);
    }

    public override bool QOnCooldown()
    {
        return skills[0].OnCooldown();
    }

    public override bool WOnCooldown()
    {
        return skills[1].OnCooldown();
    }

    public override bool EOnCooldown()
    {
        return skills[2].OnCooldown();
    }
    public override bool ROnCooldown()
    {
        return skills[3].OnCooldown();
    }
}
