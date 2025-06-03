using AutoMapper;
using Domain.Entities;
using Repository.Repositories;
using Repository.Repositories.Interface;
using Service.DTO.Admin.AskUsFrom;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class AskUsFromService : IAskUsFromService
    {
        private readonly IAskUsFromRepository _askUsFromRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public AskUsFromService(IAskUsFromRepository askUsFromRepository,
                                IAccountService accountService,
                                IMapper mapper)
        {
            _askUsFromRepository = askUsFromRepository;
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task CreateAsync(AskUsFromCreateDto model)
        {
            var existingUser = await _accountService.GetUserByEmailAsync(model.Email);
            if (existingUser == null) throw new Exception("First be register.");
            var subscription = _mapper.Map<AskUsFrom>(model);
            subscription.CreatedDate = DateTime.UtcNow;
            await _askUsFromRepository.CreateAsync(subscription);
        }

        public async Task DeleteAsync(int id)
        {
           var askUs = await _askUsFromRepository.GetByIdAsync(id);
           if (askUs == null) throw new KeyNotFoundException($"AskUsFrom with ID {id} not found.");
           await _askUsFromRepository.DeleteAsync(askUs); 

        }

        public async Task<IEnumerable<AskUsFromDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<AskUsFromDto>>(await _askUsFromRepository.GetAllAsync());
        }
    }
}
