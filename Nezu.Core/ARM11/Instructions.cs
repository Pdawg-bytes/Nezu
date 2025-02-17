﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nezu.Core.Enums;

namespace Nezu.Core.ARM11
{
    public partial class ARM11Core
    {
        private void ExecuteInstruction(uint instruction)
        {
            var conditionCode = (ConditionCode)(instruction >> 28);
            if (!IsConditionSet(conditionCode))
                return;

            // See ARMv6 manual, Figure A3-1

            // Special unconditional instructions, see Figure A3-6
            if (conditionCode is ConditionCode.UD)
                ExecuteUnconditionalInstruction(instruction);

            uint group = (instruction >> 25) & 0b111;
            switch (group)
            {
                case 0b000:
                    uint opcode = (instruction >> 21) & 0b1111;
                    if ((opcode >> 2) == 0b10)
                        ExecuteMiscInstruction(instruction);
                    else
                        ExecuteDataProcessingInstruction(instruction);
                    break;

                case 0b001:
                    opcode = (instruction >> 21) & 0b1111;
                    bool s = IsBitSet(instruction, 20);
                    if ((opcode >> 2) == 0b10 && !s)
                    {
                        // Assert not undefined instruction
                        Debug.Assert((opcode & 1) != 0);

                        ExecuteMoveImmediateToStatusRegisterInstruction(instruction);
                    }
                    else
                    {
                        ExecuteDataProcessingImmediateInstruction(instruction);
                    }
                    break;

                case 0b010:
                    ExecuteLoadStoreOffsetInstruction(instruction, false);
                    break;

                case 0b011:
                    if (IsBitSet(instruction, 4))
                    {
#if DEBUG
                        // Assert not architecturally undefined instruction
                        bool higherUnset = ((~instruction >> 20) & 0b11111) != 0;
                        bool lowerUnset = ((~instruction >> 4) & 0b1111) != 0;
                        Debug.Assert(higherUnset || lowerUnset);
#endif
                        ExecuteMediaInstruction(instruction);
                    }
                    else
                    {
                        ExecuteLoadStoreOffsetInstruction(instruction, true);
                    }
                    break;

                case 0b100:
                    ExecuteLoadStoreMultipleInstruction(instruction);
                    break;

                case 0b101:
                    ExecuteBranchInstruction(instruction);
                    break;

                case 0b110:
                    ExecuteCoprocessorTransferInstruction(instruction);
                    break;

                case 0b111:
                    if (IsBitSet(instruction, 24))
                        ExecuteSoftwareInterrupt(instruction);
                    else
                        ExecuteCoprocessorInstruction(instruction);
                    break;

                default:
                    throw new NotImplementedException($"Unrecognized instruction: 0x{instruction:X8}");
            }
        }

        private void ExecuteCoprocessorInstruction(uint instruction)
        {
            throw new NotImplementedException();
        }

        private void ExecuteSoftwareInterrupt(uint instruction)
        {
            throw new NotImplementedException();
        }

        private void ExecuteCoprocessorTransferInstruction(uint instruction)
        {
            throw new NotImplementedException();
        }

        private void ExecuteBranchInstruction(uint instruction)
        {
            throw new NotImplementedException();
        }

        private void ExecuteLoadStoreMultipleInstruction(uint instruction)
        {
            throw new NotImplementedException();
        }

        private void ExecuteMediaInstruction(uint instruction)
        {
            throw new NotImplementedException();
        }

        private void ExecuteLoadStoreOffsetInstruction(uint instruction, bool registerIndirect)
        {
            throw new NotImplementedException();
        }

        private void ExecuteDataProcessingImmediateInstruction(uint instruction)
        {
            throw new NotImplementedException();
        }

        private void ExecuteMoveImmediateToStatusRegisterInstruction(uint instruction)
        {
            throw new NotImplementedException();
        }

        private void ExecuteDataProcessingInstruction(uint instruction)
        {
            throw new NotImplementedException();
        }

        private void ExecuteMiscInstruction(uint instruction)
        {
            throw new NotImplementedException();
        }

        private void ExecuteUnconditionalInstruction(uint instruction)
        {
            throw new NotImplementedException();
        }
    }
}
