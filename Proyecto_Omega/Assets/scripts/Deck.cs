using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class Deck : MonoBehaviour {
    private List<string> deck = new List<string>();
    //este seria el metodo de cargar, SI TUVIERA UNO!
    /*private void ReadFromLine()
    {
        StreamReader reader = new StreamReader("/Users/DK_RZ/Documents/scripts/Assets/deck/Deck.txt");
        string s;
        while ((s = reader.ReadLine()) != null)
        {
            print(s);
            deck.Add(s);
        }
    }*/
    private void OnMouseUp()
    {
        //print(RobarEspecifico("st-1"));
        //print(Robar());
        string robada = Robar();
        if (!robada.Equals("")) {
            GameObject.Find("DarkArea").GetComponent<DarArea>().Meter(robada);
        }
        else
        {

            print("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Shuffle();
        }
    }
    void Shuffle()
    {
        for (int i = deck.Count-1; i > 1; i--)
        {
            int aleatorio = Random.Range(0, deck.Count);
            string temp = deck[i];
            deck[i] = deck[aleatorio];
            deck[aleatorio] = temp;
        }
    }
    string Robar()
    {
        try
        {
            string x = deck[0];
            deck.RemoveAt(0);
            return x;
        }
        catch (System.ArgumentOutOfRangeException)
        {
            deck = GameObject.Find("DarkArea").GetComponent<DarArea>().Devolver();
            Shuffle();
            return ("");
        }

    }
    public string RobarEspecifico(string codCarta)
    {
        int i = 0;
        foreach (var item in deck)
        {
            if (item.Equals(codCarta))
            {
                string x = deck[i];
                deck.RemoveAt(i);
                return x;
            }
            i++;
        }
        throw new System.Exception();
    }

    // Use this for initialization
    void Start() {
        //ReadFromLine();
        //yo actualmente cargo el deck desde otra clase
        foreach (var item in deck)
        {
            print(item);
        }
        Shuffle();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<string> GetDeck()
    {
        return deck;
    }
    public void SetDeck(List<string> deck)
    {
        this.deck = deck;
    }
}
