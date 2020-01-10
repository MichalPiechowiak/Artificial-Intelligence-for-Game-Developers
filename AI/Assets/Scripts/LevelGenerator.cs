using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour {
    public List<GameObject> ListOfAsteroids = new List<GameObject>();
    public GameObject goAsteroid;
    public Text scoreBoard;

    public float fDestroyPosition = -6.0f;
    public float asteroidRate = 1.0f;
    private float nextAsteroid = 0.0f;
    public int iScore = 0;
    
    void Start () {
        Random.InitState(299);
        scoreBoard = GameObject.Find("AsteroidsDodged").GetComponent<Text>();
        DisplayScore();
    }
    void Update()
    {
        if (Time.time > nextAsteroid)
        {
            Vector3 vPosition = new Vector3(Random.Range(-5.0f, 5.0f), 7.0f, 0.0f);

            GameObject go = Instantiate(goAsteroid, vPosition, Quaternion.identity);
            go.transform.parent = gameObject.transform;
            ListOfAsteroids.Add(go);

            nextAsteroid = Time.time + asteroidRate;
            iScore++;
            DisplayScore();
        }
        for (int i = 0; i < ListOfAsteroids.Count; i++)
        {
            if (ListOfAsteroids[i].transform.position.y < fDestroyPosition)
            {
                Destroy(ListOfAsteroids[i].gameObject);
                ListOfAsteroids.Remove(ListOfAsteroids[i]);
            }
        }
    }
    public void DestroyAll()
    {
        for (int i = 0; i < ListOfAsteroids.Count; i++)
        {
            Debug.Log(ListOfAsteroids.Count);
            Destroy(ListOfAsteroids[i].gameObject);
            ListOfAsteroids.Remove(ListOfAsteroids[i]);
        }
    }
    void DisplayScore()
    {
        scoreBoard.text = "Score: " + iScore.ToString();
    }
}