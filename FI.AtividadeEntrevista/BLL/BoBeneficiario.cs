using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FI.AtividadeEntrevista.DAL;
using FI.AtividadeEntrevista.DML;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo Beneficiário para um cliente
        /// </summary>
        /// <param name="beneficiario"></param>
        /// <returns></returns>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera os dados do Beneficiário
        /// </summary>
        /// <param name="beneficiario"></param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            ben.Alterar(beneficiario);
        }

        /// <summary>
        /// Consulta um beneficiário por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DML.Beneficiario Consultar(long id)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Consultar(id);
        }

        /// <summary>
        /// Consulta todos os beneficiários de um cliente
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public List<DML.Beneficiario> ConsultarBeneficiariosPorCliente(long idCliente)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.ConsultarBeneficiariosPorCliente(idCliente);
        }

        /// <summary>
        /// Consulta todos os beneficiarios por CPF
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public List<DML.Beneficiario> ConsultarBeneficiariosPorCPF(string CPF)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.ConsultarBeneficiariosPorCPF(CPF);
        }

        /// <summary>
        /// Exclui um Beneficiário
        /// </summary>
        /// <param name="id"></param>
        public void Excluir(long id)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            ben.Excluir(id);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.VerificarExistencia(CPF);
        }
    }
}
