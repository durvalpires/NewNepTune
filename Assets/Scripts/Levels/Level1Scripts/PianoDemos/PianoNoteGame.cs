using System.Collections;
using System.Collections.Generic;
using Audio;
using Enums;
using Levels.Level1Game1;
using UnityEngine;

public class PianoNoteGame : MonoBehaviour
{
    public GameObject follower;
    public List<GameObject> targets;
    public float bpm;

    private TriggerManagerG1L1 _triggerManagerG1L1;
    
    private float beatTime;
    private int currentTargetIndex;
    

    void Start()
    {
        beatTime = 60f / bpm;
        currentTargetIndex = 0;
        //StartCoroutine(PlayMusic());
        _triggerManagerG1L1 = FindObjectOfType<TriggerManagerG1L1>();
    }

    IEnumerator PlayMusic()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.PlayMusic(SoundList.middleCmusic);
    }

    void Update()
    {
        if (targets.Count == 0) return;

        // Move the follower towards the current target
        follower.transform.position = Vector3.MoveTowards(follower.transform.position, targets[currentTargetIndex].transform.position, Time.deltaTime);

        // If the follower has reached the target, move on to the next target
        if (Vector3.Distance(follower.transform.position, targets[currentTargetIndex].transform.position) < 0.001f && _triggerManagerG1L1.DoNotaControl())
        {
            currentTargetIndex = (currentTargetIndex + 1) % targets.Count;
        }

        // Make the follower jump to the target on every beat
        if (AudioManager.Instance.CheckSoundLength(SoundList.middleCmusic) % beatTime < 0.01f &&  _triggerManagerG1L1.DoNotaControl())
        {
            follower.transform.position = targets[currentTargetIndex].transform.position;
        }
    }
}

  
