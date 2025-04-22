using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSript : MonoBehaviour
{
    //euler 90 = unity 0,7071068 => /127,2792172271572

    [SerializeField] private float moveSpeed;
    [SerializeField] private int secondsToDie;
    [SerializeField] private float windMultiplier;

    private Animator animator;
    private GameObject ropeConPoint;
    private GameObject startPlatform;
    private GameObject endPlatform;
    private Rigidbody rBody;
    private Collider aCollider;

    [SerializeField]public static float angle;

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
    private int p = 100;


    //Mpu integration
    [SerializeField] private float mpuAccelerator = 2.5f;
    [SerializeField] public static float realAngle;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount= 0;
        Application.targetFrameRate = 60;

        //i kenn neamd der so guaden code schreibt, fui suppa gmocht he <3
        ropeConPoint = GameObject.Find("RopeConPoint");
        startPlatform = GameObject.Find("StartPlatform");
        endPlatform = GameObject.Find("EndPlatform");
        rBody = gameObject.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        aCollider = GetComponent<Collider>();

        ropeConPoint.transform.position = new Vector3(startPlatform.transform.position.x, 0.5f, startPlatform.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------------------------------------------BaseControls--------------------------------------------------------------
        //angle keyboard
        if (!MPUScriptNew.streamIsOpen)
        {
            if (p==100)
            {
                Debug.Log("keyboard input");
                p = 0;
            }
            if (p == 10)
            {
                Debug.Log(realAngle);
                Debug.Log(angle);
            }
            p++;
            if (Input.GetKey(KeyCode.D))
            {
                Debug.Log("druckst D");
                realAngle -= 2f;
                angle -= 2f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                realAngle += 2f;
                angle += 2f;
            Debug.Log("druckst A");
            }
            else
            {
                if (ropeConPoint.transform.rotation.z > 0)
                {
                    realAngle = 0.01f;
                }
                else
                {
                    realAngle = -0.01f;
                }
            }
        }

        //angle change board
        if (MPUScriptNew.streamIsOpen && isMoving)
        {
            if (p == 100)
            {
                Debug.Log("balanceboard input");
                p = 0;
            }
            p++;
            realAngle = MPUScriptNew.mpuDaten[0];
            realAngle -= 0.4f;
            realAngle *= mpuAccelerator;
            angle = realAngle;
        }
        else if(MPUScriptNew.streamIsOpen && !isMoving)
        {
            angle = 0;
        }

        //
        //resetConditions
        if (criticalState/50>10 || Input.GetKeyDown(KeyCode.X))
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
            if (realAngle > 0)
            {
                angle = 0;
                ropeConPoint.transform.rotation = Quaternion.Euler(0, 0, 35.6381f);
            }
            else
            {
                angle = realAngle;
            }
            
        }
        if (ropeConPoint.transform.rotation.z < -0.28)
        {
            if (realAngle < 0)
            {
                angle = 0;
                ropeConPoint.transform.rotation = Quaternion.Euler(0, 0, -35.6381f);
            }
            else
            {
                angle = realAngle;
            }
        }
        //mitzön wie lang er so is
        if (ropeConPoint.transform.rotation.z > 0.28 || ropeConPoint.transform.rotation.z < -0.28)
        {
            criticalState++;
        }

        if (criticalState>=secondsToDie*50)
        {
            isDead = true;
            aCollider.enabled = false;
        }

        //apply external angle changes
        ropeConPoint.transform.Rotate(0, 0, (angle + angleChange) * Time.deltaTime);

        //check if level finished
        if (ropeConPoint.transform.position.z == endPlatform.transform.position.z)
        {
            reachedEnd = true;
            isMoving = false;
            rBody.freezeRotation = false;
            ropeConPoint.transform.rotation = new Quaternion(0,0,0,0);
            transform.SetLocalPositionAndRotation(new Vector3(0f, 1.5f, 0f), new Quaternion(0, 0, 0, 0));
        }

        //start movement
        if (Input.GetKeyDown(KeyCode.Q))
        {
            levelStarted = true;
            isMoving = true;
        }
        //movement forward
        if (!reachedEnd && isMoving)
            ropeConPoint.transform.position = Vector3.MoveTowards(ropeConPoint.transform.position, new Vector3(ropeConPoint.transform.position.x, ropeConPoint.transform.position.y, endPlatform.transform.position.z), moveSpeed);


        //Feature--------------------------------------------------------------Wind--------------------------------------------------------------
        {
            if (windActive)
            {
                CreateWind(1);
            }
            if (!windActive)
            {
                CreateWind(-1);
            }
        }

    }

    void ResetPlayer()
    {
        Debug.Log("get restetted bitch");
        reachedEnd= false;
        ropeConPoint.transform.SetLocalPositionAndRotation(new Vector3(startPlatform.transform.position.x, 0.5f, startPlatform.transform.position.z), new Quaternion(0, 0, 0, 0));
        transform.SetLocalPositionAndRotation(new Vector3(0f, 1.5f, 0f), new Quaternion(0, 0, 0, 0));
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
