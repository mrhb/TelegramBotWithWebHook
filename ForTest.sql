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
           ("@kosarRB"
           ,"salam ...."
           ,""
           ,0
           ,"1400/4/19"
           ,"11:50")
           , ("@kosarRB"
           ,"By  ...."
           ,""
           ,0
           ,"1400/4/19"
           ,"11:50")
GO 
SELECT fldid, fldMobileNumberOrId, fldMes, ImageData, fldOK, fldTime, flddate
FROM tblBotMessage;
