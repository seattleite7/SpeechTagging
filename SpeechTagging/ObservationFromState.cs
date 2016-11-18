using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTagging
{
    public struct ObservationFromState
    {
        public string observation;
        public WordType state;

        public ObservationFromState(string observation, WordType state)
        {
            this.observation = observation;
            this.state = state;

        }
    }
}
