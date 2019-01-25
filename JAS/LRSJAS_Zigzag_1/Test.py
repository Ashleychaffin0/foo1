import clr
clr.AddReference("System")
# clr.AddReference("System.Net")

from System import *
from System.Net import *

import sys

def Simple():
	print "Hello from Python"
	print "Call Dir(): "
	print dir()
	print "Print the Path: " 
	print sys.path

def GetUri(uri):
	wc = WebClient()
	return wc.DownloadString(uri)
	# return Environment.CurrentDirectory
