using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DnDPlayMap
{
    class CharakterToken
    {
        enum Affiliation { Player = 1, Ally = 2, Foe = 3 };

        enum CreatureSize { Small = 1, Medium = 2, Large = 3, ExtraLarge = 4, Gigantic = 5}

        public UIElement UIElement { get; set; }

        public TextBox InitiativeMember { get; set; }

        public int TokenID { get; set; }
        
        public int Side { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }
    }
}
