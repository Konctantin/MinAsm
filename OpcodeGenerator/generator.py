import re, os
import x86db
from fasm import FASM, FasmError, FasmStateError

#fasm = FASM("E:\\projects\\FASM\\SOURCE\\DLL\\FASM.DLL")
#print(fasm.AssembleAsStr("use32 \n pop eax", 10000))

outputDir = "E:\\temp\\testGen\\"

#immList = [ ["byte",1], ["sbyte",1], ["short",2], ["ushort":2, "int":4, "uint":4, "long":8, "ulong":8 ]

def GetBool(val):
    if val:
        return "true"
    return "false"

def GetImmSize(val):
    if val == 8:
        return "DataSize.Bit8"
    elif val == 16:
        return "DataSize.Bit16"
    elif val == 32:
        return "DataSize.Bit32"
    elif val == 64:
        return "DataSize.Bit64"
    return "DataSize.None"

class OperandDescriptor:
    def __init__(self, op):
        self.operand = op

    def ToStr(self):
        freg = self.GetFixedReg();
        if freg != None:
            return "new OperandDescriptor(%s)" % freg
        
        ot = self.GetOperandType()
        if ot in ["OperandType.Register", "OperandType.RegisterOrMemory"]:
            return "new OperandDescriptor(%s, %s)" % (ot, self.GetRegisterType())

        return "new OperandDescriptor(%s, %s)" % (ot, self.GetOperandSize())

    def GetOperandType(self):
        if self.operand.find("moff") != -1:
            return "OperandType.MemoryOffset"
        if self.operand.find("rel") != -1:
            return "OperandType.RelativeOffset"

        if re.search("r(\d+)", self.operand) != None:
            return "OperandType.Register"
        if self.operand.find("rxx") != -1:
            return "OperandType.Register"
        if self.operand.find("sreg") != -1:
            return "OperandType.Register"
        if self.operand.find("creg") != -1:
            return "OperandType.Register"
        if self.operand.find("dreg") != -1:
            return "OperandType.Register"
  
        if re.search("r(\d+)/m(\d+)", self.operand) != None:
            return "OperandType.RegisterOrMemory"
        if self.operand.find("rxx/mxx") != -1:
            return "OperandType.RegisterOrMemory"

        if re.search("m(\d+)", self.operand) != None:
            return "OperandType.Memory"
        if self.operand.find("mxx") != -1:
            return "OperandType.Memory"
        if self.operand.find("mem") != -1:
            return "OperandType.Memory"

        if self.operand.startswith("i"):
            return "OperandType.Imm"
        if self.GetFixedReg() != None:
            return "OperandType.FixedRegister"

        raise BaseException("UncnovnOperandType " + self.operand)

    def GetOperandSize(self):
        if self.operand.find("128") != -1:
            return "DataSize.Bit128"
        elif self.operand.find("64") != -1:
            return "DataSize.Bit64"
        elif self.operand.find("32") != -1:
            return "DataSize.Bit32"
        elif self.operand.find("16") != -1:
            return "DataSize.Bit16"
        elif self.operand.find("8") != -1:
            return "DataSize.Bit8"         
        return "DataSize.None"

    def GetRegisterType(self):
        if self.operand.find("r8") != -1:
            return "RegisterType.GP8"
        if self.operand.find("r16") != -1:
            return "RegisterType.GP16"
        if self.operand.find("r32") != -1:
            return "RegisterType.GP32"
        if self.operand.find("r64") != -1:
            return "RegisterType.GP64"
        if self.operand.find("rxx") != -1:
            return "RegisterType.GP"

        if self.operand.find("sreg") != -1:
            return "RegisterType.Segment"
        if self.operand.find("creg") != -1:
            return "RegisterType.Control"
        if self.operand.find("dreg") != -1:
            return "RegisterType.Debug"

        return "RegisterType.None"

    def GetFixedReg(self):
        if self.operand.find("al") != -1:
            return "Register.AL"
        elif self.operand.find("ax") != -1:
            return "Register.AX"
        elif self.operand.find("eax") != -1:
            return "Register.EAX"
        elif self.operand.find("rax") != -1:
            return "Register.RAX"

        elif self.operand.find("ds") != -1:
            return "Register.DS"
        elif self.operand.find("es") != -1:
            return "Register.ES"
        elif self.operand.find("ss") != -1:
            return "Register.SS"
        elif self.operand.find("fs") != -1:
            return "Register.FS"
        elif self.operand.find("gs") != -1:
            return "Register.GS"
        elif self.operand.find("cs") != -1:
            return "Register.CS"
        else:
            return None

class VariantsContent:
    def __init__(self, raw):
        self.vlist = raw
        pass

    def GetVariant(self, variant):
        # gen opcode bytes
        opcodeBytes =  ", new byte { %s }" % (", ".join(("0x%02X" % x) for x in variant[2]))
    
        # gen operand descriptors
        opDesc = ""
        for oo in variant[0]:
            od = OperandDescriptor(oo)
            opDesc += "                , " + od.ToStr() + "\n"

        return "new InstructionVariant(OperandEncoding.%s, 0x%02X, %u, %s, %s, %s, %s, Architectures.%s\n\
                    %s\n\
    %s\
                    )," % (\
            variant[1], # encoding \
            variant[3], # rex \
            variant[4], # opcode field \
            GetBool(variant[5]), # opcode add \
            GetBool(variant[6]), # reg \
            GetBool(variant[9]), # lock \
            GetImmSize(variant[7]), # size \
            variant[8], # Architecture \
            opcodeBytes, # \
            opDesc
            )

    def ToStr(self):
        str = ""
        for variant in self.vlist:
            str += "\n            " + self.GetVariant(variant)
        return str

class ConsrunctorsContent:
    def __init__(self, raw):
        pass

    def GetOperandType(self, order):
        if self.operand.find("moff") != -1:
            return "OperandType.MemoryOffset"
        if self.operand.find("rel") != -1:
            return "RelativeOffset operand"+str(order)

        if re.search("r(\d+)", self.operand) != None:
            return "Register operand"+str(order)
        if self.operand.find("rxx") != -1:
            return "Register operand"+str(order)
        if self.operand.find("sreg") != -1:
            return "Register operand"+str(order)
        if self.operand.find("creg") != -1:
            return "Register operand"+str(order)
        if self.operand.find("dreg") != -1:
            return "Register operand"+str(order)
  
        if re.search("r(\d+)/m(\d+)", self.operand) != None:
            return "EffectiveAddress operand"+str(order)
        if self.operand.find("rxx/mxx") != -1:
            return "EffectiveAddress operand"+str(order)

        if re.search("m(\d+)", self.operand) != None:
            return "OperandType.Memory"
        if self.operand.find("mxx") != -1:
            return "OperandType.Memory"
        if self.operand.find("mem") != -1:
            return "OperandType.Memory"

        if self.operand.startswith("i"):
            return "OperandType.Imm"
        if self.GetFixedReg() != None:
            return "OperandType.FixedRegister"

        raise BaseException("UncnovnOperandType " + self.operand)

def GetConstructorsContect(mnem, desc, vlist):
    pass

def GetFunctionsContext(mnem, desc, vlist):
    return "\
        ///<summary>\n\
        /// "+desc+"\n\
        ///</summary>\n\
        public void "+mnem+"()\n\
        {\n\
        }\
"

def GetFileContent(mnem, desc, vlist):
    return "\
// ** auto generated **\n\
\n\
using System;\n\
\n\
namespace MinAsm.Opcodes\n\
{\n\
    /// <summary>\n\
    /// "+desc+"\n\
    /// </summary>\n\
    public class "+mnem+" : Instruction\n\
    {\n\
        static InstructionVariant[] m_variants = {\
" + VariantsContent(vlist).ToStr() + "\n\
        };\n\
\n\
        public "+mnem+"()\n\
            : base(\""+mnem+"\", m_variants)\n\
        {\n\
        }\n\
    }\n\
}\n\
\n\
namespace MinAsm\n\
{\n\
    public partial class Assembler\n\
    {\n\
" + GetFunctionsContext(mnem, desc, vlist) + "\n\
    }\n\
}"

def DelFiles(directory, pattern):
    for f in os.listdir(directory):
        if re.search(pattern, f):
            os.remove(os.path.join(directory, f))
    pass

def CreateFile(mnem, desc, varList):
    content = GetFileContent(mnem, desc, varList)
    fname = "%sOpcode_%s.gen.cs" % (outputDir, mnem)
    #print("Create " + fname)
    f = open(fname, "w+")
    f.write(content)
    f.close()
    pass

# clear folder
DelFiles(outputDir, "Opcode_(\w+).gen.cs")
c=0
for instr in x86db.OpcodeVariants:
    try:
        CreateFile(instr[0], instr[1], instr[2])
    except BaseException as e:
        c+=1;
        print([instr[0], e])
        
print('Errors:',c)

