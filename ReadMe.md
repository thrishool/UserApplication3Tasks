# List of Questions with their answers
#Question 1:
#1. appsettings.json is the file with config settings to create data file "BasePath": "C:\\Developer" 
#2. UserController.cs is having the logic for post type of request.
	SaveUserData is the method which will get called from swagger/rest api call.
	IDataStorageService - which is having logic to validate and save users in the file.
	FileExtension - Custom class to define Extension method for Filename.
#3. Program.cs -> builder.Services.AddScoped<IDataStorageService, DataStorageService>(); register the service here.

#Question 2:
AnswerForQuestion2.html you can open this file directly in browser and check the output in browser console

#Question 3:
AnswerForQuestion3.sql to Create Model table and 2 solutions for deleting duplicates while keeping max id element.