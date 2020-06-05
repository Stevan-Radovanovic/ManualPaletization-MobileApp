using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SphereMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;
    

    [SerializeField]
    Text gameOver;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, vertical);
        this.transform.position += movement * Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadSceneAsync("SampleScene");
        }
    }

    
}
