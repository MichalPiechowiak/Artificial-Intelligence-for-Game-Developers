using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    public List<float> genes = new List<float>();
    public float fFitness;

    public DNA(int genomeLenght = 50)
    {
        for (int i = 0; i < genomeLenght; i++)
        {
            genes.Add(Random.Range(-1.0f, 1.0f));
        }
    }
    public DNA(DNA parent, DNA partner, float fMutationRate = 0.05f)
    {
        for (int i = 0; i < parent.genes.Count; i++)
        {
            float fMutationChance = Random.Range(0.0f, 1.0f);
            if (fMutationChance <= fMutationRate)
            {
                genes.Add(Random.Range(-1.0f, 1.0f));
            }
            else
            {
                int iCrossOverChance = Random.Range(0, 2);
                if (iCrossOverChance == 0)
                {
                    genes.Add(parent.genes[i]);
                }
                else
                {
                    genes.Add(partner.genes[i]);
                }
            }
        }
    }
}