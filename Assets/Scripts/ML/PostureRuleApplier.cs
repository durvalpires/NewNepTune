using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using TMPro;
using Mediapipe.Unity.Sample.Holistic;
using Mediapipe.Unity;
using Mediapipe;
using System.Collections.Generic;

[RequireComponent(typeof(HolisticTrackingSolution))]
public class PostureRuleApplier : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI neckText;
    [SerializeField] private TextMeshProUGUI wristText;
    [SerializeField] private TextMeshProUGUI fingerText;


    private HolisticTrackingSolution _solution;
    private HolisticTrackingGraph _runner;
    private NormalizedLandmarkList _poseLm;
    private NormalizedLandmarkList _lhLm;
    private NormalizedLandmarkList _rhLm;

    private Queue<string> _ttsQueue = new Queue<string>();
    private bool _isSpeaking = false;

    private int _neckStableCount, _wristStableCount, _fingerStableCount;
    private const int StabilityFrames = 5;
    private string _stableNeckStatus = "Neck Good", _stableWristStatus = "Wrist Good", _stableFingerStatus = "Fingers Good";

    private string _neckStatus = "";
    private string _wristStatus = "";
    private string _fingerStatus = "";
    private volatile bool _feedbackAvailable = false;

    private void Awake()
    {
        _solution = GetComponent<HolisticTrackingSolution>();
        if (_solution == null)
        {
            Debug.LogError("HolisticTrackingSolution component not found.");
            return;
        }

    
        FieldInfo fi = null;
        var t = _solution.GetType();
        while (t != null && fi == null)
        {
            fi = t.GetField("graphRunner", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            t = t.BaseType;
        }
        if (fi != null)
        {
            _runner = fi.GetValue(_solution) as HolisticTrackingGraph;
        }
        if (_runner == null)
        {
            Debug.LogError("Failed to retrieve the internal HolisticTrackingGraph runner.");
            return;
        }
    }

    private IEnumerator Start()
    {
        if (_runner == null) yield break;
        yield return new WaitForSeconds(0.5f);
        _runner.OnPoseLandmarksOutput += OnPoseLandmarks;
        _runner.OnLeftHandLandmarksOutput += OnLeftHandLandmarks;
        _runner.OnRightHandLandmarksOutput += OnRightHandLandmarks;
    }

    private void OnDestroy()
    {
        if (_runner == null) return;
        _runner.OnPoseLandmarksOutput -= OnPoseLandmarks;
        _runner.OnLeftHandLandmarksOutput -= OnLeftHandLandmarks;
        _runner.OnRightHandLandmarksOutput -= OnRightHandLandmarks;
    }

    // Apply text updates on main thread
    private void Update()
    {
        if (_feedbackAvailable)
        {
            neckText.text = _neckStatus;
            wristText.text = _wristStatus;
            fingerText.text = _fingerStatus;
            _feedbackAvailable = false;
        }

        //if (!_isSpeaking && _ttsQueue.Count > 0)
        //{
        //    SpeakNext();
        //}
    }

    private void OnPoseLandmarks(object _, OutputStream<NormalizedLandmarkList>.OutputEventArgs e)
    {
        _poseLm = e.packet?.Get(NormalizedLandmarkList.Parser);
        ComputeFeedback();
    }

    private void OnLeftHandLandmarks(object _, OutputStream<NormalizedLandmarkList>.OutputEventArgs e)
    {
        _lhLm = e.packet?.Get(NormalizedLandmarkList.Parser);
        ComputeFeedback();
    }

    private void OnRightHandLandmarks(object _, OutputStream<NormalizedLandmarkList>.OutputEventArgs e)
    {
        _rhLm = e.packet?.Get(NormalizedLandmarkList.Parser);
        ComputeFeedback();
    }

    // Compute statuses in any thread
    private void ComputeFeedback()
    {
        if (_poseLm == null) return;

        var L = _poseLm;
        var p0 = L.Landmark[0];
        var p11 = L.Landmark[11];
        var p12 = L.Landmark[12];
        float y0 = 1f - (float)p0.Y, y11 = 1f - (float)p11.Y, y12 = 1f - (float)p12.Y;
        Vector3 nose = new Vector3((float)p0.X, y0, 0f);
        Vector3 ls = new Vector3((float)p11.X, y11, 0f);
        Vector3 rs = new Vector3((float)p12.X, y12, 0f);
        Vector3 mid = (ls + rs) * 0.5f;
        float neckTilt = Vector3.Angle(nose - mid, Vector3.up);
        float neckTurn = ((float)p0.X - mid.x) / (Vector3.Distance(ls, rs) > 0f ? Vector3.Distance(ls, rs) : 1f);
        if (neckTurn > 0f) neckTurn = -neckTurn;

        float wristAng = 0f, wristOff = 0f, fingerDip = 0f;
        var H = _lhLm ?? _rhLm;
        if (H != null)
        {
            Vector3 a = ToV(H.Landmark[14]) - ToV(H.Landmark[16]);
            Vector3 b = ToV(H.Landmark[20]) - ToV(H.Landmark[16]);
            wristAng = Vector3.Angle(a, b);
            Vector3 hips = (ToV(L.Landmark[23]) + ToV(L.Landmark[24])) * 0.5f;
            float torso = Vector3.Distance(mid, hips);
            wristOff = torso > 0f ? (ToV(H.Landmark[16]).y - ToV(H.Landmark[14]).y) / torso : 0f;
            fingerDip = Mathf.Min(
                Vector3.Angle(ToV(H.Landmark[6]) - ToV(H.Landmark[7]), ToV(H.Landmark[8]) - ToV(H.Landmark[7])),
                Vector3.Angle(ToV(H.Landmark[10]) - ToV(H.Landmark[11]), ToV(H.Landmark[12]) - ToV(H.Landmark[11]))
            );
        }

        float tiltMin = 5f, tiltMax = 35f;
        float turnMin = -0.6f, turnMax = -0.05f;
        float angMin = 130f, angMax = 210f;
        float offMin = -0.6f, offMax = 0.4f;
        float dipMax = 150f;

        string candidateNeck;
        if (neckTilt < tiltMin || neckTilt > tiltMax)
            candidateNeck = "Neck Bad adjust tilt";
        else if (neckTurn < turnMin)
            candidateNeck = "Neck Bad turn more right";
        else if (neckTurn > turnMax)
            candidateNeck = "Neck Bad turn more left";
        else
            candidateNeck = "Neck Good";

        string candidateWrist;
        if (H == null)
            candidateWrist = "No hand";
        else if (wristAng < angMin || wristAng > angMax)
            candidateWrist = "Wrist Bad straighten wrist";
        else if (wristOff < offMin)
            candidateWrist = "Wrist Bad lift hand up";
        else if (wristOff > offMax)
            candidateWrist = "Wrist Bad drop hand down";
        else
            candidateWrist = "Wrist Good";

        string candidateFinger;
        if (H == null)
            candidateFinger = "No Fingers";
        else if (fingerDip >= dipMax)
            candidateFinger = "Fingers Bad curve fingers";
        else
            candidateFinger = "Fingers Good";

        if (candidateNeck == _stableNeckStatus) _neckStableCount = 0;
        else if (++_neckStableCount >= StabilityFrames) { _stableNeckStatus = candidateNeck; _neckStableCount = 0; }
        if (candidateWrist == _stableWristStatus) _wristStableCount = 0;
        else if (++_wristStableCount >= StabilityFrames) { _stableWristStatus = candidateWrist; _wristStableCount = 0; }
        if (candidateFinger == _stableFingerStatus) _fingerStableCount = 0;
        else if (++_fingerStableCount >= StabilityFrames) { _stableFingerStatus = candidateFinger; _fingerStableCount = 0; }



        _neckStatus = _stableNeckStatus;
        _wristStatus = _stableWristStatus;
        _fingerStatus = _stableFingerStatus;
        _feedbackAvailable = true;
        //EnqueueFeedback(_stableNeckStatus, _stableWristStatus, _stableFingerStatus);
    }
//    private void EnqueueFeedback(params string[] lines)
//    {
//        _ttsQueue.Clear();
//        foreach (var line in lines)
//            if (line.Contains("Bad"))
//                _ttsQueue.Enqueue(line);
       
//    }

//    private void SpeakNext()
//    {
//        if (_isSpeaking || _ttsQueue.Count == 0) return;
//        var msg = _ttsQueue.Dequeue();
//        _isSpeaking = true;

//#if UNITY_ANDROID && !UNITY_EDITOR
//        NeptuneTTS.Speak(msg);
//        // your Android plugin must call:
//        //   UnityPlayer.UnitySendMessage("PostureRuleApplier","OnTtsDone","");

//#elif UNITY_IOS || UNITY_STANDALONE_OSX
//        AppleTTS.Speak(msg);
//        // your iOS/macOS native plugin should call UnitySendMessage("PostureRuleApplier","OnTtsDone","");

//#elif UNITY_EDITOR
//        // Editor (macOS or Windows): use our AppleTTS shim which shells out to 'say' or powershell
//        AppleTTS.Speak(msg);
//        // schedule the callback after an estimated duration
//        var delay = Mathf.Clamp(msg.Length * 0.05f, 0.5f, 3f);
//        Invoke(nameof(OnTtsDone), delay);

//#else
//        // fallback immediately
//        OnTtsDone();
//#endif
//    }

//    public void OnTtsDone()
//    {
//        _isSpeaking = false;
//        SpeakNext();
//    }
    private Vector3 ToV(NormalizedLandmark l) => new Vector3((float)l.X, (float)l.Y, 0f);
}
