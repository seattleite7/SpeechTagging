using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    class Program
    { 

        static Dictionary<string, Dictionary<string, int>> wordAndNextWords;
        static Dictionary<string, List<WordAndPercentage>> wordAndNextWordProbabilities;
        static Random chaos = new Random();

        static WordAndPercentage getTopWord(string word)
        {

           
            if (!wordAndNextWordProbabilities.ContainsKey(word))
                return null;

            double sample = chaos.NextDouble();
           for (int n = 0; n < wordAndNextWordProbabilities[word].Count; n++)
            {
                sample -= wordAndNextWordProbabilities[word][n].percent;
                if (sample <= 0)
                    return wordAndNextWordProbabilities[word][n];
               
            }
            return null;
        
        }

        /*
        static WordAndPercentage predictNextWord(string word)
        {
            if (wordAndNextWords.ContainsKey(word))
            {
                string topWord = "";
                int times = 0;
                int total = 0;
                foreach(KeyValuePair<string, int> pair in wordAndNextWords[word])
                {
                    if(pair.Value > times)
                    {
                        topWord = pair.Key;
                        times = pair.Value;
                    }
                    total++;
                }
                double percent = (double)times / (double)total;
                return new WordAndPercentage(topWord, percent);
            }
            return new WordAndPercentage("word isn't in database", 100);
        }
        */

        static WordAndPercentage predictNextWord(string word)
        {
            WordAndPercentage w = getTopWord(word);
            if (w == null)
                return new WordAndPercentage("Word does not exist", 0);
            return w;
        }

        static void loadTests(List<Word> words)
        {
            HashSet<string> myDictionary = new HashSet<string>();
            for (int i = 0; i < words.Count - 1; i++)
            {
                myDictionary.Add(words[i].Content.ToLower());
                string wordA = words[i].Content.ToLower();
                string wordB = words[i + 1].Content.ToLower();
                if (!wordAndNextWords.ContainsKey(wordA)) //If we haven't seen wordA yet
                {
                    Dictionary<string, int> newNext = new Dictionary<string, int>();
                    newNext.Add(wordB, 1);
                    wordAndNextWords.Add(wordA, newNext);

                }
                else //seen wordA
                {
                    if (wordAndNextWords[wordA].ContainsKey(wordB)) //If there's already an instance of the wordB following the word A
                    {
                        wordAndNextWords[wordA][wordB]++;
                    }
                    else //Word B hasn't followed word A before
                    {
                        wordAndNextWords[wordA].Add(wordB, 1);
                    }
                }
            }


            wordAndNextWordProbabilities = new Dictionary<string, List<WordAndPercentage>>();
            
            foreach (var kv in wordAndNextWords)
            {
                int total = 0;
                Dictionary<string, int> nextWordAndCounts = new Dictionary<string, int>();
                foreach (var nextword in kv.Value)
                {
                    total += nextword.Value;
                    nextWordAndCounts.Add(nextword.Key, nextword.Value);
                }

                wordAndNextWordProbabilities.Add(kv.Key, new List<SpeechTagging.WordAndPercentage>());
                foreach (var nextWordAndCount in nextWordAndCounts)
                {
                    wordAndNextWordProbabilities[kv.Key].Add(new SpeechTagging.WordAndPercentage(nextWordAndCount.Key, (double)nextWordAndCount.Value / total));
                }

            }
        }

        static bool getNextWordInValue(Dictionary<string, WordAndPercentage> dict, string word)
        {
            foreach (var kv in dict)
            {
                if (kv.Value.word == word) return true;
            }
            return false;
        }
        static Dictionary<StateTransition, double> createTransitionModel(List<Word> words)
        {
            Dictionary<StateTransition, int> counter = new Dictionary<StateTransition, int>(); //counts instances of the state transition
            Dictionary<WordType, int> fromCounter = new Dictionary<WordType, int>(); //counts times we see the WordType from (needed for percentage)
            Dictionary<StateTransition, double> model = new Dictionary<StateTransition, double>(); //Holds answer
            for(int i=0; i< words.Count - 1; i++)
            {
                if (fromCounter.ContainsKey(words[i].PartOfSpeech))
                {
                    fromCounter[words[i].PartOfSpeech]++;
                } else
                {
                    fromCounter.Add(words[i].PartOfSpeech, 1);
                }
                StateTransition transition = new StateTransition(words[i].PartOfSpeech, words[i + 1].PartOfSpeech);
                if (counter.ContainsKey(transition))
                {
                    counter[transition]++;
                } else
                {
                    counter.Add(transition, 1);
                }
            }

            foreach(KeyValuePair<StateTransition, int> pair in counter)
            {
                model.Add(pair.Key, (double)pair.Value / (double)fromCounter[(WordType)pair.Key.from]);
            }


        

            return model;
        }


        static Dictionary<ObservationFromState, double> createObservationModel(List<Word> words)
        {
            Dictionary<ObservationFromState, double> model = new Dictionary<ObservationFromState, double>(); //actual model
            Dictionary<ObservationFromState, int> wordCount = new Dictionary<ObservationFromState, int>(); //how many times the ObservationFromState occurs
            Dictionary<WordType, int> typeCount = new Dictionary<WordType, int>(); //how many times WordType occurs
            for(int i=0; i<words.Count - 1; i++)
            {
                if (typeCount.ContainsKey(words[i].PartOfSpeech))
                {
                    typeCount[words[i].PartOfSpeech]++;
                } else
                {
                    typeCount.Add(words[i].PartOfSpeech, 1);
                }
                ObservationFromState observation = new ObservationFromState(words[i].Content, words[i].PartOfSpeech);
                if (wordCount.ContainsKey(observation))
                {
                    wordCount[observation]++;
                } else
                {
                    wordCount.Add(observation, 1);
                }
            }
            foreach(KeyValuePair<ObservationFromState, int> pair in wordCount)
            {
                model.Add(pair.Key, (double)pair.Value / (double)typeCount[pair.Key.state]);
            }
            return model;
        }

        static Dictionary<WordType, double> beginSentenceProb(List<Word> words)
        {
            Dictionary<WordType, double> probability = new Dictionary<WordType, double>();
            Dictionary<WordType, int> counter = new Dictionary<WordType, int>();
            int totalStarts = 1; //assuming the beginning is the start of a sentence

            for(int i = 0; i < words.Count - 1; i++)
            {
                if(words[i].PartOfSpeech == WordType.SentenceTerminator)
                {
                    totalStarts++;
                    if(counter.ContainsKey(words[i + 1].PartOfSpeech))
                    {
                        counter[words[i + 1].PartOfSpeech]++;
                    } else
                    {
                        counter.Add(words[i + 1].PartOfSpeech, 1);
                    }
                }
            }

            foreach(KeyValuePair<WordType, int> pair in counter)
            {
                probability.Add(pair.Key, (double)pair.Value / (double)totalStarts);
            }
       
            return probability;
        }


        static void Phase1()
        {
            List<Word> words2 = ParsingTools.GetListOfWords(ParsingTools.ProjectDirectory + "training_dataset.txt");
            words2.AddRange(ParsingTools.GetListOfWords(ParsingTools.ProjectDirectory + "testing_dataset.txt"));


            loadTests(words2);
            // while (true)
            // {
            Console.WriteLine("Enter a seed word:");
            string searchNext = Console.ReadLine();
            Console.WriteLine("Enter how many words to generate:");
            int genNum = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            for (int n = 0; n < genNum; n++)
            {
                WordAndPercentage answer = predictNextWord(searchNext);
                searchNext = answer.word;
                Console.Write(searchNext + " ");
            }
        }
        static string escape(string text)
        {
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }

       
        static void Phase2()
        {
            List<Word> words2 = ParsingTools.GetListOfWords(ParsingTools.ProjectDirectory + "training_dataset.txt");
           // words2.AddRange(ParsingTools.GetListOfWords(ParsingTools.ProjectDirectory + "testing_dataset.txt"));



            Console.WriteLine("Enter your sentence. Please separate every symbol with a space (that includes periods, exclamation marks, etc.)");
            string sentenceWhole = Console.ReadLine();
            List<string> sentence = sentenceWhole.Split(' ').ToList();
            Console.WriteLine();

            Console.Write("You entered: ");
            Console.Write("[" + sentence[0] + "]");
            for (int n = 1; n < sentence.Count; n++)
            {
                Console.Write(", " + "[" + sentence[n] + "]");
            }
            Console.WriteLine();
            Console.WriteLine("Please wait...");


            Dictionary<StateTransition, double> transitionModel = createTransitionModel(words2);
            //printTransitionMatrix(transitionModel); Environment.Exit(0);

            Dictionary<ObservationFromState, double> observationModel = createObservationModel(words2);
            Dictionary<WordType, double> sentenceStarters = beginSentenceProb(words2);


            foreach (var s in sentenceStarters)
            {
                Console.WriteLine(s.Key.ToString() + ", " + s.Value.ToString());
            }


            var res = Viterbi.DoViterbi(sentence, transitionModel, observationModel, sentenceStarters);
            Console.Write("Our results: ");
            Console.Write(res[1]);
            for (int n = 2; n < res.Length; n++)
            {
                Console.Write(", " + res[n]);
            }

            Console.WriteLine();
        }

        static void Main(string[] args)
        {


            // List<string> sentence = new List<string>() { "What", "child", "is", "this", "?" };
            // List<string> sentence = new List<string>() { "With", "a", "little", "help", "from", "my", "friends", "!" };

            wordAndNextWords = new Dictionary<string, Dictionary<string, int>>();

            Phase1();


            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
