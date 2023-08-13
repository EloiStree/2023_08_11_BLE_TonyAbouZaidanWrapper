using ArduinoBluetoothAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;

public class BLSendDevicesManagerMono : MonoBehaviour
{

	

    public bool m_autoCreate = true;
	public Dictionary<string, BLDeviceConnectionNative> m_deviceBLE = new Dictionary<string, BLDeviceConnectionNative>();
	public List<BLDeviceConnectionNative> m_debugList = new List<BLDeviceConnectionNative>();

	public bool  m_useAuthorizedRequest=false;
	public bool m_useExceptionLog;
	void Awake()
	{
		if (m_useAuthorizedRequest) 
		{ 
			#if UNITY_2020_2_OR_NEWER
				#if UNITY_ANDROID
						if ( !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_SCAN")
						  || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADVERTISE")
						  || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_CONNECT"))
							Permission.RequestUserPermissions(new string[] {
					"android.permission.BLUETOOTH_SCAN",
					"android.permission.BLUETOOTH_ADVERTISE",
					"android.permission.BLUETOOTH_CONNECT"
				  });
				#endif
			#endif
		}
	}

	public void CreateConnection(string macAddress)
    {
        if (!IsDeviceConnectionExist(macAddress))
        {
			BLDeviceConnectionNative device = new BLDeviceConnectionNative(macAddress);
			device.m_listenToException = m_useExceptionLog;
			m_deviceBLE.Add(
				macAddress,
			   device
				) ;
			
			m_debugList = m_deviceBLE.Values.ToList();
		}
    }

    private bool IsDeviceConnectionExist(string macAddress)
    {
        return m_deviceBLE.ContainsKey(macAddress);
    }

	public void SendMessageToDevice(string macAddress, string message)
	{
		if (m_autoCreate && !IsDeviceConnectionExist(macAddress))
		{
			CreateConnection(macAddress);
		}
		m_deviceBLE[macAddress].SendMessageToDevice(message);
	}
	public void SendMessageToDevice( MacAddressBluetoothMessage message )
	{
		SendMessageToDevice(message.m_macAddress, message.m_message);
	}

	public void TestA() { }
	public void DisconnectAll() {

        foreach (var item in m_deviceBLE.Values)
        {
			if(item!=null)
				item.TriggerDiconnect();
        }
	 
	}

}


[System.Serializable]
public class BLDeviceConnectionNative {

	BluetoothHelper bluetoothHelper;
	public string m_lastReceivedMessage;
	public Queue<string> m_receivedMessages = new Queue<string>();

	public string m_deviceMacAddress = "98:D3:34:91:2C:9E";
	public bool m_listenToException=true;
	public void SendMessageToDevice(string message)
	{
		if (m_listenToException)
			bluetoothHelper.SendData(message);
		else {
			try { bluetoothHelper.SendData(message); } catch { }
		}
	}

	 public BLDeviceConnectionNative(string macAddress)
	{
		m_deviceMacAddress = macAddress;
		try
		{
			BluetoothHelper.BLE = false;
			bluetoothHelper = BluetoothHelper.GetNewInstance();
			bluetoothHelper.setDeviceAddress(m_deviceMacAddress);
			bluetoothHelper.OnConnected += OnConnected;
			bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
			bluetoothHelper.OnDataReceived += OnMessageReceived;
			bluetoothHelper.setTerminatorBasedStream("\n");
			TriggerConnect();

		}
		catch (Exception ex)
		{
			if (m_listenToException && m_useDebugRelease)

				Debug.Log(ex.Message);
		}
	}

	

	public bool m_useDebugRelease = true;
  

    void OnMessageReceived(BluetoothHelper helper)
	{
		m_lastReceivedMessage = helper.Read();
		m_receivedMessages.Enqueue(m_lastReceivedMessage);
		if (m_useDebugRelease)
			Debug.Log(m_lastReceivedMessage);
	}

	public bool m_shouldBeConnected;
	void OnConnected(BluetoothHelper helper)
	{
		try
		{
			helper.StartListening();
			if (m_useDebugRelease)
				Debug.Log("Start Listening: " + helper.getDeviceAddress());
		}
		catch (Exception ex)
		{
			if (m_useDebugRelease)
				Debug.Log(ex.Message);
		}
		m_shouldBeConnected = true;

	}

	void OnConnectionFailed(BluetoothHelper helper)
	{
		m_shouldBeConnected = false;
		if(m_useDebugRelease)
			Debug.Log("Connection Failed");
	}


	public void TriggerConnect()
	{
		if ( bluetoothHelper != null ) { 
			if (bluetoothHelper.isDevicePaired() )
				bluetoothHelper.Connect();
		}
	}
	public void TriggerDiconnect()
	{
		if (bluetoothHelper != null )
			bluetoothHelper.Disconnect();
	}

	
}
