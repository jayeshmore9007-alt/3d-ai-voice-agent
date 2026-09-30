using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;

[Serializable]
public class ActionData
{
    public string type;
    public string action_name;
    public string message;
}

public class AndroidAgentClient : MonoBehaviour
{
    private string apiKey = "YOUR_GEMINI_API_KEY";
    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

    public void SendTranscript(string transcript)
    {
        StartCoroutine(PostToGemini(transcript));
    }

    private IEnumerator PostToGemini(string prompt)
    {
        string jsonRequestBody = "{\"contents\":[{\"parts\":[{\"text\":\"" + prompt + "\"}]}]}";
        using (UnityWebRequest www = new UnityWebRequest(apiUrl + "?key=" + apiKey, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequestBody);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }
}
