using ATL;
using NAudio.MediaFoundation;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AudiobookBuilder.Objects
{
    public class AudioConverter
    {
        public static FileInfo Convert(DirectoryInfo folder)
        {
            var fileItems = folder.GetFiles("*.mp3").Select(s => new FileItem(s.FullName)).ToArray();
            if(fileItems?.Any()==false)
            {
                return null;
            }


            var mp3Merged = Path.GetTempFileName();
            var aacMerged = mp3Merged + ".m4a";

            try
            {


                using (var str = new FileStream(mp3Merged, FileMode.OpenOrCreate))
                {
                    foreach (var file in fileItems)
                    {
                        Mp3FileReader reader = new Mp3FileReader(file.Path);
                        if ((str.Position == 0) && (reader.Id3v2Tag != null))
                        {
                            str.Write(reader.Id3v2Tag.RawData, 0, reader.Id3v2Tag.RawData.Length);
                        }
                        Mp3Frame frame;
                        while ((frame = reader.ReadNextFrame()) != null)
                        {
                            str.Write(frame.RawData, 0, frame.RawData.Length);
                        }
                    }
                }

                using (var filestream = new MediaFoundationReader(mp3Merged))
                {
                    MediaFoundationEncoder.EncodeToAac(filestream, aacMerged);
                }


                var first = fileItems.First().TrackInfo;

                var finallyMerged =  $"{folder.FullName}{first.Artist} - {first.Album}.m4b";
                
                foreach(var c in Path.GetInvalidFileNameChars())
                {
                    finallyMerged = finallyMerged.Replace(c, '_');
                }

                File.Move(aacMerged, finallyMerged);
                Settings.MP4_createNeroChapters = true;
                Settings.MP4_createQuicktimeChapters = true;

                var aacTrack = new Track(finallyMerged);



                aacTrack.Album = first.Album;
                aacTrack.AlbumArtist = first.AlbumArtist;
                aacTrack.Artist = first.Artist;
                aacTrack.Date = first.Date;
                aacTrack.Title = first.Album;
                foreach (var pToken in first.PictureTokens)
                {
                    aacTrack.PictureTokens.Add(pToken);
                }

                foreach (var picutre in first.EmbeddedPictures)
                {
                    aacTrack.EmbeddedPictures.Add(picutre);
                }

                var timemarker = new TimeSpan();

                foreach (var fileItem in fileItems)
                {
                    var chapter = new ChapterInfo
                    {
                        Title = fileItem.TrackInfo.Title,
                        StartTime = (uint)timemarker.TotalMilliseconds,
                        StartOffset = (uint)timemarker.TotalMilliseconds
                    };
                    timemarker += TimeSpan.FromMilliseconds(fileItem.TrackInfo.DurationMs);
                    chapter.EndTime = (uint)timemarker.TotalMilliseconds;
                    chapter.EndOffset = (uint)timemarker.TotalMilliseconds;
                    aacTrack.Chapters.Add(chapter);
                }

                aacTrack.Save();
                return new FileInfo(finallyMerged);
            }
            finally
            {
                File.Delete(mp3Merged);
            }




        }         
    }

    
}
