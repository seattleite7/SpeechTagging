using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    public class PrettyTable<T>
    {
        private T[,] val;
        int rows;
        int cols;
        public PrettyTable(int row, int col) { val = new T[row, col]; rows = row; cols = col;  }

        public T this[int i, int j]
        {
            get
            {
                return val[i, j];
            }
            set
            {
                val[i, j] = value;
            }
        }

        public override string ToString()
        {
            StringBuilder strBld = new StringBuilder();
            int[] maxWidths = new int[cols];
            int maxWidthOfRows=0;
            for (int row = 1; row < rows; row++)
            {
                if (Enum.IsDefined(typeof(WordType), row))
                    maxWidthOfRows = Math.Max(maxWidthOfRows, ((WordType)row).ToString().Length);



                for (int col = 1; col < cols; col++)
                {
                    maxWidths[col] = Math.Max(maxWidths[col], val[row, col].ToString().Length);
                    maxWidths[col] = Math.Max(maxWidths[col], col.ToString().Length);
                }
            }


            for (int w = 0; w < maxWidthOfRows + 1; w++)
                strBld.Append(' ');

            for (int col = 1; col < cols; col++)
            {
                string header = col.ToString();
                for (int w = 0; w < maxWidths[col]; w++)
                {
                    if (w > header.Length - 1)
                        strBld.Append(' ');
                    else
                        strBld.Append(header[w]);
                }
                              
            }
            strBld.AppendLine();
            for (int n = 0; n < maxWidths.Aggregate((a, b) => a + b) + maxWidthOfRows + 1; n++)
                strBld.Append('_');
            strBld.AppendLine();

            for (int row = 1; row < rows; row++)
            {
                string rowHeader = ((WordType)row).ToString();
                for (int w = 0; w < maxWidthOfRows; w++)
                {
                    if (w > rowHeader.Length - 1)
                        strBld.Append(' ');
                    else
                        strBld.Append(rowHeader[w]);
                }
                strBld.Append('|');

                for (int col = 1; col < cols; col++)
                {
                    string valStr = val[row, col].ToString();
                    for (int w = 0; w < maxWidths[col]; w++)
                    {
                        if (w > valStr.Length - 1)
                            strBld.Append(' ');
                        else
                            strBld.Append(valStr[w]);
                    }
                }
                strBld.AppendLine();
            }


            return strBld.ToString();

        }
    }
}
