using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Browser.EDF
{
    /*
    public class Edf
    {
        private string m_file;
        private EdfLibHdr m_hdr;
        private Dictionary<string, double[]> m_data_dict;
        private bool m_opened = false;

        public Edf(string file)
        {
            m_file = file;
        }

        /// <summary>
        /// Close the edf.
        /// </summary>
        public bool edf_close()
        {
            int flag = 0;
            if (PInvoke.IsUsed(m_file) == 1)
            {
                flag = PInvoke.edfclose_file(m_hdr.handle);
            }

            return flag == 0;
        }

        /// <summary>
        /// Open the edf.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public bool open_edf(string file = null)
        {
            if (file != null) m_file = file;

            // Throwing exception if the file does not exist. 
            if (!File.Exists(m_file))
            {
                throw new FileNotFoundException("The edf file does not exist!");
            }

            m_hdr = new EdfLibHdr();
            IntPtr hdr_ptr = IntPtr.Zero;

            hdr_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(m_hdr));
            Marshal.StructureToPtr(m_hdr, hdr_ptr, true);

            int tags = PInvoke.edfopen_file_readonly(m_file, hdr_ptr, EdfLibConstants.EDFLIB_DO_NOT_READ_ANNOTATIONS);
            m_hdr = Marshal.PtrToStructure<EdfLibHdr>(hdr_ptr);

            // Free the pointer
            if (hdr_ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(hdr_ptr);
            }

            // 打卡失败
            if (tags < 0)
            {
                switch (m_hdr.filetype)
                {
                    //
                }

                m_opened = false;
                return false;
            }

            m_opened = true;
            return true;
        }

        /// <summary>
        /// Read data.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool read_phys_data(int count)
        {
            // Open the file before reading when the file isn`t opened.
            if (!m_opened)
            {
                if (!open_edf()) return false;
            }

            m_data_dict = new Dictionary<string, double[]>();

            int batch_size = 16384;

            int max_cnt = m_hdr.signalparam.Length;
            if (count <= 0)
            {
                throw new ArgumentException("Incoming effective value");
            }

            if (count > max_cnt)
            {
                throw new ArgumentOutOfRangeException($"The count argument is out of range. The maximum value is {max_cnt}");
            }

            // 遍历
            for (int i = 0; i < count; i++)
            {
                string label = m_hdr.signalparam[i].label;
                long total_samples = m_hdr.signalparam[i].smp_in_datarecord * m_hdr.datarecords_in_file;
                double[] bufs = new double[total_samples];

                for (int offset = 0; offset < total_samples; offset += batch_size)
                {
                    long samples_to_read = Math.Min(batch_size, total_samples - offset);
                    double[] buf = new double[(int)samples_to_read];

                    //edflib_func.edfseek(m_hdr.handle, i, offset, EdfLibConstants.EDFSEEK_SET);

                    int x = PInvoke.edfread_physical_samples(m_hdr.handle, i, (int)samples_to_read, buf);
                    if (x == -1)
                    {
                        return false;
                    }
                    else
                    {
                        Array.Copy(buf, 0, bufs, offset, samples_to_read);
                    }
                }

                // Remove redundance the spaces
                label = label.TrimEnd(' ');
                m_data_dict[label] = bufs;
            }

            return true;
        }

        public Dictionary<string, int[]> read_all_raw_data()
        {
            // Open the file before reading when the file isn`t opened.
            if (!m_opened)
            {
                if (!open_edf()) return null;
            }

            var data_dict = new Dictionary<string, int[]>();

            // Allocate memory for the buffer
            IntPtr[] buf = new IntPtr[m_hdr.edfsignals];
            for (int i = 0; i < m_hdr.edfsignals; i++)
            {
                long smpl_nums = m_hdr.signalparam[i].smp_in_datarecord * m_hdr.datarecords_in_file;
                buf[i] = Marshal.AllocHGlobal((int)smpl_nums * sizeof(int));
            }

            int x = PInvoke.edfread_all_digital_samples(m_hdr.handle, buf);
            if (x == -1)
            {
                return null;
            }

            // Copy the data to the dictionary
            for (int i = 0; i < m_hdr.edfsignals; i++)
            {
                string label = m_hdr.signalparam[i].label.TrimEnd(' ');
                long smpl_nums = m_hdr.signalparam[i].smp_in_datarecord * m_hdr.datarecords_in_file;
                int[] bufs = new int[smpl_nums];

                Marshal.Copy(buf[i], bufs, 0, (int)smpl_nums);
                data_dict[label] = bufs;
            }

            // 使用完毕后记得释放分配的内存，防止内存泄漏
            for (int j = 0; j < m_hdr.edfsignals; j++)
            {
                Marshal.FreeHGlobal(buf[j]);
            }

            return data_dict;
        }

        public Dictionary<string, int[]> read_raw_data(int count)
        {
            // Open the file before reading when the file isn`t opened.
            if (!m_opened)
            {
                if (!open_edf()) return null;
            }

            var data_dict = new Dictionary<string, int[]>();

            int batch_size = 16384;

            int max_cnt = m_hdr.edfsignals;
            if (count <= 0)
            {
                throw new ArgumentException("Incoming effective value");
            }

            if (count > max_cnt)
            {
                throw new ArgumentOutOfRangeException($"The count argument is out of range. The maximum value is {max_cnt}");
            }

            // 遍历
            for (int i = 0; i < count; i++)
            {
                string label = m_hdr.signalparam[i].label;
                long total_samples = m_hdr.signalparam[i].smp_in_datarecord * m_hdr.datarecords_in_file;
                int[] bufs = new int[total_samples];

                for (int offset = 0; offset < total_samples; offset += batch_size)
                {
                    long samples_to_read = Math.Min(batch_size, total_samples - offset);
                    int[] buf = new int[(int)samples_to_read];

                    //edflib_func.edfseek(m_hdr.handle, i, offset, EdfLibConstants.EDFSEEK_SET);

                    int x = PInvoke.edfread_digital_samples(m_hdr.handle, i, (int)samples_to_read, buf);
                    if (x == -1)
                    {
                        return null;
                    }
                    else
                    {
                        Array.Copy(buf, 0, bufs, offset, samples_to_read);
                    }
                }

                // Remove redundance the spaces
                label = label.TrimEnd(' ');
                data_dict[label] = bufs;
            }

            return data_dict;
        }

        /// <summary>
        /// Retrieve key
        /// </summary>
        /// <param name="lead1"></param>
        /// <param name="lead2"></param>
        /// <returns></returns>
        public (double[] signal1, double[] signal2) retrieve_leads(
            string lead1, string lead2) => (retrieve(lead1), retrieve(lead2));

        /// <summary>
        /// Retrieve the specified key value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private double[] retrieve(string key)
         => m_data_dict.TryGetValue(key, out double[] signal) ? signal : null;
    }

    */

    public class Edf : IDisposable
    {
        private string m_file;
        private EdfLibHdr m_edfLibHdr;

        public Edf(string file)
        {
            if (file == null && !File.Exists(file)) throw new FileNotFoundException("File not found");
            m_file = file;
        }

        public EdfLibHdr EdfLibHdr => m_edfLibHdr;

        public bool Open()
        {
            bool opened = false;

            // Open the file
            if (PInvoke.IsUsed(m_file) == 1)
            {
                opened = true;
            }
            else
            {
                m_edfLibHdr = new EdfLibHdr();
                IntPtr edfhdr = Marshal.AllocHGlobal(Marshal.SizeOf(m_edfLibHdr));
                Marshal.StructureToPtr(m_edfLibHdr, edfhdr, true);

                int result = PInvoke.OpenFile(m_file, edfhdr, EdfLibConstants.EDFLIB_DO_NOT_READ_ANNOTATIONS);
                if (result < 0)
                {
                    opened = false;
                }
                else
                {
                    // Convert the pointer to the structure
                    m_edfLibHdr = Marshal.PtrToStructure<EdfLibHdr>(edfhdr);
                }

                // Free the pointer
                if (edfhdr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(edfhdr);
                }
            }

            return opened;
        }

        public void Close() => Dispose();

        public void Dispose()
        {
            // Close the file if it is opened
            if (Open())
            {
                int result = PInvoke.CloseFile(m_edfLibHdr.handle);

                // 检查返回值，如果失败则抛出异常或记录日志
                if (result != 0) // 假设0表示成功，具体值依据你的 API 文档来判断
                {
                    throw new InvalidOperationException($"Failed to close file. Error code: {result}");
                }
            }
        }
    }
}


