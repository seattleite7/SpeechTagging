using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    //http://www.comp.leeds.ac.uk/amalgam/tagsets/upenn.html
    public enum WordType
    {
        Undefined,
        Dollar,
        OpeningQuote,
        ClosingQuote,
        OpeningParenthesis,
        ClosingParenthesis,
        Comma,
        Dash,
        SentenceTerminator,
        ColorOrEllipsis,
        Conjunction,
        Numeral_Cardinal,
        Determiner,
        There_Existential,
        Foreign,
        PrepositionConjunction_Subordinating,
        AdjectiveNumeral_Ordinal,
        Adjective_Comparative,
        Adjective_Superlative,
        ListItemMarker,
        ModalAux,
        Noun_CommonSingular,
        Noun_CommonPlural,
        Noun_ProperSingular,
        Noun_ProperPlural,
        PreDeterminer,
        GenitiveMarker,
        Pronoun_Personal,
        Pronoun_Possessive,
        Adverb,
        Adverb_Comparative,
        Adverb_Superlative,
        Particle,
        Symbol,
        To_PrepositionOrInfinitive,
        Interjection,
        Verb_Base,
        Verb_Past,
        Verb_Present,
        Verb_PastParticiple,
        Verb_Present_3rdPersonSingular,
        Verb_Present_Not3rdPersonSingular,
        WH_Determiner,
        WH_Pronoun,
        WH_PossessivePronoun,
        WH_Adverb

    }
}
