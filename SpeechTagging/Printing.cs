using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    public static class Printing
    {
        public static void printTransitionMatrix(Dictionary<StateTransition, double> model)
        {
            StringBuilder output = new StringBuilder();
            WordType[] types = (WordType[])Enum.GetValues(typeof(WordType));
            StateTransition trans = new StateTransition(WordType.Undefined, WordType.Undefined);

            //Header
            output.Append("...,");
            for (int n = 0; n < types.Length; n++)
                output.Append(types[n].ToString() + ",");
            output.AppendLine();



            for (int row = 0; row < types.Length; row++)
            {
                trans.from = types[row];
                output.Append(trans.from.ToString() + ",");

                for (int col = 0; col < types.Length; col++)
                {
                    trans.to = types[col];
                    if (!model.ContainsKey(trans))
                        output.Append("NA" + ",");
                    else
                        output.Append(model[trans].ToString() + ",");
                }

                output.AppendLine();
            }

            System.IO.File.WriteAllText("transition.csv", output.ToString());


        }



        static string escape(this string text)
        {
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
        private static WordAndPercentage find(this List<WordAndPercentage> list, string word)
        {
            foreach (var l in list)
            {
                if (l.word == word)
                    return l;
            }
            return null;
        }
        const string NA = "NA,";
        public static void printWordPredictorMatrix(Dictionary<string, List<WordAndPercentage>> model, List<string> myDictionary)
        {
            StringBuilder output = new StringBuilder();
            
    


            int max = 50;


            for (int row = 0; row < myDictionary.Count; row++)
            {
                string from = myDictionary[row];
                output.Append(from.escape() + ",");
               
                for (int col = 0; col < myDictionary.Count; col++)
                {
                    string to = myDictionary[col];
                    WordAndPercentage wpTo = null;
                    if (!model.ContainsKey(from) || (wpTo = model[from].find(to)) == null)
                        ;
                    else
                        output.Append(  (wpTo.word + "---" + wpTo.percent.ToString()).escape() + ",");

                    if (col > max) break;
                }

                output.AppendLine();
                //System.IO.File.AppendAllText("predictor.csv", output.ToString());
                //output.Clear();
                if (row > max) break;
            }

            System.IO.File.WriteAllText("predictor.csv", output.ToString());


        }

        public static void printObservationMatrix(Dictionary<ObservationFromState, double> model, List<string> myDictionary)
        {
            WordType[] types = (WordType[])Enum.GetValues(typeof(WordType));

            ObservationFromState obsv = new ObservationFromState("", WordType.Undefined);

            StringBuilder output = new StringBuilder();

            //Header
            output.Append("...,");
            for (int n = 0; n < myDictionary.Count; n++)
                output.Append(myDictionary[n].escape() + ",");
            output.AppendLine();



            for (int row = 0; row < types.Length; row++)
            {
                WordType from = types[row];
                obsv.state = from;
                output.Append(from.ToString() + ",");
                for (int col = 0; col < myDictionary.Count; col++)
                {
                    obsv.observation = myDictionary[col];
                    if (model.ContainsKey(obsv))
                    {
                        output.Append(model[obsv]);
                    }
                    output.Append(",");
                }

                output.AppendLine();
            }
            System.IO.File.WriteAllText("observer.csv", output.ToString());

        }

    }
}
