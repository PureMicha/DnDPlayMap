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
    class CharakterToken
    {
        enum Affiliation { Player = 1, Ally = 2, Foe = 3 };

        enum CreatureSize { Small = 1, Medium = 2, Large = 3, ExtraLarge = 4, Gigantic = 5}

        public UIElement UIElement { get; set; }

        public UIElement InitiativeMember { get; set; }

        public int TokenID { get; set; }
        
        public int Side { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }

        // Methode zur Erstellung des UIElement für Spielfiguren
        public void BorderCreatorMethod(int MapSquareSize, int IDStarter, SolidColorBrush UnitBorder)
        {
            Brush affiliation = Brushes.Gray;

            string name = "";

            switch (this.Side)
            {
                case (int)Affiliation.Player:
                    affiliation = Brushes.Green;
                    name = "P";
                    break;
                case (int)Affiliation.Ally:
                    affiliation = Brushes.SlateBlue;
                    name = "A";
                    break;
                case (int)Affiliation.Foe:
                    affiliation = Brushes.Red;
                    name = "M";
                    break;
            }

            if (this.Name.Equals(""))
            {
                name = name + IDStarter.ToString();
                this.Name = name;
            }
            else
            {
                name = this.Name;
            }

            Border borderSet = new Border()
            {
                Width = MapSquareSize * this.Size,
                Height = MapSquareSize * this.Size,
                Background = affiliation,
                BorderThickness = new Thickness(4, 4, 4, 4),
                BorderBrush = UnitBorder,
                CornerRadius = new CornerRadius(100),
                Child = new TextBlock()
                {
                    Text = name,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                }

            };

            this.UIElement = borderSet;
        }

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
