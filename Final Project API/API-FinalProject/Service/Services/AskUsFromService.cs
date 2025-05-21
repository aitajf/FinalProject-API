using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interface;
using Service.DTO.Admin.AskUsFrom;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class AskUsFromService : IAskUsFromService
    {
        private readonly IAskUsFromRepository _askUsFromRepository;
        private readonly IMapper _mapper;
        public AskUsFromService(IAskUsFromRepository askUsFromRepository,
                                IMapper mapper)
        {
            _askUsFromRepository = askUsFromRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(AskUsFromCreateDto model)
        {
           await _askUsFromRepository.CreateAsync(_mapper.Map<AskUsFrom>(model));
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
