using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class Carta : MonoBehaviour
{
    public string[] Code;
}
public class Deck : MonoBehaviour {
    private List<string> deck = new List<string>();
    public Carta[] mazo = new Carta[29];
    //este seria el metodo de cargar, SI TUVIERA UNO!
    private void ReadFromLine()
    {
        StreamReader reader = new StreamReader("/Users/DK_RZ/Documents/scripts/Assets/deck/Deck.txt");
        string s;
        while ((s = reader.ReadLine()) != null)
        {
            print(s);
            deck.Add(s);
        }
    }
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
            Mezclar();
        }
    }
    void Mezclar()
    {
        print("a");
        for (int i = 29; i > 1; i--)
        {
            int aleatorio = Random.Range(0, 29);
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
            Mezclar();
            return ("");
        }

    }
    public string RobarEspecifico(string codCarta)
    {
        int i = 0;
        foreach (var item in deck)
        {
            print(item + " " + codCarta);
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
        ReadFromLine();
        //yo actualmente cargo el deck desde otra clase
        //deck = CNTdrag.deck;
        foreach (var item in mazo)
        {
            print(item);
        }
        Mezclar();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
