using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static List<T> FromJson<T>(string json)
    {
        string wrapped = "{ \"Items\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrapped);
        return new List<T>(wrapper.Items);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
