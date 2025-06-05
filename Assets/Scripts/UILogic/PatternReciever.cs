using UnityEngine;

public class PatternReceiver : MonoBehaviour
{
    public static PatternReceiver Instance { get; private set; }

    private Pattern selectedPattern;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UsePattern(Pattern pattern)
    {
        selectedPattern = pattern;
    }

    public Pattern GetPattern()
    {
        return selectedPattern;
    }
}