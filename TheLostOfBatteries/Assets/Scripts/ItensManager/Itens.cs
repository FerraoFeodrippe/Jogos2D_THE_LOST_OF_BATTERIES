using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itens : MonoBehaviour
{
    public BoundSlot BoundSlot;
    public GUISkin Skin;
    public float OffsetFactor = 1;

    private IList<string> _itens;
    private  volatile int _itemActual;
    private static readonly int _limite = 4;
    private GUIStyle _focusItemStyle;
    private volatile bool _canHeal;

    public enum Acao
    {
        Nada,
        Fist,
        LifeRecover,
        Projectile
    }

    public void Awake()
    {
        _canHeal = true;
        _itemActual = 0;
        _itens = new List<string>();
        _itens.Add("Fist");
        _focusItemStyle = Skin.GetStyle("FocusItem");
    }

    public void OnGUI()
    {
        if (_itens.Count <= 0)
            return;

        var startS = Camera.main.WorldToScreenPoint(BoundSlot.Start.position);
        var endS = Camera.main.WorldToScreenPoint(BoundSlot.End.position);
        var offsetX = Screen.width / 12 * OffsetFactor;

        for (int i = 0; i < _itens.Count; i++)
        {
            GUILayout.BeginArea(new Rect(startS.x + offsetX * i, Screen.height - startS.y, endS.x - startS.x, startS.y - endS.y), Skin.GetStyle(_itens[i]));
            {
                if (i == _itemActual)
                {
                    GUILayout.Box(new GUIContent(), _focusItemStyle);
                }

            }
            GUILayout.EndArea();
        }
    }

    public bool Add(string item)
    {
        if (_itens.Count >= _limite)
            return false;
        _itens.Add(item);
        return true;
    }

    public string ChangeItem()
    {
        var nextItem = (_itemActual + 1) % _itens.Count;
        _itemActual = nextItem;
        return _itens[_itemActual];
    }

    public Acao GetAcao()
    {
        try
        {
            if (_itens[_itemActual] == "Fist")
                return Acao.Fist;

            if (_itens[_itemActual].Contains("Arma"))
                return Acao.Projectile;

            if (_itens[_itemActual].Contains("Life") && _canHeal)
            {
                _canHeal = false;
                _itens.RemoveAt(_itemActual);
                _itemActual = Mathf.Max(0, _itemActual - 1);
                StartCoroutine(HealLife());
                return Acao.LifeRecover;
            }

            return Acao.Nada;

        }
        catch(Exception e)
        {
            return Acao.Nada;
        }

    }

    private IEnumerator HealLife()
    {
        yield return new WaitForSeconds(1);
        _canHeal = true;
        yield break;
    }
}


