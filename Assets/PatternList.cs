using UnityEngine;

public class PatternList : MonoBehaviour
{
    public ScriptableObject[] patterns;

    public ScriptableObject getObjectAtIndex(int index)
    {
        return patterns[index];
    }

    public int getObjectCount()
    {
        return patterns.Length;
    }
}
