using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiaMedFight.Templates
{
    public class StringBindingCollection
    {

        
        public string[] Rules { get;  private set; } = new string[6];
        public string[] Title { get; private set; } = new string[6];

        public StringBindingCollection()
        {
            Title[0] = "Mål:";
            Title[1] = "Regler";
            Rules[0] ="Försök att flytta dina pjäser runt spelplanen och nå målet innan din motståndare. Spelarna slåss genom att slå tärningen och skicka varandra 'hem' till boet.";
            Rules[1] = "Här är regler\n" +
            "* Regel 1\n" +
            "* Regel 2\n" +
            "* Regel 3";
        }



    }
}
