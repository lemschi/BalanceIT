using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    [SerializeField] internal float mpuDecelerator = 10;
    [SerializeField] internal float maxLean = 11;
    float mpuX;
    float mpuZ;

    internal GameObject plane;

    //euler 90 = unity 0,7071068 => /127.2792172271572
    void Start()
    {
        plane = GameObject.Find("Plane");
    }
    private void FixedUpdate()
    {
        mpuX = MPUScriptNew.mpuDaten[0];
        mpuZ = MPUScriptNew.mpuDaten[1];


        if (plane != null)
        {
            MainMovement();
        }

        //reset
        if (Input.GetKeyDown(KeyCode.X))
        {
            transform.SetLocalPositionAndRotation(new Vector3(0,0,0), new Quaternion(0,0,0,0));
        }
    }
    void MainMovement()
    {
        //rotation x Axis
        if (transform.rotation.x <= maxLean / 127.2792172271572 && transform.rotation.x >= -maxLean / 127.2792172271572)
        {
            transform.Rotate(mpuX / mpuDecelerator, 0, 0);
        }
        else //check if player is trying to go back if so let him else stop rotation
        {
            if (transform.rotation.x <= 10 / 127.2792172271572 && mpuX > 0 || transform.rotation.x >= -10 / 127.2792172271572 && mpuX < 0)
                transform.Rotate(mpuX / mpuDecelerator, 0, 0);
        }
        //rotation z Axis
        if (transform.rotation.z <= maxLean / 127.2792172271572 && transform.rotation.z >= -maxLean / 127.2792172271572)
        {
            transform.Rotate(0, 0, mpuZ / mpuDecelerator);
        }
        else //check if player is trying to go back if so let him else stop rotation
        {
            if (transform.rotation.z <= 10 / 127.2792172271572 && mpuZ > 0 || transform.rotation.z >= -10 / 127.2792172271572 && mpuZ < 0)
                transform.Rotate(0, 0, mpuZ / mpuDecelerator);
        }
        //hinder y rotation
        transform.SetLocalPositionAndRotation(transform.position, new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w));

        //transform.Rotate(MPUScriptNew.mpuDaten[0] / mpuDecelerator, 0, MPUScriptNew.mpuDaten[1] / mpuDecelerator);
    }
}
