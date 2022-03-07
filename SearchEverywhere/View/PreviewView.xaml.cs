using System;
using System.Windows.Threading;
using Microsoft.Toolkit.Mvvm.Messaging;
using SearchEverywhere.Model;

namespace SearchEverywhere.View;

public partial class PreviewView
{
    private readonly DispatcherTimer timer = new()
    {
        Interval = TimeSpan.FromMilliseconds(500)
    };

    private bool IsMute;

    private bool IsPlaying;

    public PreviewView()
    {
        InitializeComponent();
        timer.Tick += TimerEvent;
        WeakReferenceMessenger.Default.Register<PreviewView, string, string>(this, "PausePlayToken",
            (r, msg) =>
            {
                switch (msg)
                {
                    case "play":
                        if (!IsPlaying)
                        {
                            timer.Start();
                            r.VideoPlayer.Play();
                            IsPlaying = !IsPlaying;
                            PlayBtn.Content = "\ue718";
                        }
                        else
                        {
                            timer.Stop();
                            r.VideoPlayer.Pause();
                            IsPlaying = !IsPlaying;
                            PlayBtn.Content = "\ue6cf";
                        }

                        break;
                    case "stop":
                        r.VideoPlayer.Stop();
                        break;
                }
            });
        WeakReferenceMessenger.Default.Register<PreviewView, VideoSliderModel, string>(this, "JumpToTimeCommand",
            (r, msg) =>
            {
                timer.Stop();
                Console.WriteLine(TimeSpan.FromSeconds(msg.CurrentValue));
                r.VideoPlayer.Position = TimeSpan.FromSeconds(msg.CurrentValue);
                timer.Start();
            });
        VideoPlayer.MediaOpened += (r, e) =>
        {
            IsPlaying = true;
            PlayBtn.Content = "\ue718";
        };
        VideoPlayer.MediaEnded += (r, e) =>
        {
            VideoPlayer.Stop();
            PlayBtn.Content = "\ue6cf";
            IsPlaying = false;
        };
        WeakReferenceMessenger.Default.Register<PreviewView, string, string>(this, "MuteToken", (r, msg) =>
        {
            if (!IsMute)
            {
                VideoPlayer.Volume = 0;
                MuteBtn.Content = "\ue766";
                IsMute = true;
            }
            else
            {
                VideoPlayer.Volume = 100;
                MuteBtn.Content = "\ue6e3";
                IsMute = false;
            }
        });
    }


    private void TimerEvent(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new CurrentTimeModel(VideoPlayer.Position,
            VideoPlayer.NaturalDuration.TimeSpan), "ChangeVideoTimeToken");
    }
}