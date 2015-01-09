using System;
using System.Threading.Tasks;
using System.Numerics;
using ComplexExtention;

namespace FourierTransformProvider
{
    
    /// <summary>
    /// Code used from psi-logic.narod.ru//fft//fftf.htm
    /// </summary>
    public class FourierTransform
    {
        #region FFT
        private static Complex[] W2n = new Complex[32]{
            new Complex(-1.00000000000000000000000000000000,  0.00000000000000000000000000000000), // W2 calculator (copy/paste) : po, ps
            new Complex(0.00000000000000000000000000000000, -1.00000000000000000000000000000000), // W4: p/2=o, p/2=s
            new Complex(0.70710678118654752440084436210485, -0.70710678118654752440084436210485), // W8: p/4=o, p/4=s
            new Complex(0.92387953251128675612818318939679, -0.38268343236508977172845998403040), // p/8=o, p/8=s
            new Complex( 0.98078528040323044912618223613424, -0.19509032201612826784828486847702), // p/16=
            new Complex( 0.99518472667219688624483695310948, -9.80171403295606019941955638886e-2), // p/32=
            new Complex( 0.99879545620517239271477160475910, -4.90676743274180142549549769426e-2), // p/64=
            new Complex(0.99969881869620422011576564966617, -2.45412285229122880317345294592e-2), // p/128=
            new Complex(0.99992470183914454092164649119638, -1.22715382857199260794082619510e-2), // p/256=
            new Complex(0.99998117528260114265699043772857, -6.13588464915447535964023459037e-3), // p/(2y9)=
            new Complex( 0.99999529380957617151158012570012, -3.06795676296597627014536549091e-3), // p/(2y10)=
            new Complex( 0.99999882345170190992902571017153, -1.53398018628476561230369715026e-3), // p/(2y11)=
            new Complex( 0.99999970586288221916022821773877, -7.66990318742704526938568357948e-4), // p/(2y12)=
            new Complex( 0.99999992646571785114473148070739, -3.83495187571395589072461681181e-4), // p/(2y13)=
            new Complex( 0.99999998161642929380834691540291, -1.91747597310703307439909561989e-4), // p/(2y14)=
            new Complex( 0.99999999540410731289097193313961, -9.58737990959773458705172109764e-5), // p/(2y15)=
            new Complex( 0.99999999885102682756267330779455, -4.79368996030668845490039904946e-5), // p/(2y16)=
            new Complex( 0.99999999971275670684941397221864, -2.39684498084182187291865771650e-5), // p/(2y17)=
            new Complex( 0.99999999992818917670977509588385, -1.19842249050697064215215615969e-5), // p/(2y18)=
            new Complex( 0.99999999998204729417728262414778, -5.99211245264242784287971180889e-6), // p/(2y19)=
            new Complex( 0.99999999999551182354431058417300, -2.99605622633466075045481280835e-6), // p/(2y20)=
            new Complex( 0.99999999999887795588607701655175, -1.49802811316901122885427884615e-6), // p/(2y21)=
            new Complex( 0.99999999999971948897151921479472, -7.49014056584715721130498566730e-7), // p/(2y22)=
            new Complex(0.99999999999992987224287980123973, -3.74507028292384123903169179084e-7), // p/(2y23)=
            new Complex(0.99999999999998246806071995015625, -1.87253514146195344868824576593e-7), 
            new Complex(0.99999999999999561701517998752946, -9.36267570730980827990672866808e-8), 
            new Complex(0.99999999999999890425379499688176, -4.68133785365490926951155181385e-8), 
            new Complex(0.99999999999999972606344874922040, -2.34066892682745527595054934190e-8), 
            new Complex(0.99999999999999993151586218730510, -1.17033446341372771812462135032e-8), 
            new Complex(0.99999999999999998287896554682627, -5.85167231706863869080979010083e-9), 
            new Complex(0.99999999999999999571974138670657, -2.92583615853431935792823046906e-9), 
            new Complex(0.99999999999999999892993534667664, -1.46291807926715968052953216186e-9), 
        };
        /// <summary>
        /// Provide FFT
        /// </summary>
        /// <param name="ComplexData">Actual Data</param>
        /// <param name="Complement">false = direct transform, true = inverse transform</param>
        /// <returns></returns>
        public Complex[] FastFourierTransform(Complex[] ComplexData, bool Complement)
        {
            var data = ComplexData.AsFourierDataSet();//new FourierDataSet(ComplexData);
            data = FastFourierTransform(data, Complement);
            return data.ComplexData;
            

        }
        /// <summary>
        /// Provide FFT
        /// </summary>
        /// <param name="Data">Actual Data</param>
        /// <param name="Complement">false = direct transform, true = inverse transform</param>
        /// <returns></returns>
        public  FourierDataSet FastFourierTransform(FourierDataSet Data, bool Complement)
        {
            int PowerOfTwo = 0;
            int SamplesNumber = Data.Length;
            if (!TransformTools.AssertDataArray(Data, out PowerOfTwo))
                throw new NotSupportedException("Array length is not power of 2");
            //Create WStore before calculation
            var Wstore = CreateWStore(SamplesNumber);
            return FastFourierTransform(Data, PowerOfTwo, Complement, Wstore);
            
        }

        public FourierDataSet FastFourierTransform(FourierDataSet Data, int T, bool Complement, Complex[] Wstore)
        {
            int PowerOfTwo = T;
            int SamplesNumber = Data.Length;
            
            TransformTools.BinaryReverseSwapData(Data, PowerOfTwo);

            int levelN, halfLevelN, k, CurrentLevelSample, CurrentLevelPairSample,  Skew, WStoreIndex;
            Complex  Temp;
            for (levelN = 2, halfLevelN = 1, Skew = SamplesNumber >> 1; levelN <= SamplesNumber; halfLevelN = levelN, levelN += levelN, Skew >>= 1)
            {
                for (WStoreIndex = 0, k = 0; k < halfLevelN; k++, WStoreIndex += Skew)
                {
                    for (CurrentLevelSample = k; CurrentLevelSample < SamplesNumber; CurrentLevelSample += levelN)
                    {
                        Temp = Wstore[WStoreIndex];
                        if (Complement)
                            Temp = new Complex(Temp.Real, -Temp.Imaginary);
                        CurrentLevelPairSample = CurrentLevelSample + halfLevelN;
                        Temp *= Data[CurrentLevelPairSample];
                        Data[CurrentLevelPairSample] = Data[CurrentLevelSample] - Temp;
                        Data[CurrentLevelSample] += Temp;
                    }
                }
            }
            
            return Data;

        }

        #endregion

        #region UniversalFFT
            
        public FourierDataSet UniversalFastFourierTransform(FourierDataSet Data, bool Complement)
        {
            var N = Data.Length;
            Complex[] X_, w,Wstore;

            int T = (int)Math.Floor(Math.Log(N, 2) + 0.5);
            if(1<<T==N)
            {
                Wstore = CreateWStore(N);
                return FastFourierTransform(Data,T, Complement,Wstore);
            }

            //find N', T
            int N2 = N + N;
            int N_;
            double arg;
            for (N_ = 1,T=0; N_ < N2; N_+=N_,T++){}
            //find --2pi/N/2 = Pi/N
            double piN = Math.PI / N;
            if (Complement)
                piN = -piN;

            //find X_[n] = x[n]*e^--2*j*pi*n*n/N/2 = x[n]*e^j*piN*n*n
            X_ = new Complex[N_];
            Complex v;
            int n;
            for (n = 0; n < N; ++n)
            {
                arg = piN * n * n;
                v = Complex.FromPolarCoordinates(1, arg);
                X_[n] = Data[n] * v;
            }

            for (; n < N_; n++)
            {
                X_[n] = new Complex(0, 0);
            }

            //find w[n]

            w = new Complex[N_];
            var N22 = 2 * N - 2;
            for (n = 0; n < N_; ++n,--N22)
            {
                arg = -piN * N22 * N22;
                w[n] = Complex.FromPolarCoordinates(1, arg);
            }
            
            Wstore = CreateWStore(N_);
            //Possible problem with threading
            Parallel.For(0, 2, (i) =>
            {
                
                switch (i)
                {
                        //FFT1
                    case 0:  X_=FastFourierTransform(new FourierDataSet(X_), T, false, Wstore).ComplexData; break;
                        //FFT2
                    case 1:  w=FastFourierTransform(new FourierDataSet(w), T, false, Wstore).ComplexData;  break;
                    default:
                        break;
                }
            });
            //FFT1
            //X_=FastFourierTransform(new FourierDataSet(X_),T, false,Wstore).ComplexData;
            //FFT2
            //w= FastFourierTransform(new FourierDataSet(w), T, false, Wstore).ComplexData;
            //zgortka
            for (n = 0; n < N_; ++n)
            {
                X_[n] *= w[n];
            }
            //FFT2(complement)
            X_ = FastFourierTransform(new FourierDataSet(X_), T, true, Wstore).ComplexData;
            //Find X[n]
            for (n = 0,N22 = 2*N-2; n < N; ++n,--N22)
            {
                arg = piN * n * n;
                v = Complex.FromPolarCoordinates(1, arg);
                v /= N_;
                if (Complement)
                    v /= N;
                Data[n] = X_[N22] * v;
            }

            return Data;
        }

        private Complex[] CreateWStore(int Nmax)
        {
            int N, Skew, Skew2;
            Complex[] Wstore;
            int WarrayInd;
            Complex WN;
            int pWN;

            Skew2 = Nmax >> 1;
            Wstore = new Complex[Skew2];
            Wstore[0] = new Complex(1, 0);

            for (N = 4,pWN = 1,Skew = Skew2>>1; N <= Nmax; N+=N,pWN++,Skew2=Skew,Skew>>=1)
            {
                WN = W2n[pWN];
                for (WarrayInd = 0; WarrayInd < Wstore.Length; WarrayInd+=Skew2)
                {
                    Wstore[WarrayInd + Skew] = Wstore[WarrayInd] * WN;
                }
            }
            return Wstore;
        }
        #endregion

        #region DFT
        private Complex[] m_RotationCoefficientsDFT;

        /// <summary>
        /// Initialize rotary coeffs for DFT
        /// </summary>
        /// <param name="FourierTransformDataLength">length of data to be transformed</param>
        /// <param name="RangeToTransform">range within FourierTransformDataLength for which you need coefficients </param>
        /// <param name="Complement">false = dicerc transform, true = inverse transform</param>
        public void InitDFTRotaryCoeffs(int FourierTransformDataLength, IndexRange RangeToTransform,bool Complement)
        {
            var ComplementCoef = (Complement ? 1 : -1) * 2 * Math.PI / FourierTransformDataLength;
            m_RotationCoefficientsDFT = new Complex[FourierTransformDataLength];// Note: Possibly too long array... Can be resolved farther
            foreach (var k in RangeToTransform)
                m_RotationCoefficientsDFT[k] = Complex.FromPolarCoordinates(1, ComplementCoef * k);//ComplementCoef * 2 * Math.PI * k / FourierTransformDataLength);
        }

        
        /// <summary>
        /// Provide DFT
        /// </summary>
        /// <param name="InputData">Actual Data</param>
        /// <param name="RangeToTransform">Range to calculate FT</param>
        /// <param name="Complement">false = direct transform, true = inverse transform</param>
        /// <returns></returns>
        public Complex[] DiscreteFourierTransform(Complex[] InputData,IndexRange RangeToTransform , bool Complement)
        {

            var data = new FourierDataSet(InputData);
            return DiscreteFourierTransform(data, RangeToTransform, Complement).ComplexData;
            
        }


        /// <summary>
        /// Provide DFT
        /// </summary>
        /// <param name="InputData">Actual Data</param>
        /// <param name="RangeToTransform">Range to calculate FT</param>
        /// <param name="Complement">false = direct transform, true = inverse transform</param>
        /// <returns></returns>
        public FourierDataSet DiscreteFourierTransform(FourierDataSet InputData, IndexRange RangeToTransform, bool Complement)
        {
            var SamplesNumber = InputData.Length;
            var OutputData = new FourierDataSet(SamplesNumber);//,InputData.TransformRange);
            var ComplementCoef = (Complement ? 1 : -1)*2*Math.PI/SamplesNumber;

            Complex W;
            Parallel.ForEach<int>(RangeToTransform, (k) =>
            {
                Parallel.For(0, SamplesNumber, (n) =>
                {
                    if (null != m_RotationCoefficientsDFT)
                    {
                        W = m_RotationCoefficientsDFT[k].Power(n);
                    }
                    else
                        W = Complex.FromPolarCoordinates(1, ComplementCoef * k * n);
                    OutputData[k] += InputData[n] * W;
                });
                if (Complement)
                    OutputData[k] /= SamplesNumber;
            });
            
            return OutputData;
        }
        public Complex[] ForwardDFT(double[] DataArray, IndexRange RangeToTransform)
        {
            Complex[] data = new Complex[DataArray.Length];
            for (int i = 0; i < DataArray.Length; i++)
            {
                data[i] = new Complex(DataArray[i], 0);
            }

            return DiscreteFourierTransform(data, RangeToTransform, false);
        }
        public Complex[] ForwardDFT(Complex[] DataArray, IndexRange RangeToTransform)
        {
            return DiscreteFourierTransform(DataArray,RangeToTransform, false);
        }
        public FourierDataSet ForwardDFT(FourierDataSet DataArray, IndexRange RangeToTransform)
        {
            return DiscreteFourierTransform(DataArray, RangeToTransform, false);
        }

        public Complex[] InverseDFT(double[] DataArray, IndexRange RangeToTransform)
        {
            Complex[] data = new Complex[DataArray.Length];
            for (int i = 0; i < DataArray.Length; i++)
            {
                data[i] = new Complex(DataArray[i], 0);
            }

            return DiscreteFourierTransform(data, RangeToTransform, true);
        }
        public Complex[] InverseDFT(Complex[] DataArray,IndexRange RangeToTransform)
        {
            return DiscreteFourierTransform(DataArray, RangeToTransform, true);
        }
        public FourierDataSet InverseDFT(FourierDataSet DataArray, IndexRange RangeToTransform)
        {
            return DiscreteFourierTransform(DataArray, RangeToTransform, true);
        }

        #endregion
    }
}
