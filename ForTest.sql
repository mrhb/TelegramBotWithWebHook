-- SQLite
INSERT INTO tblBotMessage
        (
            fldMobileNumberOrId
            , fldMes
            , ImageData
            ,fldOK  
            , flddate
            ,fldTime
        )
     VALUES
           ("KosarRB"
           ,"salam ...."
           ,""
           ,0
           ,"1400/4/19"
           ,"11:50")
           , ("09151575793"
           ,"By  ...."
           ,""
           ,0
           ,"1400/4/19"
           ,"11:50")
GO 
SELECT fldid, fldMobileNumberOrId, fldMes, ImageData, fldOK, fldTime, flddate
FROM tblBotMessage;
