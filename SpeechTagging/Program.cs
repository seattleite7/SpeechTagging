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

        static void loadTests(List<Word> words)
        {
            for (int i = 0; i < words.Count - 1; i++)
            {
                string wordA = words[i].Content.ToLower();
                string wordB = words[i].Content.ToLower();
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
            //Use this function to get words instead:
            var words2 = ParsingTools.GetListOfWords(ParsingTools.ProjectDirectory + "testing_dataset.txt"); 
            //wordAndNextWords key: word, value: dictionary where the key is a following word and value is 
            //the number of times the following word occured
            
            loadTests(words2);
            string answer = predictNextWord("doctor");
        }
    }
}
