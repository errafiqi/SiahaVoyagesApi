using SiahaVoyages.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Threading;
using Volo.Abp.Users;

namespace SiahaVoyages.App
{
    public class ClientAppService : SiahaVoyagesAppService, IClientAppService
    {
        IRepository<Client, Guid> _clientRepository;

        IAccountAppService _accountAppService;

        IRepository<IdentityUser, Guid> _userRepository;

        IRepository<IdentityRole, Guid> _roleRepository;

        ICurrentUser _currentUser;

        IdentityUserManager UserManager { get; }

        public ClientAppService(IRepository<Client, Guid> ClientRepository, IAccountAppService accountAppService,
            IRepository<IdentityUser, Guid> userRepository, IRepository<IdentityRole, Guid> roleRepository,
            IdentityUserManager userManager, ICurrentUser currentUser)
        {
            _clientRepository = ClientRepository;
            _accountAppService = accountAppService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            UserManager = userManager;
            _currentUser = currentUser;
        }

        public async Task<ClientDto> GetAsync(Guid id)
        {
            var client = (await _clientRepository.WithDetailsAsync(c => c.User)).FirstOrDefault(c => c.Id == id);
            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public async Task<ClientDto> GetWithUserIdAsync(Guid id)
        {
            var client = (await _clientRepository.WithDetailsAsync(c => c.User)).FirstOrDefault(c => c.UserId == id);
            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public async Task<PagedResultDto<ClientDto>> GetListAsync(GetClientListDto input)
        {
            var query = await _clientRepository.WithDetailsAsync(c => c.User);

            var clients = query.WhereIf(!string.IsNullOrEmpty(input.Filter), c => c.User.Name.Contains(input.Filter))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(d => d.LastModificationTime != null ? d.LastModificationTime : d.CreationTime)
                .ToList();

            var totalCount = clients.Any() ? clients.Count() : 0;

            return new PagedResultDto<ClientDto>(
                totalCount,
                ObjectMapper.Map<List<Client>, List<ClientDto>>(clients)
            );
        }

        public async Task<ClientDto> CreateAsync(CreateClientDto input)
        {
            var registerUserDto = new RegisterDto
            {
                UserName = input.UserName,
                EmailAddress = input.Email,
                Password = input.Password,
                AppName = "SiahaVoyages"
            };
            var userDto = await _accountAppService.RegisterAsync(registerUserDto);

            var user = (await _userRepository.WithDetailsAsync()).FirstOrDefault(u => u.Id == userDto.Id);
            var roleId = (await _roleRepository.GetAsync(r => r.Name.Equals("client"))).Id;
            user.AddRole(roleId);
            user.Name = input.Name ?? "";
            user.Surname = input.Surname ?? "";
            user.SetPhoneNumber(input.PhoneNumber ?? "", true);

            user = await _userRepository.UpdateAsync(user);
            var client = ObjectMapper.Map<CreateClientDto, Client>(input);
            client.User = user;
            client.UserId = user.Id;

            var insertedClient = await _clientRepository.InsertAsync(client);

            return ObjectMapper.Map<Client, ClientDto>(insertedClient);
        }

        public async Task<ClientDto> UpdateAsync(Guid ClientId, UpdateClientDto input)
        {
            var client = (await _clientRepository.WithDetailsAsync(d => d.User))
                .FirstOrDefault(d => d.Id == ClientId);

            client.User.Name = input.Name;
            client.User.Surname = input.Surname;
            client.User.SetPhoneNumber(input.PhoneNumber ?? "", true);
            client.Adresse = input.Adresse;
            client.ICE = input.ICE;
            client.IF = input.IF;
            client.TP = input.TP;
            client.RC = input.RC;
            client.RIB = input.RIB;
            client.Contact = input.Contact;
            client = await _clientRepository.UpdateAsync(client);

            var user = await _userRepository.GetAsync(u => u.Id == client.UserId);

            var changeEmailToken = await UserManager.GenerateChangeEmailTokenAsync(user, input.Email);

            await UserManager.ChangeEmailAsync(user, input.Email, changeEmailToken);
            await UserManager.ConfirmEmailAsync(user, changeEmailToken);
            await UserManager.SetEmailAsync(user, input.Email);

            await UserManager.SetUserNameAsync(user, input.UserName);
            if (!string.IsNullOrEmpty(input.Password))
            {
                await EditPassword(ClientId, input.Password);
            }

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _clientRepository.DeleteAsync(id);
        }

        public async Task<ClientDto> EditPassword(Guid clientId, string newPassword)
        {
            var userId = _currentUser.Id.Value;
            var user = (await _userRepository.WithDetailsAsync()).FirstOrDefault(u => u.Id == userId);

            var resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);

            await UserManager.ResetPasswordAsync(user, resetToken, newPassword);

            var client = await _clientRepository.GetAsync(d => d.Id == clientId);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public async Task ChangeLogo(ChangeLogoDto input)
        {
            var client = await _clientRepository.GetAsync(input.clientId);
            client.Logo = input.base64Image;
        }
    }
}
