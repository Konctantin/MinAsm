import InstrParser
import variants

import os,re

class CSharpClassGenerator:
    def __init__(self, db):
        self.db = db
    pass

    def GetFileContent(self, mnem, instrs):
        return ""
    pass

    def DelFiles(self, directory, pattern):
        if not os.path.exists(directory):
            os.makedirs(directory);
            print("Created directory: " + directory)

        for f in os.listdir(directory):
            if re.search(pattern, f):
                os.remove(os.path.join(directory, f))
    pass

    def generate(self, outputFolder):
        self.DelFiles(outputFolder, "Opcode_(\w+).gen.cs");
        for mnem in sorted(self.db.map.keys()):
            instrs = self.db.map[mnem];
            try:
                fileName = "Opcode_%s.gen.cs" % mnem;
                fullFileName = os.path.join(directory, fileName);
                content = self.GetFileContent(mnem, instrs);

                print("Create file: " + fileName)
                #f = open(fullFileName, "w+")
                #f.write(content)
                #f.close()
            except BaseException as ex:
                print(mnem, ex)             
    pass
pass

class CSharpTestsGenerator:
    def __init__(self, db):
        self.db = db
    pass
pass 

instr = variants.instructions
db = InstrParser.X86Database(instr)
classGen = CSharpClassGenerator(db)
classGen.generate("E:\\temp\\testGen\\")
