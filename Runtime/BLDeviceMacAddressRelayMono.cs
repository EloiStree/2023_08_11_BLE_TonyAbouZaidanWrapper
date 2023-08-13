using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BLDeviceMacAddressRelayMono : MonoBehaviour
{

    public string m_aliasName;
    public string m_macAddress;

    [TextArea(0,5)]
    public string m_textToSend;
    public MacAddressBluetoothMessageEvent m_onMessagePushed;

    [ContextMenu("Push field text")]
    public void Push()
    {
        m_onMessagePushed.Invoke(new MacAddressBluetoothMessage(
            m_macAddress,
            m_textToSend
            ));
    }

    public void Push(string text)
    {
        m_textToSend = text;
        Push();
    }
    public void Test() { }
}

