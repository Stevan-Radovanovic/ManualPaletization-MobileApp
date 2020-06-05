using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pallet : MonoBehaviour
{
    GameObject pallet;
    // Start is called before the first frame update
    void Start()
    {
        pallet = GameObject.Find("Plane Finder");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
