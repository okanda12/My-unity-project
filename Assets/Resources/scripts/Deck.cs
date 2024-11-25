using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck 
{
    private List<ICard> cards;

    public Deck()
    {
        cards = new List<ICard>();//interface�ňقȂ��ނ̃J�[�h���f�b�L�ɓ������
    }

    public void AddCard(ICard card)
    {
        cards.Add(card);
    }

    public ICard DrawCard()
    {
        if (cards.Count == 0) return null;
        ICard drawnCard = cards[0];
        cards.RemoveAt(0);//�������J�[�h���f�b�L����폜
        return drawnCard;

    }

    public void Shuffle()
    {
        for (int i=0; i<cards.Count; i++)
        {
            int randomIndex = Random.Range(i, cards.Count);
            ICard temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    public int DeckCount
    {
        get { return cards.Count; }

    }
    public void PrintCardNames()
    {
        foreach (ICard card in cards)
        {
            Debug.Log(card.getCardName());
        }
    }



}
