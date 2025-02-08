using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes.Model
{
    public interface IState
    {
        void Execute();
    }

    public class StateA : IState
    {
        private StateMachine _sm;

        public StateA(StateMachine sm)
        {
            _sm = sm;
        }

        public void Execute()
        {
            Debug.WriteLine("State A");
            _sm.Current = new StateB(_sm);
        }
    }

    public class StateB : IState
    {
        private StateMachine _sm;

        public StateB(StateMachine sm)
        {
            _sm = sm;
        }

        public void Execute()
        {
            Debug.WriteLine("State B");
            _sm.Current = new StateA(_sm);
        }
    }

    public class StateMachine
    {
        public IState Current { get; set; }

        public void Execute()
        {
            Current.Execute();
        }
    }
}
