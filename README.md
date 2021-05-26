=================================================
*** Welcome to Supplies Price Lister solution ***
*** Author: Scott SHI
*** Time: 26 May 2021
=================================================

This solution is responsible for reading supplies price information from input files and print them in price descending order.

Following is structure of project:

	\Model
	\ViewModel
	\Utility
	\Log
	\Program.cs
	\appsettings.json

*In Model folder, 2 classes are created for mapping supplies information from input files: humphries.csv and megacorp.json
 This is a strict mapping from input records to instancess. 

*In ViewModel folder, the view model is created to be align to the output format in requirement.

*In Utility folder, there are 2 static classes created. LogWriter.cs is used to write information/error into log file for debug.
 SupplyProcessor.cs is used to read input file, process lists and output sorted list into console.

*In Log folder, users can check the log in log.txt file for execution details.

*The Program.cs is the entry point of this project.

*The appsettings.json includes configurations, such as input file path, log output path, and exchange rate.
