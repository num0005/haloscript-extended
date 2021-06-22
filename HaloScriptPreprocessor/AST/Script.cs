﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloScriptPreprocessor.AST
{
    public class Script : NodeNamed
    {
        public Script(Parser.Value? source, Atom atom) : base(source, null) {
            _name = atom.Clone(this);
        }
        public Script(Parser.Expression source, ScriptType type,  Atom name, LinkedList<Value> code, ValueType? valueType = null, List<(ValueType type, string name)>? arguments = null) : base(source, name)
        {   
            Type = type;
            Codes = code;
            ReturnValueType = valueType;
            Arguments = arguments;
        }

        public ScriptType Type;
        public ValueType? ReturnValueType;
        public Atom ScriptName => Name;
        public LinkedList<Value> Codes = new();
        public List<(ValueType type, string name)>? Arguments;

        public override uint NodeCount {
            get
            {
                uint count = 1;
                foreach (var code in Codes)
                    count += code.NodeCount;
                return count;
            }
        }

        public override Script Clone(Node? parent = null)
        {
            Script clonedScript = new(Source, ScriptName);
            clonedScript.ReturnValueType = ReturnValueType;
            if (Arguments is not null)
                clonedScript.Arguments = Arguments.ToList();
            foreach (Value code in Codes)
                clonedScript.Codes.Append(code.Clone(clonedScript));
            return clonedScript;
        }
    }
}
