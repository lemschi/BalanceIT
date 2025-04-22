using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private GameObject[] goals;

    public static bool ballFell;
    public static int ballcount;
    public static int score;
    // Start is called before the first frame update
    void Start()
    {
        goals = GameObject.FindGameObjectsWithTag("goals");
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -20)
        {
            ballFell= true;
            ballcount--;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("goals"))
        {
            Destroy(gameObject);
            score++;
        }
    }
}
