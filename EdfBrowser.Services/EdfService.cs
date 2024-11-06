using Browser.EDF;
using EdfBrowser.Models;
using System;
using System.Collections.Generic;


namespace EdfBrowser.Services
{
    public class EdfService : IEdfService
    {
        public EdfInfo ParseEdf(string filePath)
        {
            EdfInfo edfInfo = new EdfInfo();

            using (Edf edf = new Edf(filePath))
            {
                edf.Open();

                edfInfo.FilePath = filePath;
                EdfLibHdr hdr = edf.EdfLibHdr;
                edfInfo.Subject = hdr.patient;
                edfInfo.Recording = hdr.recording;

                DateTime startDateTime = new DateTime(hdr.startdate_year, hdr.startdate_month, hdr.startdate_day, hdr.starttime_hour, hdr.starttime_minute, hdr.starttime_second);
                edfInfo.StartDateTime = startDateTime.ToString("yyyy-MMM-dd HH:mm:ss");

                long unit = hdr.datarecord_duration / EdfLibConstants.EDFLIB_TIME_DIMENSION;
                long numSamples = hdr.datarecords_in_file;
                int second = (int)(numSamples / unit);
                TimeSpan ts = new TimeSpan(0, 0, second);

                DateTime endDateTime = startDateTime + ts;
                edfInfo.EndDateTime = endDateTime.ToString("yyyy-MMM-dd HH:mm:ss");

                edfInfo.Duration = ts.ToString();


                Dictionary<string, int> signalInfo = new Dictionary<string, int>();
                for (int i = 0; i < hdr.edfsignals; i++)
                {
                    signalInfo[hdr.signalparam[i].label] = hdr.signalparam[i].smp_in_datarecord;
                }

                edfInfo.SignalInfo = signalInfo;
            }

            return edfInfo;
        }
    }
}
