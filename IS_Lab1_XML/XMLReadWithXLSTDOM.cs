using System.Data;
using System.Security.Cryptography;
using System.Xml.XPath;
using System.Xml;

internal class XMLReadWithXLSTDOM
{
    private static void Read(string filepath)
    {
        XPathDocument document = new XPathDocument(filepath);
        XPathNavigator navigator = document.CreateNavigator();
        XmlNamespaceManager manager = new
        XmlNamespaceManager(navigator.NameTable);
        manager.AddNamespace("x","http://rejestrymedyczne.ezdrowie.gov.pl/rpl/eksport-danychv1.0");
        XPathExpression query = navigator.Compile("/x:produktyLecznicze/x:produktLeczniczy[@postac='Krem' and @nazwaPowszechnieStosowana='Mometasonifuroas']");
        query.SetContext(manager);
        int count = navigator.Select(query).Count;
        Console.WriteLine("Liczba produktów leczniczych w postaci kremu, których jedyną substancją czynną jest Mometasoni furoas {0}", count );

    }

}