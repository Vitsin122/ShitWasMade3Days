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

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для InfWin4.xaml
    /// </summary>
    public partial class InfWin4 : Window
    {
        public enum Choice
        {
            Yes,
            No
        }
        public Choice InfWin4Result { get; private set; }
        public InfWin4()
        {
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            InfWin4Result = Choice.Yes;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            InfWin4Result = Choice.No;
            Close();
        }
    }
}
