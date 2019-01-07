using NAudio.MediaFoundation;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AudiobookBuilder.Objects
{
    public delegate void AudioConverterEventHandler(object source, ConvertEventArgs e);

    public class AudioConverter
    {
        public AudioConverter()
        {            
        }

        private CancellationTokenSource _tokenSource;

        public void Abort()
        {
            _tokenSource.Cancel(true);
            OnConvert?.Invoke(this, new ConvertEventArgs(ConvertEventType.Aborting, Properties.Resources.Convert_Aborting));
        }

        public void Convert(ICollection<InputFileItem> fileItems)
        {
            _tokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                try
                {
                    OnConvert?.Invoke(this, new ConvertEventArgs(ConvertEventType.BeginConvert, $"{Properties.Resources.Convert_Start} {fileItems.Count()} {Properties.Resources.files}"));
                    foreach (var item in fileItems)
                    {
                        if(_tokenSource.IsCancellationRequested)
                        {
                            _tokenSource.Token.ThrowIfCancellationRequested();
                        }
                        
                        if (!item.Path.EndsWith("m4a") && !item.Path.EndsWith(".m4b"))
                        {
                            OnConvert?.Invoke(this, new ConvertEventArgs(ConvertEventType.ConvertToAAC, $"{Properties.Resources.Converting}: {item.FileName}"));
                            using (var filestream = new MediaFoundationReader(item.Path))
                            {
                                item.WorkingPath = Path.GetTempFileName();
                                MediaFoundationEncoder.EncodeToAac(filestream, item.WorkingPath);                                
                            }
                        }



                    }
                    OnConvert?.Invoke(this, new ConvertEventArgs(ConvertEventType.EndConvert));
                }
                catch(OperationCanceledException oe)
                {
                    OnConvert?.Invoke(this, new ConvertEventArgs(ConvertEventType.Cancelled,oe.Message));
                }
                catch (Exception e)
                {
                    OnConvert?.Invoke(this, new ConvertEventArgs(ConvertEventType.Error, e.Message));
                }
                finally
                {
                    foreach(var item in fileItems)
                    {
                        File.Delete(item.WorkingPath);
                    }
                }
            }, _tokenSource.Token);
        }        

        public event AudioConverterEventHandler OnConvert;
    }
}
