using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace Assets.CommandNew
{
    public sealed class CommandInvoker
    {
        //all wrappers by CommandType
        private Dictionary<CommandType, ICommandWrapper> _commandWrappers = new();

        //wrappers
        private readonly CommandWrapper<CommandSpawnCubeArgs, CommandSpawnCubeUndoArgs> _spawnCube;
        private readonly CommandWrapper<CommandMoveObjectArgs, CommandMoveObjectUndoArgs> _moveObject;

        //undo and redo
        private readonly List<CommandType> _undoCommandTypes = new(_maxUndoCount);
        private readonly Stack<CommandType> _redoCommandTypes = new();

        private const int _maxUndoCount = 4;    //undo memory limit

        public CommandInvoker()
        {
            //create wrappers
            _spawnCube = new(new CommandSpawnCube(), CommandType.SpawnCube);
            _moveObject = new(new CommandMoveObject(), CommandType.MoveObject);

            //add them to dictionary
            _commandWrappers.Add(CommandType.SpawnCube, _spawnCube);
            _commandWrappers.Add(CommandType.MoveObject, _moveObject);
        }

        public void ExecuteSpawnCube(CommandSpawnCubeArgs args)
        {
            _spawnCube.Execute(args);    //ask wrapper to execute
            AddToUndo(CommandType.SpawnCube);  //add command type to history stack
            ClearRedo();
        }

        public void ExecuteMoveObject(CommandMoveObjectArgs args)
        {
            _moveObject.Execute(args);
            AddToUndo(CommandType.MoveObject);
            ClearRedo();
        }

        public void Undo()
        {
            var commandType = _undoCommandTypes[_undoCommandTypes.Count - 1];  //pop command type from history stack
            _undoCommandTypes.RemoveAt(_undoCommandTypes.Count - 1);    //remove undone command from history

            _commandWrappers[commandType].Undo();   //ask wrapper of this command type to undo

            _redoCommandTypes.Push(commandType);    //push command in redo stack
        }

        public void Redo()
        {
            var commandType = _redoCommandTypes.Pop();

            _commandWrappers[commandType].Redo();

            AddToUndo(commandType);
        }

        private void ClearRedo()
        {
            _redoCommandTypes.Clear();
            _spawnCube.ClearRedo();
            _moveObject.ClearRedo();
        }

        private void AddToUndo(CommandType commandUndo)
        {
            if (_undoCommandTypes.Count + 1 > _maxUndoCount)
            {
                //erase oldest element in history
                _commandWrappers[_undoCommandTypes[0]].EraseOldestHistoryEntry();
                _undoCommandTypes.RemoveAt(0);
            }
            _undoCommandTypes.Add(commandUndo);
        }
    }

    public enum CommandType
    {
        SpawnCube,
        MoveObject,
    }
}
