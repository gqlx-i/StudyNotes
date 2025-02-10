using StudyNotes.Common;
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
        public async void StateMachineRun()
        {
            await Task.Run(() =>
            {
                var Token = GlobalService.Instance.CancellationTokenSource.Token;

                StateMachine sm = new StateMachine();
                StateA s = new StateA(sm);
                sm.Current = s;
                while (true)
                {
                    if (Token.IsCancellationRequested)
                    {
                        break;
                    }
                    sm.Execute();
                    Thread.Sleep(1000);
                }
            });
        }
    }
}
