using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    public static Console _Instance;

    public static void Log(string message)
    {
        if (_Instance != null)
        {
            _Instance.AddMessage(message);
        }
    }

    public Text ConsoleText;
    public int MessageCount;

    public InputField Field;
    public Button SendButton;

    private List<string> Messages = new List<string>();

    private void Awake()
    {
        _Instance = this;
        SendButton.onClick.AddListener(SendMessage);
    }

    private void Start()
    {
        Log("You can move console window by dragging with green header");
        Log("and scroll messages by gragging console body");
        Log("Current console messages count set to: " + MessageCount);
    }

    private void SendMessage()
    {
    //    JSBridge.LogMessageBrowser(Field.text);
    }

    private void AddMessage(string message)
    {
        Messages.Add(Time.realtimeSinceStartup.ToString("#######.####") + ": " + message);

        if (Messages.Count > MessageCount)
        {
            Messages.RemoveAt(0);
        }

        UpdateConsole();
    }

    private void UpdateConsole()
    {
        ConsoleText.text = "";

        foreach (string message in Messages)
        {
            ConsoleText.text += message + "\n";
        }
    }

    private void OnDestroy()
    {
        SendButton.onClick.RemoveAllListeners();
    }
}
