using ARMeilleure.Decoders;
using ARMeilleure.Translation;

using static ARMeilleure.Instructions.InstEmitFlowHelper;
using static ARMeilleure.IntermediateRepresentation.OperandHelper;

namespace ARMeilleure.Instructions
{
    static partial class InstEmit
    {
        public static void Brk(ArmEmitterContext context)
        {
            EmitExceptionCall(context, nameof(NativeInterface.Break));
        }

        public static void Svc(ArmEmitterContext context)
        {
            EmitExceptionCall(context, nameof(NativeInterface.SupervisorCall));
        }

        private static void EmitExceptionCall(ArmEmitterContext context, string name)
        {
            OpCodeException op = (OpCodeException)context.CurrOp;

            context.StoreToContext();

            context.Call(typeof(NativeInterface).GetMethod(name), Const(op.Address), Const(op.Id));

            context.LoadFromContext();

            if (context.CurrBlock.Next == null)
            {
                EmitTailContinue(context, Const(op.Address + 4));
            }
        }

        public static void Und(ArmEmitterContext context)
        {
            OpCode op = context.CurrOp;

            string name = nameof(NativeInterface.Undefined);

            context.StoreToContext();

            context.Call(typeof(NativeInterface).GetMethod(name), Const(op.Address), Const(op.RawOpCode));

            context.LoadFromContext();

            if (context.CurrBlock.Next == null)
            {
                EmitTailContinue(context, Const(op.Address + 4));
            }
        }
    }
}