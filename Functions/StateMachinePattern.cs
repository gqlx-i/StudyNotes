using StudyNotes.CustomAttribute;
using StudyNotes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes.Functions
{
    public class StateMachinePattern
    {
        /// <summary>
        /// 状态机开始
        /// </summary>
        [Method]
        public void StateMachineRun()
        {
            StateMachine sm = new StateMachine();
            StateA s = new StateA(sm);
            sm.Current = s;
            while (true)
            {
                sm.Execute();
                Thread.Sleep(1000);
            }
        }
    }
}
