using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionSkills : MonoBehaviour
{
    public SkillState[] skills;
    
    [HideInInspector]
    public GameObject[] skillInstances;

    [HideInInspector]
    public Champion thisChampion;

    [HideInInspector]
    public Image qIcon;

    [HideInInspector]
    public Image wIcon;

    [HideInInspector]
    public Image eIcon;

    [HideInInspector]
    public Image rIcon;

    [HideInInspector]
    public Text levelText;

    [HideInInspector]
    public Image expBar;

    public virtual void Init()
    {

    }

    public virtual void Q()
    {

    }

    public virtual void W()
    {

    }

    public virtual void E()
    {

    }

    public virtual void R()
    {

    }

    public virtual bool QOnCooldown()
    {
        return false;
    }

    public virtual bool WOnCooldown()
    {
        return false;
    }

    public virtual bool EOnCooldown()
    {
        return false;
    }

    public virtual bool ROnCooldown()
    {
        return false;
    }
}
