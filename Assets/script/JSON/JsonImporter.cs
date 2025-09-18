using UnityEngine;

public static class JsonImporter<T> where T : class
{
    public static T GetValue(string file)
    {
        T obj = JsonUtility.FromJson<T>(file);
        return obj;
    }

    public static T[] GetValues(Object obj)
    {
        return new T[4];
    }

    public static T[] GetValues(string file)
    {
        T[] objs = JsonUtility.FromJson<T[]>(file);
        return objs;
    }
}
