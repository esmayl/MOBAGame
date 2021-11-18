using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public Image hpBarPrefab;

    public static UIHandler instance;

    List<Image> hpBars = new List<Image>();
    List<Transform> transformsToFollow = new List<Transform>();
    Vector3 defaultScale;
    int index = 0;
    Camera cachedCam;
    Vector2 temp;
    Vector2 uiOffset;
    RectTransform canvasRect;

    void Awake()
    {
        instance = this;
        defaultScale = hpBarPrefab.transform.localScale;
        canvasRect = GetComponent<RectTransform>();
        
        uiOffset = new Vector2((float)canvasRect.sizeDelta.x / 2f, (float)canvasRect.sizeDelta.y / 2f); ;
    }

    void Start()
    {
        cachedCam = Camera.main;
    }

    void Update()
    {
        index = 0;

        foreach(Image img in hpBars)
        {
            if (!transformsToFollow[index]) { continue; }

            temp = cachedCam.WorldToViewportPoint(transformsToFollow[index].position);

            temp = new Vector2(temp.x * canvasRect.sizeDelta.x, temp.y * canvasRect.sizeDelta.y);
            uiOffset.x = (float)canvasRect.sizeDelta.x / 2f;
            uiOffset.y = (float)canvasRect.sizeDelta.y / 2f;

            img.rectTransform.localPosition = temp - uiOffset+new Vector2(-cachedCam.scaledPixelWidth * 0.025f, cachedCam.scaledPixelHeight * 0.03f);
            index++;
        }
    }


    public int AddHpBar(Transform transformToFollow)
    {
        hpBars.Add(Instantiate(hpBarPrefab, transform));
        transformsToFollow.Add(transformToFollow);
        return hpBars.Count-1;
    }

    public void UpdateHp(int id,float newHpPercentage)
    {
        hpBars[id].transform.GetChild(0).transform.localScale = new Vector3(newHpPercentage * defaultScale.x, defaultScale.y, defaultScale.z);
        
        if(newHpPercentage == 1)
        {
            hpBars[id].gameObject.SetActive(true);
        }

        if (newHpPercentage <= 0)
        {
            hpBars[id].gameObject.SetActive(false);
        }

    }
}
