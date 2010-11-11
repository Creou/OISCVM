using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler.Instructions
{
    public class InstructionFactory
    {
        public Instruction GenerateInstruction(String sourceLine, int sourceLineNumber, int sourceAddress)
        {
            String trimmedSourceLine = sourceLine.Trim().RemoveMultipleSpaces();

            if (!String.IsNullOrEmpty(trimmedSourceLine))
            {
                if (trimmedSourceLine.StartsWith("//"))
                {
                    // This source line is a comment.
                    return new CommentInstruction(trimmedSourceLine, sourceLineNumber);
                }
                else
                {
                    String[] instructionData = trimmedSourceLine.Split(' ');

                    if (instructionData[0].EndsWith(LexicalSymbols.Label))
                    {
                        String sourceLabel = instructionData[0].Replace(LexicalSymbols.Label, String.Empty);
                        if (instructionData.Length == 4)
                        {
                            String operand_a = instructionData[1];
                            String operand_b = instructionData[2];
                            String operand_c = instructionData[3];
                            return new SubleqInstruction(trimmedSourceLine, sourceLineNumber, sourceAddress, sourceLabel, operand_a, operand_b, operand_c);
                        }
                        if (instructionData.Length == 3)
                        {
                            String operand_a = instructionData[1];
                            String operand_b = instructionData[2];
                            return new SubleqInstruction(trimmedSourceLine, sourceLineNumber, sourceAddress, sourceLabel, operand_a, operand_b, true);
                        }
                        else if (instructionData.Length == 2)
                        {
                            String initialValue = instructionData[1];
                            return new AddressableMemoryInstruction(trimmedSourceLine, sourceLineNumber, sourceLabel, initialValue);
                        }
                        else
                        {
                            //TODO: Need to handle invalid source code.
                            return null;
                        }
                    }
                    else
                    {
                        if (instructionData.Length == 3)
                        {
                            String operand_a = instructionData[0];
                            String operand_b = instructionData[1];
                            String operand_c = instructionData[2];
                            return new SubleqInstruction(trimmedSourceLine, sourceLineNumber, sourceAddress, operand_a, operand_b, operand_c);
                        }
                        if (instructionData.Length == 2)
                        {
                            String operand_a = instructionData[0];
                            String operand_b = instructionData[1];
                            return new SubleqInstruction(trimmedSourceLine, sourceLineNumber, sourceAddress, operand_a, operand_b, true);
                        }
                        else
                        {
                            //TODO: Need to handle invalid source code.
                            return null;
                        }
                    }
                }
            }
            else
            {
                return null;
            }
        }
    }
}
