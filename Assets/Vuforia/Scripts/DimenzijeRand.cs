using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class DimenzijeRand : MonoBehaviour
{
    public static Text importantText;
    public static bool groundPlaneFixed;

    [SerializeField]
    TextAsset xmlFile;

    [SerializeField]
    Material red;

    [SerializeField]
    Material green;

    //Saves dimensions for all boxes in the database
    Dictionary<string, Vector3> boxes = new Dictionary<string, Vector3>();

    //MultiTargets - Actual Boxes
    static string boxName = "";
    static GameObject currBox = null;
    static GameObject[] allBoxes = new GameObject[7];

    //Cubes - Position Boxes
    static GameObject positionBox = null;
    //Checks if box is rightly positioned (Used for checking box level/floor)
    static bool[] checkBoxes = new bool[7];
    //Saves the corresponding level/floor for each box
    static int[] floorBoxes = new int[7];

    //Current box to be matched
    static int currBoxIndex = -1;

    //Checks if the user has finished scanning the boxes
    static bool scanFinished = false;
    static bool justOneScan = false;
    //Checks if the previous box is placed
    static bool boxMatched = true;
  
    //Pointer to the next boxed to be placed
    static int next = 0;

    public void GroundPlaneFixed ()
    {
        groundPlaneFixed = true;
    }

    public void SetScanFinished()
    {
        scanFinished = true;
        importantText.text = "Scanning finished!";
    }

    private static void ResetAllBoxes()
    {
        for (int i = 0; i < 7; i++)
        {
            allBoxes[i] = null;
        }

    }

    private static void ResetFlags()
    {
        for (int i = 0; i < 7; i++)
        {
            checkBoxes[i] = false;
        }
    }

    private static void LoadFloors()
    {
        while (GroundPlaneRand.boxLoaded == false) ;
        floorBoxes = GroundPlaneRand.LoadBoxLevels();
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
                if (allBoxes[i] != null && floorBoxes[i] < floor && checkBoxes[i] == false)
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
        if (scanFinished == true)
        {
            
            if (currBoxIndex == index && index == (next-1))
            { 
                if (CheckFloor(floorBoxes[index]))
                {
                    checkBoxes[index] = true;
                    positionBox = posBox;
                    positionBox.SetActive(true);
                    currBoxIndex = index;
                    importantText.text = "Box scanned! Match it to its position";
                }
                else
                {
                  
                    //Debug.Log("Previous floor not completed!");
                    posBox.SetActive(false);
                } 
            }
        }
    
    }

    public static void OnObjectFound(object sender, string detectedObjectName, GameObject gameobject)
    {
        if (scanFinished == false)
        {
            boxName = detectedObjectName;
            currBox = gameobject;
            if (allBoxes[int.Parse(boxName[1] + "") - 1] == null)
            {
                allBoxes[int.Parse(boxName[1] + "") - 1] = currBox;
            
                Debug.Log(boxName + " scanned.");
            }
        }
        else
        {
            if (boxName != detectedObjectName)
            {
                boxName = detectedObjectName;
                currBox = gameobject;
                Debug.Log(boxName + " detected.");
            }
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
        GameObject go = GameObject.Find("Canvas");
        Debug.Log(go.name);
        importantText = go.GetComponentInChildren<Text>();

        LoadDimensions();
        LoadFloors();
        ResetAllBoxes();
        ResetFlags();
    }

    string boxNameSwitch (int i)
    {

        switch (i)
        {
            case 1:
                return "Kozel";
             
            case 2:
                return "Tuborg";
                
            case 3:
                return "Multivita";
                
            case 4:
                return "Bambi";
                
            case 5:
                return "Nesquik";
                
            case 6:
                return "Snickers";
                
            case 7:
                return "Guinness";
            default:
                return "";
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (justOneScan == false && scanFinished == false)
        {
            string pomocniImportant = "";

            for (int i = 0; i < 7; i++)
            {
                if (allBoxes[i] != null)
                {
                    pomocniImportant += boxNameSwitch(i + 1);
                    pomocniImportant += ',';
                }
            }
            importantText.text = "Scanned: ";
            importantText.text += pomocniImportant;
            importantText.text = importantText.text.Substring(0, importantText.text.Length - 1);
        }

        if (justOneScan == false && scanFinished==true)
        {
            next = 0;
            justOneScan = true;
            importantText.text = "All boxes successfully scanned! Fix your pallete!";
            //Debug.Log("All boxes successfully scanned.");
        }

        if (scanFinished && boxMatched && groundPlaneFixed)
        {
            while (next < 7 && allBoxes[next] == null) next++;
            if (next < 7)
            {
                next++;
                importantText.text = "Scan this box: " + boxNameSwitch(next);
                Debug.Log("Scan this box: k" + next);
                currBoxIndex = next - 1;
                boxMatched = false;
                currBox = null;
            }
            else
            {
                Debug.Log("Congrats! You have finished!");
            }
            
        }

        if (currBox != null && positionBox != null)
        {
            if (currBoxIndex != (next - 1) && currBoxIndex == -1)
            {
                //Debug.Log("That's not the right box!");
            }
            else
            {
                if (Math.Abs(currBox.transform.position.x - positionBox.transform.position.x) < 0.05f &&
                Math.Abs(currBox.transform.position.y - positionBox.transform.position.y) < 0.05f &&
                Math.Abs(currBox.transform.position.z - positionBox.transform.position.z) < 0.05f)
                {
                    Quaternion check = Quaternion.FromToRotation(currBox.transform.position, positionBox.transform.position);
                    //Debug.Log(currBox.transform.rotation + " BOX /" +  positionBox.transform.rotation + " RED /" + check + " Check");

                    if (Math.Abs(check.x) < 0.01f && Math.Abs(check.y) < 0.01f && Math.Abs(check.z) < 0.01f
                        /*&& Math.Abs(currBox.transform.rotation.eulerAngles.x - positionBox.transform.rotation.eulerAngles.x) < 0.1f
                        && Math.Abs(currBox.transform.rotation.eulerAngles.y - positionBox.transform.rotation.eulerAngles.y) < 0.1f
                        && Math.Abs(currBox.transform.rotation.eulerAngles.z - positionBox.transform.rotation.eulerAngles.z) < 0.1f*/)
                    {
                        Debug.Log("Match.");
                        MatchFound(positionBox);
                        currBoxIndex = -1;
                        boxMatched = true;
                        importantText.text = "Box matched!";
                    }
                    //else Debug.Log("Rotation does not match.");
                }
            }


        } 
    }

}
