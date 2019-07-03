# i18n
A tool for parsing and translating HTML files (including creation of PO/POT that translators can use)

## Usage

### Necessities
This tool is built on top of .NET Core 2.2 and will require the [sdk](https://dotnet.microsoft.com/download/dotnet-core/2.2) to be installed.  
An Executable of the tool can be found under the "Dit" folder.

### Arguments
The tools arguments are as follow: 
1. **path** (p) - The html file path (required).
2. **output** (o) – Name of the output folder (optional, will default to “output”). 
3. **source** (s) – The language that was used in the html file (optional, will default to “en-us”).
4. **target** (t) – The language that we want to use for the translation (required). 

### Output
The tool wil process the HTML file and create the following files:
1. **I18n HTML Template** - an HTML based on the source containing unique Ids that will be used in the translation process.
2. **POT File** - A reference file for translators containing the original text parsed from the HTML.
3. **PO File** (Source Language) - A PO file (translation) that serves as a backup of the original text parsed from the HTML, in case the tool would be ran in reversed translation (starting from Japanese to English and then executed again with English to Japanese).
4. **PO File** (Target Language) - A PO (translation) file containing the translated values (specified target language) that a translator can use.
5. **HTML File** - The translated HTML file in the specified target language.

## Application Flow

1.	Tool executed the 1st time on a new translated language:  
•	Execute tool.  
•	Parse HTML.  
•	Generate Unique Ids (new i18n html template, PO file in source language and POT file).  
•	Extract values from the PO file (for source language).  
•	Translate Values using Google Translate.  
•	Create PO file for the target language.   
•	Update the HTML using the i18n html template with the translation values.  
•	Save the new translated HTML file. 

2.	Tool executed the 2nd time on the same translated language:  
•	Execute tool.  
•	Parse HTML.  
•	Generate Unique Ids (new i18n html template, PO file in source language and POT file).  
•	Extract values from the PO file (for target language).  
•	Update the HTML using the i18n html template with the translation values.   
•	Save the new translated HTML file. 

![alt text](https://i.ibb.co/dPYQSzH/Blank-Diagram.png)
