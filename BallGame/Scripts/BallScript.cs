using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private GameObject[] goals;
    private GameObject platform;


    private int count;
    internal static bool ballFell;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 70, transform.position.z);
        goals = GameObject.FindGameObjectsWithTag("goals");
        platform = GameObject.FindGameObjectWithTag("platform");
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -20)
        {
            transform.position = new Vector3(0, 10, 0);
            ballFell= true;
            count = 0;
            
        }
        if (ballFell)
        {
            print(count);
            if (count <= 60)
            {
                count++;
                platform.transform.Rotate(0, 3, 0);
            }
            else 
            { 
                count = 0;
                ballFell= false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("goals"))
        {
            gameObject.SetActive(false);
        }
    }
}
