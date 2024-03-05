using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace FiaMedFight.Templates
{
    /// <summary>
    /// Represents a collection of string bindings for the game.
    /// </summary>
    public class StringBindingCollection
    {
        /// <summary>
        /// Gets the titles associated with the game rule pages.
        /// </summary>
        public List<string> Title { get; private set; } = new List<string>();

        /// <summary>
        /// Gets the rules associated with the game pages.
        /// </summary>
        public List<string> Rules { get; private set; } = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBindingCollection"/> class. 
        /// <list type="number">Collection of rules
        /// <item>Not Yet Implemented</item>
        /// </list>
        /// </summary>
        /// <param name="collection">Determines which string collection to use</param>
        public StringBindingCollection(int collection)
        {
            switch(collection)
            {
                case 0: SetRuleStrings(); break;
                default: break;
            }
        }

        private void SetRuleStrings() { 
            Title.Insert(0, "Mål");
            Rules.Insert(0, "Försök att flytta dina pjäser runt spelplanen och nå målet innan din motståndare. \n" + 
                "Spelarna slåss genom att slå tärningen och skicka varandra tillbaka till start.\n" +
                "Den första spelaren som lyckas flytta alla sina pjäser runt spelplanen och nå målet vinner spelet.");
            
            Title.Insert(1, "Spelupplägg");
            Rules.Insert(1, "Start: Varje spelare tilldelas ett lag och 4 pjäser placeras i deras bo på spelplanen.\n" +
            "Turordning: Spelarna turas om att slå tärningen och väljer sedan vilken pjäs de vill flytta så många steg som tärningen visar.\n" +
            "Flytta pjäsen: Efter att ha slagit tärningen, flyttar spelaren en av sina pjäser framåt på spelplanen med det antal steg som tärningen visar. Endast en pjäs kan flyttas per tärningsslag. \n" +
            "Utgång från boet: För att komma ut från boet måste en spelare slå antingen en etta eller en sexa på tärningen.\n" +
            "Kollision: Om en spelares pjäs hamnar på samma ruta som motståndarens pjäs, utlöses en strid. Båda spelarna slår tärningen, den spelare som får högst värde vinner striden och får stå kvar. Motståndarens pjäs flyttas tillbaka 'hem' till deras bo. Om båda slår lika vinner den pjäs som startade striden, dvs den aktiva spelarens pjäs." );

            Title.Insert(2, "Genvägar");
            Rules.Insert(2, "Om en spelare landar på en ruta med en genväg kan de flytta ett antal extra steg framåt, men har man otur kan en genväg även vara en “senväg”: \n" +
            "    - Om spelaren slår en etta eller sexa får den gå två steg framåt. \n" +
            "    - Om spelaren slår en tvåa eller fyra får den gå ett steg bakåt. \n" +
            "    - Om spelaren slår en trea eller femma står den kvar på samma ruta.");
            
            Title.Insert(3, "Fällor");
            Rules.Insert(3, "Om en spelare hamnar på en ruta med en fälla kan olika händelser inträffa:\n" +
            "    - Spelaren står över ett kast: Spelaren missar sin nästa tur och får inte slå tärningen.\n" +
            "    - Flytta bak ett antal steg: Pjäsen flyttas tillbaka ett antal steg på spelplanen.\n" +
            "    - Slå en etta eller sexa: Pjäsen kan bara flytta med en etta, en sexa, eller vid en kollision av en annan pjäs. Spelarens övriga pjäser kan flytta som vanligt");
        }
    }
}
