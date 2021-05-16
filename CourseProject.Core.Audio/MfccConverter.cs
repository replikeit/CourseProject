using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using MathNet.Numerics.IntegralTransforms;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NAudio.Wave;

namespace CourseProject.Core.Audio
{
    static class MfccConverter
    {
        public static double[] frame;        
        public static double[,] frame_mass;      
        public static Complex[,] frame_mass_FFT; 

        static int[] filter_points = {6,18,31,46,63,82,103,127,154,184,218,
                                  257,299,348,402,463,531,608,695,792,901,1023};
        static double[,] H = new double[20, 1024];   

        static float[] MFCC = new float[25000];

        public static Task<float[]> GetMfccAsync(string filePath) => Task.Run(() => GetMfcc(filePath));
        public static Task<float[]> GetMfccAsync(float[] wavPCM) => Task.Run(() => GetMfcc(wavPCM));

        public static float[] GetMfcc(string filePath)
        {
            var wavReader = new WaveFileReader(filePath);
            var samples = new List<float>();
            float[] sample;
            while ((sample = wavReader.ReadNextSampleFrame()) != null)
            {
                samples.AddRange(sample);
            }

            return GetMfcc(samples.ToArray());
        }
        
        public static float[] GetMfcc(float[] wavPCM)
        {
            int count_frames = (wavPCM.Length * 2 / 2048) + 1; 

            RMS_gate(wavPCM);          
            Normalize(wavPCM);         
            frame_mass = Set_Frames(wavPCM);       
            Hamming_window(frame_mass, count_frames);       
            frame_mass_FFT = FFT_frames(frame_mass, count_frames);      


            double[,] MFCC_mass = new double[count_frames, 20];         

            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 1024; j++)
                {
                    if (j < filter_points[i]) H[i, j] = 0;
                    if ((filter_points[i] <= j) & (j <= filter_points[i + 1]))
                        H[i, j] = ((double)(j - filter_points[i]) / (filter_points[i + 1] - filter_points[i]));
                    if ((filter_points[i + 1] <= j) & (j <= filter_points[i + 2]))
                        H[i, j] = ((double)(filter_points[i + 2] - j) / (filter_points[i + 2] - filter_points[i + 1]));
                    if (j > filter_points[i + 2]) H[i, j] = 0;
                }

            for (int k = 0; k < count_frames; k++)
            {
                double[] S = new double[20];
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 1024; j++)
                    {
                        S[i] += Math.Pow(frame_mass_FFT[k, j].Magnitude, 2) * H[i, j];
                    }
                    if (S[i] != 0) S[i] = Math.Log(S[i], Math.E);
                }

                for (int l = 0; l < 20; l++)
                    for (int i = 0; i < 20; i++) MFCC_mass[k, l] += S[i] * Math.Cos(Math.PI * l * ((i * 0.5) / 20));
            }

            for (int i = 0; i < 20; i++)
            {
                for (int k = 0; k < count_frames; k++) MFCC[i] += (float)MFCC_mass[k, i];
                MFCC[i] = MFCC[i] / count_frames;
            }

            return MFCC;
        }

            private static void RMS_gate(float[] wav_PCM)
        {
            int k = 0;
            double[] buf_rms = new double[50];
            double RMS = 0;

            for (int j = 0; j < wav_PCM.Length; j++)
            {
                if (k < 100)
                {
                    RMS += Math.Pow((wav_PCM[j]), 2);
                    k++;
                }
                else
                {
                    if (Math.Sqrt(RMS / 100) < 0.005)
                        for (int i = j - 100; i <= j; i++) wav_PCM[i] = 0;
                    k = 0; RMS = 0;
                }
            }
        }

        private static void Normalize(float[] wav_PCM)
        {
            double[] abs_wav_buf = new double[wav_PCM.Length];
            for (int i = 0; i < wav_PCM.Length; i++)
                if (wav_PCM[i] < 0) abs_wav_buf[i] = -wav_PCM[i];   
                else abs_wav_buf[i] = wav_PCM[i];                   
            double max = abs_wav_buf.Max();
            double k = 1f / max;             

            for (int i = 0; i < wav_PCM.Length; i++)   
            {
                wav_PCM[i] = wav_PCM[i] * (float)k;              
            }
        }

        private static double[,] Set_Frames(float[] wav_PCM)
        {
            double[,] frame_mass_1; 
            int count_frames = 0;       
            int count_samp = 0;

            frame_mass_1 = new double[(wav_PCM.Length*2 / 2048) + 1, 2048];
            for (int j = 0; j < wav_PCM.Length; j++)
            {
                if (j >= 1024)      
                {
                    count_samp++;
                    if (count_samp >= 2049)
                    {
                        count_frames += 2;
                        count_samp = 1;
                    }
                    frame_mass_1[count_frames, count_samp - 1] = wav_PCM[j - 1024];
                    frame_mass_1[count_frames + 1, count_samp - 1] = wav_PCM[j];
                }
            }
            return frame_mass_1;
        }

        private static void Hamming_window(double[,] frames, int count_frames)
        {
            double omega = 2.0 * Math.PI / (2048f);
            for (int i = 0; i < count_frames; i++)
                for (int j = 0; j < 2048; j++)
                    frames[i, j] = (0.54 - 0.46 * Math.Cos(omega * (j))) * frames[i, j];
        }


        private static Complex[,] FFT_frames(double[,] frames, int count_frames)
        {
            Complex[,] frame_mass_complex =
                new Complex[count_frames, 2048]; 
            Complex[] FFT_frame = new Complex[2048];
            for (int k = 0; k < count_frames; k++)
            {
                for (int i = 0; i < 2048; i++) FFT_frame[i] = frames[k, i];
                Fourier.Forward(FFT_frame, FourierOptions.Matlab);
                for (int i = 0; i < 2048; i++) frame_mass_complex[k, i] = FFT_frame[i];
            }
            return frame_mass_complex;
        }
    }
}