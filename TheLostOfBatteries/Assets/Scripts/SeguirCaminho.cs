using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeguirCaminho : MonoBehaviour {

    public enum TipoSeguindo
    {
        Mover,
        MoverLento
    }

    public TipoSeguindo Sentido = TipoSeguindo.Mover;
    public PathDefination Caminho;
    public float speed = 1;
    public float MaximaDistancia = .1f;

    private IEnumerator<Transform> PontoAtual;

    public void Start()
    {
        if (Caminho == null)
        {
            Debug.LogError("Script de Caminho não definido.");
            return;
        }
        PontoAtual = Caminho.GetPathsEnumaration();
        PontoAtual.MoveNext();

        if (PontoAtual.Current == null)
            return;

        transform.position = PontoAtual.Current.position;
      }

    public void Update()
    {
        if (PontoAtual == null || PontoAtual.Current == null)
            return;

        if (Sentido == TipoSeguindo.Mover)
            transform.position = Vector3.MoveTowards(transform.position, PontoAtual.Current.position, Time.deltaTime * speed);
        else if (Sentido == TipoSeguindo.MoverLento)
            transform.position = Vector3.Lerp(transform.position, PontoAtual.Current.position, Time.deltaTime * speed);

        var distanciaQuadrado = (transform.position - PontoAtual.Current.position).sqrMagnitude;
        if (distanciaQuadrado < MaximaDistancia * MaximaDistancia)
            PontoAtual.MoveNext();
    }


}
