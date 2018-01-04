using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class Deck : MonoBehaviour {
    private List<string> deck = new List<string>();

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
            FisherYates(deck);
        }
    }
    private void Swap(List<string> deck, int primero, int segundo)
    {
        string temp = deck[primero];
        deck[primero] = deck[segundo];
        deck[segundo] = temp;
    }
    public void FisherYates(List<string> deck)
    {
        for (int i = 0; i < deck.Count - 1; i++)
        {
            int index = Random.Range(0, i);
            Swap(deck, i, index);
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
            FisherYates(deck);
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
        FisherYates(deck);
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
