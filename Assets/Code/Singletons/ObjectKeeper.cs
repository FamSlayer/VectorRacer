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
        rotators,
        slitherers,
    }
    
    public List<Musical> All_Objects;
    public List<Expand> Cubes;
    public List<Bouncer> Bars;
    public List<Strummer> Strummers;
    public List<Rotator> Rotators;
    public List<Slitherer> Slitherers;

    private void Awake()
    {
        All_Objects = new List<Musical>();
        Cubes = new List<Expand>();
        Bars = new List<Bouncer>();
        Strummers = new List<Strummer>();
        Rotators = new List<Rotator>();
        Slitherers = new List<Slitherer>();
    }

    /*
    private void Update()
    {
        print("All_Objects.Count = " + All_Objects.Count + "   Cubes.Count = " + Cubes.Count + "   RotatorsCount = " + Rotators.Count);

        foreach(Musical e in Cubes)
        {
            if(e is Rotator)
            {
                print("e is a rotator...");
            }
        }
    }
    */


    public void Snap(ObjectType type)
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
            case ObjectType.rotators:
                foreach (Musical m in Rotators)
                    m.Snap();
                break;
            case ObjectType.slitherers:
                foreach (Musical m in Slitherers)
                    m.Snap();
                break;
        }
    }

    public void Trigger(ObjectType type)
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
            case ObjectType.rotators:
                foreach (Musical m in Rotators)
                    m.Trigger();
                break;
            case ObjectType.slitherers:
                foreach (Musical m in Slitherers)
                    m.Trigger();
                break;
        }
    }

    public void Pause(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.bars:
                foreach (Musical m in Bars)
                    m.Pause();
                break;
            case ObjectType.cubes:
                foreach (Musical m in Cubes)
                    m.Pause();
                break;
            case ObjectType.strummers:
                foreach (Musical m in Strummers)
                    m.Pause();
                break;
            case ObjectType.rotators:
                foreach (Musical m in Rotators)
                    m.Pause();
                break;
            case ObjectType.slitherers:
                foreach (Musical m in Slitherers)
                    m.Pause();
                break;

        }
    }
    


    public void Remove(GameObject g)
    {
        Musical[] musicals = g.GetComponents<Musical>();
        foreach(Musical m in musicals)
        {
            All_Objects.Remove(m);
            if (m as Expand != null)
                Cubes.Remove(m as Expand);
            if (m as Bouncer != null)
                Bars.Remove(m as Bouncer);
            if (m as Strummer != null)
                Strummers.Remove(m as Strummer);
            if (m as Rotator != null)
                Rotators.Remove(m as Rotator);
            if (m as Slitherer != null)
                Slitherers.Remove(m as Slitherer);
        }
    }


    
}
