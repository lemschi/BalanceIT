using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] public int wantedBallCount = 1;

    private int ballCount;
    private GameObject ball;
    private GameObject platform;
    private Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = GameObject.Find("BallSpawnPoint").transform;
        ball = GameObject.Find("Ball");
        platform = GameObject.Find("Plane");
    }

    // Update is called once per frame
    void Update()
    {
        //get ballcount from ballscript to check of one fell down and if so spawn new one
        ballCount = BallScript.ballcount;
        if (ballCount < wantedBallCount)
        {
            GameObject newBall = Instantiate(ball, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity);
            newBall.GetComponent<Rigidbody>().useGravity = true;
            ballCount++;
            BallScript.ballcount = ballCount;
        }
    }
}
