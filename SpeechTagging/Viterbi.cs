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

        /// <summary>
        /// Does the Viberti algorithm, giving the most likely parts of speech for a sentence.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="transitionProbs">The probability of transitioning from one part of speech to another (probability: 0-1)</param>
        /// <param name="observationProbs">The probability of observing a word given a part of speech: (probability: 0-1) (i.e. if the part of speech is "Noun", what is the probability that word is "Dog" or "Cat" ? )</param>
        /// <param name="initialProbs">The probability that a part of speech appears at the beginning of a sentence (probability: 0-1)</param>
        /// <returns>Returns the 1-based (NOT 0-Based) array of the parts of speech</returns>
        public static WordType[] DoViterbi(List<string> sentence,  Dictionary<StateTransition, double> transitionProbs, Dictionary<ObservationFromState, double> observationProbs, Dictionary<WordType, double> initialProbs  )
         {
              const int NUM_POSSIBLE_STATES_K = (int)WordType.COUNT;

            int WORKING_LEN_T = sentence.Count;

            //1-based
             double[,] T1 = new double[NUM_POSSIBLE_STATES_K + 1, WORKING_LEN_T + 1];
             int[,] T2 = new int[NUM_POSSIBLE_STATES_K + 1, WORKING_LEN_T + 1];
            int[] Z = new int[WORKING_LEN_T + 1];
            WordType[] X = new WordType[WORKING_LEN_T + 1];

         
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

            double maxKTval = double.MinValue;
           
            for (int k = 0; k < NUM_POSSIBLE_STATES_K; k++)
            {
                if (T1[k, WORKING_LEN_T] > maxKTval) { maxKTval = T1[k, WORKING_LEN_T]; Z[WORKING_LEN_T] = k; }
            }
            WordType XT = (WordType)Z[WORKING_LEN_T];
            for (int i = WORKING_LEN_T; i >= 2; i--)
            {
                Z[i - 1] = T2[Z[i], i];
                X[i - 1] = (WordType)Z[i - 1];
            }

            return X;
         }


     }
 }
