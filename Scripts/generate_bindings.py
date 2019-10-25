import os, shutil, ntpath, glob, sys

# move to source directory
os.chdir(os.path.dirname(os.path.abspath(__file__)))

# copy C++ Binding Generator scripts
targetPath = '../Core/scripts/bindings'
outputPath = "./Bindings"

if os.path.exists(outputPath):
    print('Directory {} exists'.format(outputPath))
    shutil.rmtree(outputPath)
    print('Removed directory {}'.format(outputPath))

shutil.copytree(targetPath, outputPath)
print('Copied directory {} to {}'.format(targetPath, outputPath))


from Bindings import define
from Bindings.CppBindingGenerator import BindingGeneratorCSharp

# generate C# binding
args = sys.argv
lang = 'en'
if len(args) >= 3 and args[1] == '-lang':
    if args[2] in ['ja', 'en']:
        lang = args[2]
    else:
        print('python csharp.py -lang [ja|en]')

bindingGenerator = BindingGeneratorCSharp(define, lang)
bindingGenerator.output_path = '../Engine/Core.cs'
bindingGenerator.dll_name = 'Altseed_Core.dll'
bindingGenerator.namespace = 'asd'
bindingGenerator.generate()
