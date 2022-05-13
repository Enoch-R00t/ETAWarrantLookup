using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace ETAWarrantLookup.Models
{
    /// <summary>
    /// Represents a single warrant
    /// </summary>
    public class WarrantModel
    {
        public string GWARRANTDETAILSURL { get; set; }

        public string GACREC { get; set; }
        public string GCORI { get; set; }

        public string GCASE
        {
            get
            {
                return String.Format("{0}-{1}-{2}", GCASE1, GCASE2, GCASE3);
            }
        }
        public string GCASE1 { get; set; }

        public string GCASE2 { get; set; }

        public string GCASE3 { get; set; }

        public string GTYPE { get; set; }

        public string GWHOLENAME
        {
            get
            {
                return string.Format("{0}, {1}", GLNAME, GFNAME);
            }
        }
        public string GLNAME { get; set; }

        public string GMNAME { get; set; }
        public string GFNAME { get; set; }
        public string GADDR { get; set; }

        //private string _gCity;
        public string GCITY { get; set; }
        //get
        //{
        //    Debug.WriteLine(_gCity);

        //    if(_gCity.ToString() != null && _gCity.Length > 0 )
        //    {
        //        //return string.Format(string.Concat(_gCity.AsSpan(0, 1), _gCity[1..].ToLower() + " Municipal Court"));
        //        //return _gCity[..1] + _gCity[1..].ToLower() + " Municipal Court";

        //    }
        //    else
        //    {
        //        return "None Specified";
        //    }
        //}
        //set { _gCity = value; }

        //}

        public string GSTATE { get; set; }

        public string GZIP5 { get; set; }
        public string GZIP4 { get; set; }
        public string GRACE { get; set; }
        public string GSEX { get; set; }
        public string GHGT { get; set; }
        public string GWHT { get; set; }
        public string GEYES { get; set; }
        public string GHAIR { get; set; }
        public string GOFFD { get; set; }

        private string _gofdate;
        public string GODATE {
            get
            {
                // this can be of the form
                // 80682 (should be 08/06/82)
                // 122979 (12/29/29)
                if (!string.IsNullOrWhiteSpace(_gofdate) && _gofdate.Length >= 5)
                {
                    if (_gofdate.Length == 5)
                    {
                        _gofdate = _gofdate.Insert(0, "0");
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append(_gofdate[0]);
                    sb.Append(_gofdate[1]);
                    sb.Append('/');
                    sb.Append(_gofdate[2]);
                    sb.Append(_gofdate[3]);
                    sb.Append('/');
                    sb.Append(_gofdate[4]);
                    sb.Append(_gofdate[5]);

                    return sb.ToString();
                }

                return _gofdate;
            }
            set
            {
                _gofdate = value;
            }
        }

        private string _gwdate;
        public string GWDATE
        {
            get
            {
                // this can be of the form
                // 80682 (should be 08/06/82)
                // 122979 (12/29/29)
                if (!string.IsNullOrWhiteSpace(_gwdate) && _gwdate.Length >= 5)
                {
                    if (_gwdate.Length == 5)
                    {
                        _gwdate = _gwdate.Insert(0, "0");
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append(_gwdate[0]);
                    sb.Append(_gwdate[1]);
                    sb.Append('/');
                    sb.Append(_gwdate[2]);
                    sb.Append(_gwdate[3]);
                    sb.Append('/');
                    sb.Append(_gwdate[4]);
                    sb.Append(_gwdate[5]);

                    return sb.ToString();
                }

                return "00/00/00"; // _gwdate;
            }
            set
            {
                _gwdate = value;
            }
        }

        private string _gbdate;
        public string GBDATE {
            get
            {
                // this can be of the form
                // 80682 (should be 08/06/82)
                // 122979 (12/29/29)
                if (!string.IsNullOrWhiteSpace(_gbdate) && _gbdate.Length >= 5)
                {
                    if (_gbdate.Length == 5)
                    {
                        _gbdate = _gbdate.Insert(0, "0");
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append(_gbdate[0]);
                    sb.Append(_gbdate[1]);
                    sb.Append('/');
                    sb.Append(_gbdate[2]);
                    sb.Append(_gbdate[3]);
                    sb.Append('/');
                    sb.Append(_gbdate[4]);
                    sb.Append(_gbdate[5]);

                    return sb.ToString();
                }

                return _gbdate;
            }
            set
            {
                _gbdate = value;
            }
        }

        public int GAGE
        {
            get {
                // try to get a real date
                if (DateTime.TryParseExact(_gbdate, "MMddyy" , CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime birthday))
                {
                    int years = DateTime.Now.Year - birthday.Year;

                    if ((birthday.Month > DateTime.Now.Month) || (birthday.Month == DateTime.Now.Month && birthday.Day > DateTime.Now.Day))
                        years--;

                    return years;
                }
                return 0;
            }                                   
        }

        public string GUDATE { get; set; }
        public string GPORI { get; set; }
        public string GTAGNO { get; set; }
        public string GTAGST { get; set; }
        public string GSSN { get; set; }
        public string GDRLIC { get; set; }
        public string GDLST { get; set; }
        public string GNAME { get; set; }
        public string GBADR {get;set;}
        public string GCSZ { get; set; }
        public string GPHONE { get; set; }
        public string GPCONT { get; set; }

    }
}
