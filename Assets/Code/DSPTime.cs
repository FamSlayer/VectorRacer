using UnityEngine;
using System.Collections;

// The code example shows how to implement a metronome that procedurally generates the click sounds via the OnAudioFilterRead callback.
// While the game is paused or the suspended, this time will not be updated and sounds playing will be paused. Therefore developers of music scheduling routines do not have to do any rescheduling after the app is unpaused

[RequireComponent(typeof(AudioSource))]
public class DSPTime : MonoBehaviour
{
    
    public double bpm;// = 140.0F;
    public float gain;// = 0.5F;
    public int signatureHi;// = 4;
    public int signatureLo;// = 4;
    private double nextTick = 0.0F;
    //private float amp = 0.0F;
    //private float phase = 0.0F;
    private double sampleRate = 0.0F;
    private int accent;
    private bool running = false;


    private int fast_beat_counter = 0;
    private int real_beat_counter = 0;


    private bool increment = true;


    private void Awake()
    {
        bpm = TimeKeeper.global.bpm * 2;
    }

    void Start()
    {
        accent = signatureHi;
        sampleRate = AudioSettings.outputSampleRate;
        //running = true;
    }

    public void BeginPlay()
    {
        double startTick = AudioSettings.dspTime;
        nextTick = startTick * sampleRate;
        GetComponent<AudioSource>().Play();
        running = true;
        TimeKeeper.global.SendBeat(0);
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running)
            return;
        
        double samplesPerTick = sampleRate * 60.0F / bpm * 4.0F / signatureLo;
        double sample = AudioSettings.dspTime * sampleRate;
        //print("dpsTime: \t" + AudioSettings.dspTime);
        int dataLen = data.Length / channels;
        int n = 0;
        while (n < dataLen)
        {
            while (sample + n >= nextTick)
            {
                nextTick += samplesPerTick;
                if (increment)
                {
                    real_beat_counter++;
                    TimeKeeper.global.SendBeat(real_beat_counter);
                    //increment = !increment;
                }
                else
                {
                    increment = !increment;
                }
                
            }
            n++;
        }
    }
}