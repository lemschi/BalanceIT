using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    GameObject balanceBoardImg;
    // Start is called before the first frame update
    void Start()
    {
        string etwas = MPUScriptNew.mpuDaten[1].ToString();
        balanceBoardImg = GameObject.Find("BalanceBoardStance");
    }

    // Update is called once per frame
    void Update()
    {
        BalanceBoardStance();
    }

    void BalanceBoardStance()
    {
        balanceBoardImg.transform.rotation = Quaternion.Euler(0, 0, MPUScriptNew.mpuDaten[0] * 3f);
        //balanceBoardImg.transform.rotation = Quaternion.Euler(0, 0, PlayerSript.angle);
        //balanceBoardImg.transform.rotation.z + PlayerSript.angle
    }
}
