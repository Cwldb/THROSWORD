using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    public bool Active = false;
    [Header("�Ƿ翧")]
    public int SlideEA = 10;
    public float SlideTime = 0.1f;

    [Header("�ܻ� RGB ����")]
    public float RedMin = 0;
    public float RedMax = 255;
    public float GreenMin = 0;
    public float GreenMax = 255;
    public float BlueMin = 0;
    public float BlueMax = 255;

    GameObject Bank;
    List<GameObject> SilhouetteList = new List<GameObject>();
    int Limit = 0;
    int SlideNow = 0;
    float delta = 0;
    bool ErrorDebug = false;

    private void Awake()
    {
        if (!GetComponent<SpriteRenderer>())
        {
            Debug.Log("Not Find SpriteRender. from Move_Slide for " + gameObject.name);
            ErrorDebug = true;
        }
    }

    void SlideCreate()
    {
        if (!Bank)
        {
            Bank = new GameObject(gameObject.name + " SilhouetteListList Bank");
            
            if (SlideNow > SilhouetteList.Count)
            {
                for (int i = SilhouetteList.Count; SlideNow > i; i++)
                {
                    GameObject SpriteCopy = new GameObject(transform.gameObject.name + " SilhouetteList " + i);
                    SpriteCopy.transform.parent = Bank.transform;
                    SpriteCopy.AddComponent<SpriteRenderer>();
                    SilhouetteList.Insert(i, SpriteCopy);
                }
            }
        }
    }

    void DefaultSet()
    {
        delta = 0;
        Limit = 0;

        SilhouetteList.Clear();
        Destroy(Bank);
    }

    void Update()
    {
        if (SlideNow != SlideEA)
        {
            SlideNow = SlideEA;
            DefaultSet();
        }

        if (ErrorDebug && SlideNow > 0)
            return;


        SlideCreate();
        delta += Time.deltaTime;

        if (delta > SlideTime)
        {
            delta = 0;
            if (Active)
            {
                if (SilhouetteList.Count > 0)
                {
                    SilhouetteList[Limit].transform.position = transform.position;
                    SilhouetteList[Limit].transform.position += new Vector3(0, 0, 1);
                    SilhouetteList[Limit].GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                    SilhouetteList[Limit].GetComponent<SpriteRenderer>().sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
                    SilhouetteList[Limit].transform.localScale = transform.localScale;

                    float R = Random.Range(RedMin, RedMax), G = Random.Range(GreenMin, GreenMax), B = Random.Range(BlueMin, BlueMax);
                    SilhouetteList[Limit].GetComponent<SpriteRenderer>().color = new Color(R / 255, G / 255, B / 255, 1);

                    Limit++;
                    if (Limit > SilhouetteList.Count - 1)
                    {
                        Limit = 0;
                    }
                }
            }

            for (int i = 0; SilhouetteList.Count > i; i++)
            {
                SilhouetteList[i].GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1f / SilhouetteList.Count);
            }
        }
    }
}