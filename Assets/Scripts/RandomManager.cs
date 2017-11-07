using UnityEngine;
using System.Collections;
using System;

public class RandomManager
{
    private static RandomManager instance = null;

    private System.Random r = new System.Random();
    public int kindsRandom;         //生成的物品种类随机(水果或几何图形)
    public int fruitsRandom1;        //生成的水果种类随机。
    public int fruitsRandom2;        //生成的水果种类随机。

    private RandomManager() { }

    public static RandomManager Instance()
    {
        if (instance == null)
            instance = new RandomManager();
        return instance;
    }

    public void createRandom(int level)
    {
        fruitsRandom1 = r.Next(0, 7);
        while (true)
        {
            fruitsRandom2 = r.Next(0, 7);
            if (fruitsRandom1 != fruitsRandom2)
                break;
        }
        fromLevel(level);
    }

    public void fromLevel(int level)
    {
        switch (level)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            default:
                break;
        }
    }
}
