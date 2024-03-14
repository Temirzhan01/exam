declare 
cur OUT SYS_REFCURSOR;

TYPE myType is RECORD(
     CHOOSE_PRC VARCHAR2(100)
);

my myType;

BEGIN 
     dbms_output.enable;
     kzc_bwx.opt_spm.CHOOSE_PRC_FOR_IBFL@PROD.KZC('010904500574', cur);
     fetch cur into my;
     dbms_output.put_line('res' || my.CHOOSE_PRC);
     close cur;
END;
