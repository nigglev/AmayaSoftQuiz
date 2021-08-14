using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public static class Utils
{
    public static T GetRandomElement<T>(this T[] in_array)
    {
        if (in_array == null || in_array.Length == 0)
            return default(T);
        int index = UnityEngine.Random.Range(0, in_array.Length);
        T element = in_array[index];
        return element;
    }

    public static T GetRandomElement<T>(this List<T> in_list, bool in_delete_element = false)
    {
        if (in_list == null || in_list.Count == 0)
            return default(T);
        int index = UnityEngine.Random.Range(0, in_list.Count);
        T element = in_list[index];
        if (in_delete_element)
            in_list.RemoveAt(index);
        return element;
    }
}
