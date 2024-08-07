namespace UseCases;

public interface IView {
    Task<Entities.Investment> View(string id);
}