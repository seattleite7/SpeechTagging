using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    public struct StateTransition
    {
        public WordType from;
        public WordType to;
        public StateTransition(WordType from, WordType to)
        {
            this.from = from;
            this.to = to;
        }
        public override string ToString()
        {
            return from.ToString() + " -> " + to.ToString();
        }
    }
}
