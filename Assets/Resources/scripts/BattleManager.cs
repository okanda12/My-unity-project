using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{


    //カードのプレファブを入れる
    [SerializeField] CardController cardPrefab;
    [SerializeField] GameObject ARMAdevpre;
    [SerializeField] GameObject RAIKAdevpre;
    [SerializeField] GameObject Result_Prefab;
    
    //ヒーローのプレファブを入れる
    // [SerializeField] heroPrefab;


    public Transform PlayerHandTransform, EnemyHandTransform;
    public Transform PlayerFieldTransform, EnemyFieldTransform;
    public Transform PlayerHEROfield, EnemyHEROfield;
    public Transform PlayerDeckTransform, EnemyDeckTransform;
    public Transform PlayerCostframe, EnemyCostframe;
    public Transform canvas2; //攻撃するときにカードが一番上に表示されるように移動するキャンバス
    public Transform MagiCasPlace;//魔法をキャストする場所
    public Transform EnemyDevice, PlayerDevice;//デバイスを置く場所


    public Deck playerDeck;
    public Deck enemyDeck;

    //プレイヤーのターンかの判定
    public bool isPlayerTurn;//Turndisplayerに出力

    public string PlayerHERO = "ARMA";
    public string EnemyHERO = "RAIKA";


    public int Player_Mana;
    public int Player_maxMana;

    public int Enemy_Mana;
    public int Enemy_maxMana;

    //マナを管理する
    public Manasystem manasys;



    public string Winner = "nobody";
    public string Loser = "nobody";

    //特殊デバイス
    private RAIKAdevice RAIKAdevice;


    public CardController selectedCard;//現在選択されているカード
    public List<CardController> enemyCards;//敵カードのリスト

    public CardController PlayerHerocon;
    public CardController EnemyHerocon;


    //敵のターン,以下2つのフラッグがtrueになったらターン終了
    private bool EMcastflag = false;//キャストできるカードがないとtrue;
    private bool EMatackflag = false; //アタックできるカードが無いとtrue;

    public bool isAnimating = false;//アニメーション中かどうか判断


    private bool EnemyCasted = false;//敵がカードを使ったか見る


    //シングルトンにするための呪文
    public static BattleManager Instance { get; private set; }

    private void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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


        StartCoroutine(StartGame());



        
    }

    private IEnumerator StartGame()
    {

        //ヒーろー出現モーションを入れたい

        yield return new WaitForSeconds(1.5f);
        
        //ヒーろ‐デバイスモーションを入れたい


        HeroAppears();

       



        StartCoroutine(SettingHand());
        manasys.InitializeMana(0);
        isPlayerTurn = true;
        TurnCalc();

        RAIKAdevice = FindObjectOfType<RAIKAdevice>();
    }



    public void Result()
    {

        GameObject canvas = GameObject.Find("Canvas");
        GameObject result = Instantiate(Result_Prefab, canvas.transform);





    }


    public void EnemyFieldHighlight(bool YES)
    {//敵の情報を取得.そいつらにハイライトをつける

        //ちな孫も追えます
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
    public void SelectCardToAttack(CardController card, string isAttacker)
    {

        if (isAttacker == "Attacker")//攻撃者なら
        {
            //選択しているカード
            selectedCard = card;
            if (selectedCard.canAttack == true)//攻撃できるなら
            {

                Debug.Log("攻撃者カード選択:" + selectedCard.model.name);//お名前を取得
                EnemyFieldHighlight(true);//敵のフィールドのミニオンにハイライトを付けます.


            }
            else
            {
                Debug.Log(selectedCard.model.name + "can't Attack!");

                CardAttackAnimation cantAttackAnim = card.GetComponent<CardAttackAnimation>();

                StartCoroutine(cantAttackAnim.CantAttack(card.transform));




            }


        }
        else if (isAttacker == "Defender" && selectedCard != null)//防御者なら
        {

            if (selectedCard.canAttack == true)
            {
                //既にカードが選ばれている場合はターゲット選択
                AttackTarget(selectedCard, card);//攻撃者　守備者の順
                Debug.Log("被攻撃者カード選択:" + card.model.name);

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

    public void AttackTarget(CardController attacker, CardController target)
    {
        //Transform targetparent = target.transform.parent;
        //ターゲットが敵カードであれば攻撃する
        //if (targetparent == EnemyFieldTransform || targetparent == EnemyHEROfield)
        //{
        attacker.Attack(target);//攻撃メソッド呼び出し

        Debug.Log($"{attacker.model.name}が{target.model.name}を攻撃しました");
        // }

    }




    //ヒーローが登場する部分です
    private void HeroAppears()
    {
        //ここもっと簡素化出来るかも

        ICard Playercard=LoadHeroCard(1);
        ICard Enemycard=LoadHeroCard(1);

        switch (PlayerHERO) {
            case "ARMA":
                Playercard= LoadHeroCard(1);
                playerDeck = CreateDeck("ARMA");
                Instantiate(ARMAdevpre, PlayerDevice, false);
                break;
            case "RAIKA":
                Playercard = LoadHeroCard(2);
                playerDeck = CreateDeck("RAIKA");
                Instantiate(RAIKAdevpre, PlayerDevice, false);
                break;

        }

        switch (EnemyHERO)
        {
            case "ARMA":
                Enemycard = LoadHeroCard(1);
                enemyDeck = CreateDeck("ARMA");
                Instantiate(ARMAdevpre, EnemyDevice, false);
                break;
            case "RAIKA":
                Enemycard = LoadHeroCard(2);
                enemyDeck = CreateDeck("RAIKA");
                Instantiate(RAIKAdevpre, EnemyDevice, false);
                break;

        }
       

        CreateHero(PlayerHEROfield, Playercard);
        CreateHero(EnemyHEROfield, Enemycard);


        

    }

    private Deck CreateDeck(string hero)
    {
        Deck deck = new Deck();
        ICard card;

        //minionとmagicのカードをデッキに追加
        for (int i = 1; i <= 5; i++)//3枚のミニオンを追加
        {
            card = LoadCard(hero, "minions", i);
            if (card != null) deck.AddCard(card);
        }

        //狼いっぱい入れたいという願い
        card = LoadCard(hero, "minions", 1);
        if (card != null) deck.AddCard(card);
        card = LoadCard(hero, "minions", 1);
        if (card != null) deck.AddCard(card);
        card = LoadCard(hero, "minions", 1);
        if (card != null) deck.AddCard(card);
        for (int i = 1; i <= 5; i++)//3枚のmagicを追加
        {
            card = LoadCard(hero, "magics", i);
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
            StartCoroutine(EnemyTurn());
        }
    }

    void PlayerTurn()
    {
        //プレイヤーターンの処理
        Debug.Log("プレイヤーのターンです");



        manasys.AddMana(1);
        //manasys.AddEmptyMana(1);
        manasys.RestoreMana(PlayerCostframe);

        CardController[] PlayerFieldcardList = PlayerFieldTransform.GetComponentsInChildren<CardController>();


        //攻撃フラッグを初期化する
        foreach (var card in PlayerFieldcardList)
        {
            card.canAttack = true;

        }

    }







    //敵のキャストセクション
    public IEnumerator EnemyCast()
    {
        EnemyCasted = false;//カードを使ったか見ます
        
        Debug.Log("Enemycast!");

        CardController[] EMhandCC = EnemyHandTransform.GetComponentsInChildren<CardController>();//ハンドのカードコントローラ
        CardController[] EMfieldCC = EnemyFieldTransform.GetComponentsInChildren<CardController>();//フィールドのコントローラー


        //もし手札に何もなかったら終了

        if (EMhandCC.Length == 0)
        {
            EMcastflag = true;
            Debug.Log("EMhandCC is empty");
            yield return new WaitForSeconds(0.5f);
            yield break; // コルーチンを終了
        }

        int maxIterations = EMhandCC.Length; // 無限ループ防止
        int iterationCount = 0;


        //maxIterationまで以下を繰り返す.相手がちょっと考えるような動作をとるときはこれが原因

        while (iterationCount < maxIterations)
        {


            bool cardCasted = false;




            foreach (CardController card in EMhandCC)//ハンドを全てみて　
            {
                if (card.model.cost <= Enemy_Mana && EMfieldCC.Length < 5)//手札のカード一枚を見ています
                {
                    
                    //フィールドの状態を見ている
                    List<Transform> NocardDaiza = new List<Transform>();//ここにオブジェクトを持たない台座を追加していく


                    foreach (Transform child in EnemyFieldTransform)//子オブジェクトを持たないdaizaを取得していく
                    {
                        if (child.childCount == 0) // 子オブジェクトを持たないなら葉ノード
                        {
                            NocardDaiza.Add(child);
                        }

                    }


                    CardCastAnimation castAnim = card.GetComponent<CardCastAnimation>();
                    BattleManager.Instance.manasys.UseMana(card.model.cost);

                    if (card.model.cardType=="Magic")
                    {
                        StartCoroutine(castAnim.MagicCast(NocardDaiza[0]));
                        
                    }
                    else
                    {
                        StartCoroutine(castAnim.MinionCast(NocardDaiza[0]));
                    }


                    EnemyCasted = true;


                    cardCasted = true;


                    //yield return new WaitForSeconds(0.5f);
                    break; // foreach を抜ける


                }
            }

            // 手札・フィールドを更新
            EMhandCC = EnemyHandTransform.GetComponentsInChildren<CardController>();
            EMfieldCC = EnemyFieldTransform.GetComponentsInChildren<CardController>();

            // 出せるカードがなくなったら終了
            if (!cardCasted) break;

            iterationCount++;
        }
        yield return new WaitForSeconds(0.1f);
        EMcastflag = true;



    }


    //敵の攻撃命令です
    public IEnumerator EnemyAttack()
    {
        Debug.Log("EnemyAttack!");
        bool canatEMfield = true;//何か攻撃できるカードがいるとき


        while (canatEMfield != false)
        {
            canatEMfield = false;

            CardController[] EMfieldCC = EnemyFieldTransform.GetComponentsInChildren<CardController>();//フィールドのコントローラー
            CardController[] PLfieldCC = PlayerFieldTransform.GetComponentsInChildren<CardController>();//プレイヤーフィールド


            if (EMfieldCC.Length == 0) yield break;


            if (EMfieldCC != null)
            {
                foreach (CardController card in EMfieldCC)
                {
                    EMfieldCC = EnemyFieldTransform.GetComponentsInChildren<CardController>();//フィールドのコントローラー
                    PLfieldCC = PlayerFieldTransform.GetComponentsInChildren<CardController>();//プレイヤーフィールド


                    if (card.canAttack == true)
                    {
                        //Debug.Log($"PLfieldCC{PLfieldCC},PLfieldCC.length{PLfieldCC.Length}");
                        if (PLfieldCC.Length > 0)
                        {
                            CardController EnemyCard = PLfieldCC[0];//いちばん端っこの敵(味方)を攻撃
                            AttackTarget(card, EnemyCard);

                            yield return new WaitForSeconds(0.5f);


                        }
                        else//攻撃できない
                        {
                            CardController player = PlayerHEROfield.GetComponentInChildren<CardController>();//ヒーろーを攻撃
                            AttackTarget(card, player);

                           
                        }
                        break;
                    }


                }

            }
            canatEMfield = false;

            yield return new WaitForSeconds(0.2f);

        }

        //yield return new WaitForSeconds(0.5f);

        EMatackflag = true;

    }



    public IEnumerator EnemyTurn()
    {
        CardController[] PlayerFieldcardList = PlayerFieldTransform.GetComponentsInChildren<CardController>();


        //自分のカードを攻撃できなくする処理
        foreach (var card in PlayerFieldcardList)
        {
            card.canAttack = false;

        }


        //エネミーターンの処理
        Debug.Log("敵のターンです");
        manasys.AddMana(1);
        //manasys.AddEmptyMana(1);
        manasys.RestoreMana(EnemyCostframe);

        yield return new WaitForSeconds(1f);
        //EnemyHandtransform からcardcontrollerを持つ子オブジェクトを検索


        CardController[] EnemyFieldcardList = EnemyFieldTransform.GetComponentsInChildren<CardController>();


        //攻撃フラッグを初期化する
        foreach (var card in EnemyFieldcardList)
        {
            card.canAttack = true;

        }


        int maxiteration = 5;
        int iteration = 0;
        // EMatackflag =false //攻撃不可能ならtrueに
        // EMcastflag =false　//キャスト不可能ならtrueに
        while (iteration < maxiteration)
        {


            while (isAnimating)//アニメーション中かどうか判断
            {
                yield return null;
            }
            yield return StartCoroutine(EnemyCast());//キャスト出来るカードをキャストする処理


            if (EnemyCasted)//手札からカードを出した場合,
            {
                yield return new WaitForSeconds(1f);
            }


            while (isAnimating)
            {
                yield return null;
            }
            yield return StartCoroutine(EnemyAttack());//攻撃できるカードで攻撃する処理

            if (EnemyCasted)//手札からカードを出した場合,
            {
                yield return new WaitForSeconds(1f);
            }


            iteration += 1;
            Debug.Log($"iteration{iteration}");

        }

        EMatackflag = false;//全て初期の状態に戻す現状使ってないけど,後で使うかも
        EMcastflag = false;




        //ターンを終了する
        yield return new WaitForSeconds(1f);


        ChangeTurn();



    }

    public void ChangeTurn()
    {
        //ターンの切り替え処理
        isPlayerTurn = !isPlayerTurn;

        if (isPlayerTurn)
        {

            if (playerDeck.DeckCount != 0)
            {
                ICard playerCardEntity = playerDeck.DrawCard();
                CreateHand(PlayerHandTransform, playerCardEntity);


            }
            else
            {
                Debug.Log("player cant draw!!");
            }
            //ドローする

            //ライカデバイス発動
            if (PlayerHERO == "RAIKA")
            {
                RAIKAdevice.OnTurnStart();
            }
           



        }
        else//相手のターン
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
            //ライカデバイス発動

            Debug.Log($"EnemyHERO{EnemyHERO}");

            if (EnemyHERO == "RAIKA")
            {
                RAIKAdevice.OnTurnStart();

                Debug.Log("MoonTurnStart!");
            }

        }
        TurnCalc();
    }









    public void CreateHand(Transform hand, ICard cardEntity)
    {
        CardController card;
        //一時的に親をデッキにする.
        //Transform deckTransform = BattleManager.Instance.PlayerDeckTransform;

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
            StartCoroutine(cardAnim.Drawsetparent(cardAnim, hand));
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
        for (int i = 0; i < 3; i++)
        {


            ICard playerCardEntity = playerDeck.DrawCard();
            ICard enemyCardEntity = enemyDeck.DrawCard();
            CreateHand(PlayerHandTransform, playerCardEntity);
            CreateHand(EnemyHandTransform, enemyCardEntity);
            yield return new WaitForSeconds(0.2f);
        }

    }

    private ICard LoadCard(string hero, string cardType, int cardID)
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
