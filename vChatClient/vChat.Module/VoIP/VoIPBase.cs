using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using System.Threading;
using System.IO;

namespace vChat.Module.VoIP
{
    public class VoIPBase : IDisposable
    {
        public delegate void RecordEventHandler(object sender, RecordEventArgs e);
        public event RecordEventHandler OnRecording;
        private WaveInEvent waveInEvent;
        private IWavePlayer waveOut;
        private BufferedWaveProvider waveProvider;
        private UncompressedPcmCodec pcmCodec;

        public VoIPBase()
        {
            pcmCodec = new UncompressedPcmCodec();

            waveInEvent = new WaveInEvent();
            waveInEvent.BufferMilliseconds = 100;
            waveInEvent.WaveFormat = pcmCodec.RecordFormat;
            waveInEvent.DataAvailable += new EventHandler<WaveInEventArgs>(waveInEvent_DataAvailable);
            waveInEvent.StartRecording();

            waveProvider = new BufferedWaveProvider(pcmCodec.RecordFormat);
            waveProvider.BufferDuration = TimeSpan.FromSeconds(5);            

            waveOut = new WaveOut();
            waveOut.Init(waveProvider);
            waveOut.Play();
        }

        private void waveInEvent_DataAvailable(object sender, WaveInEventArgs e)
        {
            if(OnRecording != null)
                OnRecording(this, new RecordEventArgs(e.Buffer, e.BytesRecorded));
        }

        public void AsyncPlaying(byte[] buffer, int offset, int count)
        {
            try
            {
                waveProvider.AddSamples(buffer, offset, count);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(500); //regain the buffer not lead to "buffer full" error                
            }
        }

        public void Dispose()
        {
            waveInEvent.DataAvailable -= waveInEvent_DataAvailable;
            waveInEvent.StopRecording();
            waveOut.Stop();

            waveInEvent.Dispose();
            waveOut.Dispose();           
        }
    }
}
