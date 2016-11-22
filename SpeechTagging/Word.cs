using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    public class Word
    {
        public string Content { get; set; }
        public WordType PartOfSpeech { get; set; }
        public Word() { }
        public Word(string underscored_encoding)
        {
            string[] split = underscored_encoding.Split('_');
            if (split.Length != 2)
                throw new Exception("Expected exactly 1 underscore");
            Content = split[0].ToLower();
            PartOfSpeech = ParsingTools.GetWordType(split[1]);
        }
    }
}
