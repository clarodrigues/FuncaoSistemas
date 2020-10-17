using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Text.RegularExpressions;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                Regex reg = new Regex(@"[^0-9]");
                string cpf = reg.Replace(model.CPF, string.Empty);

                bool duplicadosModel = model.Beneficiarios.GroupBy(x => x.CPF)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList().Count() > 0;

                List<string> erros;
                bool RegistrosDuplicadosCliente = boCliente.ValidarDuplicados(true, cpf, model.Id, out erros);
                
                bool beneficiariosDuplicadosModel = model.Beneficiarios.GroupBy(x => x.CPF)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList().Count() > 0;

                if (RegistrosDuplicadosCliente || beneficiariosDuplicadosModel)
                {
                    Response.StatusCode = 400;

                    if (beneficiariosDuplicadosModel)
                        erros.Add("CPF de Beneficiário duplicado na solicitação.");

                    return Json(string.Join(Environment.NewLine, erros));
                }

                model.Id = boCliente.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = cpf
                });

                if (model.Beneficiarios.Count > 0)
                {
                    model.Beneficiarios.ForEach(beneficiario =>
                    {
                        string cpfBeneficiario = reg.Replace(beneficiario.CPF, string.Empty);

                        boBeneficiario.Incluir(new Beneficiario()
                        {
                            Nome = beneficiario.Nome,
                            CPF = cpfBeneficiario,
                            IdCliente = model.Id
                        });
                    });
                }

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                Regex reg = new Regex(@"[^0-9]");
                string cpfCliente = reg.Replace(model.CPF, string.Empty);

                List<string> erros;
                bool RegistrosDuplicadosCliente = boCliente.ValidarDuplicados(false, cpfCliente, model.Id, out erros);
                
                bool beneficiariosDuplicadosModel = model.Beneficiarios.GroupBy(x => x.CPF)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList().Count() > 0;

                if (RegistrosDuplicadosCliente || beneficiariosDuplicadosModel)
                {
                    Response.StatusCode = 400;

                    if (beneficiariosDuplicadosModel)
                        erros.Add("CPF de Beneficiário duplicado na solicitação.");

                    return Json(string.Join(Environment.NewLine, erros));
                }

                boCliente.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    CPF = cpfCliente,
                    Telefone = model.Telefone
                });

                List<Beneficiario> beneficiariosCadastrados = boBeneficiario.ConsultarBeneficiariosPorCliente(model.Id);

                beneficiariosCadastrados.ForEach(beneficiario =>
                {
                    BeneficiarioModel beneficiarioEncontrado = model.Beneficiarios.Find(benef => benef.Id == beneficiario.Id);

                    if (beneficiarioEncontrado == null)
                        boBeneficiario.Excluir(beneficiario.Id);
                });

                model.Beneficiarios.ForEach(beneficiario =>
                {
                    Beneficiario beneficiarioEncontrado = beneficiariosCadastrados.Find(benef => benef.Id == beneficiario.Id);
                    string cpfBeneficiario = reg.Replace(beneficiario.CPF, string.Empty);

                    Beneficiario ben = new Beneficiario()
                    {
                        Id = beneficiario.Id,
                        Nome = beneficiario.Nome,
                        CPF = cpfBeneficiario,
                        IdCliente = model.Id
                    };

                    if (beneficiarioEncontrado != null)
                        boBeneficiario.Alterar(ben);
                    else
                        boBeneficiario.Incluir(ben);

                });

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);

            Models.ClienteModel model = null;

            if (cliente != null)
            {
                string cpf = Convert.ToUInt64(cliente.CPF).ToString(@"000\.000\.000\-00");

                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    CPF = cpf,
                    Telefone = cliente.Telefone,
                    Beneficiarios = new List<BeneficiarioModel>()
                };

                BoBeneficiario boBen = new BoBeneficiario();
                List<Beneficiario> beneficiarios = boBen.ConsultarBeneficiariosPorCliente(cliente.Id);

                List<Models.BeneficiarioModel> beneficiariosModel = new List<BeneficiarioModel>();

                if (beneficiarios != null && beneficiarios.Count > 0)
                {
                    beneficiarios.ForEach(ben =>
                    {
                        string cpfBenef = Convert.ToUInt64(ben.CPF).ToString(@"000\.000\.000\-00");
                        BeneficiarioModel benModel = new BeneficiarioModel()
                        {
                            Id = ben.Id,
                            Nome = ben.Nome,
                            CPF = cpfBenef
                        };
                        beneficiariosModel.Add(benModel);
                    });

                    model.Beneficiarios = beneficiariosModel;
                }
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}