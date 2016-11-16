using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    class Program
    {
        static void Main(string[] args)
        {
            var words = ParsingTools.GetListOfWords(ParsingTools.ProjectDirectory + "testing_dataset.txt");




            Console.ReadLine();
        }
    }
}
