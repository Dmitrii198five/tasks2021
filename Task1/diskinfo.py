import subprocess
import json
import sys

if len(sys.argv) < 2: 
	print("не указан файл, содержащий текстовую строку – указание пути к дисковому устройству в системе")
	sys.exit()

with open(sys.argv[1]) as inputFile:

	inputdev = inputFile.readline().rstrip('\n')

	output = subprocess.check_output("lsblk -bJ --paths -o path,type,size,fsavail,fstype,mountpoint", shell=True)

	data = json.loads(output)

	for p in data['blockdevices']:
		if inputdev == p['path']:
			if p['type'] in ("part", "lvm") and p['fsavail'] != None:
				print(p['path'] + " " + p['type']+ " " + str(int(round(int(p['size'])/1024**3,0)))+ "G " + str(int(round(int(p['fsavail'])/1024**2,0)))  + "M " + str(p['fstype'])+ " " + str(p['mountpoint']))
			else:
				print(p['path'] + " " + p['type']+ " " + str(int(round(int(p['size'])/1024**3,0)))+ "G")
