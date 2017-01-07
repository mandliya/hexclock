using System;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HexClock
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            try
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += timer_Tick;
                timer.Interval = new TimeSpan(0, 0, 0, 1);
                timer.Start();
                timer_Tick(null, null);
            }
            catch
            {

            }
        }

        private void timer_Tick(object sender, object e)
        {
            try
            {
                int hour = System.DateTime.Now.Hour;
                int minute = System.DateTime.Now.Minute;
                int second = System.DateTime.Now.Second;
                StringBuilder sb = new System.Text.StringBuilder("#");
                sb = (hour < 10) ? (sb.Append("0" + hour.ToString())) : (sb.Append(hour.ToString()));
                sb = (minute < 10) ? (sb.Append("0" + minute.ToString())) : (sb.Append(minute.ToString()));
                sb = (second < 10) ? (sb.Append("0" + second.ToString())) : (sb.Append(second.ToString()));
                string hexTime = sb.ToString();
                HexColor color = new HexColor(hexTime);
                SolidColorBrush bgBrush = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.B, color.G));
                mainGrid.Background = bgBrush;
                TimeHours.Text = hour.ToString();
                TimeMinutes.Text = (minute < 10) ? ("0" + minute.ToString()) : (minute.ToString());
                TimeSeconds.Text = (second < 10) ? ("0" + second.ToString()) : (second.ToString());
                DateText.Text = System.DateTime.Now.Date.ToString();
            }
            catch
            {

            }
        }
    }

    public struct HexColor
    {
        Color _color;
        const string HEX_PATTERN = @"^\#([a-fA-F0-9]{6}|[a-fA-F0-9]{8})$";
        const int LENGTH_WITH_ALPHA = 8;

        public HexColor(string hexcode)
        {
            if (hexcode == null)
            {
                throw new ArgumentNullException("hexcode required");    
            }

            if (!Regex.IsMatch(hexcode, HEX_PATTERN))
            {
                throw new ArgumentException("Format must be 6 digit hexcode preceded by a #, no whitespace", "hexcode");
            }

            // get rid of '#'
            hexcode = hexcode.Trim('#');

            // if no alpha value specified, assume no transparency (0xFF)
            if (hexcode.Length != LENGTH_WITH_ALPHA)
            {
                hexcode = String.Format("FF{0}", hexcode);   
            }

            _color = new Color();
            _color.A = byte.Parse(hexcode.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            if (_color.A < 50)
                _color.A = 50;

            _color.R = byte.Parse(hexcode.Substring(6, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            _color.G = byte.Parse(hexcode.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            _color.B = byte.Parse(hexcode.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

        }

        public static implicit operator Color(HexColor hexColor)
        {
            return hexColor._color;
        }

        public static implicit operator HexColor(Color color)
        {
            HexColor c = new HexColor("#00000000");
            c._color = color;
            return c;
        }

        public override string ToString()
        {
            return _color.ToString();
        }

        public byte A
        {
            get { return _color.A; }
            set { _color.A = value; }
        }

        public byte R
        {
            get { return _color.R; }
            set { _color.R = value; }
        }

        public byte B
        {
            get { return _color.B; }
            set { _color.B = value; }
        }

        public byte G
        {
            get { return _color.G; }
            set { _color.G = value; }
        }
    }
}
