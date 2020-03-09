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
        private readonly List<UIElement> children = new List<UIElement>();

        private Nullable<Point> dragStart = null;

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
                var element = (UIElement)sendert;
                dragStart = args.GetPosition(element);
                element.CaptureMouse();
            };
            MouseButtonEventHandler mouseUp = (sendert, args) => {
                var element = (UIElement)sendert;
                dragStart = null;
                element.ReleaseMouseCapture();
            };
            MouseEventHandler mouseMove = (sendert, args) => {
                if (dragStart != null && args.LeftButton == MouseButtonState.Pressed)
                {
                    var element = (UIElement)sendert;
                    var p2 = args.GetPosition(this.Parent as UIElement);
                    
                    InitiativeTracker.SetLeft(element, p2.X - dragStart.Value.X);
                    InitiativeTracker.SetTop(element, p2.Y - dragStart.Value.Y);
                }
            };
            Action<UIElement> enableDrag = (element) => {
                element.MouseDown += mouseDown;
                element.MouseMove += mouseMove;
                element.MouseUp += mouseUp;
            };

            enableDrag(initiativeMember);
            Children.Add(initiativeMember);

            InitiativeTracker.SetTop(initiativeMember, 10 + ((Children.Count - 1) * 23));
            InitiativeTracker.SetLeft(initiativeMember, 15);

            return initiativeMember;
        }
    }
}
