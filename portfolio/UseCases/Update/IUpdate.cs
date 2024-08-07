using Entities;

namespace UseCases;

public interface IUpdate {
    Task<Investment> Update(string investmentID, Investment investment);
}