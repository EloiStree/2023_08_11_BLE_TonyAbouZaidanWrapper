using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArduinoBluetoothAPI;
using System;
using System.Text;

public class HelloBLTest : MonoBehaviour
{
 
	// Use this for initialization
	BluetoothHelper bluetoothHelper;
	public string received_message;

	public string m_deviceMacAddress= "98:D3:34:91:2C:9E";
	public void SendMessageToDevice(string message) {
		bluetoothHelper.SendData(message);
	}



	public List<BLDevicePairedBasicInfo> m_devicePaired = new List<BLDevicePairedBasicInfo>();

	

	void Start()
	{
		try
        {
            BluetoothHelper.BLE = false;
            bluetoothHelper = BluetoothHelper.GetInstance();
            bluetoothHelper.setDeviceAddress(m_deviceMacAddress);
            bluetoothHelper.OnConnected += OnConnected;
            bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
            bluetoothHelper.OnDataReceived += OnMessageReceived;
            bluetoothHelper.setTerminatorBasedStream("\n");

            RefreshListOfPairedDevice();
        }
        catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

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

	public bool m_useDebugRelease=true;
	void OnMessageReceived(BluetoothHelper helper)
	{
		received_message = helper.Read();
		if(m_useDebugRelease)
			Debug.Log(received_message);
	}

	void OnConnected(BluetoothHelper helper)
	{
		try
		{
			helper.StartListening();
			Debug.Log("Start Listening: "+ helper.getDeviceAddress());
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}

	}

	void OnConnectionFailed(BluetoothHelper helper)
	{
		Debug.Log("Connection Failed");
	}


	public void TriggerConnect()
	{
		if (bluetoothHelper != null)
			if (bluetoothHelper.isDevicePaired())
				bluetoothHelper.Connect(); 
	}
	public void TriggerDiconnect()
	{
		if (bluetoothHelper != null)
				bluetoothHelper.Disconnect();

	}

	public void SendText(string text)
	{
		if (bluetoothHelper != null)
		    bluetoothHelper.SendData(text);
	}
	

	void OnDestroy()
	{
		TriggerDiconnect();
	}
}


