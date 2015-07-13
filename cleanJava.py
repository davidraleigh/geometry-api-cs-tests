from tempfile import mkstemp
from shutil import move
from os import remove, close
import re

def for_rewrite(new_file, line):
  #sample: " for (int i = 0, n = b.size(); i < gorilla; i++) {"
  match = re.match(r'[ \t]*(for)\s*[(]\s*[\w]*[\s]*[\w]+[\s]*[=][\s]*[\w.()]+[,][\s]*[\w]+[\s]*[=][\s]*[\w.()]+[;]', line)
  # if the line is a for loop with a double declaration
  if match:
    print line
    new_line = ''
    # grab the portion to remove
    m = re.search('[,][\s]*[\w]+[\s]*[=][\s]*[\w.()]+(?=;)', line)
    # remove includes comma
    # ", n = b.size()"
    remove = m.group(0)
    # search for a type in the declaration 
    m = re.search('(?<=[(])\s*[\w]+(?=[\s]+[\w]+[\s]*[=][\s]*[\S]+[,])', line)
    var_type = ''
    if m:
      # "int "
      var_type = m.group(0)

    white_space = ''
    m = re.search('[ \t]+(?=for)', line)
    if m:
      # " "
      white_space = m.group(0)


    # TODO it would be great to grab the white spacing from in front of the for statement
    declaration_line = white_space + var_type + remove[1:] + ';\n'
    new_file.write(declaration_line)
    print declaration_line

    for_line = line.replace(remove, '')
    new_file.write(for_line)
    print for_line
    return True
  return False

def rewrite_lines(file_path):
  #Create temp file
  fh, abs_path = mkstemp()
  with open(abs_path,'w') as new_file:
    with open(file_path) as old_file:
      for line in old_file:
        if not for_rewrite(new_file, line):
          new_file.write(line)
  close(fh)
  #Remove original file
  remove(file_path)
  #Move new file
  move(abs_path, file_path)


rewrite_lines('./javaConversions/geometry-api-java-master/src/main/java/com/esri/core/geometry/Envelope.java')
