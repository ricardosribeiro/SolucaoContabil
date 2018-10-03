﻿using SolucaoContabil.Data;
using SolucaoContabil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SolucaoContabil.Presentation.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var readerA = new StreamReader(@"C:\Users\ricar\OneDrive\Documentos\Empresa\Clientes\All Consultoria\2018.txt", Encoding.Default);
            var readerZ = new StreamReader(@"C:\Users\ricar\OneDrive\Documentos\Empresa\Clientes\All Consultoria\2018 - Copia.txt", Encoding.Default);
            var readerP = new StreamReader(@"C:\Users\ricar\OneDrive\Documentos\Empresa\Clientes\All Consultoria\A201806A.ext", Encoding.Default);


            string linha = null;
            string codProprietario = null;
            string nomeProprietario = null;
            string competencia = null;
            string codBanco = null;
            string banco = null;
            
            while ((linha = readerP.ReadLine()) != null)
            {
                //codImovel = Regex.Match(linha, @"(Imóvel:\s\d{1,5})").ToString();
                //codImovel = Regex.Replace(codImovel, @"([^0-9.])", "");
                codProprietario = Regex.Match(linha, @"(Prop:\d{1,5})").ToString();
                codBanco = Regex.Match(linha, @"(Banco\s\d{1,3})").ToString();
                if (!String.IsNullOrEmpty(codProprietario))
                {
                    codProprietario = Regex.Replace(codProprietario, @"([^0-9.])", "");
                    nomeProprietario = Regex.Match(linha, @"(Prop:\d{1,9}\s{1,6}\D{1,34}\S)").ToString();
                    nomeProprietario = nomeProprietario.Substring(nomeProprietario.IndexOf(" ")).Trim();
                    competencia = Regex.Match(linha, @"(\d{2}\/\d{4})").ToString();
                    System.Console.WriteLine($"{codProprietario.PadRight(10)}{nomeProprietario.PadRight(40)}{competencia.PadRight(30)}");
                }
                if (!string.IsNullOrEmpty(codBanco))
                {
                    codBanco = codBanco.Substring(codBanco.IndexOf(" ")).Trim();
                    banco = Regex.Match(linha, @"(Banco\s\d{1,3}\s\D{1,15}\S)").ToString();
                    banco = banco.Substring(banco.IndexOf(" ")+1);
                    System.Console.WriteLine($"{codBanco.PadRight(10)}{banco.PadRight(10)}");

                }
            }
            System.Console.ReadKey();

            //Dados dados = new Dados();
            //System.Console.WriteLine(DateTime.Now);
            //dados.ImportaSpedZRefatorado(readerZ);
            //System.Console.WriteLine(DateTime.Now);
        }
        class Dados
        {
            public void ImportarSpedZ(StreamReader streamReader)
            {
                using (var contexto = new SolucaoContabilContext())
                {
                    string linha = null;
                    string[] coluna = null;
                    IList<SpedZ> listaSpedZ = new List<SpedZ>();
                    var rgx = new Regex(@"\((\d{2}\/\d{4})\)");

                    try
                    {
                        while ((linha = streamReader.ReadLine()) != null)
                        {
                            if (linha.StartsWith("|I550"))
                            {
                                coluna = linha.Split('|');
                                string descricaoTaxa = coluna[6].Substring(6, coluna[6].Length - 6);
                                string complementoTaxa = coluna[7].ToUpper();
                                string competencia = rgx.Match(complementoTaxa).ToString();
                                string codigoTaxa = coluna[6].Substring(0, 5);
                                string tipoLancamento = coluna[4];
                                DateTime data = Convert.ToDateTime(coluna[2]);
                                string codigoImovelProprietario = coluna[3];

                                if (coluna.Length == 11)
                                {

                                    SpedZ spedZ = new SpedZ()
                                    {
                                        Data = data,
                                        CodigoImovelProprietario = codigoImovelProprietario,
                                        TipoLancamento = tipoLancamento,
                                        CodigoTaxa = codigoTaxa,
                                        DescricaoTaxa = descricaoTaxa.ToUpper(),
                                        ComplementoTaxa = complementoTaxa,
                                        Competencia = competencia
                                    };

                                    if (Convert.ToDecimal(coluna[8]) != 0)
                                        spedZ.Valor = -Convert.ToDecimal(coluna[8]);
                                    else
                                        spedZ.Valor = Convert.ToDecimal(coluna[9]);

                                    listaSpedZ.Add(spedZ);
                                }
                            }
                        }
                        contexto.SpedZ.AddRange(listaSpedZ);
                        contexto.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao importar dados. Detalhes: " + ex.Message);
                    }

                }
            }

            public void ImportaSpedZRefatorado(StreamReader streamReader)
            {
                using (var contexto = new SolucaoContabilContext())
                {
                    string linha = null;
                    string[] coluna = null;
                    IList<Lancamento> listaLancamento = new List<Lancamento>();
                    var rgx = new Regex(@"\((\d{2}\/\d{4})\)");

                    try
                    {
                        while ((linha = streamReader.ReadLine()) != null)
                        {
                            if (linha.StartsWith("|I550"))
                            {
                                coluna = linha.Split('|');
                                string descricaoTaxa = coluna[6].Substring(6, coluna[6].Length - 6);
                                string complementoTaxa = coluna[7].ToUpper();
                                string competencia = rgx.Match(complementoTaxa).ToString();
                                string codigoTaxa = coluna[6].Substring(0, 5);
                                string clienteTipoDescricao = coluna[4];
                                DateTime data = Convert.ToDateTime(coluna[2]);
                                int codCliente = Convert.ToInt32(coluna[3]);

                                if (coluna.Length == 11)
                                {

                                    Lancamento lancamento = new Lancamento()
                                    {
                                        Competencia = competencia,
                                        Data = data,
                                        ValorDebito = Convert.ToDecimal(coluna[8]),
                                        ValorCredito = Convert.ToDecimal(coluna[9])                                       
                                        
                                    };
                                    bool clienteExistente = contexto.Clientes.FirstOrDefault(p => p.Cod.Equals(codCliente))!=null;
                                    if (clienteExistente)
                                    {                                       
                                        lancamento.ClienteId = contexto.Clientes.FirstOrDefault(p => p.Cod.Equals(codCliente)).Id;
                                    }
                                    else
                                    {
                                        Cliente cliente = new Cliente()
                                        {
                                            Cod = codCliente,
                                            ClienteTipoId = contexto.ClienteTipo.FirstOrDefault(p => p.Descricao.Equals(clienteTipoDescricao)).Id,
                                            Descricao = null
                                        };
                                        contexto.Clientes.Add(cliente);
                                        contexto.SaveChanges();
                                        lancamento.ClienteId = contexto.Clientes.FirstOrDefault(p => p.Cod.Equals(codCliente)).Id;

                                    }
                                    listaLancamento.Add(lancamento);
                                }
                            }
                        }
                        contexto.Lancamentos.AddRange(listaLancamento);
                        contexto.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao importar dados. Detalhes: " + ex.Message);
                    }

                }

            }

            public void ImportarSpedA(StreamReader streamReader)
            {
                using (var contexto = new SolucaoContabilContext())
                {
                    string linha = null;
                    string[] coluna = null;
                    IList<SpedA> listaSpedA = new List<SpedA>();
                    var rgx = new Regex(@"\((\d{2}\/\d{4})\)");
                    SpedA spedA = null;

                    while ((linha = streamReader.ReadLine()) != null)
                    {
                        coluna = linha.Split('|');
                        if (linha.StartsWith("|I200"))
                        {
                            spedA = new SpedA();
                            var data = Convert.ToDateTime($"{coluna[3].Substring(0, 2)}/{coluna[3].Substring(2, 2)}/{coluna[3].Substring(4, 4)}");
                            if (coluna.Length <= 7)
                            {
                                spedA.Data = data;
                            }
                        }
                        if (linha.StartsWith("|I250"))
                        {
                            coluna = linha.Split('|');
                            string[] taxa = coluna[8].Split(' ');
                            string descricaoTaxa = coluna[8].Substring(taxa[0].Length, coluna[8].Length - taxa[0].Length).ToUpper();
                            decimal valor = Convert.ToDecimal(coluna[4]);
                            string competencia = rgx.Match(descricaoTaxa).ToString();
                            if (coluna.Length == 11)
                            {
                                spedA.CodigoTaxa = taxa[0];
                                spedA.DescricaoTaxa = descricaoTaxa;
                                spedA.Valor = valor;
                                spedA.Competencia = rgx.Match(descricaoTaxa).ToString();
                            }
                            listaSpedA.Add(spedA);
                        }
                    }
                    contexto.SpedA.AddRange(listaSpedA);
                    contexto.SaveChanges();



                    //contexto.SpedA.AddRange(listaSpedA);
                    //contexto.SaveChanges();
                }
            }


        }
    }
}
