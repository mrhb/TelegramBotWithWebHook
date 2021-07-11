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
   SELECT '09151575793'
           ,'Hello   ....'
           ,ImageFile
           ,0
           ,'1400/4/20'
           ,'11:50'
           FROM OPENROWSET(BULK N'D:\tux.png', SINGLE_BLOB) AS ImageSource(ImageFile);

SELECT fldid, fldMobileNumberOrId, fldMes, ImageData, fldOK, fldTime, flddate
FROM tblBotMessage;
