using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using UnityEngine;

public class MPUScript : MonoBehaviour
{
    SerialPort mpudata_stream = new SerialPort("COM6", 115200);

    public float deadZone;

    private string mpuDatenRAW;
    private string mpuDatenTemp;
    private float mpuDatenTempFloat;

    internal static float[] mpuDaten = new float[3];//x y z
    internal static bool streamIsOpen;
    [SerializeField] private bool streamIsOpenUnityIsDeppat;

    private int counter = 0;
    private bool foundStart;

    //debug
    internal static string[] mpuDatenDebug = new string[3];//x y z


    void Start()

    {
        mpudata_stream.Open(); //Serial data stream wird hergestellt
        streamIsOpen = mpudata_stream.IsOpen;
        streamIsOpenUnityIsDeppat = streamIsOpen;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (streamIsOpen)
        {
            mpuDatenRAW = mpudata_stream.ReadLine();

            if (mpuDatenRAW != mpuDatenTemp)
                TranslateMpuStream(mpuDatenRAW);

            //Debug.Log(mpuDatenRAW);
        }
    }

    private void TranslateMpuStream(string newData)
    {
        mpuDatenTemp = newData;
        if (mpuDatenTemp == "GyroX:")
            foundStart = true;

        if (foundStart)
        {
            switch (counter)
            {
                case 1:
                    mpuDatenTempFloat = float.Parse(mpuDatenTemp) / 100 + 1;
                    if (mpuDatenTempFloat < deadZone && mpuDatenTempFloat > deadZone)
                    {
                        mpuDaten[0] = 0;
                        mpuDatenDebug[0] = mpuDatenTemp;
                    }
                    else
                    {
                        mpuDaten[0] = mpuDatenTempFloat;
                        mpuDatenDebug[0] = mpuDatenTemp;
                    }
                    break;
                case 3:
                    mpuDatenTempFloat = float.Parse(mpuDatenTemp) / 100 + 1;
                    if (mpuDatenTempFloat < deadZone && mpuDatenTempFloat > deadZone)
                    {
                        mpuDaten[1] = 0;
                        mpuDatenDebug[1] = mpuDatenTemp;
                    }
                    else
                    {
                        mpuDaten[1] = mpuDatenTempFloat;
                        mpuDatenDebug[1] = mpuDatenTemp;
                    }
                    break;
                case 5:
                    mpuDatenTempFloat = float.Parse(mpuDatenTemp) / 100 + 1;
                    if (mpuDatenTempFloat < deadZone && mpuDatenTempFloat > deadZone)
                    {
                        mpuDaten[2] = 0; 
                        mpuDatenDebug[2] = mpuDatenTemp;
                    }
                    else
                    {
                        mpuDaten[2] = mpuDatenTempFloat;
                        mpuDatenDebug[2] = mpuDatenTemp;
                    }
                    break;
                case 7:
                    counter = 0;
                    foundStart = false;
                    break;

            }
            counter++;
        }

        //UnityEngine.Debug.Log("--------------");
        //UnityEngine.Debug.Log(mpuDaten[0]);
        //UnityEngine.Debug.Log(mpuDaten[1]);
        //UnityEngine.Debug.Log(mpuDaten[2]);
        //UnityEngine.Debug.Log("--------------");
        
        UnityEngine.Debug.Log("--------------");
        UnityEngine.Debug.Log(mpuDatenDebug[0]);
        UnityEngine.Debug.Log(mpuDatenDebug[1]);
        UnityEngine.Debug.Log(mpuDatenDebug[2]);
        UnityEngine.Debug.Log("--------------");
        
    }
}
