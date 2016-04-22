import re, os
import x86db

kCpuRegs = {"reg" : [
        "r8", "r16", "r32", "r64", "reg", "rxx",
        "al", "ah" , "ax" , "eax", "rax", "zax",
        "bl", "bh" , "bx" , "ebx", "rbx", "zbx",
        "cl", "ch" , "cx" , "ecx", "rcx", "zcx",
        "dl", "dh" , "dx" , "edx", "rdx", "zdx",
        "di", "edi", "rdi", "zdi",
        "si", "esi", "rsi", "zsi",
        "bp", "ebp", "rbp", "zbp",
        "sp", "esp", "rsp", "zsp"
    ],
    "sreg": ["sreg" , "cs", "ds", "es", "fs", "gs", "ss"],
    "creg": ["creg" , "cr0-8"  ],
    "dreg": ["dreg" , "dr0-7"  ],
    "st"  : ["st(0)", "st(i)"  ],
    "mm"  : ["mm"   , "mm0-7"  ],
    "k"   : ["k"    , "k0-7"   ],
    "xmm" : ["xmm"  , "xmm0-31"],
    "ymm" : ["ymm"  , "ymm0-31"],
    "zmm" : ["zmm"  , "zmm0-31"]
}

def isRegOp(s):
    return sum(s in x for x in kCpuRegs.values())>0

print(isRegOp("eaxa"))