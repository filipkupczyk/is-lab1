using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml;

internal class XMLReadWithSAXApproach
{
    internal static void Read(string filepath)
    {
        // konfiguracja początkowa dla XmlReadera
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true;
        settings.IgnoreProcessingInstructions = true;
        settings.IgnoreWhitespace = true;
        // odczyt zawartości dokumentu
        XmlReader reader = XmlReader.Create(filepath, settings);
        // zmienne pomocnicze
        int count = 0;
        string postac = "";
        string sc = "";
        reader.MoveToContent();
        // analiza każdego z węzłów dokumentu
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "produktLeczniczy")
            {
                postac = reader.GetAttribute("postac");
                sc = reader.GetAttribute("nazwaPowszechnieStosowana");
                if (postac == "Krem" && sc == "Mometasoni furoas")
                    count++;
            }
        }
        Console.WriteLine("Liczba produktów leczniczych w postaci kremu, których jedyną substancją czynną jest Mometasoni furoas {0}", count);

        reader = XmlReader.Create(filepath, settings);
        reader.MoveToContent();
        string nazwa;
        Dictionary<string, int> nazwyPowszechne = new Dictionary<string, int>();
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "produktLeczniczy")
            {
                nazwa = reader.GetAttribute("nazwaPowszechnieStosowana");
                if (nazwyPowszechne.ContainsKey(nazwa))
                    nazwyPowszechne[nazwa]++;
                else
                    nazwyPowszechne[nazwa] = 1;
            }
        }

        for (int i = 0; i < nazwyPowszechne.Count; i++)
        {
            if (nazwyPowszechne.ElementAt(i).Value != 1)
                Console.Write("Ilosc preparatow o nazwie {0} to {1} \n", nazwyPowszechne.ElementAt(i).Key, nazwyPowszechne.ElementAt(i).Value);
        }

        reader = XmlReader.Create(filepath, settings);
        reader.MoveToContent();
        Dictionary<string, int> podmiotyTab = new Dictionary<string, int>();
        Dictionary<string, int> podmiotyKrem = new Dictionary<string, int>();
        List<string> podmioty = new List<string>();
        string pom;
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "produktLeczniczy")
            {
                pom = reader.GetAttribute("podmiotOdpowiedzialny");
                podmioty.Add(pom);
            }
            
        }
        List<string>wynik = podmioty.Distinct().ToList();
        reader = XmlReader.Create(filepath, settings);
        reader.MoveToContent();
        string pos, podmiot;
        for (int i = 0; i < wynik.Count; i++)
        {
            podmiotyKrem.Add(wynik[i], 0);
            podmiotyTab.Add(wynik[i], 0);
        }
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "produktLeczniczy")
            {
                pos = reader.GetAttribute("postac");
                podmiot = reader.GetAttribute("podmiotOdpowiedzialny");
                if (pos == "Krem")
                {
                    if (podmiotyKrem.ContainsKey(podmiot))
                        podmiotyKrem[podmiot]++;
                }
                if (pos.Contains("Tabletki"))
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
        Console.WriteLine("Najwiecej tabletek wyprodukował {0} w ilosci {1}", podmiotyTab.ElementAt(maxTab).Key, maxTab);
    }
}