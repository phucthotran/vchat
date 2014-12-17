using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using System.Threading;
using System.IO;

namespace vChat.Module.VoIP
{
    /// <summary>
    /// Class nền tảng dùng cho ghi âm
    /// </summary>
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
            //Gửi các dữ liệu ghi âm được cho đối tượng thực thi class này
            if(OnRecording != null)
                OnRecording(this, new RecordEventArgs(e.Buffer, e.BytesRecorded));
        }

        /// <summary>
        /// Thực hiện phát một đoạn dữ liệu âm thành (Quá trình này bất đồng bộ so với quá trình ghi âm)
        /// </summary>
        /// <param name="buffer">Dữ liệu âm thanh</param>
        /// <param name="offset">Vị trí bắt đầu của dữ liệu</param>
        /// <param name="count">Vị trí kết thúc của dữ liệu</param>
        public void AsyncPlaying(byte[] buffer, int offset, int count)
        {
            try
            {
                //Thực hiện đưa dữ liệu "buffer" vào cho "waveProvier",
                //lúc nãy "waveOut" vẫn đang hoạt động và nhận dữ liệu trên "waveProvier" để tiếp tục phát âm thanh ra bên ngoài

                waveProvider.AddSamples(buffer, offset, count);
            }
            catch (InvalidOperationException) //Bắt lỗi trường hợp bị "buffer full"
            {
                Thread.Sleep(500); //Tạm dừng công việc 0.5s để "waveProvider" có thể phục hồi lại buffer, tránh tình trạng bị "buffer full"
            }
        }

        /// <summary>
        /// Hủy đối tượng
        /// </summary>
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
