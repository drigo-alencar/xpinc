namespace UseCases;

public interface ICreate{
        Task<Entities.Investment> Create(Entities.Investment investment); 
}