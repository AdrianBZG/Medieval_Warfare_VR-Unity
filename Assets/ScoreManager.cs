using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    public int initialPoints;

    private int points;
    // Use this for initialization
    void Start()
    {
        points = initialPoints;
    }

    public void AddPoints(int quantity)
    {
        points += quantity;
    }

    public void RemovePoints(int quantity)
    {
        points -= quantity;
    }
}
