using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SoundA : MonoBehaviour {

    public AudioSource source;
    public float[] spectrum;
    public static float[] freqBand;
    public float[] bandBuffer;
    float[] bufferDecrease;
    float[] freqBandHighest;
    public float[] audioBand;
    public float[] audioBandBuffer;
    float average = 0f;
    float bpm;
    float[] maxHeight;
    StreamWriter writer;
    float timer = 0;
    public float cooldown;
    int beatCount;
    public float maxCooldown = 1;
    int maxCounter = 1;
    float beatTimer;
    float lastBeatTimer;
    bool done = false;
    int energyCount = 1;
    int clicks;
    public float clickTimer;

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.Play();
        spectrum = new float[512];
        freqBand = new float[8];
        bandBuffer = new float[8];
        bufferDecrease = new float[8];
        freqBandHighest = new float[8];
        audioBand = new float[8];
        audioBandBuffer = new float[8];
        maxHeight = new float[8];
        string fileName = source.clip.name;
        writer = new StreamWriter(("Assets/Resources/Songs/" + fileName + "/beatmap.txt"));
        WriteBeatMap();
    }

    void Update()
    {
        if (!done)
        {
            if (Input.GetKeyDown("return"))
            {
                clicks++;
                maxCooldown = ((maxCooldown * (clicks - 1) + (clickTimer - .05f)) / clicks);
                clickTimer = 0;
            }
            AnalyzeSong();
            MakeFrequencyBands();
            BandBuffer();
            CreateAudioBands();
            if (source.clip.length == 0)
            {
                //Handle No Source
            }
            if (timer > source.clip.length)
            {
                bpm = (float)beatCount / ((beatTimer - (beatTimer - lastBeatTimer)) / 60);
                writer.WriteLine("BPM: " + bpm);
                writer.Close();
                done = true;
            }
            else
            {
                cooldown = WriteBeat(timer, cooldown);
            }
            if (beatCount > 0)
            {
                beatTimer += Time.deltaTime;
            }
            timer += Time.deltaTime;
            cooldown += Time.deltaTime;
            clickTimer += Time.deltaTime;
        } else
        {

        }
    }

    void AnalyzeSong()
    { 
        source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
    }    
    void MakeFrequencyBands()
    {
        int count = 0;
        
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += spectrum[count] * (count + 1);
                count++;
            }
            average /= count;
            freqBand[i] = average * 10;
            if(maxCounter == 1)
            {
                maxHeight[i] = 0;
            }
            maxHeight[i] = ((maxHeight[i] * (maxCounter - 1) + freqBand[i]) / maxCounter);
        }
        maxCounter++;
    }

    void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBand[i];
                bufferDecrease[i] = 0.005f;
            }
            if (freqBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= freqBand[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }

    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBand[i];
            }
            audioBand[i] = (bandBuffer[i] / freqBandHighest[i]);
        }
    }
    void WriteBeatMap()
    {
        writer.WriteLine("BEATMAP VERSION 1.2");
        writer.WriteLine(source.clip.name);
        writer.WriteLine("0");
        writer.WriteLine("0");
    }
    float WriteBeat(float timer ,float cooldown)
    {
        int maxCount = 0;
        float maxBand = freqBand[0];
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > (maxHeight[i] * 2) && freqBand[i] > 2 && cooldown > maxCooldown) 
            {
                //Debug.Log("Beat");
                lastBeatTimer = timer;
                cooldown = 0f;
                beatCount++;
                writer.WriteLine(timer);
                for (int j = 0; j < 8; j++)
                {
                    if (freqBand[i] > maxBand)
                    {
                        maxBand = freqBand[i];
                        maxCount = i;
                    }
                }
                writer.WriteLine(Zone(maxCount));
            }
        }
        return cooldown;
    }
    int Zone(int count)
    {
        int whichZone = 0;
        if (count < 2)
        {
            //Debug.Log("Low Zone");
            whichZone = ((int)(Random.value * 2f));
        }
        else if (count < 5)
        {
            //Debug.Log("Mid Zone");
            whichZone = ((int)(Random.value * 2f) + 2);
        } else if (count < 7)
        {
            //Debug.Log("High Zone");
            whichZone = ((int)(Random.value * 2f) + 4);
        }
        return whichZone;
    }

    void getCooldown()
    {
        clicks++;
        if (Input.GetKeyDown("Enter"))
        {
            cooldown = ((cooldown * clicks - 1) + timer) / clicks;
        }
    }
}
