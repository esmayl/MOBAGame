using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Data/Skill", order = 1)]
public class Skill : ScriptableObject
{
    public string championName;
    public GameObject skillPrefab;
    public int damage;
}
