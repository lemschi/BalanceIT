using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using UnityEngine;

public class MPUScript : MonoBehaviour
{
    SerialPort mpudata_stream = new SerialPort("COM6", 115200);

    [SerializeField] public float deadZone;
    //[SerializeField] public float standardDriftX = 3;
    //[SerializeField] public float standardDriftY = 0.5f;
    //[SerializeField] public float standardDriftZ = 0.5f;

    private string mpuDatenRAW;
    private string mpuDatenTemp;
    private float mpuDatenTempFloat;

    internal static float[] mpuDaten = new float[3];//x y z
    internal static bool streamIsOpen;
    [SerializeField] private bool streamIsOpenUnityIsDeppat;

    private int counter = 0;
    private bool foundStart;

    //calibration
    [SerializeField] public int calibrationIterationsToDo = 200;
    private int calibrationIterations = 0;
    private float[] calibrationValues = new float[3];
    private bool calibrationFinished = false;

    //debug
    internal static string[] mpuDatenDebug = new string[3];//x y z


    void Start()
    {
        mpudata_stream.Open(); //Serial data stream wird hergestellt
        streamIsOpen = mpudata_stream.IsOpen;
        streamIsOpenUnityIsDeppat = streamIsOpen;

        calibrationValues[0] = 0;
        calibrationValues[1] = 0;
        calibrationValues[2] = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P) && calibrationFinished)
        {
            UnityEngine.Debug.Log("calibration started");
            calibrationFinished = false;
        }

        if (!calibrationFinished)
        {
            if (calibrationIterations <= calibrationIterationsToDo)
            {
                StartCalibration();
                calibrationIterations++;
            }
        }


        if (streamIsOpen && calibrationFinished)
        {
            mpuDatenRAW = mpudata_stream.ReadLine();

           //UnityEngine.Debug.Log(mpuDatenRAW);

            if (mpuDatenRAW != mpuDatenTemp)
                TranslateMpuStream(mpuDatenRAW);

        }
    }

    private void StartCalibration()
    {
        if (mpuDatenRAW == "Start")
            foundStart = true;

        if (calibrationIterations % 10 == 0)
        {
            UnityEngine.Debug.Log(calibrationIterations);
        }

        if (foundStart)
        {
            switch (counter)
            {
                case 1:
                    calibrationValues[0] += float.Parse(mpuDatenRAW);
                    break;
                case 2:
                    calibrationValues[1] += float.Parse(mpuDatenRAW);
                    break;
                case 3:
                    calibrationValues[2] += float.Parse(mpuDatenRAW);
                    break;
                case 4:
                    counter = -1;
                    foundStart= false;
                    break;
            }
            counter++;
        }

        if (calibrationIterations == calibrationIterationsToDo)
        {
            calibrationValues[0] = calibrationValues[0] / calibrationIterationsToDo;
            calibrationValues[1] = calibrationValues[1] / calibrationIterationsToDo;
            calibrationValues[2] = calibrationValues[2] / calibrationIterationsToDo;

            calibrationIterations = 0;
            calibrationFinished = true;

            UnityEngine.Debug.Log("calibrations values(x,y,z):");
            UnityEngine.Debug.Log(calibrationValues[0]);
            UnityEngine.Debug.Log(calibrationValues[1]);
            UnityEngine.Debug.Log(calibrationValues[2]);
        }

    }


    private void TranslateMpuStream(string newData)
    {
        mpuDatenTemp = newData;
        if (mpuDatenTemp == "Start")
            foundStart = true;

        //UnityEngine.Debug.Log("temp;" + mpuDatenTemp);
        UnityEngine.Debug.Log("MPU 1");

        if (foundStart)
        {
            UnityEngine.Debug.Log("MPU 2");
            switch (counter)
            {
                case 1:
                    mpuDatenTempFloat = float.Parse(mpuDatenTemp) / 100 - calibrationValues[0];
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
                case 2:
                    mpuDatenTempFloat = float.Parse(mpuDatenTemp) / 100 - calibrationValues[1];
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
                    //UnityEngine.Debug.Log(mpuDaten[1]);
                    break;
                case 3:
                    mpuDatenTempFloat = float.Parse(mpuDatenTemp) / 100 - calibrationValues[2];
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
                case 4:
                    counter = -1;
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
        /*
        UnityEngine.Debug.Log("--------------");
        UnityEngine.Debug.Log(mpuDatenDebug[0]);
        UnityEngine.Debug.Log(mpuDatenDebug[1]);
        UnityEngine.Debug.Log(mpuDatenDebug[2]);
        UnityEngine.Debug.Log("--------------");
        */
    }
}
