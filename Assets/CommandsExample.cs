using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;
using static Assets.CommandNew.CommandInvoker;

namespace Assets.CommandNew
{
    public sealed class CommandsExample : MonoBehaviour
    {
        private readonly CommandInvoker _invoker = new();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Execute();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Undo();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Redo();
            }
        }

        [ContextMenu(nameof(Execute))]
        public void Execute()
        {
            for (int i = 0; i< 100; i++)
            {
                string name = $"Cube_{i:000}";

                _invoker.ExecuteSpawnCube(new CommandSpawnCubeArgs(name, Vector3.zero, Vector3.one));
                _invoker.ExecuteMoveObject(new CommandMoveObjectArgs(name, new Vector3(2f, 1f, 4f)));
            }
        }

        [ContextMenu(nameof(Undo))]
        public void Undo()
        {
            _invoker.Undo();
        }

        [ContextMenu(nameof(Redo))]
        public void Redo()
        {
            _invoker.Redo();
        }
    }
}
