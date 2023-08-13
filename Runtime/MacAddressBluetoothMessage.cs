
using UnityEngine.Events;

[System.Serializable]
public class MacAddressBluetoothMessage
{
    public string m_macAddress;
    public string m_message;

    public MacAddressBluetoothMessage(string macAddress, string message)
    {
        m_macAddress = macAddress;
        m_message = message;
    }
}


[System.Serializable]
public class MacAddressBluetoothMessageEvent : UnityEvent<MacAddressBluetoothMessage>
{

}