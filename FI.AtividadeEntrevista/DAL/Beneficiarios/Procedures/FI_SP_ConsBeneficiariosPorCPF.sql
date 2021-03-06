﻿CREATE PROC FI_SP_ConsBeneficiariosPorCPF
	@CPF BIGINT
AS
BEGIN
	IF(ISNULL(@CPF,0) = 0)
		SELECT ID, NOME, CPF, IDCLIENTE FROM BENEFICIARIOS WITH(NOLOCK)
	ELSE
		SELECT ID, NOME, CPF, IDCLIENTE FROM BENEFICIARIOS WITH(NOLOCK) WHERE CPF = @CPF
END