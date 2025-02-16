using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pessoas : MonoBehaviour
{
    private string[] pessoaTipo = { "Cidadao", "Ladrao", "Policial" };
    public string minhaPessoa;

    [SerializeField] int dinheiro;
    [SerializeField] float vida;

    private float delay;
    private bool fazerAcao = true;
    private GameObject alvo;

    private GameManager gameManager;


    void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void Start()
    {
        vida = Random.Range(3, 10);
        gameObject.tag = pessoaTipo[Random.Range(0, pessoaTipo.Length)];
        gameObject.name = gameObject.tag;
        switch (gameObject.tag)
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
        }
    }

    IEnumerator Movimentar()
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

    public int GetDinheiro()
    {
        return dinheiro;
    }

    public float GetVida()
    {
        return vida;
    }

    #region Ações dos personagens

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

        dinheiro += 1;
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

        if (alvo != null)
            alvo.GetComponent<Pessoas>().dinheiro--;
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
        switch (gameObject.tag)
        {
            case "Ladrao":
                alvo = coll.gameObject;
                Roubar();
                break;

            case "Policial":
                alvo = coll.gameObject;
                DeterESumir();
                break;
        }
    }

    void OnColisionExit2D(Collision2D coll)
    {
        alvo = null;
    }

}
