using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using Fusion.XR.Host.Grabbing;
using System.IO;

[RequireComponent(typeof(NetworkGrabbable))]
public class TextModel : NetworkBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI authorityText;
    public TextMeshProUGUI debugText;
    public SlideShow slideShow; // Reference to the SlideShow script on another object

    public string filepath = "Assets/GeneratedText/generated_text.txt";
    private List<string> textEntries = new List<string>();


    private void Awake()
    {
        debugText.text = "";
        var grabbable = GetComponent<NetworkGrabbable>();

        grabbable.onDidGrab.AddListener(OnDidGrab);
        grabbable.onDidUngrab.AddListener(OnDidUngrab);

        // Ensure slideShow reference is valid (optional for robustness)
        if (slideShow == null)
        {
            Debug.LogError("GrabbableCube: Missing reference to SlideShow script!");
        }

        // Load text entries from file
        LoadTextEntries();
    }

    private void DebugLog(string debug)
    {
        debugText.text = debug;
        Debug.Log(debug);
    }

    private void UpdateStatusCanvas()
    {
        if (Object.HasStateAuthority)
            authorityText.text = "You have the state authority on this object";
        else
            authorityText.text = "You have NOT the state authority on this object";
    }

    public override void FixedUpdateNetwork()
    {
        UpdateStatusCanvas();
    }

    void OnDidUngrab()
    {
        DebugLog($"{gameObject.name} ungrabbed");
    }

    void OnDidGrab(NetworkGrabber newGrabber)
    {
        DebugLog($"{gameObject.name} grabbed by {newGrabber.Object.InputAuthority} {newGrabber.hand.side} hand");

        if (textEntries.Count > 0)
        {
            string randomEntry = textEntries[Random.Range(0, textEntries.Count)];
            DebugLog(randomEntry);
        }
    }
    private void LoadTextEntries()
    {
        try
        {
            string[] lines = File.ReadAllLines(filepath);
            string entry = "";

            foreach (string line in lines)
            {
                if (line.Trim() == "---------------")
                {
                    if (!string.IsNullOrEmpty(entry))
                    {
                        textEntries.Add(entry.Trim());
                        entry = "";
                    }
                }
                else
                {
                    entry += line + "\n";
                }
            }

            // Add the last entry if it exists
            if (!string.IsNullOrEmpty(entry))
            {
                textEntries.Add(entry.Trim());
            }

            Debug.Log("Text entries loaded successfully.");
        }
        catch (IOException e)
        {
            Debug.LogError($"Failed to read the file: {e.Message}");
        }
    }
}
