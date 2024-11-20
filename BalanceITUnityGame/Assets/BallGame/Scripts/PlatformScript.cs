using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private GameObject plain;
    private GameObject plain2;

    public DefaultPreset Platform;
    // Start is called before the first frame update
    void Start()
    {
        plain = GameObject.FindGameObjectWithTag("platform");
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            plain.transform.Rotate(plain.transform.rotation.x, 0, plain.transform.rotation.z + 1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            plain.transform.Rotate(plain.transform.rotation.x, 0, plain.transform.rotation.z - 1);
        }
        if (Input.GetKey(KeyCode.W))
        {
            plain.transform.Rotate(plain.transform.rotation.x + 1, 0, plain.transform.rotation.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            plain.transform.Rotate(plain.transform.rotation.x - 1, 0, plain.transform.rotation.z);
        }
    }
    */
    private void FixedUpdate()
    {
        //x y z w
        if (!BallScript.ballFell)
        {
            //plain.transform.rotation = new Quaternion(MPU.qx, MPU.qy, MPU.qz, 0);
        }
    }

    private void NextPlatform()
    {

    }
}
