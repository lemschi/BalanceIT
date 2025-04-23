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

    //calibration
    [SerializeField] public int calibrationIterationsToDo = 50;
    private int calibrationIterations = 0;
    private float[] calibrationValues = new float[3];
    private bool calibrationFinished = false;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        mpudata_stream.ReadTimeout = 500;
        mpudata_stream.Open(); //Serial data stream wird hergestellt
        streamIsOpen = mpudata_stream.IsOpen;
        streamIsOpenUnityIsDeppat = streamIsOpen;
    }

    // Update is called once per frame
    void Update()
    {
        

        mpuDatenString = mpudata_stream.ReadLine().Split(',');
        if (calibrationFinished)
        {
            mpuDaten[0] = (float.Parse(mpuDatenString[0]) - calibrationValues[0]) / 50;
            mpuDaten[1] = (float.Parse(mpuDatenString[1]) - calibrationValues[1]) / 50;
            mpuDaten[2] = float.Parse(mpuDatenString[2]);
            /*
            for (int i = 0; i < 3; i++)
            {
                mpuDaten[i] = float.Parse(mpuDatenString[i]) / 50;
            }
            mpuDaten[2] /= 10;
            */
        }
        else
        {
            Calibration();
        }

        //Debug.Log(mpuDatenString[0]);
        //Debug.Log(mpuDaten[0]);
        //Debug.Log(mpuDatenString[1]);
        //Debug.Log(mpuDaten[1]);
        //Debug.Log(mpuDatenString[2]);
        //Debug.Log(mpuDaten[2]);

        //calibration
        if (Input.GetKeyDown(KeyCode.P) && calibrationFinished)
        {
            Debug.Log("calibration started");
            calibrationFinished = false;
        }
    }

    private void Calibration()
    {
        if (calibrationIterations % 10 == 0)
        {
            Debug.Log(calibrationIterations);
        }

        
        if (float.TryParse(mpuDatenString[2], out mpuDaten[2]))
        {
            calibrationValues[0] += float.Parse(mpuDatenString[0]);
            calibrationValues[1] += float.Parse(mpuDatenString[1]);
            calibrationIterations++;
        }

        //finishing calibration
        if (calibrationIterations == calibrationIterationsToDo)
        {
            calibrationValues[0] = calibrationValues[0] / calibrationIterations;
            calibrationValues[1] = calibrationValues[1] / calibrationIterations;

            calibrationIterations = 0;
            calibrationFinished = true;

            Debug.Log("calibrations values(x,y):");
            Debug.Log(calibrationValues[0]);
            Debug.Log(calibrationValues[1]);
        }
    }

    void OnApplicationQuit()
    {
        if (streamIsOpen)
        {
            mpudata_stream.Close();
        }
    }
}
