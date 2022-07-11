using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumber : MonoBehaviour
{
    //Basic Logic
    int minNumber = 0;
    int maxNumber = 9;
    public int numberOne;
    public int numberTwo;
    public int numberThree;
    public int numberFour;


    //Differentiating the discs themselves. Each Disc registers different animal as the main number.

    public enum Animal
    {
        Tiger,
        Wolf,
        Fox,
        Rabbit
    }
    public Animal type;
    public int theNumber;

    //In-game objects / TextObjects



    void Start()
    {
        numberOne = Randomize(minNumber,maxNumber);
        numberTwo = Randomize(minNumber, maxNumber);
        numberThree = Randomize(minNumber, maxNumber);
        numberFour = Randomize(minNumber, maxNumber);
        print(numberOne +""+ numberTwo +""+ numberThree +""+ numberFour);
    }
    public int Randomize(int min, int max)
    {
        int number;
        return number = Random.Range(min,max);
    }

}
