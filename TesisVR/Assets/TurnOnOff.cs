using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Fusion;

public class TurnOnOff : NetworkBehaviour
{

    public GameObject activateGameObject;
    private bool firstFrame = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    public override void FixedUpdateNetwork()
    {
    
    }

    // Update is called once per frame
    public void EncenderYApagar()
    {
        {
            if (activateGameObject.activeSelf != true)
            {
                activateGameObject.SetActive(true);
            }
            else
            {
                activateGameObject.SetActive(false);
            }
        }
    }
}
