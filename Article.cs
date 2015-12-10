using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Сводное описание для Article
/// </summary>
public class Article
{
    public Article(string key)
    {
        _Key = key;
    }
    // конструктор без параметров для десериализации
    public Article() { }

    private string _Key;
    private string _Content;

    //свойства доступные клиенту после сериализации
    public string Key
    {
        get
        {
            return _Key;
        }
        set
        {
            _Key = value;
        }
    }
    public string Content
    {
        get
        {
            return _Content;
        }
        set
        {
            _Content = value;
        }
    }
   
}