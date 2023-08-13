using ArduinoBluetoothAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BLDeviceListMono : MonoBehaviour
{

    BluetoothHelper bluetoothHelper;
    public List<BLDevicePairedBasicInfo> m_devicePaired = new List<BLDevicePairedBasicInfo>();

    void Start()
    {
        bluetoothHelper = BluetoothHelper.GetInstance();
        RefreshListOfPairedDevice();
    }

    [ContextMenu("Refresh List of Devices")]
    private void RefreshListOfPairedDevice()
    {
        m_devicePaired.Clear();
        LinkedList<BluetoothDevice> ds = bluetoothHelper.getPairedDevicesList();

        foreach (BluetoothDevice d in ds)
        {
            Debug.Log($"{d.DeviceName} {d.DeviceAddress}");
            m_devicePaired.Add(new BLDevicePairedBasicInfo(d.DeviceName, d.DeviceAddress));

        }
    }
}
