using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoradaManager : MonoBehaviour
{
    public Collider2D poprzednia;
    public Collider2D nastepna;
    int index = 0;
    public UnityEngine.UI.Text text;
    public UnityEngine.UI.Text licznikPorad;
    List<string> aktualnePorady = new List<string>();

    string[][] porady = new string[][]{
        new string[]{
            "Warto przykręcić kaloryfery na noc, zmiana temperatury o 1 stopień Celcjusza potrafi zauważalnie zmniejszyć zużycie energii.",
            "Wyjeżdżając na dłużej możesz obniżyć temperature bardziej niż zwykle w końcu nie będzie cię w domu.",
            "Wietrz dom krótko i intensywnie otwierając okno na całą szerokość, zamiast otwierać w połowie na godzine, w ten sposób powietrze się wymieni a dom nie wyziębi.",
            "Za dnia optymalną temperaturą jest 19-22 stopnie C, zaś podczas snu lepiej ustawić lekko niższą np.: 17-19 stopni Celsjusza, oszczędzamy na ogrzewaniu i wstajemy bardziej wypoczęci."
        },
        new string[]{
            "Może czasami warto wziąć zimny prysznic, potrafi otrzeźwić umysł i nie podgrzewamy dodatkowo wody.",
            "Spróbuj ograniczyć zużycie wody, czy napewno potrzebujesz nalewać pełną wannę przy każdej kąpieli ?",
            "Pamiętaj o zakręcianiu kranów, by woda nie sciekała przez całą noc.",
            "Zakręć wodę podczas mycia zębów."
        },
        new string[]{
            "Staraj się prowadzić auto bez gwałtownych chamowań i przyspieszeń.",
            "Gdy wybierasz się gdzieś dalej, pociąg będzie zdecydowanie mniej emisyjny.",
            "Dbaj o stan silnika oraz o ciśnienie w oponach, może to wpłynąć na zużycie paliwa.",
            "Staraj się korzystać z roweru, nie emituje żadnych gazów a jednocześnie jest zdrowiej.",
            "Staraj się używać komunikacji miejskiej ona i tak jeździ.",
            "Na dłuższych postojach, np.: na przejeździe kolejowym, wyłącz silnik."
        },
        new string[]{
            "Wprowadzenie umiaru w jedzeniu nie tylko ograniczy emisje ale i może zapobiec nadwadze!",
            "W domu zamontuj żarówki ledowe, są one zdecydowanie mniej prądożerne.",
            "Podczas gotowania, przykryj garnek, ciepło nie będzie uciekać i zaoszczędzimy trochę energii.",
            "Noś ze sobą torby wielorazowego użytku, oszczędzisz też na kupnie za każdym razem.",
            "Wymień urządzenia na energooszczędne, stara lodówka potrafi zużywać bardzo dużo prądu.",
            "Pamiętaj by gasić nie potrzebne światło.",
            "Warto stosować recykling, rzeczy raz użyte można ponownie wykorzystać niż produkować nowe.",
            "Kupując produkty spożywcze patrz też na opakowanie, im większe opakowanie tym większa emisja związana z jego wyprodukowaniem.",
            "Wyłączaj urządzenia których nie używasz i wyłącz w nich tryb czuwania lub odpinaj je od prądu, takie czuwanie potrafi zużyć dużo prądu w ostatecznym rozrachunku.",
            "Staraj się nie wkładać ciepłych posiłków do lodówki.",
            "Pierz tylko w pełni naładowaną pralką.",
            "Gdy podgrzewasz wodę np w czajniku, podgrzewaj jej tylko tyle ile potrzebujesz, dodatkowa woda zużyje więcej energii a podgrzewanie zajmie więcej czasu.",
            "Pomyśl czy faktycznie potrzebujesz coś drukować, może wystarczy wysłać email, albo mieć to w formie elektronicznej."
        }
    };

    private void Start()
    {
        aktualnePorady = UstalPorady();
        licznikPorad.text = "Porada 1/" + (aktualnePorady.Count);
        text.text = aktualnePorady[index];

        poprzednia.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (MainManager.CheckClicked(nastepna))
        {
            licznikPorad.text = "Porada " + ((++index)+1) + "/" + (aktualnePorady.Count);
            text.text = aktualnePorady[index];
        }
        else if (MainManager.CheckClicked(poprzednia))
        {
            licznikPorad.text = "Porada " + ((--index)+1) + "/" + (aktualnePorady.Count);
            text.text = aktualnePorady[index];
        }

        nastepna.gameObject.SetActive(index != aktualnePorady.Count - 1);
        poprzednia.gameObject.SetActive(index != 0);

    }

    //private void NowaPorada()
    //{
    //    int idx1 = Random.Range(0, porady.Length);
    //    int idx2 = Random.Range(0, porady[idx1].Length);

    //    text.text = porady[idx1][idx2];
    //}

    private List<string> UstalPorady()
    {
        List<string> wyswietlanePorady = new List<string>();

        if (ZliczaczWyniku.dom >= 1.5f)
            for (int i = 0; i < porady[0].Length; ++i)
                wyswietlanePorady.Add(porady[0][i]);

        if (ZliczaczWyniku.woda >= 0.5f) 
            for ( int i = 0; i < porady[1].Length; ++i )
                wyswietlanePorady.Add(porady[1][i]);

        if (ZliczaczWyniku.transport + ZliczaczWyniku.transportPubliczny >= 2f)
            for (int i = 0; i < porady[2].Length; ++i)
                wyswietlanePorady.Add(porady[2][i]);
        else if(ZliczaczWyniku.transport + ZliczaczWyniku.transportPubliczny >= 1.3f)
        {
            int n = porady[2].Length;
            for (int i = 0; i < 3; i++)
            {
                int r = Random.Range(0,n);

                wyswietlanePorady.Add(porady[2][r]);
                porady[2][r] = porady[2][n - 1];
                n--;
            }
        }

        if(ZliczaczWyniku.konsumpcja > 2.5f)
            for (int i = 0; i < porady[3].Length; ++i)
                wyswietlanePorady.Add(porady[3][i]);
        else if(ZliczaczWyniku.konsumpcja > 1.6f)
        {
            int n = porady[3].Length;
            for (int i = 0; i < 10; i++)
            {
                int r = Random.Range(0, n);

                wyswietlanePorady.Add(porady[3][r]);
                porady[3][r] = porady[3][n - 1];
                n--;
            }
        }
        else if(ZliczaczWyniku.konsumpcja > 1f)
        {
            int n = porady[3].Length;
            for (int i = 0; i < 6; i++)
            {
                int r = Random.Range(0, n);

                wyswietlanePorady.Add(porady[3][r]);
                porady[3][r] = porady[3][n - 1];
                n--;
            }
        }
        else
        {
            int n = porady[3].Length;
            for (int i = 0; i < 3; i++)
            {
                int r = Random.Range(0, n);

                wyswietlanePorady.Add(porady[3][r]);
                porady[3][r] = porady[3][n - 1];
                n--;
            }
        }
         
        return wyswietlanePorady;
    }
}
