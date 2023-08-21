using ArduinoBluetoothAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLDeviceListMono : MonoBehaviour
{

    BluetoothHelper bluetoothHelper;
    public List<BLDevicePairedBasicInfo> m_devicePaired = new List<BLDevicePairedBasicInfo>();

    void Start()
    {
        bluetoothHelper = BluetoothHelper.GetInstance();
        RefreshListOfPairedDevice();
    }
    public void Test() {
    
    }

    [ContextMenu("Refresh List of Devices")]
    public  void RefreshListOfPairedDevice()
    {
        m_devicePaired.Clear();
        LinkedList<BluetoothDevice> ds = bluetoothHelper.getPairedDevicesList();

        foreach (BluetoothDevice d in ds)
        {
           // Debug.Log($"{d.DeviceName} {d.DeviceAddress}");
            m_devicePaired.Add(new BLDevicePairedBasicInfo(d.DeviceName, d.DeviceAddress));

        }
    }
    public void SearchDeviceByName(string name, out bool found, out BLDevicePairedBasicInfo device)
    {
        SearchDeviceByName(name, 0, out found, out device);
    }

        public void SearchDeviceByName(string name, int indexInSearch, out bool found, out BLDevicePairedBasicInfo device) {

        found = false;
        device = null;
        int index = 0;

        for (int i = 0; i < m_devicePaired.Count; i++)
        {
            if (m_devicePaired[i].m_name.Trim().ToLower() == name.Trim().ToLower())
            {

                if (index == indexInSearch)
                {
                    found = true;
                    device = m_devicePaired[i];
                    return;
                }
                index++;
            }
        }
        for (int i = 0; i < m_devicePaired.Count; i++)
        {
            if (m_devicePaired[i].m_macAddress.Trim()== name.Trim())
            {
                    found = true;
                    device = m_devicePaired[i];
                    return;
            }
        }
    }

    public void SearchDeviceByName(string name, out List< BLDevicePairedBasicInfo> devices)
    {
        devices = new List<BLDevicePairedBasicInfo>();


        for (int i = 0; i < m_devicePaired.Count; i++)
        {
            if (m_devicePaired[i].m_name.Trim().ToLower() == name.Trim().ToLower())
            {
                devices.Add(m_devicePaired[i]);
            }
            if (m_devicePaired[i].m_macAddress.Trim() == name.Trim())
            {
                devices.Add(m_devicePaired[i]);
            }
        }
        
    }
}
