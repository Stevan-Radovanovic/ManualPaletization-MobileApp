using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundPlaneRand : MonoBehaviour
{
    static string boxName = "";

    static GameObject[] positionBoxes = new GameObject[7];

    static GameObject currBox = null;
    static GameObject positionBox = null;
    static int index = -1;

    public static bool boxLoaded = false;

    static string checkBoxChange = "";

    //test

    //DELEGAT
    public delegate void BoxScannedEventHandler(object sender, GameObject gameObject, int ind);

    //DEFINICA DOGADJAJA
    public event BoxScannedEventHandler BoxScanned;

    //PODIZANJE DOGADJAJA
    protected virtual void OnBoxScanned()
    {
        if (BoxScanned != null && index > -1)
            BoxScanned(this, positionBox, index);
        index = -1;
    }

    public static int[] LoadBoxLevels()
    {
        int[] res = new int[7];
        for (int i = 0; i < 7; i++)
        {
            if (positionBoxes[i]!=null && (positionBoxes[i].transform.position.y - positionBoxes[i].GetComponent<Renderer>().bounds.size.y / 2) > 0)
            {
                res[i] = 1;
            }
            else res[i] = 0;
        }
        return res;
    }

    public static void LoadBoxes()
    {
        for (int i = 0; i < 7; i++)
        {
            positionBoxes[i] = GameObject.Find($"k{i + 1}position");
        }
        boxLoaded = true;
    }

    public static void HideBoxes()
    {
        for (int i = 1; i < 8; i++)
        {
            string s = $"k{i}";
            Hide(s);
        }
    }
    public static void OnObjectFound(object source, string detectedObjectName, GameObject gameObject)
    {
       
        if (boxName != detectedObjectName)
        {
            boxName = detectedObjectName;

            currBox = gameObject;

            positionBox = positionBoxes[int.Parse("" + boxName[1]) - 1];

            index = int.Parse("" + boxName[1]) - 1;
        }
    }

    public static void Hide(string boxName)
    {
        GameObject box = GameObject.Find(boxName + "position");
        if (box != null) box.SetActive(false);

    }

    public void Show(string boxName)
    {
        GameObject box = positionBoxes[int.Parse("" + boxName[1]) - 1];

        if (box != null && positionBox != null)
        {
            //box.SetActive(true);

            BoxScanned += DimenzijeRand.OnBoxScanned;
            OnBoxScanned();
            BoxScanned -= DimenzijeRand.OnBoxScanned;
        }

    }
        
    // Start is called before the first frame update
    void Start()
    {
            boxName = "";
            LoadBoxes();
            HideBoxes();
    }

    // Update is called once per frame
    void Update()
    {
        if (boxName != checkBoxChange)
        {
            Show(boxName);
            checkBoxChange = boxName;
        }

    }    
}
