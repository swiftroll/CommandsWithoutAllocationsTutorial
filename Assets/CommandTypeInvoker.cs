using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CommandNew
{
    public interface ICommandWrapper
    {
        void Undo();
        void Redo();
        void ClearRedo();
        void EraseOldestHistoryEntry();
    }

    public sealed class CommandWrapper<T0, T1> : ICommandWrapper
        where T0 : struct //exec args
        where T1 : struct //undo args
    {
        public CommandType CommandType { get; private set; }

        private readonly ICommand<T0, T1> _cmd;

        private readonly List<HistoryEntry> _history = new();
        private readonly Stack<T0> _toRedo = new();

        public CommandWrapper(ICommand<T0, T1> cmd, CommandType commandType)
        {
            _cmd = cmd;
            CommandType = commandType;
        }

        public void Execute(T0 args)
        {
            ClearRedo();
            _history.Add(new(args, _cmd.Execute(args)));
        }

        public void Undo()
        {
            var undoArgs = _history[_history.Count - 1];
            _history.RemoveAt(_history.Count - 1);  //remove after taking

            _cmd.Undo(undoArgs.UndoArgs);
            _toRedo.Push(undoArgs.ExecutionArgs);
        }

        public void Redo()
        {
            var args = _toRedo.Pop();
            _history.Add(new(args, _cmd.Execute(args)));
        }

        public void ClearRedo()
        {
            _toRedo.Clear();
        }

        public void EraseOldestHistoryEntry()
        {
            _history.RemoveAt(0);
        }

        private readonly struct HistoryEntry
        {
            public readonly T0 ExecutionArgs;
            public readonly T1 UndoArgs;

            public HistoryEntry(T0 executionArgs, T1 undoArgs)
            {
                ExecutionArgs = executionArgs;
                UndoArgs = undoArgs;
            }
        }
    }
}
