using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGameScoreController
{
    private RhythmGameSettings gameSettings;
    private int playerScore = 0;
    private int currentCombo = 0;
    private float currentMultiplier = 1.0f;
    private Dictionary<HitAccuracy, int> accuracyBreakdown = new Dictionary<HitAccuracy, int>();

    public RhythmGameScoreController(RhythmGameSettings rhythmGameSettings)
    {
        this.gameSettings = rhythmGameSettings;
    }

    public Tuple<int, int> AwardScore(HitAccuracy hitAccuracy)
    {
        int scoreToAdd = 0;

        scoreToAdd = gameSettings.GetScoreForAccuracy(hitAccuracy);

        UpdateScore(scoreToAdd);
        AddCombo(hitAccuracy);

        return Tuple.Create(playerScore, currentCombo);
    }

    private void UpdateScore(int points)
    {
        int pointsToAdd = Mathf.RoundToInt(points * currentMultiplier);
        playerScore += pointsToAdd;

        // TODO: Update the score display in the UI
    }

    public void AddCombo(HitAccuracy hitAccuracy)
    {
        if (hitAccuracy == HitAccuracy.Miss)
        {
            ResetCombo();
        }
        else
        {
            currentCombo++;
            if (currentCombo % gameSettings.comboThreshold == 0)
            {
                currentMultiplier += gameSettings.comboMultiplierStep;
            }
        }

        if (accuracyBreakdown.ContainsKey(hitAccuracy))
        {
            accuracyBreakdown[hitAccuracy]++;
        }
        else
        {
            accuracyBreakdown.Add(hitAccuracy, 1);
        }

        //UpdateComboUI();
    }

    private void ResetCombo()
    {
        currentCombo = 0;
        currentMultiplier = 1.0f;
        //UpdateComboUI();
    }

    public void ResetScore()
    {
        playerScore = 0;
        foreach (var hit in accuracyBreakdown.Keys)
        {
            accuracyBreakdown[hit] = 0;
        }
    }

    private void UpdateComboUI()
    {
        throw new NotImplementedException();
    }
}
