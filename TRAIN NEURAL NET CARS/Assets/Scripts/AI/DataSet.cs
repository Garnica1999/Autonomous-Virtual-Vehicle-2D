using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSet
{
    public double[] Values { get; set; }
    public double[] Targets { get; set; }

    public DataSet(double[] values, double[] targets)
    {
        Values = values;
        Targets = targets;
    }
}
