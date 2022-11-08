using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBowl : MonoBehaviour
{
    List<WeightFish> Fishes=new List<WeightFish>();
    public delegate void RoundOver(bool RoundWon);
    public static event RoundOver onRoundOver;
    int FishSaved = 0;
    int TotalFishes = 0;
    private void Awake()
    {
        WeightFish.OnFishInfoBradcast += IncomingFishesInfo;
        WeightFish.onFishSaved += CheckFishSaved;
    }

    private void CheckFishSaved(WeightFish fish)
    {
        FishSaved++;
        if (FishSaved >= TotalFishes)
        {
            onRoundOver?.Invoke(true);
            Debug.Log("Round Won");
        }
        Debug.Log("fish saved " + fish.name);
    }

    private void IncomingFishesInfo(WeightFish fishInfo)
    {
        if (Fishes.Contains(fishInfo)) return;
        Fishes.Add(fishInfo);
        TotalFishes = Fishes.Count;
    }
    private void OnDestroy()
    {
        WeightFish.OnFishInfoBradcast -= IncomingFishesInfo;
        WeightFish.onFishSaved -= CheckFishSaved;
    }
}
