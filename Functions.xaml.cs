using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;
using System.Configuration;
using System.Windows.Markup;
using System.Data.SqlClient;
using System.Windows.Interop;
using System.Threading;
using System.Data;
using System.Text.RegularExpressions;
using System.ComponentModel;
using Kursach.Models;
using System.Numerics;
using System.Xml.Linq;

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для Functions.xaml
    /// </summary>
    public partial class Functions : Window
    {
        private byte[]? bytes;
        private string? connectionString = ConfigurationManager.ConnectionStrings["CrimeDB"].ConnectionString;
        private SqlConnection? sqlConnection = null;

        private BindingList<Crime> _crimes = null;
        private BindingList<Crime> _searchcrimes = null;
        private BindingList<Crime> _archivecrimes = null;
        
        public Functions()
        {
            InitializeComponent();
        }
        private void SetEmptyInFunctions()
        {
            Name.Text = String.Empty;
            LastName.Text = String.Empty;
            FakeName.Text = String.Empty;
            BDate.Text = String.Empty;
            Citizen.Text = String.Empty;
            BPlace.Text = String.Empty;
            LastPlace.Text = String.Empty;
            Languages.Text = String.Empty;
            CrimeProf.Text = String.Empty;
            Height.Text = String.Empty;
            EyeColor.Text = String.Empty;
            HairsColor.Text = String.Empty;
            Features.Text = String.Empty;
            ProfilePhoto.Source = new BitmapImage(new Uri("/Icons/StockPhoto.jpg", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache };
        }
        private bool isWord(string str) => !Regex.IsMatch(str, @"\d+");
        private bool CheckWords()
        {
            if (isWord(Name.Text) && isWord(LastName.Text) && isWord(FakeName.Text) && isWord(LastPlace.Text) && isWord(Citizen.Text) && isWord(Languages.Text) && isWord(CrimeProf.Text) && isWord(EyeColor.Text) && isWord(BPlace.Text) && isWord(HairsColor.Text) && isWord(Features.Text))
                return true;

            return false;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = sqlConnection;

                #region fill all values

                string? name = Name.Text != "" ? Name.Text.ToUpper() : null;
                string? lastname = LastName.Text != "" ? LastName.Text.ToUpper() : null;
                string? fakename = FakeName.Text != "" ? FakeName.Text.ToUpper() : null;
                string? bdate = BDate.Text != "" ? BDate.Text : null;
                string? citizen = Citizen.Text != "" ? Citizen.Text.ToUpper() : null;
                string? bplace = BPlace.Text != "" ? BPlace.Text.ToUpper(): null;
                string? lastplace = LastPlace.Text != "" ? LastPlace.Text.ToUpper() : null;
                string? languages = Languages.Text != "" ? Languages.Text.ToUpper() : null;
                string? crimeprof = CrimeProf.Text != "" ? CrimeProf.Text.ToUpper() : null;
                string? eyescolor = EyeColor.Text != "" ? EyeColor.Text.ToUpper() : null;
                dynamic? hairsColor = HairsColor.Text.ToUpper() != "" ? HairsColor.Text.ToUpper() : DBNull.Value;
                dynamic? features = Features.Text.ToUpper() != "" ? Features.Text.ToUpper() : DBNull.Value;

                #endregion

                if (!CheckWords())
                    throw new FormatException();

                string commandStr = String.Format("INSERT INTO GeneralTable (LastName, Name, FakeName, BirthDate, Citizenship, BirthPlace, LastPlace, Languages, CrimeProf, Height, EyeColor, HairsColor, Features, Photo) " +
                    $"VALUES (@lastname, @name, @fakename, @bdate, @citizen, " +
                    $"@bplace, @lastplace, @languages, @crimeprof, {Convert.ToDouble(Height.Text)}, @eyescolor, " +
                    $"@hairscolor, @features, @ProfilePhoto)");
                command.CommandText = commandStr;

                #region parametrs
                command.Parameters.Add("@ProfilePhoto", SqlDbType.VarBinary, 100000000);
                command.Parameters["@ProfilePhoto"].Value = bytes;
                command.Parameters.Add(new SqlParameter("@name", name != null ? name : null));
                command.Parameters.Add(new SqlParameter("@lastname", lastname != null ? lastname : null));
                command.Parameters.Add(new SqlParameter("@fakename", fakename != null ? fakename : null));
                command.Parameters.Add(new SqlParameter("@bdate", bdate != null ? bdate : null));
                command.Parameters.Add(new SqlParameter("@citizen", citizen != null ? citizen : null));
                command.Parameters.Add(new SqlParameter("@bplace", bplace != null ? bplace : null));
                command.Parameters.Add(new SqlParameter("@lastplace", lastplace != null ? lastplace : null));
                command.Parameters.Add(new SqlParameter("@languages", languages != null ? languages : null));
                command.Parameters.Add(new SqlParameter("@crimeprof", crimeprof != null ? crimeprof : null));
                command.Parameters.Add(new SqlParameter("@eyescolor", eyescolor != null ? eyescolor : null));
                command.Parameters.Add(new SqlParameter("@hairscolor", hairsColor != null ? hairsColor : null));
                command.Parameters.Add(new SqlParameter("@features", features != null ? features : null));
                #endregion

                command.ExecuteNonQuery();

                if (hairsColor is DBNull)
                    hairsColor = null;
                if (features is DBNull)
                    features = null;

                command.CommandText = "SELECT MAX(Id) FROM GeneralTable";

                int id = (int)command.ExecuteScalar();

                if (CrimeProf.Text == "Вор")
                    _crimes.Add(new Thief(id, name, lastname, fakename, bdate, citizen, bplace, lastplace, languages, crimeprof, Convert.ToDouble(Height.Text), eyescolor, hairsColor, features, (byte[])bytes.Clone()));
                else if (CrimeProf.Text == "Убийца")
                    _crimes.Add(new Killer(id, name, lastname, fakename, bdate, citizen, bplace, lastplace, languages, crimeprof, Convert.ToDouble(Height.Text), eyescolor, hairsColor, features, (byte[])bytes.Clone()));
                else if (CrimeProf.Text == "Взломщик")
                    _crimes.Add(new Hacker(id, name, lastname, fakename, bdate, citizen, bplace, lastplace, languages, crimeprof, Convert.ToDouble(Height.Text), eyescolor, hairsColor, features, (byte[])bytes.Clone()));

                InfoWin1 iw = new InfoWin1();
                iw.ShowDialog();
                SetEmptyInFunctions();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Введите рост в виде числа или не используйте цифры в других полях");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Пустыми могут быть только поля цвета волос и особых примет");
            }
        }

        private void UploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;)|*.png;";

            bytes = null;

            if (openFileDialog.ShowDialog() == true)
            {
                string filepath = openFileDialog.FileName;

                using (var br = new BinaryReader(File.OpenRead(filepath)))
                {
                    bytes = br.ReadBytes((int)br.BaseStream.Length);
                }

                if (bytes != null)
                {
                    try
                    {
                        MemoryStream memorystream = new MemoryStream();
                        memorystream.Write(bytes, 0, (int)bytes.Length);

                        BitmapImage imgsource = new BitmapImage();
                        imgsource.BeginInit();
                        imgsource.StreamSource = memorystream;
                        imgsource.EndInit();
                        ProfilePhoto.Source = imgsource;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Файл изображения повреждён");
                    }
                }

            }

                
        }

        private void FuncWindow_Loaded(object sender, RoutedEventArgs e)
        {
            sqlConnection = new SqlConnection(connectionString);

            sqlConnection?.Open();

            _crimes = new BindingList<Crime>();

            SqlCommand command = new SqlCommand("SELECT * FROM GeneralTable", sqlConnection);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if ((string)reader["CrimeProf"] == "ВОР")
                    {
                        string? features, hairscolor;

                        if (reader["Features"] is DBNull)
                            features = null;
                        else
                            features = (string)reader["Features"];

                        if (reader["HairsColor"] is DBNull)
                            hairscolor = null;
                        else
                            hairscolor = (string)reader["HairsColor"];

                        _crimes.Add(new Thief((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                    }
                    else if ((string)reader["CrimeProf"] == "УБИЙЦА")
                    {
                        string? features, hairscolor;

                        if (reader["Features"] is DBNull)
                            features = null;
                        else
                            features = (string)reader["Features"];

                        if (reader["HairsColor"] is DBNull)
                            hairscolor = null;
                        else
                            hairscolor = (string)reader["HairsColor"];

                        _crimes.Add(new Killer((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                    }
                    else if ((string)reader["CrimeProf"] == "ВЗЛОМЩИК")
                    {
                        string? features, hairscolor;

                        if (reader["Features"] is DBNull)
                            features = null;
                        else
                            features = (string)reader["Features"];

                        if (reader["HairsColor"] is DBNull)
                            hairscolor = null;
                        else
                            hairscolor = (string)reader["HairsColor"];

                        _crimes.Add(new Hacker((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                    }
                }
            }
            reader.Close();

            _archivecrimes = new BindingList<Crime>();

            command.CommandText = "SELECT * FROM Archive";

            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if ((string)reader["CrimeProf"] == "ВОР")
                    {
                        string? features, hairscolor;

                        if (reader["Features"] is DBNull)
                            features = null;
                        else
                            features = (string)reader["Features"];

                        if (reader["HairsColor"] is DBNull)
                            hairscolor = null;
                        else
                            hairscolor = (string)reader["HairsColor"];

                        _archivecrimes.Add(new Thief((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                    }
                    else if ((string)reader["CrimeProf"] == "УБИЙЦА")
                    {
                        string? features, hairscolor;

                        if (reader["Features"] is DBNull)
                            features = null;
                        else
                            features = (string)reader["Features"];

                        if (reader["HairsColor"] is DBNull)
                            hairscolor = null;
                        else
                            hairscolor = (string)reader["HairsColor"];

                        _archivecrimes.Add(new Killer((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                    }
                    else if ((string)reader["CrimeProf"] == "ВЗЛОМЩИК")
                    {
                        string? features, hairscolor;

                        if (reader["Features"] is DBNull)
                            features = null;
                        else
                            features = (string)reader["Features"];

                        if (reader["HairsColor"] is DBNull)
                            hairscolor = null;
                        else
                            hairscolor = (string)reader["HairsColor"];

                        _archivecrimes.Add(new Hacker((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                    }
                }
            }
            reader.Close();

            dgArchiveCrimes.ItemsSource = _archivecrimes;
            dgCrimes.ItemsSource = _crimes;
        }
        private void FeelDG()
        {
            _crimes = new BindingList<Crime>();

            SqlCommand command = new SqlCommand("SELECT * FROM GeneralTable", sqlConnection);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if ((string)reader["CrimeProf"] == "ВОР")
                    {
                        string? features, hairscolor;

                        if (reader["Features"] is DBNull)
                            features = null;
                        else
                            features = (string)reader["Features"];

                        if (reader["HairsColor"] is DBNull)
                            hairscolor = null;
                        else
                            hairscolor = (string)reader["HairsColor"];

                        _crimes.Add(new Thief((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                    }
                    else if ((string)reader["CrimeProf"] == "УБИЙЦА")
                    {
                        string? features, hairscolor;

                        if (reader["Features"] is DBNull)
                            features = null;
                        else
                            features = (string)reader["Features"];

                        if (reader["HairsColor"] is DBNull)
                            hairscolor = null;
                        else
                            hairscolor = (string)reader["HairsColor"];

                        _crimes.Add(new Killer((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                    }
                    else if ((string)reader["CrimeProf"] == "ВЗЛОМЩИК")
                    {
                        string? features, hairscolor;

                        if (reader["Features"] is DBNull)
                            features = null;
                        else
                            features = (string)reader["Features"];

                        if (reader["HairsColor"] is DBNull)
                            hairscolor = null;
                        else
                            hairscolor = (string)reader["HairsColor"];

                        _crimes.Add(new Hacker((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                    }
                }
            }
            reader.Close();

            _archivecrimes = new BindingList<Crime>();

            command.CommandText = "SELECT * FROM Archive";

            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if ((string)reader["CrimeProf"] == "ВОР")
                    {
                        string? features, hairscolor;

                        if (reader["Features"] is DBNull)
                            features = null;
                        else
                            features = (string)reader["Features"];

                        if (reader["HairsColor"] is DBNull)
                            hairscolor = null;
                        else
                            hairscolor = (string)reader["HairsColor"];

                        _archivecrimes.Add(new Thief((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                    }
                    else if ((string)reader["CrimeProf"] == "УБИЙЦА")
                    {
                        string? features, hairscolor;

                        if (reader["Features"] is DBNull)
                            features = null;
                        else
                            features = (string)reader["Features"];

                        if (reader["HairsColor"] is DBNull)
                            hairscolor = null;
                        else
                            hairscolor = (string)reader["HairsColor"];

                        _archivecrimes.Add(new Killer((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                    }
                    else if ((string)reader["CrimeProf"] == "ВЗЛОМЩИК")
                    {
                        string? features, hairscolor;

                        if (reader["Features"] is DBNull)
                            features = null;
                        else
                            features = (string)reader["Features"];

                        if (reader["HairsColor"] is DBNull)
                            hairscolor = null;
                        else
                            hairscolor = (string)reader["HairsColor"];

                        _archivecrimes.Add(new Hacker((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                    }
                }
            }
            reader.Close();

            dgArchiveCrimes.ItemsSource = _archivecrimes;
            dgCrimes.ItemsSource = _crimes;
        }

        private void Dosie_Click(object sender, RoutedEventArgs e)
        {
            object current = dgCrimes.CurrentCell.Item;

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

            FeelDG();
        }

        private void ArcheveDosie_Click(object sender, RoutedEventArgs e)
        {
            object current = dgArchiveCrimes.CurrentCell.Item;

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

            FeelDG();
        }

        private void ChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            AddStackPanel.Children.Clear();

            foreach(CheckBox n in GroupStack.Children)
            {
                if (n?.IsChecked ?? false)
                {
                    Label current = new Label();
                    current.Content = n.Content;
                    current.Foreground = Brushes.White;

                    TextBox textBox = new TextBox();
                    textBox.Name = n.Name + "1";

                    if (textBox.Name == "languages1")
                        textBox.ToolTip = "Вводите через запятую. В поиске учавствует полное соответствие";

                    AddStackPanel.Children.Add(current);
                    AddStackPanel.Children.Add(textBox);
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> strings = new List<string>();

                foreach (var ui in AddStackPanel.Children)
                {
                    if (ui is TextBox)
                    {
                        if ((ui as TextBox).Text != "")
                        {
                            if ((ui as TextBox).Name == "height1")
                            {
                                string str = (ui as TextBox).Name;
                                char[] chars = str.ToCharArray();
                                chars[0] = char.ToUpper(chars[0]);
                                str = new string(chars);
                                str = str.Remove(str.Length - 1);

                                strings.Add(str);

                                strings.Add(" = ");
                                strings.Add($"{Convert.ToDouble((ui as TextBox).Text)} ");
                                strings.Add("AND ");
                            }
                            if ((ui as TextBox).Name == "languages1")
                            {
                                string str = (ui as TextBox).Name;
                                char[] chars = str.ToCharArray();
                                chars[0] = char.ToUpper(chars[0]);
                                str = new string(chars);
                                str = str.Remove(str.Length - 1);

                                string[] mas = (ui as TextBox).Text.ToUpper().Replace(" ", "").Split(',');

                                foreach (string value in mas)
                                {
                                    strings.Add(str);

                                    strings.Add($" LIKE N'%{value}%'");

                                    strings.Add("OR ");
                                }
                            }
                            else
                            {
                                string str = (ui as TextBox).Name;
                                char[] chars = str.ToCharArray();
                                chars[0] = char.ToUpper(chars[0]);
                                str = new string(chars);
                                str = str.Remove(str.Length - 1);
                                strings.Add(str);

                                strings.Add(" = ");
                                strings.Add($"N'{(ui as TextBox).Text.ToUpper()}' ");
                                strings.Add("AND ");
                            }
                        }

                    }
                }

                strings.RemoveAt(strings.Count - 1);

                string commandtext = "SELECT * FROM GeneralTable WHERE ";

                foreach (string value in strings)
                {
                    commandtext += value;
                }

                SqlCommand command = new SqlCommand(commandtext, sqlConnection);

                var reader = command.ExecuteReader();

                _searchcrimes = new BindingList<Crime>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if ((string)reader["CrimeProf"] == "ВОР")
                        {
                            string? features, hairscolor;

                            if (reader["Features"] is DBNull)
                                features = null;
                            else
                                features = (string)reader["Features"];

                            if (reader["HairsColor"] is DBNull)
                                hairscolor = null;
                            else
                                hairscolor = (string)reader["HairsColor"];

                            _searchcrimes.Add(new Thief((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                        }
                        else if ((string)reader["CrimeProf"] == "УБИЙЦА")
                        {
                            string? features, hairscolor;

                            if (reader["Features"] is DBNull)
                                features = null;
                            else
                                features = (string)reader["Features"];

                            if (reader["HairsColor"] is DBNull)
                                hairscolor = null;
                            else
                                hairscolor = (string)reader["HairsColor"];

                            _searchcrimes.Add(new Killer((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                        }
                        else if ((string)reader["CrimeProf"] == "ВЗЛОМЩИК")
                        {
                            string? features, hairscolor;

                            if (reader["Features"] is DBNull)
                                features = null;
                            else
                                features = (string)reader["Features"];

                            if (reader["HairsColor"] is DBNull)
                                hairscolor = null;
                            else
                                hairscolor = (string)reader["HairsColor"];

                            _searchcrimes.Add(new Hacker((int)reader["Id"], (string)reader["Name"], (string)reader["LastName"], (string)reader["FakeName"], (string)reader["BirthDate"], (string)reader["Citizenship"], (string)reader["BirthPlace"], (string)reader["LastPlace"], (string)reader["Languages"], (string)reader["CrimeProf"], (float)reader["Height"], (string)reader["EyeColor"], hairscolor, features, (byte[])reader["Photo"]));
                        }
                    }
                }

                ViewWin viewWin = new ViewWin();
                viewWin.dgSearchCrimes.ItemsSource = _searchcrimes;

                reader?.Close();

                viewWin.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Неправильные условия ввода");
            }
        }

        private void Developer_Click(object sender, RoutedEventArgs e)
        {
            DevelopInfo developInfo = new DevelopInfo();
            developInfo.ShowDialog();
        }

        private void Discription_Click(object sender, RoutedEventArgs e)
        {
            AppInfo appInfo = new AppInfo();
            appInfo.ShowDialog();
        }
    }
}
