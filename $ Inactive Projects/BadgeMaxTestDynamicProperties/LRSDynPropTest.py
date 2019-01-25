import clr
import sys
import System

print "\n==========================\n"

# print "dir() = ", dir()
# print "sys.path =", sys.path

# print "Hello world"
# print "--name-- = ", __name__, ", length = ", __name__.__len__()
# print "sys.argv", sys.argv
# print "dir() = ", dir()

# print "dir(__builtins__) =", dir(__builtins__)
# print "dir(sys) =", dir(sys)
# print "sys.modules =", sys.modules
# print "dir(CommonLibrary) =", dir(CommonLibrary)

# BM2 = net.signup4.www.BadgeMaxField()
# BM2.FieldName = "TestName"
# BM2.Data = "My test data"
# print "BM2Field.FieldName = ", BM2.FieldName, ", Data = ", BM2Data

# print "BadgeData: ", globals()["BadgeData"]
print "Dumping globals()"
for g in globals(): print "g.Key=", g.Key, ", g.Value=", g.Value, ", dir(g)=", dir(g), ", g=",g
print "End dumping globals()"

def main():
    print "Hello from main()"

def LRSSub():
	print "Hi from LRSSub"

def LRSSub2(*args):
	print "Hi from LRSSub 2 - args =", args

def DumpGlobals():
	print "In DumpGlobals - Dumping globals()"
	for g in globals(): print "g.Key=", g.Key, ", g.Value=", g.Value, ", dir(g)=", dir(g), ", g=",g
	print "End dumping globals()"

def LRSShowLbl():
	DumpGlobals()
	# global	lbl #, SwipeData
	# print "LRSShowLbl - ", globals()["lbl"]
	# print "SwipeData[FName] = ", SwipeData["FName"]
	print "LRSShowLbl - ", lbl

if __name__== "__main__":
	print "About to call main()"
	main()
	print "Back from calling main()"

