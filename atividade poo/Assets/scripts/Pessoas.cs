using System.Collections;
using UnityEngine;

public class Pessoas : MonoBehaviour
{
    //A classe Pessoas é responsável por todo o comportamento das pessoas no jogo.
    private string[] pessoaTipo = { "Cidadao", "Ladrao", "Policial" }; //Era para ser um enum (Um tipo de coleção) mas não deu certo.

    [SerializeField] int dinheiro;
    [SerializeField] float vida;

    private float delay;
    private bool fazerAcao = true;
    private GameObject alvo;

    private GameManager gameManager;


    void Awake()
    {
        //Referência indireta do script gameManager para o script Pessoas.
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void Start()
    {

        InicializarPessoas();

        switch (gameObject.tag) //Adiciona +1 sempre que uma pessoa nova nasce.
        {
            case "Cidadao":
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                gameManager.quantCidadao++;
                break;

            case "Policial":
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                gameManager.quantPolicial++;
                break;

            case "Ladrao":
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                gameManager.quantLadrao++;
                break;
        }

        StartCoroutine(Movimentar());
    }

    void Update()
    {
        if (gameObject.tag == "Cidadao")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            TrabalharEMorrer();
        }


        if (vida <= 0)
        {
            Destroy(this.gameObject);
            gameManager.GetPainel().SetActive(false);
        }
    }

    void InicializarPessoas() //Gera as pessoas com o tipo dela e sua vida aleatória.
    {
        vida = Random.Range(3, 10);
        gameObject.tag = pessoaTipo[Random.Range(0, pessoaTipo.Length)];
        gameObject.name = gameObject.tag;
    }

    IEnumerator Movimentar() //Movimenta as pessoas para posições aleatórias dps de um tempo.
    {
        while (true)
        {
            Vector2 randPos = new Vector2(Random.Range(-8, 8), Random.Range(-5, 5));
            while ((Vector2)transform.position != randPos)
            {
                transform.position = Vector2.MoveTowards(transform.position, randPos, Time.deltaTime * 3);
                yield return null;

            }
            yield return new WaitForSeconds(1);
        }
    }

    public int GetDinheiro() //Retorna a variável encapsulada como privada responsável pelo dinheiro da pessoa.
    {
        return dinheiro;
    }

    public float GetVida() //Retorna a variável encapsulada como privada responsável pela vida da pessoa.
    {
        return vida;
    }

    #region Ações dos personagens
    // As ações foram pensadas para ter a mesma lógica facilitando a implementação.
    private void TrabalharEMorrer()
    {
        if (!fazerAcao) return;

        fazerAcao = false;
        StartCoroutine(EsperarTrabalho());
    }

    private IEnumerator EsperarTrabalho()
    {
        delay = 0;

        while (delay < 5)
        {
            delay += Time.deltaTime;
            yield return null;
        }

        dinheiro += 2;
        vida--;

        fazerAcao = true;
    }

    private void Roubar()
    {
        if (!fazerAcao) return;
        fazerAcao = false;
        StartCoroutine(EsperarRoubo());
    }

    private IEnumerator EsperarRoubo()
    {
        delay = 0;
        while (delay < 3)
        {
            delay += Time.deltaTime;
            yield return null;
        }

        if (alvo != null & alvo.GetComponent<Pessoas>().GetDinheiro() > 0)//Utilizando curto circuito para parar o if se alvo for nulo.   
            alvo.GetComponent<Pessoas>().dinheiro--; // Pegando o atributo com referência direta de outro objeto com o mesmo script.

        fazerAcao = true;
    }

    private void DeterESumir()
    {
        if (!fazerAcao) return;
        fazerAcao = false;
        StartCoroutine(EsperarDetencao());
    }

    private IEnumerator EsperarDetencao()
    {
        delay = 0;
        while (delay < 2)
        {
            delay += Time.deltaTime;
            yield return null;
        }

        if (alvo != null)
        {
            alvo.GetComponent<Pessoas>().vida--;
            vida--;
        }

        fazerAcao = true;
    }
    #endregion

    void OnCollisionEnter2D(Collision2D coll)
    {
        switch (gameObject.tag) // Ao colidir com alguma pessoa, retorna o objeto colidido como alvo para efetuar uma ação.
        {
            case "Ladrao":
                alvo = coll.gameObject;
                if (alvo.tag == "Cidadao")
                    Roubar();
                break;

            case "Policial":
                alvo = coll.gameObject;
                if (alvo.tag == "Ladrao")
                    DeterESumir();
                break;
        }
    }

    void OnColisionExit2D(Collision2D coll) //Ao sair da colisão a variável alvo recebe null para evitar erro de referência.
    {
        alvo = null;
    }

}
