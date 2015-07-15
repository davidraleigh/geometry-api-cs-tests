from tempfile import mkstemp
from shutil import move
from os import remove, close
import glob
import re

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

def is_double_declare(line):
  #sample: " for (int i = 0, n = b.size(); i < gorilla; i++) {"
  match = re.match(r'[ \t]*(for)\s*[(]\s*[\w]*[\s]*[\w]+[\s]*[=][\s]*[\w.()]+[,][\s]*[\w]+[\s]*[=][\s]*[\w.()]+[;]', line)
  # if the line is a for loop with a double declaration
  if match:
    return True
  return False

def get_second_declare(line):
  # grab the portion to remove
  m = re.search('[,][\s]*[\w]+[\s]*[=][\s]*[\w.()]+(?=;)', line)
  # remove includes comma
  # ", n = b.size()"
  return m.group(0)

def get_type(line):
  # search for a type in the declaration 
  m = re.search('(?<=[(])\s*[\w]+(?=[\s]+[\w]+[\s]*[=][\s]*[\S]+[,])', line)
  var_type = ''
  if m:
    # "int "
    var_type = m.group(0)

  return var_type

def get_white_space(line):
  white_space = ''
  m = re.search('[ \t]+(?=for)', line)
  if m:
    # " "
    white_space = m.group(0) 
  return white_space

def get_declaration_line(line):
  remove = get_second_declare(line)
  white_space = get_white_space(line)
  var_type = get_type(line)
  declaration_line = white_space + var_type + remove[1:] + ';\n'

  return declaration_line

def get_cleaned_for(line):
  remove = get_second_declare(line)
  for_line = line.replace(remove, '')

  return for_line

def bracket_count(line, previous_count):
  for character in line:
    if character is '{':
      previous_count += 1
    elif character is '}':
      previous_count -= 1
  return previous_count

def rewrite_for(new_file, line):

  if is_double_declare(line):
    new_line = ''
    
    new_file.write(get_white_space(line) + '{\n')

    declaration_line = get_declaration_line(line)
    new_file.write('    ' + declaration_line)

    for_line = get_cleaned_for(line)
    new_file.write('    ' + for_line)

    return bracket_count(for_line, 0)

def rewrite_lines(file_path):
  #Create temp file
  fh, abs_path = mkstemp()
  with open(abs_path,'w') as new_file:
    with open(file_path) as old_file:
      # the current bracket count of the double declared for loop
      count = 0
      first_not_found = False
      # white space 
      space =''
      for line in old_file:
        # if the line is a for loop
        if is_double_declare(line):
          # write the bracket, declaration and the for line
          # {
          # int n = hose
          # for (int i = blah blah blah)
          count = rewrite_for(new_file, line)
          space = get_white_space(line)
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
  close(fh)
  #Remove original file
  remove(file_path)
  #Move new file
  move(abs_path, file_path)

files = glob.glob("./javaConversions/geometry-api-java-master/src/main/java/com/esri/core/geometry/*.java")

for f in files:
  rewrite_lines(f)
