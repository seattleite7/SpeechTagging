using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    public static class ParsingTools
    {
        public static string ProjectDirectory { get {return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\";}}


        //http://www.comp.leeds.ac.uk/amalgam/tagsets/upenn.html

        public static WordType GetWordType(string tag)
        {
            switch (tag)
            {
                case "$":
                    return WordType.Dollar;
                case "``":
                    return WordType.OpeningQuote;
                case "''":
                    return WordType.ClosingQuote;
                case "(":
                    return WordType.OpeningParenthesis;
                case ")":
                    return WordType.ClosingParenthesis;
                case ",":
                    return WordType.Comma;
                case "--":
                    return WordType.Dash;
                case ".":
                    return WordType.SentenceTerminator;
                case ":":
                    return WordType.ColorOrEllipsis;
                case "CC":
                    return WordType.Conjunction_Coordinating;
                case "CD":
                    return WordType.Numeral_Cardinal;
                case "DT":
                    return WordType.Determiner;
                case "EX":
                    return WordType.There_Existential;
                case "FW":
                    return WordType.Foreign;
                case "IN":
                    return WordType.PrepositionConjunction_Subordinating;
                case "JJ":
                    return WordType.AdjectiveNumeral_Ordinal;
                case "JJR":
                    return WordType.Adjective_Comparative;
                case "JJS":
                    return WordType.Adjective_Superlative;
                case "LS":
                    return WordType.ListItemMarker;
                case "MD":
                    return WordType.ModalAux;
                case "NN":
                    return WordType.Noun_CommonSingular;
                case "NNP":
                    return WordType.Noun_ProperSingular;
                case "NNPS":
                    return WordType.Noun_ProperPlural;
                case "NNS":
                    return WordType.Noun_CommonPlural;
                case "PDT":
                    return WordType.PreDeterminer;
                case "POS":
                    return WordType.GenitiveMarker;
                case "PRP":
                    return WordType.Pronoun_Personal;
                case "PRP$":
                    return WordType.Pronoun_Possessive;
                case "RB":
                    return WordType.Adverb;
                case "RBR":
                    return WordType.Adverb_Comparative;
                case "RBS":
                    return WordType.Adverb_Superlative;
                case "RP":
                    return WordType.Particle;
                case "SYM":
                    return WordType.Symbol;
                case "TO":
                    return WordType.To_PrepositionOrInfinitive;
                case "UH":
                    return WordType.Interjection;
                case "VB":
                    return WordType.Verb_Base;
                case "VBD":
                    return WordType.Verb_Past;
                case "VBG":
                    return WordType.Verb_Present;
                case "VBN":
                    return WordType.Verb_PastParticiple;
                case "VBP":
                    return WordType.Verb_Present_Not3rdPersonSingular;
                case "VBZ":
                    return WordType.Verb_Present_3rdPersonSingular;
                case "WDT":
                    return WordType.WH_Determiner;
                case "WP":
                    return WordType.WH_Pronoun;
                case "WP$":
                    return WordType.WH_PossessivePronoun;
                case "WRB":
                    return WordType.WH_Adverb;
                default:
                    Console.WriteLine("Unrecognized tag '" + tag + "'");
                    return WordType.Undefined;


            }


        }

        public static List<Word> GetListOfWords(string filepath)
        {
            List<Word> words = new List<Word>();
            string[] lines = System.IO.File.ReadAllLines(filepath);
            foreach (string line in lines)
            {
                string[] split = line.Split(' ');
                for (int n = 0; n < split.Length; n++)
                {
                    words.Add(new Word(split[n].Trim()));
                }
            }
            return words;
        }
    }
}
