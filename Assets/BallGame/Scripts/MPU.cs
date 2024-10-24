using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class MPU : MonoBehaviour
{
    SerialPort stream = new SerialPort("COM6", 115200);


    public string strRecieved;
    public string[] strData = new string[4];
    public string[] strData_recived = new string[4];
    public static float rotation, qx, qy, qz;

    // Start is called before the first frame update
    void Start()
    {
        stream.Open();
    }

    // Update is called once per frame
    void Update()
    {
        strRecieved = stream.ReadLine();

        strData = strRecieved.Split(',');
        if (strData[0] != "" && strData[1] != "" && strData[2] != "" && strData[3] != "") //homa daten griagt
        {
            strData_recived[0] = strData[0];
            strData_recived[1] = strData[1];
            strData_recived[2] = strData[2];
            strData_recived[3] = strData[3];

            rotation = float.Parse(strData_recived[0]);
            qx = float.Parse(strData_recived[1]);
            qz = float.Parse(strData_recived[2]);
            qy = float.Parse(strData_recived[3]);
        }
    }
}
