sqlcmd -S (localdb)\MSSQLLocalDB -i "%cd%\SchoolGradesDB.sql" -v input="%cd%"
