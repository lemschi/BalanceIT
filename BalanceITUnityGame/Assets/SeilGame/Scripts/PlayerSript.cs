using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int secondsToDie;
    [SerializeField] private float windMultiplier;

    private Animator animator;
    private GameObject ropeConPoint;
    private GameObject startPlatform;
    private GameObject endPlatform;
    private Rigidbody rBody;
    private Collider aCollider;

    [SerializeField]private float angle;

    private bool isMoving;
    private bool levelStarted;
    private bool isExtremeWind;
    private bool reachedEnd = false;
    private bool windActive;
    private bool isDead;
    private float angleChange;
    private float windSpeed;
    private int l = 0;
    private int criticalState;

    //Mpu integration
    [SerializeField] private float mpuDeaccelerator = 5;
    [SerializeField] private float realAngle;

    // Start is called before the first frame update
    void Start()
    {
        //i kenn neamd der so guaden code schreibt, fui suppa gmocht he <3
        ropeConPoint = GameObject.Find("RopeConPoint");
        startPlatform = GameObject.Find("StartPlatform");
        endPlatform = GameObject.Find("EndPlatform");
        rBody = gameObject.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        aCollider = GetComponent<Collider>();

        ropeConPoint.transform.position = startPlatform.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------------------------------------------BaseControls--------------------------------------------------------------
        //angle keyboard
        if (!MPUScript.streamIsOpen)
        {
            if (transform.position.y >= 0.5)
            {
                if (Input.GetKey(KeyCode.D))
                {
                    angle -= 0.2f;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    angle += 0.2f;
                }
                else
                {
                    if (ropeConPoint.transform.rotation.z > 0)
                    {
                        angle = 0.01f;
                    }
                    else
                    {
                        angle = -0.01f;
                    }
                }
            }
            else
                angle = 0; //so u dont do looptiloops

            Debug.Log("Keyboard input");
        }

        //angle change board
        if (MPUScript.streamIsOpen)
        {
            realAngle += (float)Math.Round((MPUScript.mpuDaten[1] / mpuDeaccelerator), 1);
            realAngle = (float)Math.Round(realAngle, 2);
            if (transform.position.y >= 0.5)
            {
                angle = realAngle;
            }
        }

        //
        //resetConditions
        if (transform.position.y <= -5||Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("LevelRestarted", false);
            ResetPlayer();
        }
        //stop movement when dead
        if (isDead)
        {
            isMoving = false;
        }


        //Feature--------------------------------------------------------------Wind--------------------------------------------------------------

        if (Input.GetKeyDown(KeyCode.T))
        {
            windActive = true;
            
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            windActive = false;

            CreateWind(-1);
        }




        
        //Animation var setting
        animator.SetBool("LevelFinished", reachedEnd);
        animator.SetBool("LevelStarted", levelStarted);
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsDead", isDead);
        animator.SetFloat("AnglePlayerF", ropeConPoint.transform.rotation.z);
        animator.SetFloat("TimeUntilDeathF", (float)criticalState/50);
        animator.SetFloat("WindSpeedF", windSpeed);
    }

    //50/s
    private void FixedUpdate()
    {
        //--------------------------------------------------------------BaseControls--------------------------------------------------------------
        //dass a si ned weiter draht
        if (ropeConPoint.transform.rotation.z > 0.28)
        {
            //ropeConPoint.transform.rotation = Quaternion.Euler(ropeConPoint.transform.rotation.x, ropeConPoint.transform.rotation.y, 29f);
            ropeConPoint.transform.rotation = new Quaternion(ropeConPoint.transform.rotation.x, ropeConPoint.transform.rotation.y, 0.28f, 0);

            if (angle > 0)
                angle = 0;
        }
        if (ropeConPoint.transform.rotation.z < -0.28)
        {
            //ropeConPoint.transform.rotation = Quaternion.Euler(ropeConPoint.transform.rotation.x, ropeConPoint.transform.rotation.y, -29f);
            ropeConPoint.transform.rotation = new Quaternion(ropeConPoint.transform.rotation.x, ropeConPoint.transform.rotation.y, -0.28f, 0);
            if (angle < 0)
                angle = 0;
        }
        //mitzön wie lang er so is
        if (ropeConPoint.transform.rotation.z > 0.27 || ropeConPoint.transform.rotation.z < -0.27)
        {
            criticalState++;
        }

        if (criticalState>=secondsToDie*50)
        {
            isDead = true;
            aCollider.enabled = false;
        }

        //Debug.Log(criticalState);
        //apply external angle changes
        ropeConPoint.transform.Rotate(0, 0, (angle + angleChange) * Time.deltaTime);

        //check if level finished
        if (transform.position.z == endPlatform.transform.position.z)
        {
            reachedEnd = true;
            isMoving = false;
            rBody.freezeRotation = false;
        }

        //movement
        if (Input.GetKeyDown(KeyCode.Q))
        {
            levelStarted = true;
            isMoving = true;
        }

        if (!reachedEnd && isMoving)
            ropeConPoint.transform.position = Vector3.MoveTowards(ropeConPoint.transform.position, new Vector3(ropeConPoint.transform.position.x, ropeConPoint.transform.position.y, endPlatform.transform.position.z), moveSpeed);


        //Feature--------------------------------------------------------------Wind--------------------------------------------------------------
        if (windActive)
        {
            CreateWind(1);
        }
        if (!windActive)
        {
            CreateWind(-1);
        }


    }

    void ResetPlayer()
    {
        reachedEnd= false;
        ropeConPoint.transform.SetLocalPositionAndRotation(startPlatform.transform.position, new Quaternion(0, 0, 0, 0));
        transform.SetLocalPositionAndRotation(new Vector3(0f, 1.5f, 0f), new Quaternion(0, 0, 0, 0));
        angle = 0;
        rBody.freezeRotation = true;
        criticalState= 0;
        aCollider.enabled = true;
        isDead = false;
        //reset animator
        animator.SetBool("LevelRestarted", true);
    }

    private void CreateWind(int multiplier)//multiplier = 1 create Wind, -1 remove wind
    {
        if (multiplier==1 && windSpeed == 0)
        {
            windSpeed = UnityEngine.Random.Range(-1f, 1f);
            windSpeed = (float)Math.Round(windSpeed, 2);
            windSpeed *= windMultiplier;

            if (windSpeed == 1 || windSpeed == -1)
            {
                isExtremeWind = true;
            }

        }
        float anglePerFrame = windSpeed / 50;

        //increase wind
        if (windActive && l != 50)
        {
            l++;
            angleChange = anglePerFrame * l;
        }
        //decrease wind
        if (!windActive && l != 0)
        {
            l--;
            angleChange = anglePerFrame * l;
        }
        //reset Wind Speed
        if (!windActive && l == 0)
        {
            windSpeed = 0;
        }

        //Debug.Log(windSpeed);
        //Debug.Log(angleChange);
    }

}
