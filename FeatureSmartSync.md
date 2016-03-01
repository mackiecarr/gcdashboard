## Feature:  Smart Sync ##

Importing the entire GnuCash file is the easy way to load the data, but it is also very time consuming.  If we could perform a diff against the last loaded xml file and the current xml file, we could see what changes have been made and then only load those changes.

## See ##

Microsoft has an API for performing diffs against two xml files:

http://msdn.microsoft.com/en-us/library/aa302294.aspx