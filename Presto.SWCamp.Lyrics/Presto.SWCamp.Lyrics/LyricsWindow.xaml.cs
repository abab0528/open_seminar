using Presto.SDK;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Threading;

namespace Presto.SWCamp.Lyrics
{
    /// <summary>
    /// LyricsWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    
    public partial class LyricsWindow : Window
    {
        double time;
        string lyric;
        List<double> timestore = new List<double>();
        Dictionary<double, string> map;
        public LyricsWindow()
        {
            InitializeComponent();
            //predtosdk.prestoservice.playerstreamchanged +=song;
            //가사 읽어오기
            var lines = File.ReadAllLines(@"C:\Users\cbnu\Downloads\Presto.Lyrics.Sample\Musics\숀 (SHAUN) - Way Back Home.lrc");
            var lineCount = File.ReadAllLines(@"C:\Users\cbnu\Downloads\Presto.Lyrics.Sample\Musics\숀 (SHAUN) - Way Back Home.lrc").Length;
            map = new Dictionary<double, string>();
            var titledata= lines[1].Split(':');
            title.Text = titledata[1].Substring(0,titledata[1].Length-1);
            for (int i=3;i<lineCount; i++)
            {
                var splitdata = lines[i].Split(']');
                time = TimeSpan.ParseExact(splitdata[0].Substring(1).ToString(), @"mm\:ss\.ff", CultureInfo.InvariantCulture).TotalMilliseconds;
                //MessageBox.Show(time.TotalMilliseconds.ToString());
                lyric = splitdata[1];
                //MessageBox.Show(lyric.ToString());
                timestore.Add(time);
                map.Add(time, lyric);
            }
           
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };

            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            double position = PrestoSDK.PrestoService.Player.Position;
            if (position <= 0) return;
            int check = timestore.BinarySearch(position);
            //TextLyrics.Text = (PrestoSDK.PrestoService.Player.Position).ToString();
            /*int left = 0, right = timestore.Count;
            int result=0;
            while (left <= right)
            {

                int mid = (left + right) / 2;
                if (timestore[mid] > position)
                {
                    right = mid - 1;    
                }
                else if (timestore[mid] < position)
                {

                    left = mid + 1; 
                }
                

            }*/
            //MessageBox.Show(check.ToString());
            //if (check < 0) return;
           
            if(check<0)
            TextLyrics.Text = map[timestore[Math.Max(0, ~check - 1)]];
            
        }
    }
}
