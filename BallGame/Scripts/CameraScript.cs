using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject platform;
    // Start is called before the first frame update
    void Start()
    {
        platform = GameObject.FindGameObjectWithTag("platform");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, platform.transform.rotation.y, 0);
    }
}
