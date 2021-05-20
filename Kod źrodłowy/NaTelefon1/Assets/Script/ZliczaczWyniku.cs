using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ZliczaczWyniku : MonoBehaviour
{
    Collider2D col;
    Slide cameraScript;
    [HideInInspector] static public double woda;
    [HideInInspector] static public double dom;
    [HideInInspector] static public double transportPubliczny;
    [HideInInspector] static public double transport;
    [HideInInspector] static public double konsumpcja;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<Slide>();
        Zliczaj();
    }

    void Update()
    {
        if (cameraScript.activeMenu == 5)
        {

            //GetComponent<SpriteRenderer>().color = Color.black;
            Zliczaj();
        }
    }

    private void Zliczaj()
    {
        woda =
            Water(double.Parse(kapieleWtyg.text),
            double.Parse(pryszniceWtyg.text),
            zrodloOgrzewania.index,
            napelnienieWanny.index,
            wielkoscWanny.index);
        
        dom =
            House(int.Parse(metraz.text), int.Parse(domownicy.text),
            rodzaj.index, izolacja.index, wentylacja.index, temperaturaWzime.index, klima.index, zarowki.index, ogrzewanie.index);
        //GameObject.Find("TextPrzyklad").GetComponent<Text>().text = zrodloOgrzewania.index.ToString() + " " + napelnienieWanny.index.ToString();
         transportPubliczny =
            PublicTransport(double.Parse(autobus.text), double.Parse(tramwaj.text), double.Parse(metro.text),
            double.Parse(pociag.text), double.Parse(planeShort1.text), double.Parse(planeShort2.text),
            double.Parse(planeShort3.text), double.Parse(planeLong1.text), double.Parse(planeLong2.text),
            double.Parse(planeLong3.text));
        //Debug.Log("3-" + transportPubliczny);
         konsumpcja =
            Consumption(mieso.index, jakDuzoJesz.index, torby.index, lodowka.index, urzadzenia.indexes, int.Parse(domownicy.text));
        //.Log("4-" + konsumpcja);
         transport = 0;
        for (int i = 0; i < pojazdy.pojazdy.Count; i++)
        {
            transport += OwnTransport(pojazdy.pojazdy[i].indexes[0], pojazdy.pojazdy[i].kmtydzien,
                        pojazdy.pojazdy[i].litry100km, pojazdy.pojazdy[i].indexes[1]);
            //GameObject.Find("Text1BN").GetComponent<Text>().text += i;    
        }

        //GameObject.Find("Text1BN").GetComponent<Text>().text += "+" ;
        //GameObject.Find("Text1BN").GetComponent<Text>().text += " - " + transport;
        //Debug.Log("5-" + transport);
        double wynik = transport + transportPubliczny + konsumpcja + dom + woda;

        GameObject.Find("TextBudynek").GetComponent<Text>().text = dom.ToString();
        GameObject.Find("TextWoda").GetComponent<Text>().text = woda.ToString();
        GameObject.Find("TextTransport").GetComponent<Text>().text = (transport + transportPubliczny).ToString();
        GameObject.Find("TextKonsumpcja").GetComponent<Text>().text = konsumpcja.ToString();
        GameObject.Find("TextSuma").GetComponent<Text>().text = wynik.ToString();

        GameObject.Find("Text6").GetComponent<Text>().text = "Twoja emisja wynosi " + wynik.ToString() + " ton CO2equiv / rok";

        //wynikTekst.text = wynik.ToString();

        //GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color == Color.green ? Color.magenta : Color.green;
    }

    #region publiczne
    [Header("water")]
    public Text kapieleWtyg;
    public Text pryszniceWtyg;
    public ExpandedListManager zrodloOgrzewania;
    public ExpandedListManager napelnienieWanny;
    public ExpandedListManager wielkoscWanny;

    [Header("house")]
    public Text metraz;
    public Text domownicy;
    public ExpandedListManager  rodzaj; 
    public ExpandedListManager izolacja; 
    public ExpandedListManager wentylacja; 
    public ExpandedListManager temperaturaWzime;   
    public ExpandedListManager klima;
    public ExpandedListManager zarowki;
    public ExpandedListManager ogrzewanie;

    [Header("transport")]
    public ListManager pojazdy;
    public Text autobus;
    public Text tramwaj;
    public Text metro;
    public Text pociag;
    public Text planeShort1;
    public Text planeShort2;
    public Text planeShort3;
    public Text planeLong1;
    public Text planeLong2;
    public Text planeLong3;

    [Header("consumption")]
    public ExpandedListManager mieso;
    public ExpandedListManager jakDuzoJesz;
    public ExpandedListManager torby;
    public ExpandedListManager lodowka;
    public MultipleChooserManager urzadzenia;
    #endregion

    #region water
    private double Water(double numberOfBathPerWeek, double numberOfShowersPerWeek, int heatingSource, int howManyWaterInBath, int bathSize)
    {
        /*
         numberOfBathPerWeek - ilosc kapieli w tyg
         numberOfShowersPerWeek - ilosc prysznicy w tyg
         heatingSource - zrodlo ogrzewania
         howManyWaterInBath - napelnienie wanny
         bathSize - wielkosc wanny
         */
        //POBRANIE DANYCH i WZORY
        const int smallBathSize = 180, mediumBathSize = 300, largeBathSize = 500; //[l]
        const double averageHumanWeight = 70;   //[kg]
        const double averageHumanDensity = 1.05; //[kg/dm3]
        const double averageHumanVolume = averageHumanWeight / averageHumanDensity; //[dm3]
        const double filledBathFulfillmentCoefficient = 1.0,
                     halfFilledBathFulfillmentCoefficient = 2.0 / 3.0,
                     hardlyFilledBathFulfillmentCoefficient = 1.0 / 3.0;
        const double averageLitersPerShower = 130;
        const double waterDensity = 1, coldWaterTemperature = 10, hotWaterTemperature = 55,
            deltaTemperature = hotWaterTemperature - coldWaterTemperature,
            daysPerYear = 365, specificHeatOfWater = 4190; ///[J]
        const double correctionFactor = 0.9;

        Dictionary<KeyValuePair<int, int>, double> literConverter
             = new Dictionary<KeyValuePair<int, int>, double>();
        {
            literConverter.Add(new KeyValuePair<int, int>(0, 0), (smallBathSize - averageHumanVolume) * hardlyFilledBathFulfillmentCoefficient);
            literConverter.Add(new KeyValuePair<int, int>(0, 1), (smallBathSize - averageHumanVolume) * halfFilledBathFulfillmentCoefficient);
            literConverter.Add(new KeyValuePair<int, int>(0, 2), (smallBathSize - averageHumanVolume) * filledBathFulfillmentCoefficient);
            literConverter.Add(new KeyValuePair<int, int>(1, 0), (mediumBathSize - averageHumanVolume) * hardlyFilledBathFulfillmentCoefficient);
            literConverter.Add(new KeyValuePair<int, int>(1, 1), (mediumBathSize - averageHumanVolume) * halfFilledBathFulfillmentCoefficient);
            literConverter.Add(new KeyValuePair<int, int>(1, 2), (mediumBathSize - averageHumanVolume) * filledBathFulfillmentCoefficient);
            literConverter.Add(new KeyValuePair<int, int>(2, 0), (largeBathSize - averageHumanVolume) * hardlyFilledBathFulfillmentCoefficient);
            literConverter.Add(new KeyValuePair<int, int>(2, 1), (largeBathSize - averageHumanVolume) * halfFilledBathFulfillmentCoefficient);
            literConverter.Add(new KeyValuePair<int, int>(2, 2), (largeBathSize - averageHumanVolume) * filledBathFulfillmentCoefficient);
        }

        KeyValuePair<int, int> bathDataBind = new KeyValuePair<int, int>(bathSize, howManyWaterInBath);

        ///TE ZMIENNE SA MI POTRZEBNE W UZYCIU WZOROW I TRZEBA DOPROWADZIC DO SYTUACJI ICH UZYSKANIA JAK TUTAJ
        double litersOfOneBath = literConverter[bathDataBind]; ///ilosc zuzytych litrow z jednej kapieli

        double litersPerWeek = numberOfShowersPerWeek * averageLitersPerShower + numberOfBathPerWeek * litersOfOneBath;
        double litersPerDay = (litersPerWeek / 7.0) * (4.0 / 9.0); ///Mamy ile litrow przypada na jeden dzien


        double kWhPerYear = ((litersPerDay * waterDensity) * deltaTemperature * specificHeatOfWater * daysPerYear / 3600000) * correctionFactor;
        //                  <            masa           >                                        
        //                  <                                     J per year                                   >
        //                  <                                     kWh per year                                            >

        Dictionary<int, Tuple<double, double, double>> kWhAndCO2_Correction
             = new Dictionary<int, Tuple<double, double, double>>(); ///Poprawki w produkcji kWh uwzgledniajac wspolczynniki cieplne, sprawnosci źródeł ciepła oraz zużycia prądu na wytwarzanie ciepła przez poszczególne źrodła
                                                                     ///Trzeci element krotki to wspolczynnik by otrzymac kg CO2 z kWh
        {
            kWhAndCO2_Correction.Add(0, new Tuple<double, double, double>(1, 0, 0.341));
            kWhAndCO2_Correction.Add(1, new Tuple<double, double, double>(1.25, 280, 0.278));
            kWhAndCO2_Correction.Add(2, new Tuple<double, double, double>(1.05, 170, 0.154));
            kWhAndCO2_Correction.Add(3, new Tuple<double, double, double>(1.25, 280, 0.279));
            kWhAndCO2_Correction.Add(4, new Tuple<double, double, double>(1.4, 400, 0.0));
            kWhAndCO2_Correction.Add(5, new Tuple<double, double, double>(0, kWhPerYear / 4.0, 0.0));
            kWhAndCO2_Correction.Add(6, new Tuple<double, double, double>(1, 0, 0.778));
        }

        ///ZROBIENIE CO TRZEBA Z WYNIKIEM
        try
        {
            double real_kWhPerYear = kWhAndCO2_Correction[heatingSource].Item1 * kWhPerYear
                + kWhAndCO2_Correction[heatingSource].Item2;
            double kgOfCO2perYear = kWhAndCO2_Correction[heatingSource].Item3 * real_kWhPerYear;

            return Math.Round(kgOfCO2perYear / 1000.0, 3);
        }
        catch (Exception)
        {
            return -100;
        }

    }
    #endregion

    #region house
    double House(int metersOfHouse, int numberOfLodgers, int typeOfHouse, int insulationLevel, int typeOfVentilation,
        int temperatureInWinter, int airConditioningFrequency, int bulbType, int heatingSource)
    {
        /*
         metersOfHouse - ilosc metrow kwadratowych
         numberOfLodgers - ilsoc domownikow
         typeOfHouse - rodzaj budynku
         insulationLevel - izolacja domu
         typeOfVentilation - rodzaj wentylacji
         temperatureInWinter - dom zimą po podgrzaniu jest
         airConditioningFrequency - uzytkowanie klimatyzacji
         bulbType - rodzaj zarowek
         heatingSource - zrodlo ogrzewania
         */


        Dictionary<Tuple<int, int>, int> insulationAndTypeOfHouse
             = new Dictionary<Tuple<int, int>, int>(); //ilosc kWh/m^2 na podstawie rodzaju budynku i izolacji

        Dictionary<int, int> temperatureCoefficient
             = new Dictionary<int, int>();  //kWh/m^2 dodane z ciepła w budynku 

        Dictionary<int, int> bulbsCoefficients
            = new Dictionary<int, int>();  //kWh/m^2 dodane z żarówek

        Dictionary<Tuple<int, int>, int> ventilationCoefficients
             = new Dictionary<Tuple<int, int>, int>(); //wentylacja

        Dictionary<Tuple<int, int>, Tuple<double, int>> airConditionCoefficients
             = new Dictionary<Tuple<int, int>, Tuple<double, int>>(); //klima

        Dictionary<int, Tuple<double, double, double>> kWhAndCO2_Correction
        = new Dictionary<int, Tuple<double, double, double>>(); ///Poprawki w produkcji kWh uwzgledniajac wspolczynniki cieplne, sprawnosci źródeł ciepła oraz zużycia prądu na wytwarzanie ciepła przez poszczególne źrodła
                                                                ///Trzeci element krotki to wspolczynnik by otrzymac kg CO2 z kWh
        {
            airConditionCoefficients.Add(new Tuple<int, int>(0, 3), new Tuple<double, int>(4, 800));
            airConditionCoefficients.Add(new Tuple<int, int>(0, 2), new Tuple<double, int>(3, 500));
            airConditionCoefficients.Add(new Tuple<int, int>(0, 1), new Tuple<double, int>(1.5, 300));
            airConditionCoefficients.Add(new Tuple<int, int>(1, 3), new Tuple<double, int>(3, 800));
            airConditionCoefficients.Add(new Tuple<int, int>(1, 2), new Tuple<double, int>(2, 500));
            airConditionCoefficients.Add(new Tuple<int, int>(1, 1), new Tuple<double, int>(1, 300));
            airConditionCoefficients.Add(new Tuple<int, int>(2, 3), new Tuple<double, int>(1, 800));
            airConditionCoefficients.Add(new Tuple<int, int>(2, 2), new Tuple<double, int>(0.5, 500));
            airConditionCoefficients.Add(new Tuple<int, int>(2, 1), new Tuple<double, int>(0.3, 300));

            ventilationCoefficients.Add(new Tuple<int, int>(0, 0), 28);
            ventilationCoefficients.Add(new Tuple<int, int>(0, 1), 35);
            ventilationCoefficients.Add(new Tuple<int, int>(0, 2), 41);
            ventilationCoefficients.Add(new Tuple<int, int>(1, 0), 22);
            ventilationCoefficients.Add(new Tuple<int, int>(1, 1), 27);
            ventilationCoefficients.Add(new Tuple<int, int>(1, 2), 32);
            ventilationCoefficients.Add(new Tuple<int, int>(2, 0), 5);
            ventilationCoefficients.Add(new Tuple<int, int>(2, 1), 7);
            ventilationCoefficients.Add(new Tuple<int, int>(2, 2), 8);

            insulationAndTypeOfHouse.Add(new Tuple<int, int>(0, 0), 80);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(0, 1), 60);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(0, 2), 40);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(0, 3), 20);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(1, 0), 60);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(1, 1), 45);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(1, 2), 30);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(1, 3), 20);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(2, 0), 40);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(2, 1), 30);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(2, 2), 20);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(2, 3), 15);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(3, 0), 40);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(3, 1), 30);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(3, 2), 20);
            insulationAndTypeOfHouse.Add(new Tuple<int, int>(3, 3), 15);

            temperatureCoefficient.Add(0, -5);
            temperatureCoefficient.Add(1, 0);
            temperatureCoefficient.Add(2, 5);

            bulbsCoefficients.Add(0, 30);
            bulbsCoefficients.Add(1, 15);
            bulbsCoefficients.Add(2, 5);

        }



        try
        {
            double kWhEnergyPerM2 = 0, kWhElectricity = 0, kWhEnergy = 0;
            double A = insulationAndTypeOfHouse[new Tuple<int, int>(typeOfHouse, insulationLevel)];

            kWhEnergyPerM2 += A;

            kWhEnergyPerM2 += temperatureCoefficient[temperatureInWinter];

            kWhEnergyPerM2 += ventilationCoefficients[new Tuple<int, int>(typeOfVentilation, temperatureInWinter)];

            if (airConditioningFrequency != 0)
            {
                kWhEnergyPerM2 += airConditionCoefficients[new Tuple<int, int>(typeOfVentilation, airConditioningFrequency)].Item1;
                kWhElectricity += airConditionCoefficients[new Tuple<int, int>(typeOfVentilation, airConditioningFrequency)].Item2;
            }

            {
                kWhAndCO2_Correction.Add(0, new Tuple<double, double, double>(1, 0, 0.341));
                kWhAndCO2_Correction.Add(1, new Tuple<double, double, double>(1.25, 280, 0.278));
                kWhAndCO2_Correction.Add(2, new Tuple<double, double, double>(1.05, 170, 0.154));
                kWhAndCO2_Correction.Add(3, new Tuple<double, double, double>(1.25, 280, 0.279));
                kWhAndCO2_Correction.Add(4, new Tuple<double, double, double>(1.4, 400, 0.0));
                kWhAndCO2_Correction.Add(5, new Tuple<double, double, double>(0, A / 4.0 * metersOfHouse, 0.0));
                kWhAndCO2_Correction.Add(6, new Tuple<double, double, double>(1, 0, 0.778));
            }
            kWhEnergyPerM2 *= kWhAndCO2_Correction[heatingSource].Item1;
            kWhElectricity += kWhAndCO2_Correction[heatingSource].Item2;

            kWhElectricity += bulbsCoefficients[bulbType] * metersOfHouse;

            kWhEnergy = kWhEnergyPerM2 * metersOfHouse;

            kWhEnergy /= numberOfLodgers;
            kWhElectricity /= numberOfLodgers;

            double kgCO2Electricity = kWhElectricity * kWhAndCO2_Correction[6].Item3;
            double kgCO2Energy = kWhEnergy * kWhAndCO2_Correction[heatingSource].Item3;

            double result = kgCO2Electricity + kgCO2Energy;

            return Math.Round(result / 1000.0, 3);
        }
        catch (Exception)
        {
            return -100;

        }

    }
    #endregion

    #region ownTransport
    double OwnTransport(int pojazd, double KMperT, double LperKm, int paliwo)
    {
        double wynik = 0;

        /*
            pojazd - rodzaj pojazdu
            paliwo - rodzaj paliwa
            KMperT - ilosc km na tydzien
            LperKm - Litry na 100km
            */

        double litrow = KMperT / 100.0d * LperKm;

        if (pojazd == 0)
            goto end;

        //switch(pojazd.SelectedIndex)
        //{
        //case 0:
        //    wynik = 0.05;
        //        goto end;
        //    break;
        //case 1:
        //    wynik = 0.23;
        //    break;
        //case 2:
        //    wynik = 0.93;
        //    break;
        //case 3:
        //    wynik = 1.39;
        //    break;
        //case 4:
        //    wynik = 2.08;
        //    break;
        // case 5:
        //    wynik = 3.36;
        //        break;
        //}

        if (paliwo == 0)
            wynik += 0.141 * litrow;
        else if (paliwo == 1)
            wynik += 0.157 * litrow;
        else if (paliwo == 2)
            wynik += 0.0877 * litrow;
        else if (paliwo == 3)
            if (pojazd == 1)
                wynik += 0.00181 * KMperT;
            else if (pojazd == 2)
                wynik += 0.00722 * KMperT;
            else if (pojazd == 3)
                wynik += 0.0108 * KMperT;
            else if (pojazd == 4)
                wynik += 0.0162 * KMperT;
            else if (pojazd == 5)
                wynik += 0.0262 * KMperT;

            end:
        return Math.Round(wynik, 3);
    }
    #endregion

    #region publicTransport
    double PublicTransport(double autobus, double tramwaj, double metro, double pociag, double planeShort1, double planeShort2, double planeShort3,
        double planeLong1, double planeLong2, double planeLong3)
    {
        /*
         wszystko w km / tyg
         autobus, tramwaj, metro, pociag mowia same za siebie

        samoloty to godzine rocznie w danym samolocie
        Short oznacza loty do 2000km
        Long ponad 2000km
        1 - klasa Ekonomiczna
        2 - Biznesowa
        3 - Pierwsza
         */


        double wyynik = 0, wynikPlanee = 0;

        wyynik = 0.00217 * autobus + (tramwaj + metro) * 0.00261 + 0.00377 * pociag;


        wynikPlanee = 0.24 * planeShort1 + 0.2 * planeLong1 + 0.48 * planeShort2 + 0.4 * planeLong2 + 0.72 * planeShort3 + 0.6 * planeLong3;

        return Math.Round(wyynik + wynikPlanee, 3);
    }
    #endregion

    #region Consumption
    double Consumption(int meatF, int hmye, int bags, int frigde, bool[] devices, int households) ///devices ma miec 16 elementow
    {
        /*
            meatF - jak często jesz mięso tygodniowo
            hmye - Jak duzo jesz
            bags - jak czesto uzywasz toreb jednorazowych
            frigde - jak duza masz lodowke
            devices(boole czy posiadasz)
            komputer    DVD         Mikrofalowka        kuchenka elektryczna        zestaw audio
            toster      telewizor   zmywarka            suszarka do wlosow          pralka
            zelazko     czajnik     mikser              odkurzacz                   piekarnik
            ekspresDoKawy

            households - ilosc domownikow
            */
        const double countryAverageMeatMassPerYear = 70.0;
        const double countryAverageMeatMassDay = countryAverageMeatMassPerYear / 365.0;
        const double kgOfMeatTokgCO2 = 5.5;
        double kgCO2perDay = 0;
        double kgCO2perYear = 0;
        double kgOfMeatPerDay = 0;
        double kWhPerDay = 0;

        switch (meatF)
        {
            case 0:
                break;
            case 1:
                kgOfMeatPerDay += (countryAverageMeatMassDay * 1.5) / 7.0;
                break;
            case 2:
                kgOfMeatPerDay += (countryAverageMeatMassDay * 4.0) / 7.0;
                break;
            default:
                kgOfMeatPerDay += (countryAverageMeatMassDay * 6.5) / 7.0;
                break;
        }
        kgCO2perDay = kgOfMeatTokgCO2 * kgOfMeatPerDay;

        if (hmye == 0)
            kgCO2perDay *= 0.8;
        if (hmye == 2)
            kgCO2perDay *= 1.2;

        kgCO2perYear += bags * 0.2 * 52;

        if (devices[0] == true)
            kWhPerDay += 0.15 * 5.0 / households; ///150W 5 h dziennie
        if (devices[1] == true)
            kWhPerDay += 0.04 * 2.0 * 2.0 / 7.0 / households; //40W razy 2h filmu 2 razy na tydzien
        if (devices[2] == true)
            kWhPerDay += 1.0 * (3.0 / 60.0); //10 minut dziennie 1000W
        if (devices[3] == true)
            kWhPerDay += 5.0 * (4.0 / 7.0) / households; ///5000W 4 razy tyg 1h
        if (devices[4] == true)
            kWhPerDay += 0.2 * 2.0 / households;   ///Okolo 200W bo takie audio ma 800 srednio 1/4 glosnosci i 2h dziennie
        if (devices[5] == true)
            kWhPerDay += 0.9 * 4.0 / 7.0 * (3.0 / 60.0);    ///900W 4 razy w tygodniu po 3 minuty trwa robienie tosta
        if (devices[6] == true)
            kWhPerDay += 0.05 * 4.0 / households; ///50W 4h razy dziennie
        if (devices[7] == true)
            kWhPerDay += 200.0 / 365.0 / households; ///srednio 200kWh srednio rocznie zuzywa
        if (devices[8] == true)
            kWhPerDay += 1.8 * 0.1 * (3.0 / 7.0); ///1800W  (10 / 60) 10 minut 3 razy w tygodniu
        if (devices[9] == true)
            kWhPerDay += 150.0 / 365.0 / households; ///150kWh rocznie srednio zuzywa pralka
        if (devices[10] == true)
            kWhPerDay += 2.5 * 0.5 * 2.0 / 7.0 / households; ///2500W 2 razy w tygodniu po 10 minut
        if (devices[11] == true)
            kWhPerDay += 2.0 * 3.0 / 60.0 * 3.0; ///Trzy minuty podgrzewania wody raz dziennie 2000W
        if (devices[12] == true)
            kWhPerDay += 0.3 * (2.0 / 7.0) * (5.0 / 60.0) / households; ///300W 2 razy w tygodniu po 5 minut
        if (devices[13] == true)
            kWhPerDay += 1.6 * (8.0 / 30.0) * (15.0 / 60.0) / households;    ///1600W 8 razy w miesiacu po 15 godziny
        if (devices[14] == true)
            kWhPerDay += 2.0 * (2.0 / 7.0) / households; ///2000W przez godzine 3 razy w tyg
        if (devices[15] == true)
            kWhPerDay += 1.5 * (1.0 / 60.0) * 2; ///1500W przez minute 2 razy dziennie

        kWhPerDay *= 1.1;   ///Dodanie stanu spoczynku kazdego urzadzenia

        if (frigde == 0)
            kWhPerDay += 1.0 / households;
        if (frigde == 1)
            kWhPerDay += 2.0 / households;
        if (frigde == 2)
            kWhPerDay += 3.0 / households;

        kgCO2perDay += kWhPerDay * 0.778;

        kgCO2perYear += kgCO2perDay * 365;

        return Math.Round(kgCO2perYear / 1000.0, 3);
    }
    #endregion
    
}

