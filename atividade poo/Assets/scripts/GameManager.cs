using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    //Variáveis para ver a população
    public int quantPolicial;
    public int quantCidadao;
    public int quantLadrao;
    public TMP_Text txt_Policial;
    public TMP_Text txt_Ladrao;
    public TMP_Text txt_Cidadao;

    //Variáveis para ver as informações de cada indivíduo no Canvas

    private GameObject painel_Individuo;
    private Pessoas pessoaAlvo;

    public TMP_Text txt_vida;
    public TMP_Text txt_dinheiro;
    public TMP_Text txt_pessoaTitulo;

    void Awake()
    {
        painel_Individuo = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
    }

    void Update()
    {
        MostrarPopulacao();
        MostrarInfoPessoa();

        if (Input.GetMouseButtonDown(0))
        {
            ClickMouse();
        }
    }


    void MostrarPopulacao()
    {
        txt_Cidadao.text = "Cidadao: " + quantCidadao.ToString();
        txt_Ladrao.text = "Ladrao: " + quantLadrao.ToString();
        txt_Policial.text = "Policial: " + quantPolicial.ToString();
    }

    void MostrarInfoPessoa()
    {
        if (pessoaAlvo != null)
        {
            txt_pessoaTitulo.text = pessoaAlvo.tag;
            txt_vida.text = "Vida: " + pessoaAlvo.GetVida().ToString();
            txt_dinheiro.text = "Dinheiro: " + pessoaAlvo.GetDinheiro().ToString();
        }
    }

    void ClickMouse()
    {
        Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero);

        if (hit.collider != null)
        {
            pessoaAlvo = hit.collider.GetComponent<Pessoas>();

            if (pessoaAlvo != null)
                painel_Individuo.SetActive(true);
        }
    }
}
