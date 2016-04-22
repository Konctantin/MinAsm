import variants
import sys, re, os

# Indexes used by x86 data.
kIndexName       = 0;
kIndexOperands   = 1;
kIndexEncoding   = 2;
kIndexOpcode     = 3;
kIndexFlags      = 4;

kCpuArchitecture = [ "ANY", "X86", "X64" ]

kCpuFeatures = [
    "3DNOW",
    "3DNOW2",
    "ADX",
    "AES",
    "AVX",
    "AVX2",
    "AVX512BW",
    "AVX512CD",
    "AVX512DQ",
    "AVX512ER",
    "AVX512F",
    "AVX512PF",
    "AVX512VL",
    "BMI",
    "BMI2",
    "CLFLUSH",
    "CLFLUSH_OPT",
    "CMOV",
    "CMPXCHG8B",
    "CMPXCHG16B",
    "F16C",
    "FMA",
    "FMA4",
    "FSGSBASE",
    "FXSR",
    "GEODE",
    "I486",
    "LAHFSAHF",
    "LZCNT",
    "MMX",
    "MONITOR",
    "MOVBE",
    "MSR",
    "PCLMULQDQ",
    "POPCNT",
    "PREFETCHW",
    "PREFETCHWT1",
    "RDRAND",
    "RDSEED",
    "RDTSC",
    "RDTSCP",
    "SHA",
    "SMAP",
    "SSE",
    "SSE2",
    "SSE3",
    "SSE4_1",
    "SSE4_2",
    "SSE4A",
    "SSSE3",
    "TBM",
    "VMX",
    "XOP",
    "XSAVE",
    "XSAVE_OPT"]

# Only registers used by instructions.
kCpuRegs = {
    "r8":"reg", "r16":"reg", "r32":"reg", "r64":"reg", "reg":"reg", "rxx":"reg",

    "al":"reg", "ah" :"reg", "ax" :"reg", "eax":"reg", "rax":"reg", "zax":"reg",
    "bl":"reg", "bh" :"reg", "bx" :"reg", "ebx":"reg", "rbx":"reg", "zbx":"reg",
    "cl":"reg", "ch" :"reg", "cx" :"reg", "ecx":"reg", "rcx":"reg", "zcx":"reg",
    "dl":"reg", "dh" :"reg", "dx" :"reg", "edx":"reg", "rdx":"reg", "zdx":"reg",
    "di":"reg", "edi":"reg", "rdi":"reg", "zdi":"reg",
    "si":"reg", "esi":"reg", "rsi":"reg", "zsi":"reg",
    "bp":"reg", "ebp":"reg", "rbp":"reg", "zbp":"reg",
    "sp":"reg", "esp":"reg", "rsp":"reg", "zsp":"reg",

    "sreg":"sreg", "cs":"sreg", "ds":"sreg", "es":"sreg", "fs":"sreg", "gs":"sreg", "ss":"sreg",
    "creg":"creg", "cr0-8":"creg",
    "dreg":"dreg", "dr0-7":"dreg",
    "st(0)":"st" , "st(i)":"st",
    "mm":"mm"    , "mm0-7":"mm",
    "k":"k"      , "k0-7":"k",
    "xmm":"xmm"  , "xmm0-31":"xmm",
    "ymm":"ymm"  , "ymm0-31":"ymm",
    "zmm":"zmm"  , "zmm0-31":"zmm"
}

kCpuFlags = {
    "OF": True, # Overflow flag.
    "SF": True, # Sign flag.
    "ZF": True, # Zero flag.
    "AF": True, # Adjust flag.
    "PF": True, # Parity flag.
    "CF": True, # Carry flag.
    "DF": True, # Direction flag.
    "IF": True, # Interrupt flag.
    "AC": True, # Alignment check.
    "C0": True, # FPU's C0 flag.
    "C1": True, # FPU's C1 flag.
    "C2": True, # FPU's C2 flag.
    "C3": True  # FPU's C3 flag.
}

def rfind(pattern, text):
    """ re """
    if text == None or str.strip(text) == "":
        return False;
    return len(re.findall(pattern, text)) > 0

def tryint(s, h = 10):
    if s == "": return 0;
    return int(s, h)

# X86/X64 utilities.

# Get whether the string `s` describes a register operand.
def isRegOp(s): return kCpuRegs.has_key(s);

# Get whether the string `s` describes a memory operand.
def isMemOp(s): return rfind("^(?:mem|mxx|(?:m(?:off)?\d+(?:dec|bcd|fp|int)?)|(?:vm\d+(?:x|y|z)))$", s)

# Get whether the string `s` describes an immediate operand.
def isImmOp(s): return rfind("(?:1|ib|iw|id|iq)", s)

# Get whether the string `s` describes a relative displacement (label).
def isRelOp(s): return rfind("rel\d+", s)

# Get a register class based on string `s`, or `null` if `s` is not a register.
def regClass(s): return kCpuRegs[s];

# Get size in bytes of an immediate `s`.
# Handles "ib", "iw", "id", "iq", and also "/is4".
def immSize(s):
    if s == "1" : return 0;
    if s == "ib" or s == "/is4": return 1;
    if s == "iw": return 2;
    if s == "id": return 4;
    if s == "iq": return 8;
    return -1;

# Get size in bytes of a relative displacement.
# Handles "rel8" and "rel32".
def relSize(s):
    if s == "rel8":  return 1;
    if s == "rel32": return 4;
    return -1;
pass

class X86Operand:
    def __init__(self, data, defaultAccess):
        self.data = data
        self.defaultAccess = defaultAccess
        
        self.reg = "";          # Register operand's definition.
        self.regSize = 0;       # Register operand's size
        self.regClass = None;   # Register operand's class.
        self.regMem = "";       # Segment specified with register that is used to perform a memory IO.

        self.mem = "";          # Memory operand's definition.
        self.memSize = -1;      # Memory operand's size.
        self.memOff = False;    # Memory operand is an absolute offset (only a specific version of MOV).
        self.vsibReg = "";      # AVX VSIB register type (xmm/ymm/zmm).
        self.vsibSize = -1;     # AVX VSIB register size (32/64).
        self.bcstSize = -1;     # AVX-512 broadcast size.

        self.imm = 0;           # Immediate operand's size.
        self.rel = 0;           # Relative displacement operand's size.

        self.implicit = False;  # True if the operand is an implicit register (i.e. not encoded in binary).
        self.read = False;      # True if the operand is a read (R or X) from reg/mem.
        self.write = False;     # True if the operand is a write (W or X) to reg/mem.
        self.rwxIndex = -1;     # Operation (RWX) index.
        self.rwxWidth = -1;     # Operation (RWX) width.
        
        if self.data == "":
            return
        #
        # Handle RWX decorators prefix in "R|W|X[A:B]:" format.
        m = re.findall("(R|W|X)(\[(\d+)\:(\d+)\])?\:", data)
        if len(m) > 0:
            # RWX.
            self.setAccess(m[0][0])

            # RWX Index/Width.
            if len(m) > 0:
                a = tryint(m[0][2]);
                b = tryint(m[0][3]);

                self.rwxIndex = min(a, b);
                self.rwxWidth = abs(a - b) + 1;

            # Remove RWX information from the operand's definition.
            data = data[len(m[0][0])+1:]
        else:
            self.setAccess(defaultAccess)
            
        # Handle AVX-512 broadcast possibility specified as "/bN" suffix.
        m = re.findall("\/b(\d+)", data);
        if len(m) > 0:
            self.bcstSize = tryint(m[0][0]);

            # Remove broadcast from the operand's definition; it's not needed anymore.
            #data = data[0: m.index] + data[m.index + len(length):];

        # Handle an implicit operand.
        if data[0:1] == "<" and data[-1:] == ">":
            self.implicit = True;
            # Remove "<...>" from the operand's definition.
            data = data[1:-1];

        # In case the data has been modified it's always better to use the stripped off
        # version as we have already processed and stored all the possible decorators.    
        self.data = data
        
        # Support multiple operands separated by "/" (only used by r/m style definition).
        for op in data.split("/"):
            op = op.strip();

            # Handle segment specification if this is an implicit register performing a memory access.
            if len(re.findall("(?:ds|es)\:", op)) > 0:
                self.regMem = op[0:2];
                op = op[3:];

            if isRegOp(op):
                self.reg = op;
                self.regClass = regClass(op);
                m = re.findall("r(\d+)", op)
                if len(m) > 0:
                    self.regSize = tryint(m[0]);
                continue;

            if isMemOp(op):
                self.mem = op;

                # Handle memory size.
                m = re.findall("m(?:off)?(\d+)", op);
                self.memSize = 0;
                if len(m) > 0:
                    self.memSize = tryint(m[0]);
                self.memOff = op.find("moff") == 0;

                # Handle vector addressing mode and size "vmXXr".
                m = re.findall("vm(\d+)(x|y|z)", op);
                if len(m) > 0:
                    self.vsibReg = m[1] + "mm";
                    self.vsibSize = tryint(m[0]);

                continue;

            if isImmOp(op):
                self.imm = immSize(op);
                if op == "1":
                    self.implicit = True;
                continue;

            if isRelOp(op):
                self.rel = relSize(op);
                continue;
            
            raise BaseException("Unhandled operand: '" + op + "'");

    def setAccess(self, access):
        self.read  = (access == "R" or access == "X");
        self.write = (access == "W" or access == "X");
        pass
    
    def isReg(self): return self.reg != None
    def isMem(self): return self.mem != None
    def isImm(self): return self.imm > 0
    def isRel(self): return self.rel != None
    def isRegAndMem(self): return self.reg != None and self.mem != None
    def isRegOrMem(self):  return self.reg != None or  self.mem != None

    def toRegMem(self):
        if self.reg != None and self.mem != None:
            return self.reg + "/m";
        elif (self.mem != None and (len(self.vsibReg) > 0 or rfind("fp$|int$", self.mem))):
            return self.mem;
        elif self.mem != None:
            return "m";
        else:
            return self
            
    def __str__(self):
        return self.data;

    def __repr__(self):
        return self.data;
pass   

class X86Inst:
    def __init__(self, name, operands, encoding, opcode, flags):
        self.name     = name
        self.operands = operands
        self.encoding = encoding
        self.opcode   = opcode
        self.flags    = flags
        
        self.arch = "ANY";      # Architecture - ANY, X86, X64.

        self.prefix = "";       # Prefix - "", "3DNOW", "EVEX", "VEX", "XOP".

        self.opcode = "";       # A single opcode byte as a hex string, "00-FF".
        self.opcodeInt = 0;     # A single opcode byte as an integer (0..255).
        self.opcodeString = ""; # The whole opcode string, as specified in manual.

        self.l = "";            # Opcode L field (nothing, 128, 256, 512).
        self.w = "";            # Opcode W field.
        self.pp = "";           # Opcode PP part.
        self.mm = "";           # Opcode MM[MMM] part.
        self.vvvv = "";         # Opcode VVVV part.
        self._67h = False;      # Instruction requires a size override prefix.
        self.rm = "";           # Instruction specific payload "/0..7".
        self.rmInt = -1;        # Instruction specific payload as integer (0-7).
        self.ri = False;        # Instruction opcode is combined with register, "XX+r" or "XX+i".
        self.rel = 0;           # Opcode displacement (cb cw cd parts).

        # Encoding & operands.
        #self.encoding = "";     # Opcode encoding.
        self.operands = [];     # Instruction operands array.
        self.implicit = False;  # Instruction uses implicit operands (registers / memory).
        self.imm = 0;
        # Metadata.
        self.lock = False;      # Can be used with LOCK prefix.
        self.rep = False;       # Can be used with REP prefix.
        self.xcr = "";          # Reads or writes to/from XCR register.
        self.cpu = {};          # CPU features required to execute the instruction.
        self.eflags = {};       # CPU flags read/written/zeroed/set/undefined.

        self.volatile = False;  # Volatile instruction hint for the instruction scheduler.
        self.privilege = 3;     # Privilege level required to execute the instruction.

        self.fpu = False;       # Whether the instruction is a FPU instruction.
        self.mmx = False;       # Whether the instruction is a MMX instruction.
        self.fpuTop = 0;        # FPU top index manipulation [-1, 0, 1, 2].

        self.vsibReg = "";      # AVX VSIB register type (xmm/ymm/zmm).
        self.vsibSize = -1;     # AVX VSIB register size (32/64).

        self.broadcast = False; # AVX-512 broadcast support.
        self.bcstSize = -1;     # AVX-512 broadcast size.
        self.kmask = False;     # AVX-512 merging {k}.
        self.zmask = False;     # AVX-512 zeroing {kz}, implies {k}.
        self.sae = False;       # AVX-512 suppress all exceptions {sae}.
        self.rnd = False;       # AVX-512 embedded rounding {er}, implies {sae}.

        # Instruction element size, used by broadcast, but also defined for all
        # instructions that don't do broadcast. If the element size is ambiguous
        # (e.g. the instruction converts from one size to another) it contains
        # the source operand size, as source is used in memory broadcasts.
        self.elementSize = -1;

        # Every call to report increments invalid counter. Nonzero counter will
        # prevent generating instruction tables for AsmJit.
        self.invalid = 0;
        
        self.assignOperands(operands);
        self.assignOpcode(opcode);
        self.assignFlags(flags);
    pass
    def isAVX(self): return self.isVEX() or self.isEVEX()
    def isVEX(self): return self.prefix == "VEX" or self.prefix == "XOP"
    def isEVEX(self):return self.prefix == "EVEX"
    def getImmCount(self):
        return sum(op == "imm" for op in self.operands)
    pass    
    def assignOperands(self, s):
        if s == None:
            return;

        # First remove all flags specified as {...}. We put them into `flags`
        # map and mix with others. This seems to be the best we can do here.
        while True:
            a = s.find("{");
            b = s.find("}");
            if a == -1 or b == -1:
                break;

            # Get the `flag` and remove from `s`.
            self.assignFlag(s[a + 1: b], True);
            s = s[0: a] + s[b + 1:];

        # Split into individual operands and push them to `operands`.
        m_a = "X"
        for data in s.split(","):
            data = data.strip()
            operand = X86Operand(data, m_a);
            m_a = "R"

            # Propagate broadcast.
            if operand.bcstSize > 0:
                self.assignFlag("broadcast", operand.bcstSize);

            # Propagate implicit operand.
            if operand.implicit != None:
                self.implicit = True;

            # Propagate VSIB.
            if operand.vsibReg != None:
                if len(self.vsibReg) > 0:
                    raise BaseException("Only one operand can be vector memory address (vmNNx)");

                self.vsibReg  = operand.vsibReg;
                self.vsibSize = operand.vsibSize;

            self.operands.append(operand);
    pass
    def assignFlag(self, name, value):
        # Basics.
        if name in kCpuArchitecture:
            self.arch = name
            return
        if name in kCpuFeatures:
            self.cpu[name] = True
            return
        if kCpuFlags.has_key(name):
            self.eflags[name] = value
            return

        # Split AVX-512 flag having "-VL" suffix (shorthand) into two flags.
        if len(re.findall("AVX512\w+-VL", name)) > 0 and name[0:-3] in kCpuFeatures:
            cpuFlag = name[0: -3];
            self.cpu[cpuFlag] = true;
            self.cpu.AVX512VL = true;
            return;

        if name == "LOCK": self.lock     = True; return;
        if name == "REP" : self.rep      = True; return;
        if name == "FPU" : self.fpu      = True; return;
        if name == "XCR" : self.xcr      = value; return;
        if name == "kz"  : self.zmask    = True; # fall: {kz} implies {k}.
        if name == "k"   : self.kmask    = True; return;
        if name == "er"  : self.rnd      = True; # fall: {er} implies {sae}.
        if name == "sae" : self.sae      = True; return;

        if name == "VOLATILE" : self.volatile = True; return;
        if name == "PRIVILEGE": 
            if len(re.findall("L[0123]", value)) == 0:
                raise BaseException(self.name + ": Invalid privilege level '" + value + "'")
            self.privilege = tryint(value[1:]);
            return

        if name == "broadcast":
            self.broadcast   = true;
            self.elementSize = value;
            return;

        if name == "FPU_PUSH" : self.fpu = true; self.fpuTop = -1; return;
        if name == "FPU_POP"  : self.fpu = true; self.fpuTop = tryint(value); return;
        if name == "FPU_TOP"  :
            self.fpu = true;
            if value == "-1":
                self.fpuTop =-1;
                return;
            if value == "+1":
                self.fpuTop = 1;
                return;
            return;
        raise BaseException(self.name + ": Unhandled flag " + name + "=" + value)
    pass
    def assignFlags(self, s):
        # Parse individual flags separated by spaces.
        for flag in s.split(" "):
            flag = flag.strip();
            if len(flag) > 0:
                j = flag.find("=");
                if j != -1:
                    self.assignFlag(flag[0: j], flag[j + 1:]);
                else:
                    self.assignFlag(flag, True);
    pass
    def assignOpcode(self, s):
        self.opcodeString = s;
        parts = s.split(" ");

        if rfind("(EVEX|VEX|XOP)\.", s):
            # Parse VEX and EVEX encoded instruction.
            prefix = parts[0].split(".");
            for comp in prefix:
                # Process "EVEX", "VEX", and "XOP" prefixes.
                if rfind("(?:EVEX|VEX|XOP)", comp): self.prefix = comp; continue;
                
                # Process "NDS/NDD/DDS".
                if rfind("(?:NDS|NDD|DDS)", comp): self.vvvv = comp; continue;

                # Process `L` field.
                if rfind("^LIG$"      , comp): self.l = "LIG"; continue;
                if rfind("^128|L0|LZ$", comp): self.l = "128"; continue;
                if rfind("^256|L1$"   , comp): self.l = "256"; continue;
                if rfind("^512$"      , comp): self.l = "512"; continue;

                # Process `PP` field - 66/F2/F3.
                if comp == "P0":
                    # ignored, `P` is zero...
                    continue;
                if rfind("^(?:66|F2|F3)$", comp): self.pp = comp; continue;

                # Process `MM` field - 0F/0F3A/0F38/M8/M9.
                if rfind("^(?:0F|0F3A|0F38|M8|M9)$", comp): self.mm = comp; continue;

                # Process `W` field.
                if rfind("^WIG|W0|W1$", comp): self.w = comp; continue;

                # ERROR.
                raise BaseException("'" + self.opcodeString + "' Unhandled component: " + comp);

            for i in range(1, len(parts)):
                comp = parts[i];
                # Parse opcode.
                if rfind("^[0-9A-Fa-f]{2}$", comp):
                    self.opcode = comp.upper();
                    continue;

                # Parse "/r" or "/0-7".
                if rfind("^\/[r0-7]$", comp):
                    self.rm = comp[1:];
                    continue;

                # Parse immediate byte, word, dword, or qword.
                if rfind("^(?:ib|iw|id|iq|\/is4)$", comp):
                    self.imm += immSize(comp);
                    continue;

                raise BaseException("'" + self.opcodeString + "' Unhandled opcode component " + comp + ".");
        else:
            # Parse X86/X64 instruction (including legacy MMX/SSE/3DNOW instructions).
            for comp in parts:
                # Parse REX.W prefix.
                if comp == "REX.W":
                    self.w = "W1";
                    continue;
                   
                # ((self.mm == "" and ((self.pp === "" and /^(?:66|F2|F3)$/.test(comp)) or (self.pp === "66" and /^(?:F2|F3)$/.test(comp)))))
                # Parse `PP` prefixes. !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if ((self.mm == "" and ((self.pp == "" and rfind("^(?:66|F2|F3)$", comp)) or (self.pp == "66" and rfind("^(?:F2|F3)$", comp))))):
                    self.pp += comp;
                    continue;

                # Parse `MM` prefixes.
                if (self.mm == "" and comp == "0F") or (self.mm == "0F" and rfind("^(?:01|3A|38)$", comp)):
                    self.mm += comp;
                    continue;

                # Recognize "0F 0F /r XX" encoding.
                if self.mm == "0F" and comp == "0F":
                    self.prefix = "3DNOW";
                    continue;

                # Parse opcode byte.
                if rfind("^[0-9A-F]{2}(?:\+[ri])?$", comp):
                    # Parse "+r" or "+i" suffix.
                    if len(comp) > 2:
                        self.ri = True;
                        comp = comp[0:2];

                    # Some instructions have form 0F AE XX, we treat the last byte as an opcode.
                    if self.mm == "0F" and self.opcode == "AE":
                        self.mm += self.opcode;
                        self.opcode = comp;
                        continue;

                    # FPU instructions are encoded as "PREFIX XX", where prefix is not the same
                    # as MM prefixes used everywhere else. AsmJit internally extends MM field in
                    # instruction tables to allow storing this prefix together with other "MM"
                    # prefixes, currently the unused indexes are used, but if X86 moves forward
                    # and starts using these we can simply use more bits in the opcode DWORD.
                    if self.pp == None and self.opcode == "9B":
                        self.pp = self.opcode;
                        self.opcode = comp;
                        continue;

                    if self.mm == None and rfind("^(?:D8|D9|DA|DB|DC|DD|DE|DF)$", self.opcode):
                        self.mm = self.opcode;
                        self.opcode = comp;
                        continue;

                    if self.opcode != "":
                        if self.opcode == "67":
                            self._67h = True;
                        else:
                            raise BaseException("'" + self.opcodeString + "' Multiple opcodes, have " + self.opcode + ", found " + comp + ".");

                    self.opcode = comp;
                    continue;

                # Parse "/r" or "/0-7".
                if rfind("^\/[r0-7]$", comp) and self.rm == "":
                    self.rm = comp[1:];
                    continue;

                # Parse immediate byte, word, dword, or qword.
                if rfind("^(?:ib|iw|id|iq)$", comp):
                    self.imm += immSize(comp);
                    continue;

                # Parse displacement.
                if rfind("^(?:cb|cd)$", comp) and self.rel == 0:
                    self.rel = 4
                    if comp == "cb":
                        self.rel = 1
                    continue;

                # ERROR.
                raise BaseException("'" + self.opcodeString + "' Unhandled opcode component " + comp + ".");

        # QUIRK: Fix instructions having opcode "01".
        if self.opcode == "" and self.mm.find("0F01") == len(self.mm) - 4:
            self.opcode = "01";
            self.mm = self.mm[0: -2];

        if self.opcode != None:
            self.opcodeInt = tryint(self.opcode, 16);

        if rfind("^\/[0-7]$", self.rm):
            self.rmInt = tryint(self.rm[1:]);

        if self.opcode == None:
            raise BaseException("'" + self.opcodeString + "' Couldn't parse instruction's opcode.");
    pass
    
    # Validate the instruction's definition. Common mistakes can be checked and
    # reported easily, however, if the mistake is just an invalid opcode or
    # something else it's impossible to detect.
    def validate(self):
        isValid = True;
        immCount = self.getImmCount();

        # Verify that the immediate operand/operands are specified in instruction
        # encoding and opcode field. Basically if there is an "ix" in operands,
        # the encoding should contain "I".
        if immCount > 0:
            immEncoding = "".rjust(immCount, "I");

            # "I" or "II" should be part of the instruction encoding.
            if self.encoding.find(immEncoding) == -1:
                isValid = False;
                raise BaseException("Immediate(s) [" + str(immCount) + "] missing in encoding: " + self.encoding);

            # Every immediate should have it's imm byte ("ib", "iw", "id", or "iq")
            # in opcode data.
            #m = self.opcodeString.match(/(?:^|\s+)(ib|iw|id|iq)/g);
            #if (!m || m.length !== immCount) {
            #    isValid = false;
            #    raise BaseException("Immediate(s) [" + immCount + "] not found in opcode: " + self.opcodeString);

        return isValid;
    pass

    def __repr__(self):
        return self.name + " " + self.operands + " " + self.encoding + " " + self.opcode + " " + self.flags
    pass
    def __str__(self):
        return self.name + " " + self.operands + " " + self.encoding + " " + self.opcode + " " + self.flags
    pass
pass 

# X86 instruction database - stores X86Inst instances in a map and aggregates
# all instructions with the same name.
class X86Database:
    def __init__(self, instructions):
        # Instructions in a map, mapping an instruction name into an array of
        # all instructions defined for that name.
        self.map = { };

        # Instruction statistics.
        self.insts = 0  # Number of all instructions.
        self.groups= 0  # Number of grouped instructions (having unique name).
        self.avx   = 0  # Number of AVX instructions.
        self.xop   = 0  # Number of XOP instructions.
        self.evex  = 0  # Number of EVEX instructions.

        for instData in instructions:
            for instName in instData[kIndexName].split("/"):
                instObj = X86Inst(
                    instName,
                    instData[kIndexOperands],
                    instData[kIndexEncoding],
                    instData[kIndexOpcode],
                    instData[kIndexFlags]);
                instObj.validate();
                self.addInstruction(instObj);
    pass

    def addInstruction(self, inst):
        if not self.map.has_key(inst.name):
            self.map[inst.name] = [];
            self.groups += 1;
        
        self.map[inst.name].append(inst)
        self.insts += 1;

        # Misc s_
        if inst.prefix == "VEX":
            self.avx += 1;
        if inst.prefix == "XOP":
            self.xop += 1;
        if inst.prefix == "EVEX":
            self.evex += 1;
    pass
    
    def __repr__(self):
        return "Groups: %i, Instructions: %i, AVX: %i, XOP: %i, EVEX: %i" % (self.insts, self.groups, self.avx, self.xop, self.evex)
    pass
pass       
