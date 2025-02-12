using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using Fusion.XR.Host.Grabbing;

[RequireComponent(typeof(NetworkGrabbable))]
public class GrabbableCube2 : NetworkBehaviour
{
    public TextMeshProUGUI authorityText;
    public TextMeshProUGUI debugText;
    public SlideShow slideShow; // Reference to the SlideShow script on another object

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

        slideShow.PrevSlide();
    }
}