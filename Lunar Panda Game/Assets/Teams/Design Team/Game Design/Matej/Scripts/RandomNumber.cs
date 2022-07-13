using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomNumber : MonoBehaviour
{
    //Basic Logic
    int minNumber = 0;
    int maxNumber = 9;
    int numberOne;
    int numberTwo;
    int numberThree;
    int numberFour;


    //Differentiating the discs themselves. Each Disc registers different animal as the main number.
    //We need to keep track of this to know what the code for the safe will be 
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

    public TextMeshPro tigerNumber;
    public TextMeshPro wolfNumber;
    public TextMeshPro foxNumber;
    public TextMeshPro rabbitNumber;

    void Awake()
    {
        //Decide the numbers
        numberOne = Randomize(minNumber, maxNumber);
        numberTwo = Randomize(minNumber, maxNumber);
        numberThree = Randomize(minNumber, maxNumber);
        numberFour = Randomize(minNumber, maxNumber);
        print(numberOne + "" + numberTwo + "" + numberThree + "" + numberFour);
        //Show the numbers
        tigerNumber.text = numberOne.ToString();
        wolfNumber.text = numberTwo.ToString();
        foxNumber.text = numberThree.ToString();
        rabbitNumber.text = numberFour.ToString();
        switch (type)
        {
            case Animal.Tiger:
                theNumber = numberOne;
                break;
            case Animal.Wolf:
                theNumber = numberTwo;
                break;
            case Animal.Fox:
                theNumber = numberThree;
                break;
            case Animal.Rabbit:
                theNumber = numberFour;
                break;
            default:
                break;
        }
    }
    public int Randomize(int min, int max)
    {
        int number;
        return number = Random.Range(min, max);
    }
    public int GetNumber()
    {
        return theNumber;
    }


}
