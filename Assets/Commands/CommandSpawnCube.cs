using UnityEngine;

namespace Assets.CommandNew
{
    public sealed class CommandSpawnCube : ICommand<CommandSpawnCubeArgs, CommandSpawnCubeUndoArgs>
    {
        public CommandSpawnCubeUndoArgs Execute(CommandSpawnCubeArgs args)
        {
            var spawnedCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            spawnedCube.name = args.Name;
            spawnedCube.transform.position = args.Position;
            spawnedCube.transform.localScale = args.Scale;

            return new CommandSpawnCubeUndoArgs(spawnedCube);
        }

        public void Undo(CommandSpawnCubeUndoArgs args)
        {
            Object.Destroy(args.Cube);
        }
    }

    public readonly struct CommandSpawnCubeArgs
    {
        public readonly string Name;
        public readonly Vector3 Position;
        public readonly Vector3 Scale;

        public CommandSpawnCubeArgs(string name, Vector3 position, Vector3 scale)
        {
            Name = name;
            Position = position;
            Scale = scale;
        }
    }

    public readonly struct CommandSpawnCubeUndoArgs
    {
        public readonly GameObject Cube;

        public CommandSpawnCubeUndoArgs(GameObject cube)
        {
            Cube = cube;
        }
    }
}