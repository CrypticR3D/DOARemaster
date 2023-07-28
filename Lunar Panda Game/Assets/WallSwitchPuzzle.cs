using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSwitchPuzzle : MonoBehaviour
{
    [SerializeField] switchChanger amogusComplete;
    [SerializeField] ConnectInputsAndOutputs amogusPuzzle;
    [SerializeField] List<switchChanger> buttons;
    [SerializeField] switchChanger submitButton;

    [SerializeField] bool[] combination1 = new bool[4];
    [SerializeField] bool[] combination2 = new bool[4];
    [SerializeField] bool[] combination3 = new bool[4];
    //[SerializeField] LabMachine labMachine;
    [SerializeField] bool[] completedCombinations = new bool[3];
    List<bool[]> combinations = new List<bool[]>();
    [SerializeField] bool[] currentCombination = new bool[4];


    void Awake()
    {
        combinations.Add(combination1);
        combinations.Add(combination2);
        combinations.Add(combination3);

        for (int i = 0; i < currentCombination.Length; i++)
        {
            currentCombination[i] = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
