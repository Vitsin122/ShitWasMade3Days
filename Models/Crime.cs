using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Kursach.Models
{
    internal abstract class Crime
    {
        public int Id { get; protected set; }
        public TCP.TPCEnum tcp { get; protected set; }
        public string _name { get; protected set; }
        public string _lastname { get; protected set; }
        public string _fakename { get; protected set; }
        protected DateTime _bdate;
        public string Bdate
        {
            get => _bdate.ToShortDateString();
            set
            {
                _bdate = Convert.ToDateTime(value);
            }
        }
        public string _citizenship { get; protected set; }
        public string _bplace { get; protected set; }
        public string _lastplace { get; protected set; }
        protected string[] _languages;
        public string Languages
        {
            get
            {
                string result = "";

                foreach (var value in _languages)
                {
                    result += value + " ";
                }

                return result;
            }
        }
        
        public string _crimeprof { get; protected set; }
        public double _height { get; protected set; }
        public string _eyecolor { get; protected set; }
        public string? _hairscolor { get; protected set; }
        public string[]? _features;
        public string? Features
        {
            get
            {
                if (_features != null)
                {
                    string result = "";

                    foreach (var value in _features)
                    {
                        result += value + " ";
                    }

                    return result;
                }
                return null;
            }
        }
        public BitmapImage _bytes { get; set; }
    }
}
