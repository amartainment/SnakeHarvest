using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class MyEventSystem
{

    public static System.Action<int> worldEnd;
    public static System.Action<int> ResetHeroEvent;
    public static System.Action<float> track1Event;
    public static System.Action<int> levelComplete;

    public static System.Action<float> speedChange;

    public static System.Action<int> heroDeath;




}