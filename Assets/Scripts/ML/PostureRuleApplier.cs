using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using TMPro;
using Mediapipe.Unity.Sample.Holistic;
using Mediapipe.Unity;
using Mediapipe;

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

        bool neckOk = CheckNeck(_poseLm);
        _neckStatus = neckOk ? "Neck OK" : "Neck Bad";

        var hand = _lhLm ?? _rhLm;
        if (hand == null)
        {
            _wristStatus = "No hand";
            _fingerStatus = "No Fingers";
        }
        else
        {
            bool wristOk = CheckWristAngle(hand) && CheckWristYOffset(hand, _poseLm);
            _wristStatus = wristOk ? "Wrist OK" : "Wrist Bad";
            bool fingerOk = CheckFingerDIP(hand);
            _fingerStatus = fingerOk ? "Fingers OK" : "Fingers Bad";
        }

        _feedbackAvailable = true;
    }

    private bool CheckNeck(NormalizedLandmarkList L)
    {
        var p0 = L.Landmark[0];   // Nose
        var p11 = L.Landmark[11]; // Left shoulder
        var p12 = L.Landmark[12]; // Right shoulder

        // Flip Y because MediaPipe's Y increases downward
        float y0 = 1f - (float)p0.Y;
        float y11 = 1f - (float)p11.Y;
        float y12 = 1f - (float)p12.Y;

        Vector3 nose = new Vector3((float)p0.X, y0, 0f);
        Vector3 leftShoulder = new Vector3((float)p11.X, y11, 0f);
        Vector3 rightShoulder = new Vector3((float)p12.X, y12, 0f);
        Vector3 mid = (leftShoulder + rightShoulder) * 0.5f;

        // Vector from shoulders to nose
        Vector3 v = nose - mid;
        float angle = Vector3.Angle(v, Vector3.up);

        // Now this will behave correctly because Y is flipped
        float width = Mathf.Abs((float)p11.X - (float)p12.X);
        float dx = ((float)p0.X - ((float)p11.X + (float)p12.X) * 0.5f) / width;

        Debug.Log($"Neck direction: {dx}");
        if (dx > 0) dx = -dx;
        return angle >= 5f && angle <= 35f && dx >= -0.6f && dx <= -0.05f;
    }

    private bool CheckWristAngle(NormalizedLandmarkList H)
    {
        Vector3 a = ToV(H.Landmark[14]) - ToV(H.Landmark[16]);
        Vector3 b = ToV(H.Landmark[20]) - ToV(H.Landmark[16]);
        float ang = Vector3.Angle(a, b);
        Debug.Log($"Wrist angle: {ang}");
        return ang >= 130f && ang <= 210f;
    }

    private bool CheckWristYOffset(NormalizedLandmarkList H, NormalizedLandmarkList P)
    {
        Vector3 shoulders = (ToV(P.Landmark[11]) + ToV(P.Landmark[12])) * 0.5f;
        Vector3 hips = (ToV(P.Landmark[23]) + ToV(P.Landmark[24])) * 0.5f;
        float torso = Vector3.Distance(shoulders, hips);
        Vector3 pip = ToV(H.Landmark[14]);
        Vector3 tip = ToV(H.Landmark[16]);
        float yOffset = (tip.y - pip.y) / torso;
        Debug.Log($"Wrist Y offset: {yOffset}");
        return yOffset >= -0.6f && yOffset <= 0.4f;
    }

    private bool CheckFingerDIP(NormalizedLandmarkList H)
    {
        int[] index = { 6, 7, 8 };
        int[] middle = { 10, 11, 12 };
        float indexAngle = GetJointAngle(H, index);
        float middleAngle = GetJointAngle(H, middle);
        return indexAngle < 150f || middleAngle < 150f;
    }

    private float GetJointAngle(NormalizedLandmarkList H, int[] j)
    {
        Vector2 a = new Vector2((float)H.Landmark[j[0]].X, (float)H.Landmark[j[0]].Y);
        Vector2 b = new Vector2((float)H.Landmark[j[1]].X, (float)H.Landmark[j[1]].Y);
        Vector2 c = new Vector2((float)H.Landmark[j[2]].X, (float)H.Landmark[j[2]].Y);
        return Vector2.Angle(a - b, c - b);
    }

    private Vector3 ToV(NormalizedLandmark l) => new Vector3((float)l.X, (float)l.Y, 0f);
}
