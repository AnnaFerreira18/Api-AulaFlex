using ApplicationService.Commands.Candidato;
using Domain;
using Domain.Entities;
using Domain.Enum;
using Domain.Repositories;
using Domain.Repositories.Elastic;
using Domain.Repositories.SQL;
using NotificationHelper.Assertions;
using NotificationHelper.Commands;
using System;
using System.Linq;

namespace Service
{
    public class CandidatoInserirService : ServerCommand
    {
        private readonly CandidatoInserirCommand _command;
        private readonly ICandidatoRepository _repository;
        private readonly ICursoRepository _cursoRepository;


        public CandidatoInserirService(CandidatoInserirCommand command, ICandidatoRepository repository, ICursoRepository cursoRepository,
            ISestSenatRepository sestSenatRepository, ICandidatoElasticRepository elasticRepository, ILGPDRepository lgpdRepository, ITelefoneRepository telefoneRepository, ICandidatoLogRepository candidatoLogRepository) : base(command)
        {
            _command = command;
            _repository = repository;
            _cursoRepository = cursoRepository;
        }

        public bool Run()
        {
            Validate();

            if (HasNotifications())
                return false;

            var candidato = new Candidato();
            candidato.Inserir(_command.Cpf, _command.Nome,
                _command.Email, _command.NomeMae, _command.Senha, _command.DataNascimento, _command.IdSexo, _command.IdEstadoCivil,
                _command.Cep, _command.Endereco, _command.Bairro, _command.Cidade, _command.Pais, _command.Uf, _command.AutorizaDivulgacao, _command.DisponivelViajar, _command.ParticiparProcesso, _command.AreaProfissional, _command.NivelProfissional, _command.CargoDesejado,
                idUnidade: _command.IdUnidade, ufNascimento: _command.UfNascimento, nacionalidade: _command.Nacionalidade, cidadeNascimento: _command.CidadeNascimento,
                idRendaFamiliar: _command.IdRendaFamiliar, numero: _command.Numero, complemento: _command.Complemento, idUnidadeSigop: _command.IdUnidadeSigop,
                idEscolaridade: _command.IdEscolaridade, idSituacaoTrabalho: _command.IdSituacaoTrabalho, ufUnidadeProxima: _command.UfUnidadeProxima);

            if (!_repository.Inserir(candidato)) return false;

            var cursosRealizados = _sestSenatRepository.PesquisarCursosRealizados(_command.Cpf);


            if (cursosRealizadosSigop.Any())
            {
                foreach (var curso in cursosRealizadosSigop)
                {
                    curso.IdCurso = Guid.NewGuid();
                    curso.IdCandidato = candidato.IdCandidato;
                    curso.Ativo = true;
                    curso.IdOrigemCurso = Guid.Parse("73c2b3e1-7287-4ecb-a645-8fecfde4d30b");
                    _cursoRepository.Inserir(curso);
                }
            }

            if (cursosRealizadosEad.Any())
            {
                foreach (var curso in cursosRealizadosEad)
                {
                    curso.IdCurso = Guid.NewGuid();
                    curso.IdCandidato = candidato.IdCandidato;
                    curso.Ativo = true;
                    curso.IdOrigemCurso = Guid.Parse("57f26485-3d6e-4ef6-bfd3-4845ee473f4a");
                    _cursoRepository.Inserir(curso);
                }
            }

            return true;
        }

        public void Validate()
        {
            AddNotification(Assert.IsTrue(!_repository.ChecarEmailDuplicado(_command.Email), "Email", "Email já utilizado."));
        }
    }
}
