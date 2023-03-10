using System.ComponentModel.Design.Serialization;
using System.Data.Common;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

internal class XMLReadWithDOMApproach
{
    internal static void Read(string filepath)
    {
        // odczyt zawartości dokumentu
        XmlDocument doc = new XmlDocument();
        doc.Load(filepath);
        
        string postac;
        string sc;
        int count = 0;
        var drugs = doc.GetElementsByTagName("produktLeczniczy");
        foreach (XmlNode d in drugs)
        {
            postac = d.Attributes.GetNamedItem("postac").Value;
            sc = d.Attributes.GetNamedItem("nazwaPowszechnieStosowana").Value;
            if (postac == "Krem" && sc == "Mometasoni furoas")
                count++;
        }
        Console.WriteLine("Liczba produktów leczniczych w postaci kremu, których jedyną substancją czynną jest Mometasoni furoas : {0}", count);

        string nazwa;
        Dictionary<string, int> nazwyPowszechne = new Dictionary<string, int>();
        foreach (XmlNode i in drugs)
        {
            nazwa = i.Attributes.GetNamedItem("nazwaPowszechnieStosowana").Value;
            if (nazwyPowszechne.ContainsKey(nazwa))
                nazwyPowszechne[nazwa]++;
            else
                nazwyPowszechne[nazwa] = 1;
        }
        
        for (int i = 0; i < nazwyPowszechne.Count; i++)
        {
            if (nazwyPowszechne.ElementAt(i).Value != 1)
                Console.Write("Ilosc preparatow o nazwie {0} to {1} \n", nazwyPowszechne.ElementAt(i).Key, nazwyPowszechne.ElementAt(i).Value);
        }

        Dictionary<string, int> podmiotyTab = new Dictionary<string, int>();
        Dictionary<string, int> podmiotyKrem = new Dictionary<string, int>();
        List<string> podmioty = new List<string>();
        string pom;
        foreach (XmlNode j in drugs)
        {
            pom = j.Attributes.GetNamedItem("podmiotOdpowiedzialny").Value;
            podmioty.Add(pom);
        }

        List<string> wynik = podmioty.Distinct().ToList();
        string pos, podmiot;
        for (int i = 0; i < wynik.Count; i++)
        {
            podmiotyKrem.Add(wynik[i], 0);
            podmiotyTab.Add(wynik[i], 0);
        }
        foreach (XmlNode k in drugs)
        {
            pos = k.Attributes.GetNamedItem("postac").Value;
            podmiot = k.Attributes.GetNamedItem("podmiotOdpowiedzialny").Value;
            if (pos == "Krem")
            {
                if (podmiotyKrem.ContainsKey(podmiot))
                    podmiotyKrem[podmiot]++;
            }
            if (pos.Contains("Tabletki"))
            {
                if (podmiotyTab.ContainsKey(podmiot))
                    podmiotyTab[podmiot]++;
            }
        }
        /*
        for (int i = 0; i < podmiotyKrem.Count; i++)
        {
            Console.WriteLine("Podmiot {0} postac {1}", podmiotyKrem.ElementAt(i).Key, podmiotyKrem.ElementAt(i).Value);
        }

        for (int i = 0; i < podmiotyTab.Count; i++)
        {
            Console.WriteLine("Podmiot {0} postac {1}", podmiotyTab.ElementAt(i).Key, podmiotyTab.ElementAt(i).Value);
        }
        */
        int maxKrem = podmiotyKrem.Values.Max();
        int maxTab = podmiotyTab.Values.Max();
        Console.WriteLine("Najwiecej kremow wyprodukował {0} w ilosci {1}", podmiotyKrem.ElementAt(maxKrem).Key, maxKrem);
        Console.WriteLine("Najwiecej kremow wyprodukował {0} w ilosci {1}", podmiotyTab.ElementAt(maxTab).Key, maxTab);
    }
}