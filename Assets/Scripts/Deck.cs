using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    private bool hit = false;

    private bool testing = false;

    public int[] values = new int[52];
    public int[] shuffledValues = new int[52];

    int cardIndex = 0;

    private void Awake()
    {
        InitCardValues();
        //        Test_InitCardValues();
    }

    private void Start()
    {
        ShuffleCards();
        StartGame();
    }

    private void InitCardValues()
    {

        /* DONE */
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */



        for (int i = 0; i < 51; i++)
        {
            if (i % 13 == 0)
            {
                values[i] = 11;
            }
            else
            {
                if ((i % 13) >= 10)
                {
                    values[i] = 10;
                }
                else
                {
                    values[i] = (i % 13) + 1;
                }
            }
        }

    }

    private void ShuffleCards()
    {
        Test_ShuffledCards();

        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
    }

    void StartGame()
    {

        int test_value = 10;
        int test_repeat = 3;
        Test_ProbabilidadDe(test_value, test_repeat);


        for (int i = 0; i < 2; i++)
        {
            // Test_BlackJack(1);
            // Test_BlackJack(2);



            PushPlayer();
            PushDealer();



            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */

        }



        // distributed Cards
        string playerString = "";
        string dealerString = "";

        for (int i = 0; i < cardIndex; i++)
        {
            int par = i % 2;
            if (par == 0)
            {
                playerString = playerString + shuffledValues[i].ToString() + ",      ";
            }
            else
            {
                if (i != 1)
                {
                    dealerString = dealerString + shuffledValues[i].ToString() + ",      ";
                }
            }
        }
        Debug.Log("--------> Player cards:      " + shuffledValues[0].ToString() + ",      " + shuffledValues[2].ToString());
        Debug.Log("--------> Dealer card:      " + shuffledValues[3].ToString());
        Debug.Log("--------> Invisible card " + shuffledValues[1].ToString());

        if (testing)
        {
            ProbabilidadDe(test_value);
        }
        else
        {
        CalculateProbabilities();
        }


        /*
                int playerPoints = player.GetComponent<CardHand>().points;
                if (playerPoints == 21)
                {
                    GanaPlayer(true);
                }
        */

        // Test_PlayerGameWithoutShuffle_Loose(1);
        // Test_PlayerGameWithoutShuffle_Stand(1);
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */


        int playerPoints = player.GetComponent<CardHand>().points;
        Debug.Log("Pleayer points : " + playerPoints.ToString());

        int dealerPoints = dealer.GetComponent<CardHand>().points;
        Debug.Log("Dealer points : " + dealerPoints.ToString());

        int needed = playerPoints - (dealerPoints - shuffledValues[1]) + 1;
        // Debug.Log("Invisible points : " + shuffledValues[1].ToString());

        // Debug.Log("Needed : " + needed.ToString());

        // Debug.Log("cardIndex : " + cardIndex.ToString());

            ProbabilidadDealerMas(needed, true);

        }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],shuffledValues[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], shuffledValues[cardIndex]/*,cardCopy*/);
        cardIndex++;
        if (hit)
        {
            CalculateProbabilities();
        }

    }

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer cuando se plantar el jugador)
         */

        hit = true;
        
        //Repartimos carta al jugador
        PushPlayer();

        // Test_PlayerGameWithoutShuffle_Loose(2);
        // Test_PlayerGameWithoutShuffle_Stand(2);

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        GanaPlayer(true);

        // Test_PlayerGameWithoutShuffle_Loose(2);
        // Test_PlayerGameWithoutShuffle_Stand(2);

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        hitButton.interactable = false;
        stickButton.interactable = false;

        dealer.GetComponent<CardHand>().InitialToggle();
        int dealerPoints = dealer.GetComponent<CardHand>().points;
        
        if (dealerPoints == 21)
        {
            GanaPlayer(false);
        }
        else
        {
            /*TODO:
             * Repartimos cartas al dealer si tiene 16 puntos o menos
             * El dealer se planta al obtener 17 puntos o más
             * Mostramos el mensaje del que ha ganado
             */
             while (dealerPoints <= 16)
            {
                PushDealer();
                dealerPoints = dealer.GetComponent<CardHand>().points;
            }
            if (dealerPoints > 21)
            {
                GanaPlayer(true);
            }
            else
            {
                if (dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points)
                {
                    GanaPlayer(true);
                }
                else
                {
                    GanaPlayer(false);
                }
            }
        }

    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }

    private void GanaPlayer(bool yes)
    {
        if (yes)
        {
            int playerPoints = player.GetComponent<CardHand>().points;
            int dealerPoints = dealer.GetComponent<CardHand>().points;

            if (playerPoints > 21)
            {
                finalMessage.text = "------ Player has lost with " + playerPoints.ToString() + " points ------ ";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
            if (playerPoints == 21)
            {
                finalMessage.text = "------ CONGRATULATIONS you have WON !!!!! with " + playerPoints.ToString() + " points ------";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
        }
        else
        {
            int points = dealer.GetComponent<CardHand>().points;

            if (points > 21)
            {
                finalMessage.text = "------ Dealer has not won neither with " + points.ToString() + " points ------ ";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
            if (points == 21)
            {
                finalMessage.text = "      ------ SORRY dealer has WON !!!!!!!                     with " + points.ToString() + " points ------";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
        }
    }

    public float ProbabilidadDe(int value)
    {
        int sameVisibleCards = 0;
        float p_value;

        float inFavor = 0;
        float potential = 0;

        if ((value == 11) || (value == 1))  // precisa un As
        {
            for (int i = 0; i < cardIndex; i++)
            {
                if ((shuffledValues[i] == 11) || (shuffledValues[i] == 1))
                {
                    if (i != 1)
                    {
                        sameVisibleCards++;
                    }
                }
            }
            inFavor = 4 - sameVisibleCards;
            potential = 52 - (cardIndex - 1);
            p_value = inFavor / potential;
            Debug.Log("********  Cartas jugadas del mismo tipo: " + sameVisibleCards.ToString());
            Debug.Log("********  Cartas jugadas hasta ahora: " + (cardIndex - 1).ToString());

            Debug.Log("Probabilidad es " + p_value.ToString());
            return p_value;
        }
        else if (value == 10) // precisa una J, Q, K o As
        {
            for (int i = 0; i < cardIndex; i++)
            {
                if (shuffledValues[i] == 10)
                {
                    if (i != 1)
                    {
                        sameVisibleCards++;
                    }
                }
            }
            inFavor = 12 - sameVisibleCards;
            potential = 52 - (cardIndex - 1);
            p_value = inFavor / potential;
            Debug.Log("********  Cartas jugadas del mismo tipo: " + sameVisibleCards.ToString());
            Debug.Log("********  Cartas jugadas hasta ahora: " + (cardIndex - 1).ToString());

            Debug.Log("Probabilidad es : " + p_value.ToString());
            return p_value;

        }
        else if ((value < 10) && (value > 1)) // precisa desde un 2, 3, 4, 5, 6, 7, 8, ó 9
        {
            for (int i = 0; i < cardIndex; i++)
            {
                if (shuffledValues[i] == value)
                {
                    if (i != 1)
                    {
                        sameVisibleCards++;
                    }
                }
            }
                inFavor = 4 - sameVisibleCards;
            potential = 52 - (cardIndex - 1);
            p_value = inFavor / potential;
            Debug.Log("********  Cartas jugadas del mismo tipo: " + sameVisibleCards.ToString());
            Debug.Log("********  Cartas jugadas hasta ahora: " + (cardIndex - 1).ToString());

            Debug.Log("Probabilidad es : " + p_value.ToString());
            return p_value;
        }
        else
        {
            inFavor = 0;
            potential = 52;
            p_value = inFavor / potential;
            return p_value;
        }
    }

    public float ProbabilidadDealerMas(int needed, bool print)  // revisa.ho
    {

        if (testing)
        {
            ProbabilidadDe(needed);
            return 0;
        }

        float p_value = 0;
        /*

        // Debug.Log(">>>>>>>>>>>>>  A calcular la probabilidad de : " + needed.ToString());

        p_value = ProbabilidadDe(needed);


        if (print)
        {
            // probMessage.text = (p_value * 100).ToString("R") + " %";
        }
        */
        return p_value;
    }

    /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    Testing functionalities
    ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/



    private void Test_InitCardValues()
    {
        testing = true;
        for (int i = 0; i < 51; i++)
        {
            Debug.Log("Carta: " + i.ToString() + " has value " + shuffledValues[i].ToString());
        }
    }


    private void Test_PlayerGameWithoutShuffle_Loose(int init)
    {
        testing = true;
        int playerPoints = player.GetComponent<CardHand>().points;

        if (init == 1)
        {
            Debug.Log("Initially Player has Points: " + playerPoints.ToString());
            Hit();
            Debug.Log("Player has Points: " + playerPoints.ToString());

        }
        else
        {
            Debug.Log("Player has Points: " + playerPoints.ToString());

            if (playerPoints < 21)
            {
                Hit();
            }
            else
            {
                if (playerPoints == 21)
                {
                    Debug.Log("------ Player has won  ---------------- ");
                }
                else
                {
                    Debug.Log("------ Player has lost with " + playerPoints.ToString() + " ---------------- ");

                }
            }

        }
    }


    private List<GameObject> cardList;

    private void Test_PlayerGameWithoutShuffle_Stand(int init)
    {
        testing = true;

        int playerPoints = player.GetComponent<CardHand>().points;

        if (init == 1)
        {
            Debug.Log("Initially Player has Points: " + playerPoints.ToString());
            Hit();
            Debug.Log("Player has Points: " + playerPoints.ToString());

        }
        else
        {
            Debug.Log("Player has Points: " + playerPoints.ToString());

            Debug.Log("going to calculate future points");

            //Calculamos la puntuación previa de nuestra mano


            Debug.Log("Next card will be  : " + shuffledValues[cardIndex].ToString());
            int valFutur = 0;
            int aces = 0;
            cardList = player.GetComponent<CardHand>().cards;
            foreach (GameObject f in cardList)
            {

                if (f.GetComponent<CardModel>().value != 11)
                    valFutur += f.GetComponent<CardModel>().value;
                else
                    aces++;
            }

            if (shuffledValues[cardIndex] == 11)
            {
                aces++;
            }
            else
            {
                valFutur = valFutur + shuffledValues[cardIndex];
            }

            for (int i = 0; i < aces; i++)
            {
                if (valFutur + 11 <= 21)
                {
                    valFutur = valFutur + 11;
                }
                else
                {
                    valFutur = valFutur + 1;
                }
            }

            Debug.Log("Next points will be  : " + valFutur.ToString());

            if (valFutur < 21)
            {
                Hit();
            }
            else
            {
                Debug.Log("------ Player stands after card : " + shuffledValues[cardIndex -1]);

                Debug.Log("------ Player stands with : " + playerPoints.ToString() + " points  ---------------- ");
                Stand();
            }

        }
    }

    
    private void Test_BlackJack(int n)
    {
        testing = true;
        if (n == 1)
        {
                shuffledValues[2] = 10;
            // faces[2] =
        }
        else
        {
                shuffledValues[1] = 11;
                shuffledValues[3] = 10;
        }
        for (int i = 0; i < 51; i++)
        {
            Debug.Log("Value at index  : " + i.ToString() + " is " + shuffledValues[i].ToString());
        }
    }

    private void Test_ProbabilidadDe(int value, int repeat)  // 
    {
        Debug.Log("test of probabilidad de " + value.ToString());

        testing = true;
        switch (repeat)
        {
            case 0:
                switch (value)
                {
                    case 11:
                        shuffledValues[0] = 2;
                        shuffledValues[2] = 2;
                        shuffledValues[3] = 2;
                        break;

                    case 10:  // La baraja ordenada pero manipulada. Probabilidad de un 10 (I.e. J, Q o K)
                        shuffledValues[0] = 2;
                        shuffledValues[2] = 10;
                        shuffledValues[3] = 2;
                        break;

                    case 9:
                    case 8:
                    case 7:
                    case 6:
                    case 5:
                    case 4:
                    case 3: // La baraja ordenada pero manipulada. Probabilidad de un n
                        shuffledValues[0] = 2;
                        shuffledValues[2] = value + 1;
                        shuffledValues[3] = 2;
                        break;
                    case 2:
                    case 1: // La baraja ordenada pero manipulada. Probabilidad de un 2 ó 1
                        shuffledValues[0] = 3;
                        shuffledValues[2] = 9;
                        shuffledValues[3] = 3;
                        break;
                    default:
                        Debug.Log("test invalid");
                        break;
                }
                break;
            case 1:
                switch (value)
                {
                    case 11: // la baraja ordenada Probabilidad de un As
                        break;

                    case 10:  // La baraja ordenada pero manipulada. Probabilidad de un 10 (I.e. J, Q o K)
                        shuffledValues[0] = value - 1;
                        shuffledValues[2] = value;
                        shuffledValues[3] = 4;
                        break;

                    case 9:
                    case 8:
                    case 7:
                    case 6:
                    case 5:
                    case 4:
                    case 3: // La baraja ordenada pero manipulada. Probabilidad de un n
                        shuffledValues[0] = value - 1;
                        shuffledValues[2] = value;
                        shuffledValues[3] = 2;
                        break;

                    case 2:
                    case 1: // La baraja ordenada pero manipulada. Probabilidad de un 2 ó 1
                        shuffledValues[0] = value;
                        shuffledValues[2] = 9;
                        shuffledValues[3] = 3;
                        break;
                    default:
                        Debug.Log("test invalid");
                        break;
                }
                break;
            case 2:
                switch (value)
                {
                    case 11: // la baraja ordenada Probabilidad de un As
                        shuffledValues[0] = value;
                        shuffledValues[2] = value;
                        shuffledValues[3] = 4;
                        break;

                    case 10:  // La baraja ordenada pero manipulada. Probabilidad de un 10 (I.e. J, Q o K)
                        shuffledValues[0] = value;
                        shuffledValues[2] = value;
                        shuffledValues[3] = 4;
                        break;

                    case 9:
                    case 8:
                    case 7:
                    case 6:
                    case 5:
                    case 4:
                    case 3: // La baraja ordenada pero manipulada. Probabilidad de un n
                        shuffledValues[0] = value;
                        shuffledValues[2] = value;
                        shuffledValues[3] = 2;
                        break;
                    case 2:
                    case 1: // La baraja ordenada pero manipulada. Probabilidad de un 2 ó 1
                        shuffledValues[0] = value;
                        shuffledValues[2] = value;
                        shuffledValues[3] = 3;
                        break;
                    default:
                        Debug.Log("test invalid");
                        break;
                }
                break;
            case 3:
                switch (value)
                {
                    case 11: // la baraja ordenada Probabilidad de un As
                        shuffledValues[0] = value;
                        shuffledValues[2] = value;
                        shuffledValues[3] = value;
                        break;

                    case 10:  // La baraja ordenada pero manipulada. Probabilidad de un 10 (I.e. J, Q o K)
                        shuffledValues[0] = value;
                        shuffledValues[2] = value;
                        shuffledValues[3] = value;
                        break;

                    case 9:
                    case 8:
                    case 7:
                    case 6:
                    case 5:
                    case 4:
                    case 3:
                    case 2:
                    case 1:    // La baraja ordenada pero manipulada. Probabilidad de un n
                        shuffledValues[0] = value;
                        shuffledValues[2] = value;
                        shuffledValues[3] = value;
                        break;

                    default:
                        Debug.Log("test invalid");
                        break;
                }
                break;
            default:
                Debug.Log("invalid repeat");
                break;
        }
    }

    private void Test_ProbabilidadDealerMas(int n)  // revisa-ho
    {
        testing = true;
        switch (n)
        { 
            case 11: // la baraja ordenada // precisa un As
                Debug.Log("test of probabilidad de 11");

                Debug.Log("--------> Player cards:      " + shuffledValues[0].ToString() + ",      " + shuffledValues[2].ToString());
                Debug.Log("--------> Dealer card:      " + shuffledValues[3].ToString());
                Debug.Log("--------> Invisible card " + shuffledValues[1].ToString());
                break;

            case 10:  // La baraja ordenada pero manipulada Precisa 10 puntos más (I.e. J, Q o K)
                shuffledValues[0] = 2;
                shuffledValues[2] = 9;
                shuffledValues[3] = 2;

                Debug.Log("test of probabilidad de 10");

                Debug.Log("--------> Player cards:      " + shuffledValues[0].ToString() + ",      " + shuffledValues[2].ToString());
                Debug.Log("--------> Dealer card:      " + shuffledValues[3].ToString());
                Debug.Log("--------> Invisible card " + shuffledValues[1].ToString());

                break;
            case 3: // La baraja ordenada pero manipulada Precisa sólo un punto más
                shuffledValues[0] = 2;
                shuffledValues[2] = 2;
                shuffledValues[3] = 2;

                Debug.Log("test of probabilidad de 3");

                Debug.Log("--------> Player cards:      " + shuffledValues[0].ToString() + ",      " + shuffledValues[2].ToString());
                Debug.Log("--------> Dealer card:      " + shuffledValues[3].ToString());
                Debug.Log("--------> Invisible card " + shuffledValues[1].ToString());
                break;
            case 4: // La baraja ordenada pero manipulada Precisa sólo dos puntos más
                shuffledValues[0] = 2;
                shuffledValues[2] = 3;
                shuffledValues[3] = 2;

                Debug.Log("test of probabilidad de 4");

                Debug.Log("--------> Player cards:      " + shuffledValues[0].ToString() + ",      " + shuffledValues[2].ToString());
                Debug.Log("--------> Dealer card:      " + shuffledValues[3].ToString());
                Debug.Log("--------> Invisible card " + shuffledValues[1].ToString());
                break;
            case 5:  // La baraja ordenada pero manipulada Precisa sólo tres puntos más
                shuffledValues[0] = 2;
                shuffledValues[2] = 4;
                shuffledValues[3] = 2;

                Debug.Log("test of probabilidad de 5");

                Debug.Log("--------> Player cards:      " + shuffledValues[0].ToString() + ",      " + shuffledValues[2].ToString());
                Debug.Log("--------> Dealer card:      " + shuffledValues[3].ToString());
                Debug.Log("--------> Invisible card " + shuffledValues[1].ToString());
                break;
            case 6: // La baraja ordenada pero manipulada Precisa sólo cuatro puntos más
                shuffledValues[0] = 2;
                shuffledValues[2] = 5;
                shuffledValues[3] = 2;

                Debug.Log("test of probabilidad de 6");

                Debug.Log("--------> Player cards:      " + shuffledValues[0].ToString() + ",      " + shuffledValues[2].ToString());
                Debug.Log("--------> Dealer card:      " + shuffledValues[3].ToString());
                Debug.Log("--------> Invisible card " + shuffledValues[1].ToString());
                break;
            case 7: // La baraja ordenada pero manipulada Precisa sólo un punto más
                shuffledValues[0] = 2;
                shuffledValues[2] = 6;
                shuffledValues[3] = 2;

                Debug.Log("test of probabilidad de 7");

                Debug.Log("--------> Player cards:      " + shuffledValues[0].ToString() + ",      " + shuffledValues[2].ToString());
                Debug.Log("--------> Dealer card:      " + shuffledValues[3].ToString());
                Debug.Log("--------> Invisible card " + shuffledValues[1].ToString());
                break;
            case 8: // La baraja ordenada pero manipulada Precisa sólo un punto más
                shuffledValues[0] = 2;
                shuffledValues[2] = 7;
                shuffledValues[3] = 2;

                Debug.Log("test of probabilidad de 8");

                Debug.Log("--------> Player cards:      " + shuffledValues[0].ToString() + ",      " + shuffledValues[2].ToString());
                Debug.Log("--------> Dealer card:      " + shuffledValues[3].ToString());
                Debug.Log("--------> Invisible card " + shuffledValues[1].ToString());
                break;
            case 9:   // La baraja ordenada pero manipulada Precisa 9 puntos más (i.e. máximo carta normal)
                shuffledValues[0] = 2;
                shuffledValues[2] = 8;
                shuffledValues[3] = 2;

                Debug.Log("test of probabilidad de 9");

                Debug.Log("--------> Player cards:      " + shuffledValues[0].ToString() + ",      " + shuffledValues[2].ToString());
                Debug.Log("--------> Dealer card:      " + shuffledValues[3].ToString());
                Debug.Log("--------> Invisible card " + shuffledValues[1].ToString());
                break;

            default:
                Debug.Log("test invalid");

                break;
        }
    }



    private void Test_ShuffledCards()
    {
        testing = true;
        for (int i = 0; i < 51; i++)
        {
            if (i % 13 == 0)
            {
                shuffledValues[i] = 11;
            }
            else
            {
                if ((i % 13) >= 10)
                {
                    shuffledValues[i] = 10;
                }
                else
                {
                    shuffledValues[i] = (i % 13) + 1;
                }
            }
        }
    }

}

