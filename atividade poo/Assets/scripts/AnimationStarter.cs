using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStarter : MonoBehaviour
{
    public Animator anin;
    public string[] parametroNome = { "D", "V", "H" };
    int randIndex;
    public float delayTime;
    void Start()
    {
        anin = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("StartAnimation");
        }
    }

    IEnumerator StartAnimation()
    {
        randIndex = Random.Range(0, parametroNome.Length);
        anin.SetBool(parametroNome[randIndex], true);
        yield return new WaitForSeconds(delayTime);
        anin.SetBool(parametroNome[randIndex], false);
    }
}
