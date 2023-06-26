using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kursach.Models
{
    internal class Killer : Crime, Actions
    {
        public Killer (int id, string name, string lastname, string fakename, string bdate, string citizen, string bplace, string lastplace, string languages, string crimeprof, double height, string eyecolor, string? hairscolor, string? features, byte[] bytes)
        {
            Id = id;
            tcp = TCP.TPCEnum.KILLER;
            _name = name;
            _lastname = lastname;
            _fakename = fakename;
            Bdate = bdate;
            _citizenship = citizen;
            _bplace = bplace;
            _lastplace = lastplace;
            _languages = languages.Replace(" ", "").Split(',');
            _crimeprof = crimeprof;
            _height = height;
            _eyecolor = eyecolor;
            _hairscolor = hairscolor;
            _features = features?.Replace(" ", "")?.Split(',');

            _bytes = new BitmapImage();

            MemoryStream memorystream = new MemoryStream(bytes);

            var newb = new BitmapImage();


            memorystream.Position = 0;
            newb.BeginInit();
            newb.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            newb.CacheOption = BitmapCacheOption.OnLoad;
            newb.UriSource = null;
            newb.StreamSource = memorystream;
            newb.EndInit();

            newb.Freeze();

            _bytes = (BitmapImage)newb.Clone();
        }

        public void ArchiveCrimes()
        {
            using (var connetcion = new SqlConnection(ConfigurationManager.ConnectionStrings["CrimeDB"].ConnectionString))
            {
                connetcion.Open();

                SqlCommand command = new SqlCommand($"SELECT * FROM Archive WHERE Id = {Id}", connetcion);

                var reader = command.ExecuteReader();

                if (reader != null && reader.HasRows)
                {
                    reader.Close();
                    throw new Exception();
                }
                else
                {
                    reader?.Close();
                    command.CommandText = $"INSERT INTO Archive (LastName, Name, FakeName, BirthDate, Citizenship, BirthPlace, LastPlace, Languages, CrimeProf, Height, EyeColor, HairsColor, Features, Photo) SELECT LastName, Name, FakeName, BirthDate, Citizenship, BirthPlace, LastPlace, Languages, CrimeProf, Height, EyeColor, HairsColor, Features, Photo FROM GeneralTable WHERE Id = {Id}";
                    command.ExecuteNonQuery();
                    command.CommandText = $"DELETE FROM GeneralTable WHERE Id = {Id}";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCrimes()
        {
            using (var connetcion = new SqlConnection(ConfigurationManager.ConnectionStrings["CrimeDB"].ConnectionString))
            {
                connetcion.Open();

                SqlCommand command = new SqlCommand($"SELECT * FROM Archive WHERE Id = {Id}", connetcion);

                var reader = command.ExecuteReader();

                if (reader != null && reader.HasRows)
                {
                    reader.Close();
                    command.CommandText = $"DELETE FROM Archive WHERE Id = {Id}";
                    command.ExecuteNonQuery();
                    return;
                }
                else
                {
                    reader?.Close();
                    command.CommandText = $"DELETE FROM GeneralTable WHERE Id = {Id}";
                    command.ExecuteNonQuery();
                    return;
                }
            }
        }

        public void GeneralTableAdding()
        {
            using (var connetcion = new SqlConnection(ConfigurationManager.ConnectionStrings["CrimeDB"].ConnectionString))
            {
                connetcion.Open();

                SqlCommand command = new SqlCommand($"SELECT * FROM GeneralTable WHERE Id = {Id}", connetcion);

                var reader = command.ExecuteReader();

                if (reader != null && reader.HasRows)
                {
                    reader.Close();
                    throw new Exception();
                }
                else
                {
                    reader?.Close();
                    command.CommandText = $"INSERT INTO GeneralTable (LastName, Name, FakeName, BirthDate, Citizenship, BirthPlace, LastPlace, Languages, CrimeProf, Height, EyeColor, HairsColor, Features, Photo) SELECT LastName, Name, FakeName, BirthDate, Citizenship, BirthPlace, LastPlace, Languages, CrimeProf, Height, EyeColor, HairsColor, Features, Photo FROM Archive WHERE Id = {Id}";
                    command.ExecuteNonQuery();
                    command.CommandText = $"DELETE FROM Archive WHERE Id = {Id}";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Show()
        {
            Dosie dosie = new Dosie();
            dosie.DosieGrid.DataContext = this;
            dosie.ShowDialog();
        }



    }
}
