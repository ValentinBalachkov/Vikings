using Unity.VisualScripting;
using UnityEngine;

public static class DebugLogger
{
    public static void SendMessage(string message, Color color)
    {
        Debug.Log($"<color=#{color.ToHexString()}> {message} </color>");
    }
}