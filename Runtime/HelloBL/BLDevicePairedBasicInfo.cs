using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BLDevicePairedBasicInfo
{
    public string m_name;
    public string m_macAddress;

    public BLDevicePairedBasicInfo()
    {
    }

    public BLDevicePairedBasicInfo(string name, string macAddres)
    {
        m_name = name;
        m_macAddress = macAddres;
    }
}


[System.Serializable]
public class BLDeviceGroup {

    public List<BLDevicePairedBasicInfo> m_devices = new List<BLDevicePairedBasicInfo>();


    public void SearchFor(string macAddress, out bool found, out BLDevicePairedBasicInfo device) {
        macAddress = macAddress.Trim();

        for (int i = 0; i < m_devices.Count; i++)
        {
            if (m_devices[i].m_macAddress == macAddress) {
                found = true;
                device = m_devices[i];
                return;
            }

        }
        found = false;
        device = null;
    }

    public void Add(BLDevicePairedBasicInfo device)
    {

        RemoveByAddress(device.m_macAddress);
    }
    public void Add(string name , string macAddress)
    {
        macAddress = macAddress.Trim();
        Add(new BLDevicePairedBasicInfo(name, macAddress));
    }

    public void RemoveDoubleByAddress() {
        m_devices = m_devices.GroupBy(x => x.m_macAddress).Select(y => y.FirstOrDefault()).ToList();
    }

    public void RemoveByAddress(string macAddress)
    {
        macAddress= macAddress.Trim();
           m_devices = m_devices.Where(k => k.m_macAddress != macAddress).ToList();
    }
    public void RemoveAllByName(string name)
    {
        m_devices = m_devices.Where(k => k.m_name != name).ToList();
    }
}