using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class StringUnityEvent : UnityEvent<string> {}
[Serializable] public class StringArrayUnityEvent : UnityEvent<string[]> {}
[Serializable] public class DoubleUnityEvent : UnityEvent<double> {}
[Serializable] public class FloatUnityEvent : UnityEvent<float> {}
[Serializable] public class LongUnityEvent : UnityEvent<long> {}
[Serializable] public class IntUnityEvent : UnityEvent<int> {}
[Serializable] public class ActionUnityEvent : UnityEvent<Action> {} 
[Serializable] public class ColorUnityEvent : UnityEvent<Color> {}
[Serializable] public class BoolUnityEvent : UnityEvent<bool> { }
[Serializable] public class IntStringEvent : UnityEvent<int,string> {}
[Serializable] public class IntIntUnityEvent : UnityEvent<int,int> {}
[Serializable] public class Vector2UnityEvent : UnityEvent<Vector2> {}
[Serializable] public class Vector3UnityEvent : UnityEvent<Vector3> {}
[Serializable] public class TransformUnityEvent : UnityEvent<Transform> {}


