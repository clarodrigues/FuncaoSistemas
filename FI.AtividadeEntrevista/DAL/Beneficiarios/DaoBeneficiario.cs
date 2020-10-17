using System.Collections.Generic;
using System.Data;
using System.Linq;
using FI.AtividadeEntrevista.DML;

namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Classe de acesso a dados do Beneficiário
    /// </summary>
    internal class DaoBeneficiario : AcessoDados
    {
        /// <summary>
        /// Inclui novo beneficiário
        /// </summary>
        /// <param name="beneficiario"></param>
        /// <returns></returns>
        internal long Incluir(DML.Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IdCliente", beneficiario.IdCliente));

            DataSet ds = base.Consultar("FI_SP_IncBeneficiario", parametros);

            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Consulta um beneficiário por ID
        /// </summary>
        /// <param name="IdCliente"></param>
        /// <returns></returns>
        internal DML.Beneficiario Consultar(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("@ID", Id));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<DML.Beneficiario> listBen = Converter(ds);

            return listBen.FirstOrDefault();
        }

        /// <summary>
        /// Consulta dos beneficiários de um cliente
        /// </summary>
        /// <param name="IdCliente"></param>
        /// <returns></returns>
        internal List<DML.Beneficiario> ConsultarBeneficiariosPorCliente(long IdCliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("@IDCLIENTE", IdCliente));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiariosPorCliente", parametros);
            List<DML.Beneficiario> listBen = Converter(ds);

            return listBen;
        }

        /// <summary>
        /// Consulta beneficiarios por CPF
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        internal List<DML.Beneficiario> ConsultarBeneficiariosPorCPF(string CPF)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("@CPF", CPF));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiariosPorCPF", parametros);
            List<DML.Beneficiario> listBen = Converter(ds);

            return listBen;
        }

        /// <summary>
        /// Valida se um CPF de Beneficiário já foi cadastrado anteriormente
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        internal bool VerificarExistencia(string CPF)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", CPF));

            DataSet ds = base.Consultar("FI_SP_VerificaBeneficiario", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Altera os dados do Beneficiário
        /// </summary>
        /// <param name="beneficiario"></param>
        internal void Alterar(DML.Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", beneficiario.Id));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IdCliente", beneficiario.IdCliente));
            
            base.Executar("FI_SP_AltBeneficiario", parametros);
        }

        /// <summary>
        /// Exclui um Beneficiário
        /// </summary>
        /// <param name="Id"></param>
        internal void Excluir(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            base.Executar("FI_SP_DelBeneficiario", parametros);
        }

        /// <summary>
        /// Converte um DataSet para um Lista de Beneficiários
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private List<DML.Beneficiario> Converter(DataSet ds)
        {
            List<DML.Beneficiario> lista = new List<DML.Beneficiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.Beneficiario ben = new DML.Beneficiario();
                    ben.Id = row.Field<long>("Id");
                    ben.Nome = row.Field<string>("Nome");
                    ben.CPF = row.Field<string>("CPF");
                    ben.IdCliente = row.Field<long>("IdCliente");
                    lista.Add(ben);
                }
            }

            return lista;
        }
    }
}
