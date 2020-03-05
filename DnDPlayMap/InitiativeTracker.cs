using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DnDPlayMap
{
    class InitiativeTracker : Grid
    {
        private readonly List<UIElement> children = new List<UIElement>();

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
        public TextBox InitiativeMemberCreator(CharakterToken Unit)
        {
            TextBox initiativeMember = new TextBox()
            {
                Width = 120,
                Height = 23,
                IsReadOnly = true,
                Text = Unit.Name,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 10 + ((children.Count) * 23), 0, 0)
            };

            AddChild(initiativeMember);

            return initiativeMember;
        }
    }
}
