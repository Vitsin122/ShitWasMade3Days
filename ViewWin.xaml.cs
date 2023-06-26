using Kursach.Models;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для ViewWin.xaml
    /// </summary>
    public partial class ViewWin : Window
    {
        public ViewWin()
        {
            InitializeComponent();
        }

        private void SearchDosie_Click(object sender, RoutedEventArgs e)
        {
            object current = dgSearchCrimes.CurrentCell.Item;

            if (current is Killer)
            {
                ((Killer)current).Show();
            }
            else if (current is Thief)
            {
                ((Thief)current).Show();
            }
            else if (current is Hacker)
            {
                ((Hacker)current).Show();
            }
        }
    }
}
