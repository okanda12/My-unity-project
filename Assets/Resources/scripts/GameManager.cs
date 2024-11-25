using UnityEngine;
using System.Collections.Generic;

using System.Collections;

public class GameManager : MonoBehaviour
{
    

    //カードのプレファブを入れる
    [SerializeField] CardController cardPrefab;
    //ヒーローのプレファブを入れる
   // [SerializeField] heroPrefab;

    //手札のTransformを入れる
    public Transform PlayerHandTransform, EnemyHandTransform;
    public Transform PlayerFieldTransform, EnemyFieldTransform;
    public Transform PlayerHEROfield, EnemyHEROfield;
    public Transform PlayerDeckTransform, EnemyDeckTransform;
    public Transform PlayerCostframe, EnemyCostframe;

    private Deck playerDeck;
    private Deck enemyDeck;

    //プレイヤーのターンかの判定
    public bool isPlayerTurn;//Turndisplayerに出力

    public string PlayerHERO = "ARMA";
    public string EnemyHERO = "ZERO2";


    public int Player_Mana;
    public int Player_maxMana;

    public int Enemy_Mana;
    public int Enemy_maxMana;

    //manasystemとかの方が良くない？
    public Manasystem manasys;



    public CardController selectedCard;//現在選択されているカード
    public List<CardController> enemyCards;//敵カードのリスト

    public CardController PlayerHerocon;
    public CardController EnemyHerocon;

    



    //シングルトンにするための呪文
    public static GameManager Instance { get; private set; }

    private void Awake()
    {

        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }
    //

    void Start()
    {

        if (manasys == null)
        {
            manasys = new Manasystem();
        }


        StartGame();



        isPlayerTurn = true;
        TurnCalc();
    }

    private void StartGame()
    {

        HeroAppears();

        
       


        StartCoroutine(SettingHand());
        manasys.InitializeMana(0);


    }


    public void EnemyFieldHighlight(bool YES)
    {//敵の情報を取得.そいつらにハイライトをつける
        

        CardController[] EnemyFieldcardList = EnemyFieldTransform.GetComponentsInChildren<CardController>();

        if (YES == true)
        {
            foreach (var card in EnemyFieldcardList)
            {
                CardHighlightController cardhighlight = card.GetComponent<CardHighlightController>();
                cardhighlight.ShowRedHighlight();
                //Debug.Log($"Card Name: {card.name}, ATK: {card.model.at}, HP: {card.model.hp}");
            }
        }
        else
        {
            foreach (var card in EnemyFieldcardList)
            {
                CardHighlightController cardhighlight = card.GetComponent<CardHighlightController>();
                cardhighlight.RemoveHighlight();
                //Debug.Log($"Card Name: {card.name}, ATK: {card.model.at}, HP: {card.model.hp}");
            }

        }

    }



    //カードが選ばれたときの処理
    //cardには攻撃元カードが入ってくる
    public void SelectCardToAttack(CardController card,string isAttacker)
    {

        if (isAttacker == "Attacker" )
        {
            //最初にカードを選ぶ
            selectedCard = card;
            if (selectedCard.canAttack == true)
            {
                
                Debug.Log("攻撃者カード選択:" + selectedCard.model.name);//お名前を取得
                EnemyFieldHighlight(true);//ハイライトを付けます.
            }
            else
            {
                Debug.Log(selectedCard.model.name + "can't Attack!");

                CardAttackAnimation cantAttackAnim = card.GetComponent<CardAttackAnimation>();

                StartCoroutine(cantAttackAnim.CantAttack(card.transform));

               


            }
            
            
        }
        else if(isAttacker=="Defender" && selectedCard !=null)//ここに条件を追加
        {

            if (selectedCard.canAttack == true)
            {
                //既にカードが選ばれている場合はターゲット選択
                AttackTarget(selectedCard, card);//攻撃者　守備者の順
                Debug.Log("被攻撃者カード選択:" + card.model.name);
                selectedCard.canAttack = false;
                selectedCard = null;//攻撃後は選択を解除

                EnemyFieldHighlight(false);//ハイライトを消します
            }
            else
            {
                Debug.Log(selectedCard.model.name + "can't Attack!");

                //防御者側にはいらなかったわ
                //CardAttackAnimation cantAttackAnim = card.GetComponent<CardAttackAnimation>();

                //StartCoroutine(cantAttackAnim.CantAttack(card.transform));

            }


        }

    }

    public void AttackTarget(CardController attacker,CardController target)
    {
        Transform targetparent = target.transform.parent;
        //ターゲットが敵カードであれば攻撃する
        if (targetparent == EnemyFieldTransform || targetparent == EnemyHEROfield)
        {
            attacker.Attack(target);//攻撃メソッド呼び出し



            Debug.Log($"{attacker.model.name}が{target.model.name}を攻撃しました");
        }

    }





    private void HeroAppears()
    {
        

        
        ICard Enemycard= LoadHeroCard(2);//02
        ICard Playercard = LoadHeroCard(1);//ARMA

        CreateHero(PlayerHEROfield, Playercard);
        CreateHero(EnemyHEROfield, Enemycard);




        playerDeck = CreateDeck("ARMA");
        enemyDeck = CreateDeck("ZERO2");

    }

    private Deck CreateDeck(string hero)
    {
        Deck deck = new Deck();

        //minionとmagicのカードをデッキに追加
        for(int i = 1; i <= 3; i++)//3枚のミニオンを追加
        {
            ICard card = LoadCard(hero, "minions", i);
            if (card != null) deck.AddCard(card);
        }
        for (int i = 1; i <= 3; i++)//3枚のmagicを追加
        {
            ICard card = LoadCard(hero, "magics", i);
            if (card != null) deck.AddCard(card);
        }
        deck.Shuffle();//シャッフル

        Debug.Log(deck.DeckCount);
        deck.PrintCardNames();//デバック用関数

        return deck;
        
    }

    void TurnCalc()
    {
        //ターンの進行の処理をする.
        if (isPlayerTurn)
        {
            PlayerTurn();
            
        }
        else
        {
            EnemyTurn();
        }
    }

    void PlayerTurn()
    {
        //プレイヤーターンの処理
        Debug.Log("プレイヤーのターンです");
        manasys.AddMana(1);
        manasys.AddmaxMana(1);
        manasys.RestoreMana(PlayerCostframe);

        CardController[] PlayerFieldcardList = PlayerFieldTransform.GetComponentsInChildren<CardController>();


        //攻撃フラッグを初期化する
        foreach (var card in PlayerFieldcardList)
        {
            card.canAttack = true;

        }

    }

    void EnemyTurn()
    {
        //エネミーターンの処理
        Debug.Log("敵のターンです");
        manasys.AddMana(1);
        manasys.AddmaxMana(1);
        manasys.RestoreMana(EnemyCostframe);
        //EnemyHandtransform からcardcontrollerを持つ子オブジェクトを検索
        CardController[] cardList= EnemyHandTransform.GetComponentsInChildren<CardController>();
        //場に出すカードを選択
        CardController card = cardList[0];
        //カードを移動する
        card.movement.SetCardTransform(EnemyFieldTransform);
        //ターンを終了する
        ChangeTurn();
    }

    public void ChangeTurn()
    {
        //ターンの切り替え処理
        isPlayerTurn = ! isPlayerTurn;

        if (isPlayerTurn)
        {

            if (playerDeck.DeckCount!=0)
            {
                ICard playerCardEntity = playerDeck.DrawCard();
                CreateHand(PlayerHandTransform, playerCardEntity);
                
            }
            else
            {
                Debug.Log("player cant draw!!");
            }
            //ドローする
            
            

        }
        else
        {
            //ドローする

            if (enemyDeck.DeckCount != 0)
            {
                ICard enemyCardEntity = enemyDeck.DrawCard();
                CreateHand(EnemyHandTransform, enemyCardEntity);
            }
            else
            {
                Debug.Log("Enemy cant draw!!");
            }

            
            
        }
        TurnCalc();
    }









    void CreateHand(Transform hand, ICard cardEntity)
    {
        CardController card;
        //一時的に親をデッキにする.
        //Transform deckTransform = GameManager.Instance.PlayerDeckTransform;

        if (hand == PlayerHandTransform)//もしプレイヤーなら
        {
            card = Instantiate(cardPrefab, PlayerDeckTransform, false);


        }
        else//敵なら
        {
            card = Instantiate(cardPrefab, EnemyDeckTransform, false);
        }

        


        //()にカードIDを入れる
        card.Init(cardEntity);
        //controllerはMonobehaviorを継承しているのでgetできる
        CardDrawAnimation cardAnim = card.GetComponent<CardDrawAnimation>();
        if (cardAnim != null)
        {
            //cardAnim.transform.SetParent(hand);
            StartCoroutine(cardAnim.Drawsetparent(cardAnim,hand));
        }

        
        



    }




    
    void CreateHero(Transform herofield, ICard cardEntity)
    {
        CardController card;

        if (herofield == PlayerHEROfield)//もしプレイヤーなら
        {
            card = Instantiate(cardPrefab, PlayerHEROfield, false);

            PlayerHerocon = card;

        }
        else//敵なら
        {
            card = Instantiate(cardPrefab, EnemyHEROfield, false);
            EnemyHerocon = card;
        }

        //カード初期化
        card.Init(cardEntity);

    }








    public IEnumerator SettingHand()
    {
        //それぞれの手札に3枚カードを配る
        for (int i=0;i<3;i++)
        {


            ICard playerCardEntity = playerDeck.DrawCard();
            ICard enemyCardEntity = enemyDeck.DrawCard();
            CreateHand(PlayerHandTransform, playerCardEntity);
            CreateHand(EnemyHandTransform, enemyCardEntity);
            yield return new WaitForSeconds(0.2f);
        }

    }

    private ICard LoadCard(string hero,string cardType,int cardID)
    {

        //以下を簡略化するためのコード
        //ICard EnemyCardEntity = Resources.Load<ZERO2minion>("CardEntityList/ZERO2/minions/minion " + 1);
        return Resources.Load<ScriptableObject>($"CardEntityList/{hero}/{cardType}/{cardType} {cardID}") as ICard;
    }


    private ICard LoadHeroCard(int cardID)
    {

        //PlayerHEROfield, EnemyHEROfield;
        return Resources.Load<ScriptableObject>($"CardEntityList/Heros/Heros {cardID}") as ICard;
    }


}
