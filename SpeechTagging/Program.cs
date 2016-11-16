using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    class Program
    {
        /*public class NextWordTracker
        {

        }*/

        static Dictionary<string, Dictionary<string, int>> wordAndNextWords;
        static string predictNextWord(string word)
        {
            if (wordAndNextWords.ContainsKey(word))
            {
                string topWord = "";
                int times = 0;
                foreach(KeyValuePair<string, int> pair in wordAndNextWords[word])
                {
                    if(pair.Value > times)
                    {
                        topWord = pair.Key;
                        times = pair.Value;
                    }
                }
                return topWord;
            }
            return "hi";
        }

        static void loadTests(string[] words)
        {
            for (int i = 0; i < words.Length - 1; i++)
            {
                char[] underscore = { '_' };
                string[] wordAndTag = words[i].Split(underscore);
                string[] nextWord = words[i + 1].Split(underscore);
                string wordA = wordAndTag[0].ToLower(); ;
                string wordB = nextWord[0].ToLower();
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

        static void Main(string[] args)
        {
            //wordAndNextWords key: word, value: dictionary where the key is a following word and value is 
            //the number of times the following word occured
            wordAndNextWords = new Dictionary<string, Dictionary<string, int>>();
            string inputFile = "trainingDataset.txt"; //put this in the bin folder - we can talk about how we want to do this later
            string inputFile2 = "testingDataset.txt";
            string text = System.IO.File.ReadAllText(inputFile);
            string text2 = System.IO.File.ReadAllText(inputFile2);
            char[] delimiterChars = {' '};
            string[] words = text.Split(delimiterChars);
            loadTests(words);
            words = text2.Split(delimiterChars);
            loadTests(words);
            string answer = predictNextWord("doctor");
        }
    }
}
