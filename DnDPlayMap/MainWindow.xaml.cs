using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DnDPlayMap
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Größe der Karten-Kacheln
        const int MapSquareSize = 60;
        // Farbe Rand der Spielfiguren
        private SolidColorBrush UnitBorder = (SolidColorBrush)(new BrushConverter().ConvertFrom("#535353"));
        // Wert für Karten-Raster Umschaltung
        private bool OnOff = false;
        // CharakterToken TokenID Starter
        private int IDStarter = 0;
        // Liste der Spielfiguren (UIElement)
        private List<CharakterToken> spielerFiguren = new List<CharakterToken>();
        private List<CharakterToken> monster = new List<CharakterToken>();
        private List<CharakterToken> allies = new List<CharakterToken>();
        // Liste der Karten-Kacheln (UIElement)
        private List<UIElement> kartenKacheln = new List<UIElement>();
        // Wert für Start-Position für Drag&Drop
        private Nullable<Point> dragStart = null;
        // Wert für die Zugehörigkeit von Charakteren
        enum Affiliation { Player = 1, Ally = 2, Foe = 3 };
        // Wert für die Größe von Charakteren
        enum CreatureSize { Medium = 1, Large = 2, ExtraLarge = 3, Gigantic = 4, Small = 1 }

        Line distance = new Line();
        Point distanceStart = new Point(0, 0);
        TextBlock distanceValue = new TextBlock();


        public MainWindow()
        {
            InitializeComponent();
        }

        // Button für Raster Umschaltung
        private void Zeichnung_AnAus_Click(object sender, RoutedEventArgs e)
        {
            if (OnOff)
            {
                GameMap.EditingMode = InkCanvasEditingMode.None;
                Zeichnung_AnAus.Content = "Zeichnung ist Aus";

            } else
            {
                GameMap.EditingMode = InkCanvasEditingMode.Ink;
                Zeichnung_AnAus.Content = "Zeichnung ist An";
            }

            OnOff = !OnOff;
            
        }

        // Button für Rücksetzung aller Canvas Elemente
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (Player1.IsChecked == true)
            {
                foreach (CharakterToken unit in spielerFiguren)
                {
                    GameMap.Children.Remove(unit.UIElement);
                }
                spielerFiguren.Clear();
            }
            if (Ally1.IsChecked == true)
            {
                foreach (CharakterToken unit in allies)
                {
                    GameMap.Children.Remove(unit.UIElement);
                }
                allies.Clear();
            }
            if (Foes.IsChecked == true)
            {
                foreach (CharakterToken unit in monster)
                {
                    GameMap.Children.Remove(unit.UIElement);
                }
                monster.Clear();
            }
            if (ArtDraw.IsChecked == true)
            {
                GameMap.Strokes.Clear();
            }

        }

        // Funktion für die vollständige Renderung des Programms (momentan obsolet)
        private void Window_ContentRendered(object sender, EventArgs e)
        {

            distance.Stroke = Brushes.Black;
            GameMap.Children.Add(distance);
            distance.Visibility = Visibility.Collapsed;

            GameMap.Children.Add(distanceValue);
            distanceValue.Visibility = Visibility.Collapsed;
            distanceValue2.Visibility = Visibility.Collapsed;

            RadioBlack.IsChecked = true;
        }

        // Methode zur Erstellung der Einrasterung
        private void DrawMapArea(bool OnOff)
        {
            if (OnOff)
            {
                bool doneDrawingBackground = false;
                int nextX = 0, nextY = 0;
                int rowCounter = 0;
                bool nextIsOdd = false;

                while (doneDrawingBackground == false)
                {
                    Rectangle rect;
                    //if (OnOff)
                    //{
                        rect = new Rectangle
                        {
                            Width = MapSquareSize,
                            Height = MapSquareSize,
                            Fill = nextIsOdd ? Brushes.Wheat : Brushes.LightGray
                        };
                    //}
                    //else
                    //{
                    //    rect = new Rectangle()
                    //    {
                    //        Width = MapSquareSize,
                    //        Height = MapSquareSize,
                    //        Fill = Brushes.Wheat
                    //    };
                    //}

                    GameMap.Children.Add(rect);
                    kartenKacheln.Add(rect);
                    Canvas.SetTop(rect, nextY - 10);
                    Canvas.SetLeft(rect, nextX - 10);

                    nextIsOdd = !nextIsOdd;
                    nextX += MapSquareSize;

                    if (nextX >= GameMap.ActualWidth)
                    {
                        nextX = 0;
                        nextY += MapSquareSize;
                        rowCounter++;
                        nextIsOdd = (rowCounter % 2 != 0);
                    }

                    if (nextY >= GameMap.ActualHeight)
                        doneDrawingBackground = true;
                }
            }
            
        }

        // Button für Erstellung einer Spielfigur
        private void CharButton_Click(object sender, RoutedEventArgs e)
        {
            CharakterToken Unit = new CharakterToken();

            Point startPunkt = new Point(180, 60);
            Unit.TokenID = IDStarter++;

            // Erstellung des UIElement
            if(Unit.UIElement == null)
            {
                // Check und Erstellung der Spielfiguren auf dem Canvas
                if ((bool)Player.IsChecked)
                {

                    Unit.Size = (int)CreatureSize.Medium;
                    Unit.Name = UnitName.Text;
                    Unit.Side = (int)Affiliation.Player;
                    Unit.UIElement = BorderCreatorMethod(Unit);
                    spielerFiguren.Add(Unit);

                } else if ((bool)Ally.IsChecked)
                {
                    Unit.Size = (int)CreatureSize.Medium;
                    Unit.Name = UnitName.Text;
                    Unit.Side = (int)Affiliation.Ally;
                    Unit.UIElement = BorderCreatorMethod(Unit);
                    allies.Add(Unit);

                } else if ((bool)FoeM.IsChecked)
                {
                    Unit.Size = (int)CreatureSize.Medium;
                    Unit.Name = UnitName.Text;
                    Unit.Side = (int)Affiliation.Foe;
                    Unit.UIElement = BorderCreatorMethod(Unit);
                    monster.Add(Unit);

                } else if ((bool)FoeL.IsChecked)
                {
                    Unit.Size = (int)CreatureSize.Large;
                    Unit.Name = UnitName.Text;
                    Unit.Side = (int)Affiliation.Foe;
                    Unit.UIElement = BorderCreatorMethod(Unit);
                    monster.Add(Unit);

                } else
                {
                    Unit.Size = (int)CreatureSize.ExtraLarge;
                    Unit.Name = UnitName.Text;
                    Unit.Side = (int)Affiliation.Foe;
                    Unit.UIElement = BorderCreatorMethod(Unit);
                    monster.Add(Unit);
                }
                
            }

            // Drag&Drop Eventhandler für die Spielfiguren
            MouseButtonEventHandler mouseDown = (sendert, args) => {
                var element = (UIElement)sendert;
                distance.X1 = InkCanvas.GetLeft(element) + (element.RenderSize.Width / 2);
                distance.Y1 = InkCanvas.GetTop(element) + (element.RenderSize.Height / 2);
                dragStart = args.GetPosition(element);
                element.CaptureMouse();
            };
            MouseButtonEventHandler mouseUp = (sendert, args) => {
                var element = (UIElement)sendert;
                distance.Visibility = Visibility.Collapsed;
                distanceValue.Visibility = Visibility.Collapsed;
                distanceValue2.Visibility = Visibility.Collapsed;
                dragStart = null;
                distance.X1 = 0;
                distance.Y1 = 0;
                distance.X2 = 0;
                distance.Y2 = 0;
                element.ReleaseMouseCapture();
            };
            MouseEventHandler mouseMove = (sendert, args) => {
                if (dragStart != null && args.LeftButton == MouseButtonState.Pressed)
                {
                    string distanceText = (Math.Round(((Math.Sqrt(Math.Pow(Math.Abs(distance.X1 - distance.X2), 2) + Math.Pow(Math.Abs(distance.Y1 - distance.Y2), 2))) / 40), 2)).ToString() + " m";
                    var element = (UIElement)sendert;
                    var p2 = args.GetPosition(GameMap);
                    
                    InkCanvas.SetLeft(element, p2.X - dragStart.Value.X);
                    InkCanvas.SetTop(element, p2.Y - dragStart.Value.Y);
                    distance.Visibility = Visibility.Visible;
                    distance.X2 = InkCanvas.GetLeft(element) + (element.RenderSize.Width / 2);
                    distance.Y2 = InkCanvas.GetTop(element) + (element.RenderSize.Height / 2);

                    distanceValue.Text = distanceText;
                    distanceValue2.Text = distanceText;
                    distanceValue.FontSize = 16;
                    distanceValue.Visibility = Visibility.Visible;
                    distanceValue2.Visibility = Visibility.Visible;
                    InkCanvas.SetLeft(distanceValue, (Math.Abs(distance.X1 + distance.X2) / 2) + 15);
                    InkCanvas.SetTop(distanceValue, (Math.Abs(distance.Y1 + distance.Y2) / 2) - 30);
                }
            };
            MouseButtonEventHandler mouseRightClick = (sendert, args) =>
            {
                GameMap.Children.Remove((UIElement)sendert);
            };
            Action<UIElement> enableDrag = (element) => {
                element.MouseDown += mouseDown;
                element.MouseMove += mouseMove;
                element.MouseUp += mouseUp;
                element.MouseRightButtonUp += mouseRightClick;
            };

            // Eventhandler für das neue Element aktivieren
            enableDrag(Unit.UIElement);
            GameMap.Children.Add(Unit.UIElement);

            // Position des neuen Element im Canvas setzen
            InkCanvas.SetTop(Unit.UIElement, startPunkt.Y - 10);
            InkCanvas.SetLeft(Unit.UIElement, startPunkt.X - 10);
        }

        // Methode zur Erstellung des UIElement für Spielfiguren
        private Border BorderCreatorMethod(CharakterToken Unit)
        {
            Brush affiliation = Brushes.Gray;

            string name = "";

            switch (Unit.Side)
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

            if (UnitName.Text.Equals(""))
            {
                name = name + IDStarter.ToString();
            } else
            {
                name = UnitName.Text;
            }

            Border borderReturn = new Border()
            {
                Width = MapSquareSize * Unit.Size,
                Height = MapSquareSize * Unit.Size,
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

            return borderReturn;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (RadioBlack.IsChecked == true)
            {
                GameMap.DefaultDrawingAttributes.Color = Colors.Black;
            }
            if (RadioBlue.IsChecked == true)
            {
                GameMap.DefaultDrawingAttributes.Color = Colors.Blue;
            }
            if (RadioRed.IsChecked == true)
            {
                GameMap.DefaultDrawingAttributes.Color = Colors.Red;
            }
        }
    }
}
