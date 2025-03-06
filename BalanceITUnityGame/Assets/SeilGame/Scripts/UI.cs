using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    GameObject balanceBoardImg;
    // Start is called before the first frame update
    void Start()
    {
        balanceBoardImg = GameObject.Find("BalanceBoardStance");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BalanceBoardStance()
    {
        balanceBoardImg.transform.rotation = Quaternion.Euler(0, 0, MPUScript.mpuDaten[0]);
    }
}
