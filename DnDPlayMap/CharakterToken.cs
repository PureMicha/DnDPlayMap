using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DnDPlayMap
{
    class CharakterToken : Border
    {
        enum Affiliation { Player = 1, Ally = 2, Foe = 3 };

        enum CreatureSize { Small = 1, Medium = 2, Large = 3, ExtraLarge = 4, Gigantic = 5}

        public Border InitiativeMember { get; set; }

        public int TokenID { get; set; }
        
        public int Side { get; set; }

        public int Size { get; set; }

        public CharakterToken(int MapSquareSize, int ID, string name, int side, int size)
        {
            if (name.Equals(""))
            {
                switch(side)
                {
                    case (int)Affiliation.Player:
                        name = "P";
                        break;
                    case (int)Affiliation.Ally:
                        name = "A";
                        break;
                    case (int)Affiliation.Foe:
                        name = "M";
                        break;
                }
                name = name + ID.ToString();
            }
            this.Name = name;
            this.Side = side;
            this.Size = size;
            this.TokenID = ID;

            BorderCreatorMethod(MapSquareSize, ID);
        }

        // Methode zur Erstellung des UIElement für Spielfiguren
        private void BorderCreatorMethod(int MapSquareSize, int IDStarter)
        {
            Width = MapSquareSize * this.Size;
            Height = MapSquareSize * this.Size;
            Child = new TextBlock()
            {
                Text = this.Name,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
        }

        // Methode für die Farbe der Spielfigur
        public SolidColorBrush getColor()
        {

            SolidColorBrush colorReturn = Brushes.White;

            switch (this.Side)
            {
                case (int)Affiliation.Player:
                    colorReturn = Brushes.Green;
                    break;
                case (int)Affiliation.Ally:
                    colorReturn = Brushes.SlateBlue;
                    break;
                case (int)Affiliation.Foe:
                    colorReturn = Brushes.Red;
                    break;
            }

            return colorReturn;
        }

    }
}
