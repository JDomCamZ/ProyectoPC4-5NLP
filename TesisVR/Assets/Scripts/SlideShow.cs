using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEditor;
using UnityEngine.Events;
using Fusion;

public class SlideShow : NetworkBehaviour
{
    public Sprite[] imageArray;

    private int currentImage = 0;

    float deltaTime = 0.0f;

    public float timer1 = 45.0f;
    public float timer1Remaining = 45.0f;
    public bool timer1IsRunning = true;
    public string timer1Text;

    //UnityEvent OnActive;

    private bool firstFrame = true;
    private bool firstFrame2 = true;
    // Start is called before the first frame update


    void Start()
    {
        //timer1Remaining = timer1;
    }

    public override void FixedUpdateNetwork()
    {
        Updating();
    }

    // Update is called once per frame
    void Updating()
    {

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        if (timer1Remaining > 0)
        {
            timer1Remaining -= Time.deltaTime;

        }

        else
        {
            Debug.Log("Time has run out!");

            NextSlide();

            timer1Remaining = timer1;
        }

    }

    public void NextSlide()
    {
        currentImage++;
        if (currentImage >= imageArray.Length)
            currentImage = 0;

        this.gameObject.GetComponent<SpriteRenderer>().sprite = imageArray[currentImage];
    }

    public void PrevSlide()
    {
        currentImage--;
        if (currentImage < 0)
            currentImage = imageArray.Length - 1;

        this.gameObject.GetComponent<SpriteRenderer>().sprite = imageArray[currentImage];
    }
}