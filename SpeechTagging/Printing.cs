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



        /*
        public static void printWordPredictorMatrix(Dictionary<string, List<WordAndPercentage>> model)
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
        */
    }
}
