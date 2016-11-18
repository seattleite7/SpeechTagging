﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    class Program
    { 

        static Dictionary<string, Dictionary<string, int>> wordAndNextWords;
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

        static void loadTests(List<Word> words)
        {
            for (int i = 0; i < words.Count - 1; i++)
            {
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
            Console.WriteLine(wordAndNextWords.Count);
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

        static void Main(string[] args)
        {
            //Use this function to get words instead:
            List<Word> words2 = ParsingTools.GetListOfWords(ParsingTools.ProjectDirectory + "testing_dataset.txt");
            //wordAndNextWords key: word, value: dictionary where the key is a following word and value is 
            wordAndNextWords = new Dictionary<string, Dictionary<string, int>>();
            //the number of times the following word occured
            Dictionary<StateTransition, double> transitionModel = createTransitionModel(words2);
            loadTests(words2);
            Console.WriteLine("Please enter a word and I'll tell you your suggested next word.");
            string searchNext = Console.ReadLine();
            WordAndPercentage answer = predictNextWord(searchNext);
            Console.WriteLine("Predicted Next: " + answer.word + " " + answer.percent * 100);
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
