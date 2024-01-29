using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StabbingGame : MonoBehaviour
{
    public int level = 1;
    public static StabbingGame instance;

    private void Awake() 
    {         
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        {
            instance = this; 
        } 
    }

    public Stack<Gap> GapPatternFactory()
    {
        if (level == 1)
        {
            return Level1PatternFactory();
        }
        if (level == 2)
        {
            return Level2PatternFactory();
        }
        if (level == 3)
        {
            return Level3PatternFactory();
        }
        return new();
    }

    public Stack<Gap> Level1PatternFactory()
    {
        Stack<Gap> pattern = new();
        for (int i = 0; i < 4; i++)
        {
            pattern.Push(Player.instance.gaps[i]);
        }
        return pattern;
    }
    
    public Stack<Gap> Level2PatternFactory()
    {
        Stack<Gap> pattern = Level1PatternFactory(); // level 2 is just a messy level 1
        List<Gap> g = pattern.OrderBy( x => Random.value ).ToList();
        foreach (Gap gap in g)
        {
            pattern.Push(gap);
        }
        return pattern;
    }
    
    public Stack<Gap> Level3PatternFactory()
    {
        List<Gap> g = Player.instance.gaps;
        if (Random.Range(0, 2) == 1)
        {
            g.Reverse();
        }
        Stack<Gap> pattern = new();
        int gapCount = Random.Range(1, 8);
        for (int i = 0; i < gapCount; i++)
        {
            pattern.Push(g[i]);
        }
        return pattern;
    }
}
