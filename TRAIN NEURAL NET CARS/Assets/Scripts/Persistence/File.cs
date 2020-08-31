using MathNet.Numerics.LinearAlgebra;
using NumSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

public class File
{
    private string path;
    private string[] columns;

    private string directory;
    private string nameDataset;

    private const string SEPARATOR = ",";

    public File(string directory, string nameDataset)
    {
        this.directory = directory;
        this.nameDataset = nameDataset;

        this.path = directory + "\\" + nameDataset + ".csv";


        this.columns = null;
    }

    public Matrix<double> openFile()
    {
        bool primeraVez = true;
        int rows = this.GetLines() - 1;
        int cols = 0;
        int countRows = 0;
        Matrix<double> data = null;
        using (var reader = new StreamReader(this.path))
        {
            while (!reader.EndOfStream)
            {

                var line = reader.ReadLine();
                string[] values = line.Split(',');
                if (primeraVez)
                {
                    this.columns = values;
                    cols = this.columns.Length;
                    data = Matrix<double>.Build.Dense(rows, cols, 0);
                }
                else
                {
                    for (int index = 0; index < values.Length; index++)
                    {
                        data[countRows, index] =  Convert.ToDouble(values[index]);
                    }
                    countRows++;
                }
                primeraVez = false;
            }
        }
        return data;
    }

    public int GetLines()
    {
        List<string> listA = new List<string>();
        using (var reader = new StreamReader(this.path))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                listA.Add(values[0]);
            }
        }
        //Console.WriteLine(listA.Count);
        return listA.Count;
    }

    public List<DataSet> SplitDataset(Matrix<double> dataset, int[] XColumns)
    {

        List<DataSet> tuples = new List<DataSet>();
        int rows = dataset.RowCount;
        for (int i = 0; i < rows; i++)
        {

            NDArray row = np.array(dataset.Row(i).ToArray());
            int[] YColumns = { XColumns[1], row.Shape[0] };

            String Xslide = XColumns[0] + ":" + XColumns[1];
            String Yslide = YColumns[0] + ":" + YColumns[1];

            double[] inputs = row[Xslide].ToArray<double>();
            double[] outputs = row[Yslide].ToArray<double>();

            tuples.Add(new DataSet(inputs, outputs));
        }

        return tuples;

    }

    public NDArray CopyToNDArray(Matrix<double> matrix)
    {
        NDArray newMatrix = np.empty(matrix.RowCount, matrix.ColumnCount);
        for (int i = 0; i < newMatrix.Shape[0]; i++)
        {
            for (int j = 0; i < newMatrix.Shape[1]; j++)
            {
                newMatrix[i, j] = matrix[i, j];
            }
        }
        return newMatrix;
    }

    public Vector<double> GetColumnFromDataset(Matrix<double> dataset, int index)
    {
        return dataset.Column(index);
    }

    public void CreateDataset()
    {
        this.CreateDirectory(directory);

        if (!this.ExistFile())
        {
            string contents = "";

            contents = "Sensor1" + SEPARATOR +
                "Sensor2" + SEPARATOR +
                "Sensor3" + SEPARATOR +
                "Sensor4" + SEPARATOR +
                "Sensor5" + SEPARATOR +
                "Velocity" + SEPARATOR +
                "PositionX" + SEPARATOR +
                "PositionY" + SEPARATOR +
                "HorizontalInput" + SEPARATOR +
                "VerticalInput" + Environment.NewLine;

            System.IO.File.AppendAllText(this.path, contents);
        }
    }

    public void SaveData(GameObject car, double[] sensorsOutputs)
    {
        CarController carController = car.GetComponent<CarController>();
        double[] CarOutputs = carController.CarMovement.CarOutputs;

        string contents = sensorsOutputs[0] + SEPARATOR +
            sensorsOutputs[1] + SEPARATOR +
            sensorsOutputs[2] + SEPARATOR +
            sensorsOutputs[3] + SEPARATOR +
            sensorsOutputs[4] + SEPARATOR +
            carController.CarMovement.Velocity + SEPARATOR +
            car.transform.position.x + SEPARATOR +
            car.transform.position.y + SEPARATOR +
            CarOutputs[0] + SEPARATOR +
            CarOutputs[1] + Environment.NewLine;

        System.IO.File.AppendAllText(this.path, contents);


    }

    public void CreateDirectory(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }


    public bool ExistFile()
    {
        return System.IO.File.Exists(this.path);
    }
}
