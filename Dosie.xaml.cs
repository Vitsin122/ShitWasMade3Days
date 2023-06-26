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
    /// Логика взаимодействия для Dosie.xaml
    /// </summary>
    public partial class Dosie : Window
    {
        public Dosie()
        {
            InitializeComponent();
        }

        private void AddArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object current = DosieGrid.DataContext;

                if (current is Killer)
                {
                    ((Killer)current).ArchiveCrimes();
                }
                else if (current is Thief)
                {
                    ((Thief)current).ArchiveCrimes();
                }
                else if (current is Hacker)
                {
                    ((Hacker)current).ArchiveCrimes();
                }

                InfWin3 infWin3 = new InfWin3();
                infWin3.ShowDialog();
            }
            catch (Exception ex)
            {
                InfWin6 infWin6 = new InfWin6();
                infWin6.ShowDialog();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            object current = DosieGrid.DataContext;

            InfWin4 infWin4 = new InfWin4();
            infWin4.ShowDialog();

            if (infWin4.InfWin4Result == InfWin4.Choice.Yes)
            {
                if (current is Killer)
                {
                    ((Killer)current).DeleteCrimes();
                }
                else if (current is Thief)
                {
                    ((Thief)current).DeleteCrimes();
                }
                else if (current is Hacker)
                {
                    ((Hacker)current).DeleteCrimes();
                }

                InfWin2 infWin2 = new InfWin2();
                infWin2.ShowDialog();
            }
        }

        private void GenerallAddingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object current = DosieGrid.DataContext;

                if (current is Killer)
                {
                    ((Killer)current).GeneralTableAdding();
                }
                else if (current is Thief)
                {
                    ((Thief)current).GeneralTableAdding();
                }
                else if (current is Hacker)
                {
                    ((Hacker)current).GeneralTableAdding();
                }

                InfWin5 infWin5 = new InfWin5();
                infWin5.ShowDialog();
            }
            catch (Exception ex)
            {
                InfWin7 infWin7 = new InfWin7();
                infWin7.ShowDialog();
            }
        }
    }
}
