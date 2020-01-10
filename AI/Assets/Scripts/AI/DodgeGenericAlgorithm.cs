using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeGenericAlgorithm : MonoBehaviour
{
    public DNA dna;
    int iPathIndex = 0;
    public bool bHasFinished = false;
    public bool bHasCrashed = false;
    bool bHasBeenInitialized = false;
    float fNextPoint;
    public void InitCreature(DNA newDNA)
    {
        dna = newDNA;
        fNextPoint = transform.position.x;
        bHasBeenInitialized = true;
    }
    private void Update()
    {

        if (bHasBeenInitialized && !bHasFinished)
        {
            if(iPathIndex == dna.genes.Count)
            {
                bHasFinished = true;
            }
            if(transform.position.x == fNextPoint)
            {
                if (transform.position.x + dna.genes[iPathIndex] > 5.0f)
                {
                    dna.fFitness -= 10;
                }
                else if (transform.position.x + dna.genes[iPathIndex] < -5.0f)
                {
                    dna.fFitness -= 10;
                }
                else
                {
                    fNextPoint = transform.position.x + dna.genes[iPathIndex];
                }
                iPathIndex++;
            }
            else
            {
                Vector2 newVector2 = new Vector2(fNextPoint, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, newVector2, 10.0f * Time.deltaTime);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            bHasFinished = true;
            bHasCrashed = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}