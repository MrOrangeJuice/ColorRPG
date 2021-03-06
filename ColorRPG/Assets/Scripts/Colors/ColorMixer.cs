using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorMixer
{
    //Data and method for mixing reflectance curves from this blog post: http://scottburns.us/reflectance-curves-from-srgb/
    //Sorry for the formatting i figured out a better way to do this halfway through
    static float[,] B12 = new float[,] { { 0.0933f, - 0.1729f, 1.0796f }, { 0.0933f, - 0.1728f, 1.0796f, }, { 0.0932f, - 0.1725f, 1.0794f }, { 0.0927f, - 0.1710f, 1.0783f }, { 0.0910f, - 0.1654f, 1.0744f }, { 0.0854f, - 0.1469f, 1.0615f }, { 0.0723f, - 0.1031f, 1.0308f }, { 0.0487f, - 0.0223f, 0.9736f }, { 0.0147f,  0.0980f,  0.8873f }, { -0.0264f, 0.2513f, 0.7751f }, { -0.0693f, 0.4234f,  0.6459f }, { -0.1080f,   0.5983f,  0.5097f }, { -0.1374f, 0.7625f,  0.3749f }, { -0.1517f, 0.9032f, 0.2486f }, { -0.1437f, 1.0056f,  0.1381f },
    { -0.1080f, 1.0581f,  0.0499f},
    {-0.0424f, 1.0546f,  -0.0122f},
    {0.0501f,  0.9985f,  -0.0487f},
    {0.1641f,  0.8972f,  -0.0613f},
    {0.2912f,  0.7635f,  -0.0547f},
    {0.4217f,  0.6129f,  -0.0346f},
    {0.5455f,  0.4616f,  -0.0071f},
    {0.6545f,  0.3238f,  0.0217f },
    {0.7421f,  0.2105f,  0.0474f },
    {0.8064f,  0.1262f,  0.0675f },
    {0.8494f,  0.0692f,  0.0814f },
    {0.8765f,  0.0330f,  0.0905f },
    {0.8922f,  0.0121f,  0.0957f },
    {0.9007f,  0.0006f,  0.0987f },
    {0.9052f,  -0.0053f, 0.1002f },
    {0.9073f,  -0.0082f, 0.1009f },
    {0.9083f,  -0.0096f, 0.1012f },
    {0.9088f,  -0.0102f, 0.1014f },
    {0.9090f,  -0.0105f, 0.1015f },
    {0.9091f,  -0.0106f, 0.1015f },
    {0.9091f,  -0.0107f, 0.1015f }
    };

    static float[] rho_r = new float[] { 0.021592459f, 0.020293111f, 0.021807906f, 0.023803297f, 0.025208132f, 0.025414957f, 0.024621282f, 0.020973705f, 0.015752802f, 0.01116804f, 0.008578277f, 0.006581877f, 0.005171723f, 0.004545205f, 0.00414512f, 0.004343112f, 0.005238155f, 0.007251939f, 0.012543656f, 0.028067132f, 0.091342277f, 0.484081092f, 0.870378324f, 0.939513128f, 0.960926994f, 0.968623763f, 0.971263883f, 0.972285819f, 0.971898742f, 0.972691859f, 0.971734812f, 0.97234454f, 0.97150339f, 0.970857997f, 0.970553866f, 0.969671404f };
    static float[] rho_g = new float[] { 0.010542406f, 0.010878976f, 0.011063512f, 0.010736566f, 0.011681813f, 0.012434719f, 0.014986907f, 0.020100392f, 0.030356263f, 0.063388962f, 0.173423837f, 0.568321142f, 0.827791998f, 0.916560468f, 0.952002841f, 0.964096452f, 0.970590861f, 0.972502542f, 0.969148203f, 0.955344651f, 0.892637233f, 0.5003641f, 0.116236717f, 0.047951391f, 0.027873526f, 0.020057963f, 0.017382174f, 0.015429109f, 0.01543808f, 0.014546826f, 0.015197773f, 0.014285896f, 0.015069123f, 0.015506263f, 0.015545797f, 0.016302839f };
    static float[] rho_b = new float[] { 0.967865135f, 0.968827912f, 0.967128582f, 0.965460137f, 0.963110055f, 0.962150324f, 0.960391811f, 0.958925903f, 0.953890935f, 0.925442998f, 0.817997886f, 0.42509696f, 0.167036273f, 0.078894327f, 0.043852038f, 0.031560435f, 0.024170984f, 0.020245519f, 0.01830814f, 0.016588218f, 0.01602049f, 0.015554808f, 0.013384959f, 0.012535491f, 0.011199484f, 0.011318274f, 0.011353953f, 0.012285073f, 0.012663188f, 0.012761325f, 0.013067426f, 0.013369566f, 0.013427487f, 0.01363574f, 0.013893597f, 0.014025757f };

    static float[,] T = new float[,]
    {
        { 5.47813E-05f, 0.000184722f, 0.000935514f, 0.003096265f, 0.009507714f, 0.017351596f, 0.022073595f, 0.016353161f, 0.002002407f, -0.016177731f,    -0.033929391f,    -0.046158952f,    -0.06381706f, -0.083911194f,    -0.091832385f,    -0.08258148f, -0.052950086f,    -0.012727224f,    0.037413037f, 0.091701812f, 0.147964686f, 0.181542886f, 0.210684154f, 0.210058081f, 0.181312094f, 0.132064724f, 0.093723787f, 0.057159281f, 0.033469657f, 0.018235464f, 0.009298756f, 0.004023687f, 0.002068643f, 0.00109484f,  0.000454231f, 0.000255925f },
        {-4.65552E-05f,    -0.000157894f,    -0.000806935f,    -0.002707449f,    -0.008477628f,    -0.016058258f,    -0.02200529f, -0.020027434f,    -0.011137726f,    0.003784809f, 0.022138944f, 0.038965605f, 0.063361718f, 0.095981626f, 0.126280277f, 0.148575844f, 0.149044804f, 0.14239936f,  0.122084916f, 0.09544734f,  0.067421931f, 0.035691251f, 0.01313278f,  -0.002384996f,    -0.009409573f,    -0.009888983f,    -0.008379513f,    -0.005606153f,    -0.003444663f,    -0.001921041f,    -0.000995333f,    -0.000435322f,    -0.000224537f,    -0.000118838f,    -4.93038E-05f,    -2.77789E-05f },
        { 0.00032594f,  0.001107914f, 0.005677477f, 0.01918448f,  0.060978641f, 0.121348231f, 0.184875618f, 0.208804428f, 0.197318551f, 0.147233899f, 0.091819086f, 0.046485543f, 0.022982618f, 0.00665036f,  -0.005816014f,    -0.012450334f,    -0.015524259f,    -0.016712927f,    -0.01570093f, -0.013647887f,    -0.011317812f,    -0.008077223f,    -0.005863171f,    -0.003943485f,    -0.002490472f,    -0.001440876f,   -0.000852895f,    -0.000458929f,    -0.000248389f,    -0.000129773f,    -6.41985E-05f,    -2.71982E-05f,    -1.38913E-05f,    -7.35203E-06f,    -3.05024E-06f,    -1.71858E-06f }
    };
    
   

    static private float[,] MultiplyMatrices(float[,] a, float[,] b)
    {
        float[,] result = new float[a.GetLength(0), b.GetLength(1)];
        for (int i = 0; i < a.GetLength(0); i++)
        {
            for (int j = 0; j < b.GetLength(1); j++)
            {
                for (int k = 0; k < a.GetLength(1); k++)
                {
                    result[i, j] += a[i, k] * b[k, j];
                }
            }
        }

        return result;
    }

    static private void PrintMatrix(float[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                row += matrix[i, j] + " ";
            }
            Debug.Log(row);
        }
    }

    static private float[] CalculateReflectance(Color c)
    {
        float[] r = new float[rho_r.Length];
        for(int i = 0; i < r.Length; i++)
        {
            r[i] = c.r * rho_r[i] + c.g * rho_g[i] + c.b * rho_b[i];
        }
        return r;
    }

    static private float[,] RGBFromReflectance(float[] reflectance)
    {
        float[,] reflectanceVector = new float[reflectance.Length,1];
        //make reflectance multidimensional
        for(int i = 0; i < reflectance.Length; i++)
        {
            reflectanceVector[i, 0] = reflectance[i];
        }
        return MultiplyMatrices(T, reflectanceVector);
    }

    static public Color MixColor(Color a, Color b, float weightA, float weightB)
    {
        
        float[] reflectanceA = CalculateReflectance(a);
        float[] reflectanceB = CalculateReflectance(b);
        float[] mix = new float[reflectanceA.GetLength(0)];
        for (int i = 0; i < reflectanceA.GetLength(0); i++)
        {
            mix[i] = Mathf.Pow(reflectanceA[i], weightA) * Mathf.Pow(reflectanceB[i], weightB);
        }
        float[,] col = RGBFromReflectance(mix);
        return new Color(col[0, 0], col[1, 0], col[2, 0]);

    }

    static public float ColorDistance(Color a, Color b)
    {
        float sqrDist = Mathf.Pow(a.r - b.r, 2) + Mathf.Pow(a.g - b.g, 2) + Mathf.Pow(a.b - b.b, 2);
        return Mathf.Sqrt(sqrDist);
    }

    static public Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }

    
}
