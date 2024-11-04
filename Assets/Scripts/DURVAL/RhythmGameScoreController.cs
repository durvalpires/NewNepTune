using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGameScoreController
{
    private RhythmGameSettings gameSettings;
    public int PlayerScore { get; private set; }
    private int currentCombo;
    private float currentMultiplier;
    private readonly int noteCount;
    public int NotesHit { get; private set; }
    public Dictionary<HitAccuracy, int> AccuracyBreakdown { get; private set; }

    private bool star1Active = false;
    private bool star2Active = false;
    private bool star3Active = false;
    public int PlayerStars { get; private set; }
    private float incrementPerNote;
    public Action OnStarAchieved; 

    public RhythmGameScoreController(RhythmGameSettings rhythmGameSettings, int noteCount = 0)
    {
        this.PlayerScore = 0;
        this.PlayerStars = 0;
        this.currentCombo = 0;
        this.currentMultiplier = 1.0f;
        this.AccuracyBreakdown = new Dictionary<HitAccuracy, int>();
        this.noteCount = noteCount;
        this.gameSettings = rhythmGameSettings;

        incrementPerNote = 100f / noteCount;
    }

    public Tuple<int, int> AwardScore(HitAccuracy hitAccuracy)
    {
        int scoreToAdd = 0;

        if (hitAccuracy != HitAccuracy.Miss)
        {
            NotesHit++;
            VerifyIfStarAchieved();
        }

        scoreToAdd = gameSettings.GetScoreForAccuracy(hitAccuracy);

        UpdateScore(scoreToAdd);
        AddCombo(hitAccuracy);

        return Tuple.Create(PlayerScore, currentCombo);
    }

    private void VerifyIfStarAchieved()
    {
        var percentageHit = (float)NotesHit / noteCount;

        if(!star1Active && percentageHit >= 0.3)
        {
            OnStarAchieved?.Invoke();
            star1Active = true;
            ++PlayerStars;
        }

        if(!star2Active && percentageHit >= 0.6)
        {
            OnStarAchieved?.Invoke();
            star2Active = true;
            ++PlayerStars;
        }

        if(!star3Active && percentageHit >= 0.9)
        {
            OnStarAchieved?.Invoke();
            star3Active = true;
            ++PlayerStars;
        }
    }

    private void UpdateScore(int points)
    {
        int pointsToAdd = Mathf.RoundToInt(points * currentMultiplier);
        PlayerScore += pointsToAdd;

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

        if (AccuracyBreakdown.ContainsKey(hitAccuracy))
        {
            AccuracyBreakdown[hitAccuracy]++;
        }
        else
        {
            AccuracyBreakdown.Add(hitAccuracy, 1);
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
        PlayerScore = 0;
        foreach (var hit in AccuracyBreakdown.Keys)
        {
            AccuracyBreakdown[hit] = 0;
        }
    }

    private void UpdateComboUI()
    {
        throw new NotImplementedException();
    }
}
