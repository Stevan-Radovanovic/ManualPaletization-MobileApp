using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class DimenzijeTower : MonoBehaviour
{
    //public static Text importantText;

    [SerializeField]
    TextAsset xmlFile;

    [SerializeField]
    Material red;

    [SerializeField]
    Material green;

    
    Dictionary<string, Vector3> boxes = new Dictionary<string, Vector3>();

    static string boxName = "";
    static GameObject currBox = null;
    static GameObject positionBox = null;

    static bool[] checkBoxes = new bool[7];
    static int[] floorBoxes = new int[7];
    static int positionBoxIndex = -1;

    static int floorNum = 0;

    private static void ResetFlags()
    {
        for (int i = 0; i < 7; i++)
        {
            checkBoxes[i] = false;
        }
    }

    private static void LoadFloors()
    {
        //while (GroundPlaneTower.boxLoaded == false) ;
        floorBoxes = GroundPlaneTower.LoadBoxLevels();
        for (int i = 0; i < 7; i++)
        {
            Debug.Log(floorBoxes[i]);
        }

    }

    private static void ResetFloors()
    {
        for (int i = 0; i < 7; i++)
        {
            floorBoxes[i] = -1;
        }
    }

    private static bool CheckFloor(int floor)
    {
        bool res = true;
        if (floor != 0)
        {
            for (int i = 0; i < 7; i++)
            {
                if (floorBoxes[i] < floor && checkBoxes[i] == false)
                {
                    res = false;
                    break;
                }
            }
        }

        return res;
    }

    private void LoadDimensions()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFile.text);

        var allBoxes = xmlDoc.DocumentElement.GetElementsByTagName("MultiTarget");

        foreach (XmlElement box in allBoxes)
        {

            var name = box.GetAttribute("name");

            float x = 0, y = 0, z = 0;
            string frontD = name + ".Front";
            string rightD = name + ".Right";


            var allDimensions = xmlDoc.DocumentElement.GetElementsByTagName("ImageTarget");

            foreach (XmlElement dimension in allDimensions)
            {
                if (dimension.GetAttribute("name").Equals(frontD))
                {
                    string val = dimension.GetAttribute("size");
                    string[] xy = val.Split(' ');

                    x = float.Parse(xy[0]);
                    y = float.Parse(xy[1]);
                }

                if (dimension.GetAttribute("name").Equals(rightD))
                {
                    string val = dimension.GetAttribute("size");
                    string[] zz = val.Split(' ');

                    z = float.Parse(zz[0]);
                }

            }

            x = (float)Math.Round(x, 3);
            y = (float)Math.Round(y, 3);
            z = (float)Math.Round(z, 3);

            boxes.Add(name, new Vector3(x, y, z));
        }

    }
    
    public static void OnBoxScanned(object groundPlane, GameObject posBox, int index)
    {
        Debug.Log("GOOD scanned");

        if (positionBoxIndex != index && checkBoxes[index] == false)
        {
            positionBox = posBox;
            positionBoxIndex = index;

            string boxName = (positionBox.name.Substring(0, 2)).ToUpper();

            Debug.Log(boxName);

            int boxNameIndex = Convert.ToInt32(boxName[1]) - 48;

            positionBox.SetActive(true);

            /*
            if (floorBoxes[index] == floorNum)
            {
                Debug.Log("Completed!");
                positionBox.SetActive(true);
                //if (checkBoxes[boxNameIndex] == false)
                    //importantText.text = $"Box {boxName} scanned! Match it to its position";
            }
            else
            {
                //UI Message
                //importantText.text = "This box cannot be scanned yet, scan another one!";
                Debug.Log("Not completed!");
                positionBox = null;
                currBox = null;
            }
            */
        }  
    }
    
    public static void OnObjectFound(object sender, string detectedObjectName, GameObject gameobject)
    {
        Debug.Log("GOOD");
        if (currBox == null && checkBoxes[int.Parse(detectedObjectName[1]+"")-1]==false )
        {
            boxName = detectedObjectName;
            currBox = gameobject;
            positionBoxIndex = -1;
        }
    }
    
    public void MatchFound(GameObject box)
    {
        Renderer rend = box.GetComponent<Renderer>();
        //Set the main Color of the Material to green
        rend.material = green;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //GameObject go = GameObject.Find("Canvas");
        //Debug.Log(go.name);
        //importantText = go.GetComponentInChildren<Text>();
        LoadDimensions();
        LoadFloors();
        ResetFlags();
    }
        

    // Update is called once per frame
    void Update()
    {
        if (currBox != null && positionBox != null)
        {
            if (Math.Abs(currBox.transform.position.x - positionBox.transform.position.x) < 0.05f &&
                Math.Abs(currBox.transform.position.y - positionBox.transform.position.y) < 0.05f &&
                Math.Abs(currBox.transform.position.z - positionBox.transform.position.z) < 0.05f)
            {
                //Debug.Log("Match.");
                Quaternion check = Quaternion.FromToRotation(currBox.transform.position, positionBox.transform.position);

                if (Math.Abs(check.x) < 0.01f && Math.Abs(check.y) < 0.01f && Math.Abs(check.z) < 0.01f)
                {
                    MatchFound(positionBox);
                    checkBoxes[positionBoxIndex] = true;
                    Debug.Log("Match.");

                    //Scan next message

                    positionBox.SetActive(false);
                    //positionBox.transform.position = new Vector3(positionBox.transform.position.x, positionBox.transform.position.y + positionBox.transform.localScale.y, positionBox.transform.position.z);
                    positionBox.GetComponent<Renderer>().material = red;
                    floorBoxes[positionBoxIndex]++;

                    positionBoxIndex = -1;
                    currBox = null;
                    positionBox = null;
                    
                    //importantText.text = "Box matched! Scan your next box!";
                }
               
            }

        }

        if (CheckFloor(floorNum + 1))
        {
            floorNum++;
            ResetFlags();
            GroundPlaneTower.MoveUp();
            GroundPlaneTower.LoadBoxes();
            GroundPlaneTower.HideBoxes();
            
        }

        /*int currentFloor = 1;
        if(CheckFloor(currentFloor)==true)
        {
            currentFloor++;
           //GroundPlane.positionBoxes;
        }
        */
    }

}
