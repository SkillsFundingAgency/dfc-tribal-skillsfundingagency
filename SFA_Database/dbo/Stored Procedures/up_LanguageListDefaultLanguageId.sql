CREATE PROCEDURE [dbo].[up_LanguageListDefaultLanguageId]
     
AS
/*
*    Name:       [up_LanguageGetDefaultLanguageId]
*    System:     Stored procedure interface module
*    Description:   Gets the default language Id
*
*    Return Values:    0 = No problem detected
*            1 = General database error.
*    Copyright:    (c) Tribal Data Solutions Ltd, 2012
*            All rights reserved.
*
*    $Log:  $
*/ 

SELECT LanguageID FROM [Language] L WHERE L.IsDefaultLanguage = 1
 
IF @@ERROR <> 0
BEGIN
    RETURN 1
END
 
RETURN 0