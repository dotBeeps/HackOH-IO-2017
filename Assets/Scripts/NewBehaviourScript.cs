using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public AudioSource song;
    public GameObject cube;

    float[] historyBuffer = new float[43];

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //compute instant sound energy
        float[] channelRight = new float[1024];
        song.GetComponent<AudioSource>().GetSpectrumData(channelRight, 1, FFTWindow.Hamming);
        float[] channelLeft = new float[1024];
        song.GetComponent<AudioSource>().GetSpectrumData(channelLeft, 2, FFTWindow.Hamming);

        float e = sumStereo(channelLeft, channelRight);

        //compute local average sound evergy
        float E = sumLocalEnergy() / historyBuffer.Length; // E being the average local sound energy

        //calculate variance
        float sumV = 0;
        for (int i = 0; i < 43; i++)
            sumV += (historyBuffer[i] - E) * (historyBuffer[i] - E);

        float V = sumV / historyBuffer.Length;
        float constant = (float)((-0.0025714 * V) + 1.5142857);

        float[] shiftingHistoryBuffer = new float[historyBuffer.Length]; // make a new array and copy all the values to it

        for (int i = 0; i < (historyBuffer.Length - 1); i++)
        { // now we shift the array one slot to the right
            shiftingHistoryBuffer[i + 1] = historyBuffer[i]; // and fill the empty slot with the new instant sound energy
        }

        shiftingHistoryBuffer[0] = e;

        for (int i = 0; i < historyBuffer.Length; i++)
        {
            historyBuffer[i] = shiftingHistoryBuffer[i]; //then we return the values to the original array
        }

        //float constant = 1.5f;

        if (e > (constant * E))
        { // now we check if we have a beat
            //cube.GetComponent<SpriteRenderer>().color = Color.red;
            cube.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
        else
        {
            cube.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }

        Debug.Log("Avg local: " + E);
        Debug.Log("Instant: " + e);
        Debug.Log("History Buffer: " + historybuffer());

        Debug.Log("sum Variance: " + sumV);
        Debug.Log("Variance: " + V);

        Debug.Log("Constant: " + constant);
        Debug.Log("--------");
    }

    float sumStereo(float[] channel1, float[] channel2)
    {
        float e = 0;
        for (int i = 0; i < channel1.Length; i++)
        {
            e += ((channel1[i] * channel1[i]) + (channel2[i] * channel2[i]));
        }

        return e;
    }

    float sumLocalEnergy()
    {
        float E = 0;

        for (int i = 0; i < historyBuffer.Length; i++)
        {
            E += historyBuffer[i] * historyBuffer[i];
        }

        return E;
    }

    string historybuffer()
    {
        string s = "";
        for (int i = 0; i < historyBuffer.Length; i++)
        {
            s += (historyBuffer[i] + ",");
        }
        return s;
    }
}
