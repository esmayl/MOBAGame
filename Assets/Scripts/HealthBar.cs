using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public int myId;

    void Start()
    {
        myId = UIHandler.instance.AddHpBar(transform);
        Debug.Log(transform.name +" "+transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHpBar(float newHpPercentage)
    {
        UIHandler.instance.UpdateHp(myId, newHpPercentage);
    }
}
