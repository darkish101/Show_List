GO
/****** Object:  StoredProcedure [dbo].[sp_SET_Translation]    Script Date: 2021-01-27 4:58:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
 
-- =============================================    
CREATE PROCEDURE [dbo].[sp_SET_Translation]     
 -- Add the parameters for the stored procedure here    
 @pModule_ID  VARCHAR(4) = 'Show',    
 @pActivity_ID VARCHAR(500),    
 @pOBJECT_NAME VARCHAR(500),    
 @pLanguage  VARCHAR(5),    
 @pCONTROL_TYPE VARCHAR(500),    
 @pDEFAULT_LABEL NVARCHAR(500),    
 @pLABEL   NVARCHAR(500)    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    -- Insert statements for procedure here    
 IF NOT EXISTS (SELECT 1 FROM     
    LANGUAGE_TRANSLATION T     
    WHERE     
         T.MODULE_ID = @pModule_ID     
     AND t.ACTIVITY_ID= @pActivity_ID    
     AND t.[OBJECT_NAME] = @pOBJECT_NAME    
     AND t.[LANGUAGE] = @pLanguage    
    )      
 BEGIN    
    INSERT INTO LANGUAGE_TRANSLATION (MODULE_ID, ACTIVITY_ID, [OBJECT_NAME], [LANGUAGE], CONTROL_TYPE, DEFAULT_LABEL, [LABEL])     
    VALUES (@pModule_ID, @pActivity_ID, @pOBJECT_NAME, @pLanguage, @pCONTROL_TYPE, @pDEFAULT_LABEL, @pLABEL)    
 END    
END 



