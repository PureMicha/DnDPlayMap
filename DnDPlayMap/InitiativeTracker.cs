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

        private void AddChild(UIElement element)
        {
            children.Add(element);
            Children.Add(element);
        }

        public void RemoveChild(UIElement element)
        {
            if (children.Remove(element))
            {
                Children.Remove(element);
            }
        }

        // Methode zur Erstellung der Spielfigur im InitiativenTracker 
        public TextBlock InitiativeMemberCreator(CharakterToken Unit)
        {
            TextBlock initiativeMember = new TextBlock()
            {
                Width = 120,
                Height = 23,
                Text = Unit.Name,
                Background = Brushes.White,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                //muss überarbeitet werden
                Margin = new Thickness(0, 10 + ((children.Count) * 23), 0, 0)
            };

            // Drag&Drop Eventhandler für die Spielfiguren
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
                    var p2 = args.GetPosition(element);
                    
                    Canvas.SetLeft(element, p2.X - dragStart.Value.X);
                    Canvas.SetTop(element, p2.Y - dragStart.Value.Y);
                }
            };
            Action<UIElement> enableDrag = (element) => {
                element.MouseDown += mouseDown;
                element.MouseMove += mouseMove;
                element.MouseUp += mouseUp;
            };

            enableDrag(initiativeMember);
            AddChild(initiativeMember);

            return initiativeMember;
        }
    }
}
