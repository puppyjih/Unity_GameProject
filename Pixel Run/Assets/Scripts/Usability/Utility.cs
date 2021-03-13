using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Utility
{
    public static float Abs(float f) {
        if (f > 0f) return f;
        else return -f;
    }

    public static int Abs(int f) {
        if (f > 0) return f;
        else return -f;
    }

    public static int Min(int a, int b) {
        return a > b ? b : a;
    }

    public static float Min(float a, float b) {
        return a > b ? b : a;
    }

    public static float Power(float t) {
        return t * t;
    }

    public static int Power(int t) {
        return t * t;
    }

    public static float ToDot5(float t) {
        return (int)t + 0.5f;
    }

    public static Vector2 ClampPositionToDot5(Vector2 v) {
        return new Vector2(ToDot5(v.x), ToDot5(v.y));
    }

    /// <summery>
    /// Returns true if the distance between two points a and b is less than l.
    /// </summery>
    /// <param name="a">Point a</param>
    /// <param name="b">Point b</param>
    /// <param name="l">length l</param>
    public static bool DistanceIn(Vector2 a, Vector2 b, float l) {
        return Power(a.x - b.x) + Power(a.y - b.y) <= l * l;
    }

    public static T GetRandom<T>(in T[] t) {
        return t[UnityEngine.Random.Range(0, t.Length)];
    }

    public static T GetRandom<T>(in List<T> t) {
        return t[UnityEngine.Random.Range(0, t.Count)];
    }

    public static float GetRandomRange(float l, float r) {
        return UnityEngine.Random.Range(l, r);
    }

    public static int GetRandomRange(int l, int r) {
        return UnityEngine.Random.Range(l, r);
    }
}