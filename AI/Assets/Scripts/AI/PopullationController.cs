using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PopullationController : MonoBehaviour
{
    List<DodgeGenericAlgorithm> population = new List<DodgeGenericAlgorithm>();
    List<GameObject> levelReset = new List<GameObject>();

    public GameObject creaturePrefab;
    public GameObject generateLevelObject;
    public Text generationNumberText;

    public int iPopulationSize = 100;
    public int iGenomeLenght = 100;
    public int iSurvivorsKeep;
    private int iGenerationNumber;
    public float fCutOff = 0.3f;
    public float fStartTime;
    private float fTime; 

    public Transform SpawnPoint;
    void InitializePopulation()
    {
        for (int i = 0; i < iPopulationSize; i++)
        {
            GameObject go = Instantiate(creaturePrefab, SpawnPoint.position, Quaternion.identity);
            go.GetComponent<DodgeGenericAlgorithm>().InitCreature(new DNA(iGenomeLenght));
            population.Add(go.GetComponent<DodgeGenericAlgorithm>());
        }
    }
    void NextGeneration()
    {
        int iSurvivors = Mathf.RoundToInt(iPopulationSize * fCutOff);
        List<DodgeGenericAlgorithm> lSurvivors = new List<DodgeGenericAlgorithm>();
        for (int i = 0; i < iSurvivors; i++)
        {
            lSurvivors.Add(GetFitesst());
        }
        for (int i = 0; i < population.Count; i++)
        {
            Destroy(population[i].gameObject);
        }
        population.Clear();

        for(int i = 0; i < iSurvivorsKeep; i++)
        {
            GameObject go = Instantiate(creaturePrefab, SpawnPoint.position, Quaternion.identity);
            go.GetComponent<DodgeGenericAlgorithm>().InitCreature(lSurvivors[i].dna);
            population.Add(go.GetComponent<DodgeGenericAlgorithm>());
            population[i].GetComponent<DodgeGenericAlgorithm>().dna.fFitness = 0;
        }

        while (population.Count < iPopulationSize)
        {
            for (int i = 0; i < lSurvivors.Count; i++)
            {
                GameObject go = Instantiate(creaturePrefab, SpawnPoint.position, Quaternion.identity);
                go.GetComponent<DodgeGenericAlgorithm>().InitCreature(new DNA(lSurvivors[i].dna, lSurvivors[Random.Range(0, lSurvivors.Count)].dna));
                population.Add(go.GetComponent<DodgeGenericAlgorithm>());
                if (population.Count >= iPopulationSize)
                {
                    break;
                }
            }

        }
        for (int i = 0; i < lSurvivors.Count; i++)
        {
            Destroy(lSurvivors[i].gameObject);
        }
    }
    private void Awake()
    {
        fTime = Time.time;
        iGenerationNumber++;
        generationNumberText.text = "Generation: " + iGenerationNumber.ToString();

        
        GameObject go = Instantiate(generateLevelObject, new Vector2(0.0f, 0.0f), Quaternion.identity);
        levelReset.Add(go);
        InitializePopulation();
        iSurvivorsKeep = (int)(population.Count * 0.1f);
    }
    private void Update()
    {
        if (Time.time >= fTime + 1.0f)
        {
            fTime = Time.time;
            for (int i = 0; i < population.Count; i++)
            {
                if (!population[i].bHasFinished || !population[i].bHasCrashed)
                {
                    population[i].dna.fFitness += 50;
                }
            }
        }
        if (!HasActive())
        {
            CreateGameLog();
            iGenerationNumber++;
            generationNumberText.text = "Generation: " + iGenerationNumber.ToString();
            Destroy(levelReset[0].gameObject);
            levelReset.Remove(levelReset[0]);
            GameObject go = Instantiate(generateLevelObject, new Vector2(0.0f, 0.0f), Quaternion.identity);
            levelReset.Add(go);
            NextGeneration();
        }
    }
    DodgeGenericAlgorithm GetFitesst()
    {
        float fMaxFitness = float.MinValue;
        int iIndex = 0;
        for (int i = 0; i < population.Count; i++)
        {
            if (population[i].dna.fFitness > fMaxFitness)
            {
                fMaxFitness = population[i].dna.fFitness;
                iIndex = i;
            }
        }
        DodgeGenericAlgorithm fittest = population[iIndex];
        population.Remove(fittest);
        return fittest;
    }
    public bool HasActive()
    {
        for (int i = 0; i < population.Count; i++)
        {
            if (!population[i].bHasFinished)
            {
                return true;
            }
        }
        return false;
    }
    void CreateGameLog()
    {
        string textPath = Application.dataPath + "/GameLog.txt";
        if (!File.Exists(textPath))
        {
            File.WriteAllText(textPath, "AI outputs \n");
        }
        string textContent = "Generation: " + iGenerationNumber + "    Fitesst: " + GetFitesst().dna.fFitness + "             Score: " + levelReset[0].GetComponent<LevelGenerator>().iScore + "\n";
        File.AppendAllText(textPath, textContent);
    }
}