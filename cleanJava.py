from tempfile import mkstemp
from shutil import move
from os import remove, close
import glob
import re

# 4 spaces of indentation
SPACING = '    '

def replace_line(file_path, pattern, subst, line_number):
  #Create temp file
  fh, abs_path = mkstemp()
  with open(abs_path,'w') as new_file:
    with open(file_path) as old_file:
      count = 1
      for line in old_file:
        if count == line_number:
          new_file.write(line.replace(pattern, subst))
        else:
          new_file.write(line)
        count += 1

  close(fh)
  #Remove original file
  remove(file_path)
  #Move new file
  move(abs_path, file_path)

# is multiple initialization
def is_multiple_initialized(line):
  #sample: " for (int i = 0, n = b.size(); i < gorilla; i++) {"
  match = re.match(r'[ \t]*(for)\s*[(]\s*[\w]*[\s]*[\w]+[\s]*[=][\s]*[\w.()]+[,][\s]*[\w]+[\s]*[=][\s]*[\w.()]+[;]', line)
  # if the line is a for loop with a multiple initialization
  if match:
    return True
  return False

def get_extra_initializors(line):
  # grab initializations after first initialization
  m = re.findall('(?<=[,])[\s]*[\w]+[\s]*[=][\s]*[\w.()]+(?=[;,])', line)
  # remove includes comma
  # ", n = b.size()"
  return m

def get_declaration_type(line):
  # search for a type in the declaration 
  m = re.search('(?<=[(])\s*[\w]+(?=[\s]+[\w]+[\s]*[=][\s]*[\S]+[,])', line)
  var_type = ''
  if m:
    # "int "
    var_type = m.group(0)

  return var_type

def get_indentation(line):
  white_space = ''
  m = re.search('[ \t]+(?=for)', line)
  if m:
    # " "
    white_space = m.group(0) 
  return white_space

def create_initializor_lines(line):
  initializor_lines = ''
  initializors = get_extra_initializors(line)
  white_space = get_indentation(line)
  var_type = get_declaration_type(line)
  for initializor in initializors:
    initializor = initializor.strip()
    if len(var_type) is 0:
      initializor_lines += white_space + SPACING + initializor + ';\n'
    else:
      initializor_lines += white_space + SPACING + var_type + ' ' + initializor + ';\n'

  return initializor_lines

# get single initialized for line from multiple initialized for line
def create_simple_for_line(line):
  m = re.search('([,][\s]*[\w]+[\s]*[=][\s]*[\w.()]+)+(?=[;,])', line)
  return SPACING + line.replace(m.group(0), '')

# accumulate bracket count 
def bracket_count(line, previous_count):
  for character in line:
    if character is '{':
      previous_count += 1
    elif character is '}':
      previous_count -= 1
  return previous_count

def rewrite_for(new_file, line):
  if is_multiple_initialized(line):
    new_line = ''
    # create scoping bracket
    new_file.write(get_indentation(line) + '{\n')
    # write all separated initializor lines
    new_file.write(create_initializor_lines(line))
    # write new for loop start
    new_file.write(create_simple_for_line(line))
    
    return bracket_count(line, 0)

def rewrite_file(file_path):
  #Create temp file
  fh, abs_path = mkstemp()
  with open(abs_path,'w') as new_file:
    with open(file_path) as old_file: 
      rewrite_lines(new_file, old_file, 0)
  close(fh)
  #Remove original file
  remove(file_path)
  #Move new file
  move(abs_path, file_path)      

def rewrite_lines(new_file, old_file, start_line):
  # the current bracket count of the multiple declared for loop
  count = 0
  first_not_found = False
  # white space 
  space =''
  for i, line in enumerate(old_file):
    #line = old_file[i]
    #print line
    # if the line is a for loop
    if is_multiple_initialized(line):
      # write the bracket, declaration and the for line
      # {
      # int n = hose
      # for (int i = blah blah blah)
      count = rewrite_for(new_file, line)
      space = get_indentation(line)
      if count is 0:
        first_not_found = True
    elif count != 0 or first_not_found:
      new_file.write('    ' + line)
      count = bracket_count(line, count)

      # handle case where there are no brackets
      if first_not_found and count == 0:
        if line.strip()[-1] == ';':
          new_file.write(space + '}\n')
          first_not_found = False
        continue
        
      first_not_found = False
      
      if count is 0:
        new_file.write(space + '}\n')
    else:
      new_file.write(line)




files = glob.glob("./javaConversions/geometry-api-java-master/src/main/java/com/esri/core/geometry/*.java")

for f in files:
  rewrite_file('./javaConversions/geometry-api-java-master/src/main/java/com/esri/core/geometry/Clipper.java')
