using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Incluir(cliente);
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF);
        }

        /// <summary>
        /// Obter quantidade de clientes por CPF
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public List<DML.Cliente> ObterRegistrosPorCPF(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarCPFCadastrado(CPF);
        }

        /// <summary>
        /// Valida clientes e beneficiarios duplicados
        /// </summary>
        /// <param name="Inclusao"></param>
        /// <param name="CPF"></param>
        /// <param name="benefModelDuplic"></param>
        /// <param name="erros"></param>
        /// <returns></returns>
        public bool ValidarDuplicados(bool Inclusao, string CPF, long idCliente, out List<string> erros)
        {
            bool clientesDuplicados = false;
            List<DML.Cliente> clientesCadastrados = ObterRegistrosPorCPF(CPF);
            int registros = clientesCadastrados.Count();

            if (Inclusao)
                clientesDuplicados = registros > 0;
            else
            {
                if (registros == 0)
                    clientesDuplicados = false;
                else
                    clientesDuplicados = (clientesCadastrados.First().Id != idCliente && registros >= 1) || registros > 1;
            }

            erros = new List<string>();

            if (clientesDuplicados)
                erros.Add("CPF de Cliente já cadastrado.");

            return clientesDuplicados;
        }
    }
}
