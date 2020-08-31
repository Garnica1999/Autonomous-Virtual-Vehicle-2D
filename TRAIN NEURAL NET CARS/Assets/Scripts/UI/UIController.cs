using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Text HorizontalInput;
    [SerializeField]
    private Text VerticalInput;
    [SerializeField]
    private Text Velocity;

    public UIController Instance {
        get
        {
            return this;
        }
    }

    private void Update()
    {
        UpdateValues();
    }

    void UpdateValues()
    {
        double[] carOutputs = CarController.Instance.CarMovement.CarOutputs;
        double HorizontalInput = carOutputs[0];
        double VerticalInput = carOutputs[1];
        float velocity = CarController.Instance.CarMovement.Velocity;

        string format = "#.######";

        this.HorizontalInput.text = HorizontalInput.ToString();
        this.VerticalInput.text = VerticalInput.ToString();
        this.Velocity.text = velocity.ToString();
    }
}
