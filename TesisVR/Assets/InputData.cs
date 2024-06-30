using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Fusion;

public class InputData : NetworkBehaviour
{

    public InputDevice _rightController;
    public InputDevice _leftController;


    public override void FixedUpdateNetwork()
    {
        Updating();
    }

    void Updating()
    {
        if (!_rightController.isValid || !_leftController.isValid)
        {
            InitializeInputDevices();
        }
    }

    private void InitializeInputDevices()
    {
        if (!_rightController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref _rightController);
        if (!_leftController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref _leftController);
    }

    private void InitializeInputDevice(InputDeviceCharacteristics inputCharecteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputCharecteristics, devices);

        if (devices.Count > 0)
        {
            inputDevice = devices[0];
        }
    }
}