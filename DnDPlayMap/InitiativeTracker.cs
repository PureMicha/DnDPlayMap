using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DnDPlayMap
{
    class InitiativeTracker : Canvas
    {
        // "Liste" für die Reihenfolge der Initiative
        private readonly IDictionary<Border, int> initPosition = new Dictionary<Border, int>();

        // momentanes Objekt als Aktionsausführer
        private KeyValuePair<Border, int> action = new KeyValuePair<Border, int>(null, -1);

        // Punkt für den Anfang der Bewegung für Drag&Drop
        private Nullable<Point> dragStart = null;

        // Farbe für den Rand zur Erkennung des momentanen Aktionsausführer
        private SolidColorBrush NotSelected = Brushes.Black;
        private SolidColorBrush Selected = Brushes.White;

        // Methode zur Erstellung der Spielfigur im InitiativenTracker 
        public Border InitiativeMemberCreator(CharakterToken Unit)
        {
            Border initiativeMember = new Border()
            {
                Width = 120,
                Height = 23,
                BorderThickness = new Thickness(2),
                BorderBrush = Brushes.Black,
                Background = Unit.getColor(),
                Child = new TextBlock()
                {
                    Text = Unit.Name,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                }
            };

            // Drag&Drop Eventhandler für die Reihenfolge im Initiativetracker
            MouseButtonEventHandler mouseDown = (sendert, args) => {
                var element = (Border)sendert;
                dragStart = args.GetPosition(element);
                element.CaptureMouse();
            };
            MouseButtonEventHandler mouseUp = (sendert, args) => {
                var element = (Border)sendert;
                dragStart = null;
                element.ReleaseMouseCapture();

                if (initPosition.TryGetValue(element, out int result))
                {
                    InitiativeTracker.SetTop(element, 10 + (result * 23));
                    InitiativeTracker.SetLeft(element, 15);
                }
            };
            MouseEventHandler mouseMove = (sendert, args) => {
                if (dragStart != null && args.LeftButton == MouseButtonState.Pressed)
                {
                    var element = (Border)sendert;
                    var p2 = args.GetPosition(this.Parent as UIElement);

                    InitiativeTracker.SetLeft(element, p2.X - dragStart.Value.X);
                    InitiativeTracker.SetTop(element, p2.Y - dragStart.Value.Y);

                    if (initPosition.TryGetValue(element, out int result))
                    {
                        foreach (KeyValuePair<Border, int> item in initPosition)
                        {
                            if (item.Key.Equals(element)) continue;

                            if ((result > item.Value && InitiativeTracker.GetTop(element) < InitiativeTracker.GetTop(item.Key)) || (result < item.Value && InitiativeTracker.GetTop(element) > InitiativeTracker.GetTop(item.Key)))
                            {
                                int temp = item.Value;
                                initPosition[item.Key] = result;
                                initPosition[element] = temp;
                                break;
                            }
                        }
                    }

                    foreach (KeyValuePair<Border, int> item in initPosition)
                    {
                        if (item.Key.Equals(element)) continue;

                        InitiativeTracker.SetTop(item.Key, 10 + (item.Value * 23));
                        InitiativeTracker.SetLeft(item.Key, 15);
                    }
                    
                }
            };
            Action<UIElement> enableDrag = (element) => {
                element.MouseDown += mouseDown;
                element.MouseMove += mouseMove;
                element.MouseUp += mouseUp;
            };

            enableDrag(initiativeMember);

            initPosition.Add(initiativeMember, Children.Count);

            Children.Add(initiativeMember);
            
            InitiativeTracker.SetTop(initiativeMember, 10 + ((Children.Count - 1) * 23));
            InitiativeTracker.SetLeft(initiativeMember, 15);

            return initiativeMember;
        }

        // Methode für den nächsten Aktionsausführer
        public Border MoveForward()
        {
            if (initPosition.Count == 0) return null;

            if (action.Value == -1)
            {
                action = initPosition.FirstOrDefault(x => x.Value == 0);

                action.Key.BorderBrush = Selected;

            } else
            {
                action.Key.BorderBrush = NotSelected;

                if (action.Value == initPosition.Count - 1)
                {
                    action = initPosition.FirstOrDefault(x => x.Value == 0);

                    action.Key.BorderBrush = Selected;

                } else
                {
                    action = initPosition.FirstOrDefault(x => x.Value == action.Value + 1);

                    action.Key.BorderBrush = Selected;
                }
            }

            return action.Key;
        }

        // Methode für den vorherigen Aktionsausführer
        public Border MoveBack()
        {
            if (initPosition.Count == 0) return null;

            if (action.Value == -1)
            {
                action = initPosition.FirstOrDefault(x => x.Value == 0);

                action.Key.BorderBrush = Selected;

            } else
            {
                action.Key.BorderBrush = NotSelected;

                if (action.Value == 0)
                {
                    action = initPosition.FirstOrDefault(x => x.Value == initPosition.Count - 1);

                    action.Key.BorderBrush = Selected;

                } else
                {
                    action = initPosition.FirstOrDefault(x => x.Value == action.Value - 1);

                    action.Key.BorderBrush = Selected;
                }
            }

            return action.Key;
        }
    }
}
