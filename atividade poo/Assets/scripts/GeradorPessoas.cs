
using UnityEngine;

public class GeradorPessoas : MonoBehaviour
{
    //Somente Instancia os objetos Prefabs para a cena
    [SerializeField] GameObject pessoa;
    void Start()
    {
        Invoke("CriarPessoa", 1);
    }

    void CriarPessoa()
    {
        Instantiate(pessoa, new Vector2(Random.Range(-7, 7), Random.Range(-4, 4)), Quaternion.identity);
        Invoke("CriarPessoa", 8);
    }
}
