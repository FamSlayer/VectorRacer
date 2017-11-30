using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectKeeper : Singleton<ObjectKeeper>
{
    public enum ObjectType
    {
        cubes,
        bars,
        strummers,
    }
    
    public float expand_beats;
    public float shrink_beats;

    public List<Musical> All_Objects;
    public List<Expand> Cubes;
    public List<Bouncer> Bars;
    public List<Strummer> Strummers;

    public TextAsset bp_melody_txt;
    public TextAsset bp_bassline_txt;


    public List<int> Melody_Beats;
    public List<int> Bassline_Beats;
    

    private int melodyBeatCounter = 1;
    private int basslineBeatCounter = 0;
    //private string txt;

    private void Awake()
    {
        All_Objects = new List<Musical>();
        Cubes = new List<Expand>();
        Bars = new List<Bouncer>();
        
        Melody_Beats = new List<int>();
        Bassline_Beats = new List<int>();

        LoadMelodyBeats(bp_melody_txt);
        LoadBasslineBeats(bp_bassline_txt);

        SnapCubes();
    }
    

    public void SendBeat(int beat_num)
    {
        if(melodyBeatCounter < Melody_Beats.Count)
        {
            if (beat_num - expand_beats == Melody_Beats[melodyBeatCounter])
            {
                //SnapCubes();
                //SnapStrummers();
                SnapAll(ObjectType.cubes);
                SnapAll(ObjectType.strummers);

                melodyBeatCounter++;
            }
            else if (beat_num == Melody_Beats[melodyBeatCounter])
            {
                TriggerAll(ObjectType.cubes);
                TriggerAll(ObjectType.strummers);
            }
        }
        else
        {
            //print("Further than recorded melody...");
        }



        if(basslineBeatCounter < Bassline_Beats.Count)
        {
            if (beat_num - expand_beats == Bassline_Beats[basslineBeatCounter])
            {
                SnapAll(ObjectType.bars);
                //SnapBars();
                basslineBeatCounter++;
            }
            else if (beat_num == Bassline_Beats[basslineBeatCounter])
            {
                TriggerAll(ObjectType.bars);
                RaiseBars();
            }
        }
        else
        {
            //print("Further than recorded bassline...");
        }


    }

    
    private void SnapAll(ObjectType type)
    {
        switch(type)
        {
            case ObjectType.bars:
                foreach (Musical m in Bars)
                    m.Snap();
                break;
            case ObjectType.cubes:
                foreach (Musical m in Cubes)
                    m.Snap();
                break;
            case ObjectType.strummers:
                foreach (Musical m in Strummers)
                    m.Snap();
                break;
        }
    }

    private void TriggerAll(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.bars:
                foreach (Musical m in Bars)
                    m.Trigger();
                break;
            case ObjectType.cubes:
                foreach (Musical m in Cubes)
                    m.Trigger();
                break;
            case ObjectType.strummers:
                foreach (Musical m in Strummers)
                    m.Trigger();
                break;
        }
    }


    private void SnapCubes()
    {
        foreach (Expand e in Cubes)
        {
            e.Snap();
        }
    }

    private void InflateCubes()
    {
        foreach (Expand e in Cubes)
        {
            e.Trigger();
        }
    }

    private void SnapStrummers()
    {
        foreach (Strummer s in Strummers)
        {
            s.Snap();
        }
    }

    private void TriggerStrummer()
    {
        foreach (Strummer s in Strummers)
        {
            s.Trigger();
        }
    }

    private void SnapBars()
    {
        foreach (Musical b in Bars)
        {
            b.Snap();
        }
    }

    private void RaiseBars()
    {
        foreach (Musical b in Bars)
        {
            b.Trigger();
        }
    }


    private void LoadMelodyBeats(TextAsset t)
    {
        /* Parse the text type files */
        char[] txt_delimiters = { '\t', '\n' };
        string[] txt_pieces = t.text.Split(txt_delimiters);

        double bps = TimeKeeper.global.bpm / 60.0;
        double beat_rate = 1.0 / bps;

        for (int i = 0; i < txt_pieces.Length - 1; i += 2)
        {
            double beat_time = double.Parse(txt_pieces[i]);
            int num_beats_count = (int)(4.0 * beat_time * bps);
            Melody_Beats.Add(num_beats_count);
            //print("res: " + beat_time / beat_rate);
            //print("\t[" + i.ToString() + "] -   beat_time: " + res + "\tnum_beats_count = " + num_beats_count);
        }
    }

    private void LoadBasslineBeats(TextAsset t)
    {
        /* Parse the text type files */
        char[] txt_delimiters = { '\t', '\n' };
        string[] txt_pieces = t.text.Split(txt_delimiters);

        double bps = TimeKeeper.global.bpm / 60.0;
        double beat_rate = 1.0 / bps;

        for (int i = 0; i < txt_pieces.Length - 1; i += 2)
        {
            double beat_time = double.Parse(txt_pieces[i]);
            int num_beats_count = (int)(4.0 * beat_time * bps);
            Bassline_Beats.Add(num_beats_count);
            //print("res: " + beat_time / beat_rate);
            //print("\t[" + i.ToString() + "] -   beat_time: " + res + "\tnum_beats_count = " + num_beats_count);

        }
    }

}
