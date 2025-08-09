using EducaMBAXpert.Api.Authentication;
using EducaMBAXpert.Api.Interfaces;
using EducaMBAXpert.Api.ViewModels.User;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Alunos.Application.Interfaces;
using EducaMBAXpert.Alunos.Application.ViewModels;
using EducaMBAXpert.Alunos.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace EducaMBAXpert.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/alunos")]
    public class AlunosController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IAlunoConsultaAppService _alunoConsultaAppService;
        private readonly IAlunoComandoAppService _alunoComandoAppService;
        private readonly IMediator _mediator;


        public AlunosController(SignInManager<IdentityUser> signInManager,
                                  UserManager<IdentityUser> userManager,
                                  RoleManager<IdentityRole> roleManager,
                                  IOptions<JwtSettings> jwtSettings,
                                  IAppIdentityUser appIdentityUser,
                                  NotificationContext _notificationContext ,
                                  IMediator mediator,
                                  IAlunoConsultaAppService alunoConsultaAppService,
                                  IAlunoComandoAppService alunoComandoAppService) : base(mediator, _notificationContext, appIdentityUser)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _alunoConsultaAppService = alunoConsultaAppService;
            _alunoComandoAppService = alunoComandoAppService;   
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        [SwaggerOperation(Summary = "Registra um novo usuário", Description = "Cria um novo aluno e retorna um token JWT.")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (registerUser == null)
                return CustomResponse(HttpStatusCode.BadRequest, "Falha ao registrar o aluno");

            if (!ModelState.IsValid) 
                return ValidationProblem(ModelState);


            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (!result.Succeeded)
            {
                NotificarErro("Falha ao registrar o aluno.");
                return CustomResponse(HttpStatusCode.InternalServerError);
            }


            if (!await _roleManager.RoleExistsAsync("USER"))
                await _roleManager.CreateAsync(new IdentityRole("USER"));

            await _userManager.AddToRoleAsync(user, "USER");
            await _signInManager.SignInAsync(user, false);

            var aluno = new AlunoInputModel(id: Guid.Parse(user.Id) ,nome: registerUser.Nome, email: user.Email , ativo:true);
            await _alunoComandoAppService.Adicionar(aluno);

            return CustomResponse(HttpStatusCode.OK,(await GerarJwt(user.Email)));

        }

        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Autentica o aluno e retorna o token JWT")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (!result.Succeeded)
            {
                NotificarErro("Aluno ou senha incorretos.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var jwtResult = await GerarJwt(loginUser.Email);
            var user = await _userManager.FindByEmailAsync(loginUser.Email);

            var response = new
            {
                UserId = user?.Id,
                Token = jwtResult
            };
            return CustomResponse(HttpStatusCode.OK, response);

        }


        [HttpPost("{id:guid}/enderecos")]
        [SwaggerOperation(Summary = "Adiciona um endereço ao aluno")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AdicionarEndereco(Guid id, [FromBody] EnderecoInputModel endereco)
        {
            var aluno = await ObterAluno(id);
            if (aluno == null)
                return NotFoundResponse("Aluno não encontrado.");

            endereco.AlunoId = id;

            await _alunoComandoAppService.AdicionarEndereco(endereco);
            return CustomResponse(HttpStatusCode.OK);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter_todos")]
        [SwaggerOperation(Summary = "Lista todos os alunos")]
        [ProducesResponseType(typeof(IEnumerable<AlunoViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodos()
        {
            var alunos = await _alunoConsultaAppService.ObterTodos();
            return CustomResponse(HttpStatusCode.OK, alunos);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter/{id:guid}")]
        [SwaggerOperation(Summary = "Obtém um aluno por ID")]
        [ProducesResponseType(typeof(AlunoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var aluno = await _alunoConsultaAppService.ObterPorId(id);
            if (aluno == null)
                return NotFoundResponse("Usuário não encontrado.");

            return CustomResponse(HttpStatusCode.OK, aluno);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}/inativar")]
        [SwaggerOperation(Summary = "Inativa um aluno")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Inativar(Guid id)
        {
            var aluno = await ObterAluno(id);
            if (aluno == null)
                return NotFoundResponse("Aluno não encontrado.");

            var resultado = await _alunoComandoAppService.Inativar(id);
            if (!resultado)
            {
                NotificarErro("Não foi possível inativar o aluno.");
                return CustomResponse(HttpStatusCode.BadRequest);
            }

            return CustomResponse(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}/ativar")]
        [SwaggerOperation(Summary = "Ativa um aluno", Description = "Ativa o aluno informado.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Ativar(Guid id)
        {
            var aluno = await ObterAluno(id);
            if (aluno == null)
            {
                NotificarErro("Aluno não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var resultado = await _alunoComandoAppService.Ativar(id);
            if (!resultado)
            {
                NotificarErro("Não foi possível ativar o aluno.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.NoContent);
        }


        private async Task<string> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id),
                new (ClaimTypes.Name, user.UserName ?? ""),
                new (ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            // Adicionar roles como claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }

        private async Task<AlunoViewModel?> ObterAluno(Guid id)
        {
            AlunoViewModel aluno = await _alunoConsultaAppService.ObterPorId(id);
            if (aluno == null)
            {
                return null;
            }

            return aluno;
        }

        private IActionResult NotFoundResponse(string message)
        {
            NotificarErro(message);
            return CustomResponse(HttpStatusCode.NotFound);
        }
    }
}
