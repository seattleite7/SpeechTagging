using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    public class Viterbi
    {
        //Φ[t](i) = argmax[j](δ[t-1](j) * a[ji])
        //P(X at time t) = max[for each i = prevstate] (P(i at time t - 1) * P(X|i) * P(observation at time t|X)
        // A = transition matrix
        // B = emission matrix, B[ij] = prob of observing o[j] from state s[i]
        /*N=length(O); # number of observation categories
 K=length(S); # number of hidden states
 T=length(Y); # length of observation series
 */

         public static List<WordType> DoViterbi(List<string> sentence,  Dictionary<StateTransition, double> transitionProbs, Dictionary<ObservationFromState, double> observationProbs, Dictionary<WordType, double> initialProbs  )
         {
             const int NUM_OBSERVATION_CATS_N = (int)WordType.COUNT + 1;
            const int NUM_POSSIBLE_STATES_K = NUM_OBSERVATION_CATS_N;

            int WORKING_LEN_T = sentence.Count + 1;

             double[,] T1 = new double[NUM_POSSIBLE_STATES_K, WORKING_LEN_T];
             int[,] T2 = new int[NUM_POSSIBLE_STATES_K, WORKING_LEN_T];

             //TODO: Initial probablilities?
             double INITIAL_PROB_TODO = 0.1;

             for (int i = 0; i < NUM_POSSIBLE_STATES_K; i++)
             {
                 T1[i, 1] = initialProbs[(WordType)i] ;
                 T2[i, 1] = 0;
             }
             for (int i = 2; i <= WORKING_LEN_T; i++)
             {
                 foreach (WordType sj in Enum.GetValues(typeof(WordType)))
                 {
                     int j = (int)sj;
                    int maxK2 = int.MinValue;
                    double maxKVal1 = double.MinValue;
                    double maxKVal2 = double.MinValue;

                    StateTransition stateTrans = new SpeechTagging.StateTransition(WordType.Undefined, (WordType)(i - 1));
                    ObservationFromState observationTrans = new ObservationFromState(sentence[i - 1], sj);


                     for (int k = 0; k < NUM_POSSIBLE_STATES_K; k++)
                     {
                         stateTrans.from = (WordType)k;
                        double val = T1[k, i - 1] * transitionProbs[stateTrans] * observationProbs[observationTrans];
                        if (val > maxKVal1) { maxKVal1 = val; }
                        val = T1[k, i - 1] * transitionProbs[stateTrans];
                        if (val > maxKVal2) { maxKVal2 = val;  maxK2 = k; }

                     }

                    T1[j, i] = maxKVal1;
                    T2[j, i] = maxK2;

                 }
             }

            int maxZT = int.MinValue;
            double maxZTval = double.MinValue;
            StateTransition stateTrans = new SpeechTagging.StateTransition(WordType.Undefined, )
            for (int k = 0; k < NUM_POSSIBLE_STATES_K; k++)
            {

            }

            for (int i = WORKING_LEN_T; i >= 2; i--)
            {
               
            }

         }


     }
 }
