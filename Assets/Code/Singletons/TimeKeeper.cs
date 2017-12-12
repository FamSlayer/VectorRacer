using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeKeeper : Singleton<TimeKeeper>
{
    public int bpm;

    public float expand_beats;
    public float shrink_beats;

    public TextAsset bp_melody_txt;
    public TextAsset bp_bassline_txt;
    public TextAsset bp_drumkick_txt;
    public TextAsset bp_drumsnare_txt;
    public TextAsset bp_woodblock_txt;

    public List<int> MelodyBeats;
    public List<int> BasslineBeats;
    public List<int> WoodblockBeats;
    public List<int> DrumKickBeats;
    public List<int> DrumSnareBeats;

    private int melodyBeatCounter = 0;
    private int basslineBeatCounter = 0;
    private int drumkickCounter = 0;
    private int drumsnareCounter = 0;
    private int woodblockBeatCounter = 0;

    private bool woodblock_bool = false;

    private void Awake()
    {
        MelodyBeats = new List<int>();
        BasslineBeats = new List<int>();
        DrumKickBeats = new List<int>();
        DrumSnareBeats = new List<int>();
        WoodblockBeats = new List<int>();

        LoadBeatsTo(bp_melody_txt, MelodyBeats);
        LoadBeatsTo(bp_bassline_txt, BasslineBeats);
        LoadBeatsTo(bp_drumkick_txt, DrumKickBeats);
        LoadBeatsTo(bp_drumsnare_txt, DrumSnareBeats);
        LoadBeatsTo(bp_woodblock_txt, WoodblockBeats);
    }
    
    public void SendBeat(int beat_num)
    {
        // melody
        if (melodyBeatCounter < MelodyBeats.Count)
        {
            if (beat_num - expand_beats * 2 >= MelodyBeats[melodyBeatCounter])
            {
                //ObjectKeeper.global.Snap(ObjectKeeper.ObjectType.cubes);
                ObjectKeeper.global.Snap(ObjectKeeper.ObjectType.strummers);
                melodyBeatCounter++;
            }
            else if (beat_num - expand_beats >= MelodyBeats[melodyBeatCounter])
            {
                //ObjectKeeper.global.Trigger(ObjectKeeper.ObjectType.cubes);
                ObjectKeeper.global.Trigger(ObjectKeeper.ObjectType.strummers);
            }
        }
        else
        {
            //print("Further than recorded melody...");
        }

        // bassline
        if (basslineBeatCounter < BasslineBeats.Count)
        {
            if (beat_num - expand_beats * 2 >= BasslineBeats[basslineBeatCounter])
            {
                ObjectKeeper.global.Snap(ObjectKeeper.ObjectType.bars);
                ObjectKeeper.global.Snap(ObjectKeeper.ObjectType.colorers);
                //ObjectKeeper.global.Snap(ObjectKeeper.ObjectType.rotators);
                basslineBeatCounter++;
            }
            else if (beat_num  - expand_beats >= BasslineBeats[basslineBeatCounter])
            {
                ObjectKeeper.global.Trigger(ObjectKeeper.ObjectType.bars);
                ObjectKeeper.global.Trigger(ObjectKeeper.ObjectType.colorers);
                //ObjectKeeper.global.Trigger(ObjectKeeper.ObjectType.rotators);
            }
        }
        else
        {
            //print("Further than recorded bassline...");
        }

        // kick drum
        if(drumkickCounter < DrumKickBeats.Count)
        {
            if(beat_num - expand_beats * 2 >= DrumKickBeats[drumkickCounter])
            {
                // snap
                ObjectKeeper.global.Snap(ObjectKeeper.ObjectType.slitherers);
                drumkickCounter++;
            }
            else if(beat_num  - expand_beats >= DrumKickBeats[drumkickCounter])
            {
                // trigger
                ObjectKeeper.global.Trigger(ObjectKeeper.ObjectType.slitherers);

            }
        }

        //print("beat num: " + beat_num + "   waiting for: " + DrumSnareBeats[drumsnareCounter]);
        // snare drum
        if (drumsnareCounter < DrumSnareBeats.Count)
        {
            if (beat_num - expand_beats * 2 >= DrumSnareBeats[drumsnareCounter])
            {
                // snap
                ObjectKeeper.global.Snap(ObjectKeeper.ObjectType.cubes);
                ObjectKeeper.global.Snap(ObjectKeeper.ObjectType.slitherers);
                //print("SNAPPED CUBES!");
                drumsnareCounter++;
            }
            else if (beat_num  - expand_beats >= DrumSnareBeats[drumsnareCounter])
            {
                // trigger
                ObjectKeeper.global.Trigger(ObjectKeeper.ObjectType.cubes);
                ObjectKeeper.global.Trigger(ObjectKeeper.ObjectType.slitherers);
            }
        }



        // wood block
        if (woodblockBeatCounter < WoodblockBeats.Count)
        {
            if(beat_num - expand_beats * 2 == WoodblockBeats[woodblockBeatCounter])
            {
                woodblockBeatCounter++;
                if (woodblock_bool)
                {
                    ObjectKeeper.global.Snap(ObjectKeeper.ObjectType.rotators);
                    //ObjectKeeper.global.Snap(ObjectKeeper.ObjectType.cubes);
                    woodblock_bool = false;
                }
                else
                {
                    woodblock_bool = true;
                }
            }
            else if(beat_num  - expand_beats >= WoodblockBeats[woodblockBeatCounter])
            {
                ObjectKeeper.global.Trigger(ObjectKeeper.ObjectType.rotators);
                //ObjectKeeper.global.Trigger(ObjectKeeper.ObjectType.cubes);
            }
        }


    }





    private void LoadBeatsTo(TextAsset t, List<int> beat_list)
    {
        /* Parse the text file */
        char[] txt_delimiters = { '\t', '\n' };
        string[] txt_pieces = t.text.Split(txt_delimiters);

        double bps = bpm / 60.0;
        double beat_rate = 1.0 / bps;

        for (int i = 0; i < txt_pieces.Length - 1; i += 2)
        {
            double beat_time = double.Parse(txt_pieces[i]);
            int num_beats_count = (int)(4.0 * beat_time * bps);
            beat_list.Add(num_beats_count);
        }


        beat_list.Sort();
        /*  THE REASON TO SORT:
         *      It's possible I would want to have a list the combines two different instruments and responds to both of them.
         *      In songs with instruments that are only used a few times (like the wood block at the beginning of Blue Powder 1984)
         *          this lets me annotate each individual instrument in different annotation layers in Sonic Visualizer
         *          
         *      -- In other words, this means that once I've marked up all the instruments I want for a specific song, I don't
         *          have to change the data I've created for it if I later want to change the way objects respond to instruments.
         */
    }

}
