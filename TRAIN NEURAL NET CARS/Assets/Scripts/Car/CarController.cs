using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public bool ModeDataset = true;
    public CarMovement CarMovement {
        get;
        private set;
    }

    public static CarController Instance
    {
        get;
        private set;
    }

    private File file;
    private Sensor[] sensors;
    private NeuralNet NN;
    private bool entrenado = false;
    private float TimeSave;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        this.CarMovement = new CarMovement(this.gameObject);
        this.sensors = GetComponentsInChildren<Sensor>();

        this.file = new File("Dataset", "Dataset");
        this.file.CreateDataset();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.CarMovement.Initialize();
        foreach(Sensor s in sensors)
        {
            s.Show();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.TimeSave += Time.deltaTime;
        double[] sensorsOutputs = new double[this.sensors.Length];

        for(int i = 0; i < sensorsOutputs.Length; i++)
        {
            sensorsOutputs[i] = this.sensors[i].Output;
        }

        if (ModeDataset)
        {
            this.CarMovement.MoveCar();
            if (this.TimeSave >= 0.5f)
            {
                this.file.SaveData(this.gameObject, sensorsOutputs);
                this.TimeSave = 0f;
            }
            
        }
        else
        {
            if (!entrenado)
            {
                this.InstantiateNeuralNet();
                this.entrenado = true;
            }
            float Velocity = this.CarMovement.Velocity;
            float XPosition = this.gameObject.transform.position.x;
            float YPosition = this.gameObject.transform.position.y;

            List<double> XPred = new List<double>();

            foreach(double so in sensorsOutputs)
            {
                XPred.Add(so);
            }
            XPred.Add(Velocity);
            XPred.Add(XPosition);
            XPred.Add(YPosition);
            double[] xPredArr =  XPred.ToArray();

            double[] outputs = this.NN.Compute(xPredArr);

            this.CarMovement.horizontalInput = outputs[0];
            this.CarMovement.verticalInput = outputs[1];

            Debug.Log(sensorsOutputs[0] + "-" + sensorsOutputs[1] + "-" + sensorsOutputs[2] + "-" + sensorsOutputs[3] + "-" + sensorsOutputs[4] + "-" + Velocity + "-" + XPosition + "-" + YPosition);

            Debug.Log(outputs[0] + "-" + outputs[1]);

            this.CarMovement.ApplyInput();
            this.CarMovement.ApplyVelocity();
            this.CarMovement.ApplyFriction();
        }
    }

    void InstantiateNeuralNet()
    {
        Debug.Log("Iniciando modo de entrenamiento...");
        int numInputs = 8;
        int numHiddenLayer = 3;
        int hiddenSize = 4;
        int numOutputs = 2;

        Debug.Log("Iniciando red neuronal");
        this.NN = new NeuralNet(numInputs, hiddenSize, numOutputs, numHiddenLayer);

        Debug.Log("Abriendo archivo");
        Matrix<double> ds = this.file.openFile();

        Debug.Log("Partiendo el dataset...");
        List<DataSet> datasets = this.file.SplitDataset(ds, new int[] { 0, numInputs });

        Debug.Log("Entrenando el modelo");
        this.NN.Train(datasets, 2500);
    }
}
