using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    public class WordAndPercentage
    {
        public String word;
        public double percent;

        public WordAndPercentage(string w, double p)
        {
            word = w;
            percent = p;
        }
    }
}
