import re

import opcodes
import variants

def GetOpcode(opcode):
    pref = opcode.replace("REX.W","").replace("+r","").replace("/r","").replace("ib","").replace("iw","").replace("id","").replace("iq","")
    op = re.sub("/(\d)", "", pref).strip()
    r = ", ".join([("0x"+str(x).strip()) for x in op.split(" ")])
    return ("[ "+r+" ]").strip()
def GetImm(opcode):
    if opcode.find("ib") != -1:
        return " 8"
    elif opcode.find("iw") != -1:
        return "16"
    elif opcode.find("id") != -1:
        return "32"
    elif opcode.find("iq") != -1:
        return "64"  
    return " 0"
def GetOperands(oper):
    if oper.strip()=="":
        return "[]"
    oper=oper.replace("X:", "").replace("W:", "").replace("R:", "")
    r = ", ".join([("\""+str(x).strip()+"\"") for x in oper.split(',')])
    return "["+r+"]"
def GetOpcodeAdd(opcode):
    if opcode.find("+r") != -1:
        return "True"
    return "False"
def GetOpcodeField(opcode):
    m = re.search("/(\d)", opcode)
    if m != None:
        return m.group(0)[1:]
    return "0"
def GetReg(opcode):
    if opcode.find("/r") != -1:
        return "True"
    return "False"
def Get0fPrefix(opc):
    if opc.find("0F") != -1:
        return "0x0F"
    return "0x00"
def GetRex(opc):
    if opc.find("REX.W") != -1:
        return "0x48"
    return "0x00"
def GetAch(str):
    if str.find("X64") != -1:
        return "\"X64\""
    elif str.find("X86") != -1:
        return "\"X86\""
    elif str.find("ANY") != -1:
        return "\"ANY\""   
    return ""
def GetLock(str):
    if str.find("LOCK") != -1:
        return "True"
    return "False"
def GetDescription(opcode):
    for o in opcodes.OpcodeList:
        if o[0].upper() == opcode.upper():
            return o[1]
    return ""
def GetVariants(opcode):
    vv = []
    for o in variants.instructions:
        if o[0] == opcode:
            vv.append([o[1], o[2], o[3], o[4]])
    return vv

def PutQStr(s, count):
    return ("\""+s.strip()+"\"").ljust(count, ' ')

oplist = []
for v in variants.instructions:
    if not v[0] in oplist and v[0].find("/") == -1:
        oplist.append(v[0])

print("# Instructions: %u" % len(oplist))
print("# Field description:")
print("#  |- [0] - Mnemonic")
print("#  |- [1] - Description")
print("#  |---- [ 0] - Operands")
print("#  |---- [ 1] - Encoding (NONE, I, MI, MR, RM, O, M, II, RMI, D, MRI)")
print("#  |---- [ 2] - Opcode")
print("#  |---- [ 3] - Rex.W prefix (0x00 or 0x48)")
print("#  |---- [ 4] - Opcode field (0..7)")
print("#  |---- [ 5] - Opcode add (True, False)")
print("#  |---- [ 6] - Reg (True, False)")
print("#  |---- [ 7] - Immedicate size (0=None, 8, 16, 32, 64)")
print("#  |---- [ 8] - Architecture (ANY, X86, X64)")
print("#  |---- [ 9] - Lock (True, False)")
print("#  |---- [10] - Description")
print("")
print("OpcodeVariants = [")

for opcode in oplist:
    print("    [ \"%s\", \"%s\", [" % (opcode.upper(), GetDescription(opcode)))
    for v in GetVariants(opcode):
        print("        [ %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, \"\" ]," % (\
            GetOperands(v[0]).ljust(50),\
            PutQStr(v[1], 6),\
            GetOpcode(v[2]).ljust(20, ' '),\
            GetRex(v[2]),\
            #Get0fPrefix(v[2]),\
            GetOpcodeField(v[2]),\
            GetOpcodeAdd(v[2]).ljust(5, ' '),\
            GetReg(v[2]).ljust(5, ' '),\
            GetImm(v[2]),\
            GetAch(v[3]),\
            GetLock(v[3]).ljust(5, " ")\
            ))
    print("    ]],")
print("]")
