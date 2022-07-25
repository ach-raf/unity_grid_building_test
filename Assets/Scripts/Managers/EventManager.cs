using System;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{

    public static event Action<Buildable> BuildableDestroyEvent;
    public static void OnBuildableDestroy(Buildable buildable_object) => BuildableDestroyEvent?.Invoke(buildable_object);




}
