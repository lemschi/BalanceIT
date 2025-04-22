using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class MPUScriptNew : MonoBehaviour
{
    SerialPort mpudata_stream = new SerialPort("COM11", 115200);

    internal static string[] mpuDatenString = new string[3];//x y z accel
    internal static float[] mpuDaten = new float[3];//x y z accel
    internal static bool streamIsOpen;
    [SerializeField] private bool streamIsOpenUnityIsDeppat;
    [SerializeField] public float mpuDecelerator;

    // Start is called before the first frame update
    void Start()
    {
        mpudata_stream.ReadTimeout = 500;
        mpudata_stream.Open(); //Serial data stream wird hergestellt
        streamIsOpen = mpudata_stream.IsOpen;
        streamIsOpenUnityIsDeppat = streamIsOpen;
    }

    // Update is called once per frame
    void Update()

    {
        mpuDatenString = mpudata_stream.ReadLine().Split(',');

        for (int i = 0; i < 3; i++)
        {
            mpuDaten[i] = float.Parse(mpuDatenString[i]) / 50;
        }
        mpuDaten[2] /= 10;

        //Debug.Log(mpuDatenString[0]);
        Debug.Log(mpuDaten[0]);
        //Debug.Log(mpuDatenString[1]);
        Debug.Log(mpuDaten[1]);
        //Debug.Log(mpuDatenString[2]);
        Debug.Log(mpuDaten[2]);
    }

    void OnApplicationQuit()
    {
        if (streamIsOpen)
        {
            mpudata_stream.Close();
        }
    }
}
