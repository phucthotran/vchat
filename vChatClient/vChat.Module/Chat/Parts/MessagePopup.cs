using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Collections;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Input;
using System.IO;
using System.Reflection;
using NAudio.Wave;

namespace vChat.Module.Chat.Parts
{
    public class MessagePopup
    {
        private Task _taskPopup;

        public static MessagePopup Instance = new MessagePopup();

        private LinkedList<Popup> _popups = new LinkedList<Popup>();
        private Dictionary<string, WaveStream> _sound = new Dictionary<string, WaveStream>();

        private double _latestHeight = 30;
        IWavePlayer waveOutDevice = new WaveOut();
        WaveStream mainOutputStream;
        WaveChannel32 volumeStream;

        private WaveStream CreateInputStream(string fileName)
        {
            WaveChannel32 inputStream;
            WaveStream mp3Reader = new Mp3FileReader(fileName);
            inputStream = new WaveChannel32(mp3Reader);
            volumeStream = inputStream;
            return volumeStream;
        }

        public static void Display(string content, string sound, Action click)
        {
            if (Instance._taskPopup == null || Instance._taskPopup.IsCompleted)
                Instance._taskPopup = Task.Factory.StartNew(() => { });
            Instance._taskPopup = Instance._taskPopup.ContinueWith(t => Instance.DisplayAsync(new object[] { content, sound, click }), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void DisplayAsync(object data)
        {
            try
            {
                mainOutputStream = CreateInputStream(String.Format("Sounds/{0}.mp3", ((object[])data)[1].ToString()));
                waveOutDevice.Init(mainOutputStream);
                waveOutDevice.Play();
            }
            catch { }
            Popup popup = new Popup();
            popup.Tag = Guid.NewGuid().ToString();
            TextBlock textblock = new TextBlock();
            textblock.Text = ((object[])data)[0].ToString();
            textblock.Padding = new Thickness(10, 5, 10, 5);
            textblock.Background = Brushes.White;
            textblock.Foreground = Brushes.Black;
            textblock.Cursor = Cursors.Hand;
            textblock.FontSize = 14;
            textblock.MouseEnter += new MouseEventHandler(delegate
            {
                textblock.TextDecorations = TextDecorations.Underline;
                textblock.Foreground = Brushes.DarkBlue;
            });
            textblock.MouseLeave += new MouseEventHandler(delegate
            {
                textblock.TextDecorations = TextDecorations.Baseline;
                textblock.Foreground = Brushes.Black;
            });
            textblock.Tag = null;
            textblock.MouseDown += new System.Windows.Input.MouseButtonEventHandler(delegate
            {
                if (textblock.Tag == null)
                {
                    if (((object[])data)[2] != null)
                        ((Action)((object[])data)[2]).Invoke();
                    textblock.Tag = "...";
                }
            });
            textblock.Width = 200;
            textblock.Height = 30;
            Border border = new Border();
            border.Child = textblock;
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#3b5998");
            popup.Child = border;
            _popups.AddLast(popup);
            popup.AllowsTransparency = true;
            popup.PopupAnimation = PopupAnimation.Fade;
            popup.Placement = PlacementMode.Custom;
            popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(delegate
            {
                Point point = SystemParameters.WorkArea.BottomRight;
                textblock.Dispatcher.Invoke((Action)(() =>
                {
                    point.X -= textblock.Width;
                    point.Y -= _latestHeight;
                }));
                CustomPopupPlacement placement = new CustomPopupPlacement(point, PopupPrimaryAxis.Horizontal);
                return new CustomPopupPlacement[] { placement };
            });
            popup.Opened += delegate
            {
                _latestHeight += 30;
            };
            popup.IsOpen = true;
            DispatcherTimer time = new DispatcherTimer();
            time.Interval = TimeSpan.FromSeconds(3);
            time.Start();
            time.Tick += delegate
            {
                popup.Closed += delegate
                {
                    time.Stop();
                    LinkedListNode<Popup> p = _popups.Find(popup);
                    int index = 0;
                    while (p.Next != null)
                    {
                        p.Next.Value.VerticalOffset = 0;
                        p.Next.Value.VerticalOffset = 30 * (_popups.Count - index);
                        p = p.Next;
                        index++;
                    }
                    _popups.RemoveFirst();
                    _latestHeight -= 30;
                };
                popup.IsOpen = false;
            };
        }
    }
}
