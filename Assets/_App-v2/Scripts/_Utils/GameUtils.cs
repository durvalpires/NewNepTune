using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameUtils
{
    public static int RoundToInt(float val)
    {
        return Mathf.FloorToInt(val);
    }

    public static int StringToInt(string val)
    {
        if (int.TryParse(val, out var floatVal))
        {
            return floatVal;
        }
        return 0;
    }
    public static float Parse(string val)
    {
        return StringToFloat(val);
    }
    public static Vector3 WorldPointToUI(Camera worldCamera, Camera uiCamera, Transform canvas, Vector3 pos3d)
    {
        // Convert the 3D position to screen space using the WorldCamera
        Vector3 screenPos = worldCamera.WorldToScreenPoint(pos3d);
        // Convert the screen position to a position on the UI canvas using the UICamera
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, uiCamera, out localPos);

        return localPos;
    }
    public static string GenerateUnitID => Guid.NewGuid().ToString();
    public static float StringToFloat(string val)
    {
        if (float.TryParse(val, out var floatVal))
        {
            return floatVal;
        }
        var str = val.Replace('.', ',');
        if (float.TryParse(str, out var floatValue))
        {
            
            // var floatValue = float.Parse(str);
            return floatValue;
        }
        return 0f;

        /** /
        //todo make it better with float.TryParse
        try
        {
            var floatValue = float.Parse(val);
            return floatValue;
        }
        catch (Exception e)
        {
            var str = val.Replace('.', ',');
            var floatValue = float.Parse(str);
            return floatValue;
        }
        /**/
    }
    static int SelectRandomWeightedIndex(float[] weights)
    {
        float totalWeight = 0f;

        // Sum all the weights
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        // Generate a random value in the range from 0 to the total weight
        float randomValue = (float)new Random().NextDouble() * totalWeight;

        // Select the index of the element based on the probability weight
        float weightSum = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            weightSum += weights[i];

            if (randomValue < weightSum)
            {
                return i;
            }
        }

        // Return the last index if something went wrong
        return weights.Length - 1;
    }
    /// <summary>
    ///  Return new point on radius from center with angle shift form direction of curPoint
    /// </summary>
    public static Vector3 PointOnRadius(Vector3 center, Vector3 curPoint, float radius, float angle)
    {
        var curAOffset = Angle(curPoint, center);
        float x1 = center.x;
        float y1 = center.z;

        float x2 = x1 + radius * (float) Math.Cos((curAOffset-angle) * Math.PI / 180);
        float y2 = y1 + radius * (float) Math.Sin((curAOffset -angle) * Math.PI / 180);
        return new Vector3(x2, 0, y2);
    }
    public static double Angle(Vector3 p1, Vector3 p2)
    {
        double x1 = p1.x;
        double y1 = p1.z;
        double x2 = p2.x;
        double y2 = p2.z;

        double dx = x2 - x1;
        double dy = y2 - y1;

        double angle = Math.Atan2(dy, dx) * 180 / Math.PI+180;

        return angle;
    }

    public static void CancelDelay(Utility.DelayedCallbackItem callBackItem)
    {
        Utility.CancelDelay(callBackItem);
    }
    public static bool IsColorLight(Color color)
    {
        var luminanceThreshold = 0.5f;
        float luminance = 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
        return luminance > luminanceThreshold;
    }
}
public static class Utility
{
    //Using:
    //this.Invoke(()=>Debug.Log("Lambdas also work"), 1f);
    /// <summary>
    /// Exmple: this.Invoke(()=>Debug.Log("Lambdas also work"), 1f);
    /// </summary>
    /// <param name="mb"></param>
    /// <param name="f"></param>
    /// <param name="delay"></param>
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    /// <summary>
    /// ex2: 0.5f.Delay(SomeMethode, this);
    /// ex1: 0.5f.Delay(()=>SomeMethode(), this);
    /// ex1: 0.5f.Delay(()=>{SomeMethode();}, this);
    /// </summary>
    /// <param name="f"></param>
    /// <param name="target"></param>
    /// <param name="method"></param>
    /// <param name="delay"></param>
    private static DelayMonoHelper _delayHelper;
    // public static async void DelayThread(this float number, Action onComplete)
    // {
    //     if (!_delayHelper)
    //     {
    //         var go = new GameObject("DelayHelper");
    //         _delayHelper = go.AddComponent<DelayMonoHelper>();
    //     } 
    //     
    //     await Task.Run(() =>
    //     {
    //         System.Threading.Thread.Sleep((int) (number * 1000));
    //         DelayMonoHelper.DoStack.Add(onComplete);
    //     });
    // }

    // public static DelayMono DelayGO(this float number, Action onComplete)
    // {
    //     return _Delay(number, onComplete);
    // }
    public static void CancelDelay(DelayedCallbackItem callBackItem)
    {
        if (!_delayHelper)
        {
            var go = new GameObject("DelayHelper");
            _delayHelper = go.AddComponent<DelayMonoHelper>();
        } 
        _delayHelper.Remove(callBackItem);
    }
    public static DelayedCallbackItem Delay(this float value, Action action)
        {
            if (!_delayHelper)
            {
                var go = new GameObject("DelayHelper");
                _delayHelper = go.AddComponent<DelayMonoHelper>();
            } 
            if (value < 0.01f)
                value = 0.01f;
            return _delayHelper.DelayedCall(value, action);           
        }
    public static DelayMono DelayGO(this float number, Action onComplete)
    {
        var go = new GameObject("delay"+number);
        var delayComp = go.AddComponent<DelayMono>();
        delayComp.StartCoroutine(InvokeRoutine(onComplete, number, delayComp));
        return delayComp;
    }
    
    private static IEnumerator InvokeRoutine(System.Action onComplete, float delay, DelayMono mono)
    {
        yield return new WaitForSeconds(delay);
        onComplete();
        GameObject.Destroy(mono.gameObject);
    }
    public class DelayMono : MonoBehaviour { }

    public class DelayMonoHelper : MonoBehaviour
    {
        private List<DelayedCallbackItem> delayedCalls;
        public int DelayedCallsCount;

        void Awake()
        {
            delayedCalls = new List<DelayedCallbackItem> ();
        }

        public void Remove(DelayedCallbackItem item)
        {
            if(delayedCalls.Contains(item))
                delayedCalls.Remove(item);
        }
        private void Update ()
        {
            Action dCall;
            bool checkNull;
            if (delayedCalls == null)
                return;

            for (int i = 0; i < delayedCalls.Count; i++)
            {
                if (delayedCalls[i].Delay <= Time.time)
                {
                    dCall = delayedCalls[i].DelCallback;
                    checkNull = delayedCalls[i].CheckNull;
                    if (delayedCalls.Count > i)
                    {
                        delayedCalls.RemoveAt(i);
                        i--;
                    }

                    if (dCall != null && (!checkNull || !IsNull(dCall.Target)))
                    {
                        dCall();
                    }
                }
            }
            DelayedCallsCount = delayedCalls.Count;
        }
        private static bool IsNull(System.Object aObj)
        {
            return aObj == null || aObj.Equals(null);
        }
        public DelayedCallbackItem DelayedCall(float delay, Action callback, bool checkNull = false)
        {
            var delayedCallbackItem = new DelayedCallbackItem(delay + Time.time, callback, checkNull);
            delayedCalls.Add(delayedCallbackItem);
            DelayedCallsCount = delayedCalls.Count;
            return delayedCallbackItem;
        }
    }

    public class DelayedCallbackItem
    {
        public float Delay;
        public Action DelCallback;
        public bool CheckNull;
        //public string Id;
        
        public DelayedCallbackItem(float delay, Action callback, bool checkNull = false)
        {
            //Id = id;
            Delay = delay;
            DelCallback = callback;
            CheckNull = checkNull;
        }
        
    }
    public static void Delay(this float f,Action method,  MonoBehaviour target)
    {
        target.StartCoroutine(InvokeRoutine(method, f));
    }
    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
    
    
    //todo good to have something like that 
    // public static float ToFloat(this string val, string test)
    // {
    //     //todo make it better with float.TryParse
    //     try
    //     {
    //         var floatValue = float.Parse(val);
    //         return floatValue;
    //     }
    //     catch (Exception e)
    //     {
    //         var str = val.Replace('.', ',');
    //         var floatValue = float.Parse(str);
    //         return floatValue;
    //     }
    //     return 0;
    // }
    private static Random rng = new Random();  

    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
   
}
