using UnityEngine;

namespace Assets.CommandNew
{
    public sealed class CommandMoveObject : ICommand<CommandMoveObjectArgs, CommandMoveObjectUndoArgs>
    {
        public CommandMoveObjectUndoArgs Execute(CommandMoveObjectArgs args)
        {
            var target = GameObject.Find(args.TargetName);

            var originalPosition = target.transform.position;
            target.transform.position = args.Position;

            return new CommandMoveObjectUndoArgs(args.TargetName, originalPosition);
        }

        public void Undo(CommandMoveObjectUndoArgs args)
        {
            var target = GameObject.Find(args.TargetName);

            target.transform.position = args.OriginalPosition;
        }
    }

    public readonly struct CommandMoveObjectArgs
    {
        public readonly string TargetName;
        public readonly Vector3 Position;

        public CommandMoveObjectArgs(string targetName, Vector3 position)
        {
            TargetName = targetName;
            Position = position;
        }
    }
    public readonly struct CommandMoveObjectUndoArgs
    {
        public readonly string TargetName;
        public readonly Vector3 OriginalPosition;

        public CommandMoveObjectUndoArgs(string targetName, Vector3 originalPosition)
        {
            TargetName = targetName;
            OriginalPosition = originalPosition;
        }
    }
}