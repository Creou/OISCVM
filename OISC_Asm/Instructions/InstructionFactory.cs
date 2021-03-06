﻿using System;
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
                            // Labeled subleq instruction.
                            Address operand_a = new Address(instructionData[1]);
                            Address operand_b = new Address(instructionData[2]);
                            Address operand_c =new Address( instructionData[3]);
                            return new SubleqInstruction(trimmedSourceLine, sourceLineNumber, sourceAddress, sourceLabel, operand_a, operand_b, operand_c);
                        }
                        if (instructionData.Length == 3)
                        {
                            // Labeled subleq instruction with auto branch next.
                            Address operand_a = new Address( instructionData[1]);
                            Address operand_b = new Address( instructionData[2]);
                            return new SubleqInstruction(trimmedSourceLine, sourceLineNumber, sourceAddress, sourceLabel, operand_a, operand_b, true);
                        }
                        else if (instructionData.Length == 2)
                        {
                            // Labeled memory value.
                            String value = instructionData[1];
                            if (value.StartsWith(LexicalSymbols.LabelAddress)) 
                            {
                                Address valueAddress = new Address(value);
                                return new AddressableMemoryInstruction(trimmedSourceLine, sourceLineNumber, sourceAddress, sourceLabel, valueAddress);
                            }
                            else
                            {
                                return new AddressableMemoryInstruction(trimmedSourceLine, sourceLineNumber, sourceAddress, sourceLabel, value);
                            }
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
                            // Unlabeled subleq instruction.
                            Address operand_a = new Address( instructionData[0]);
                            Address operand_b = new Address( instructionData[1]);
                            Address operand_c = new Address( instructionData[2]);
                            return new SubleqInstruction(trimmedSourceLine, sourceLineNumber, sourceAddress, operand_a, operand_b, operand_c);
                        }
                        if (instructionData.Length == 2)
                        {
                            // Unlabeled subleq instruction with auto branch next.
                            Address operand_a = new Address( instructionData[0]);
                            Address operand_b = new Address( instructionData[1]);
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
