using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiaMedFight.Templates
{
    internal class StringBindingCollection
    {
        public string Rules { get; set; }
        public string Title { get; set; }

        public static StringBindingCollection GetCollection()
        {
            var collection = new StringBindingCollection()
            {
                Rules = "Här är regler\n" +
                "* Regel 1\n" +
                "* Regel 2\n" +
                "* Regel 3",
                Title = "Regler"
            };
            return collection;
        }

       
        
    }
}
